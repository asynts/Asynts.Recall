using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Asynts.Recall.Backend.Persistance.Data;
using Asynts.Recall.Backend.Utility;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Schema.Generation;

namespace Asynts.Recall.Backend.Services;

/// <summary>
/// Parses <see cref="PageData" /> objects from string.
/// </summary>
/// <remarks>
/// <para>
/// <list type="bullet">
/// <item>The input files are split into sections using an <c>---</c> indicator followed by the name of the section.</item>
/// <item>Some sections are optional but none may appear more than once.</item>
/// <item>The marker can only occur at the start of a line.</item>
/// <item>There must not be any whitespace before the first marker.</item>
/// <item>The section names are case sensitive.</item>
/// </list>
/// </para>
/// <para>
/// The following sections can be defined:
/// <list type="table">
/// <item>
/// <term>Metadata</term>
/// <description>
/// JSON document with the following properties: <c>id</c>, <c>title</c> and <c>tags</c>.
/// </description>
/// </item>
/// <item>
/// <term>Comment</term>
/// <description><em>(optional)</em> Can include additional information about the file itself, not indexed.</description>
/// </item>
/// <item>
/// <term>Summary</term>
/// <description>Summary of the contents that is shown directly in search results.</description>
/// </item>
/// <item>
/// <term>Details</term>
/// <description><em>(optional)</em> The contents of the documents if they do not fit into the summary.
/// Only shown when an individual page is viewed.</description>
/// </item>
/// </list>
/// </para>
/// </remarks>
/// <example>
/// <code>
/// --- Metadata
/// {
///     "id": "37cf9645-ce5d-43cd-b5fc-3cd386860d32",
///     "title": "This is an example page.",
///     "tags": [ "example/foo/", "hello/" ]
/// }
/// --- Comment
/// This is not indexed by the system and can be used to comment on the file itself.
/// --- Summary
/// This is the summary that is shown in the search results.
/// --- Details
/// This can be very long and is only shown when an individual page is viewed.
/// </code>
/// </example>
public class PageParserService : IPageParserService
{
    private readonly ILogger _logger;
    private readonly JSchema _metadataSchema;

    public PageParserService(ILogger<PageParserService> logger)
    {
        _logger = logger;

        _metadataSchema = JSchema.Parse("""
        {
            "type": "object",
            "properties": {
                "id": { "type": "integer" },
                "title": { "type": "string" ] },
                "tags": {
                    "type": [ "array" ],
                    "items": { "type": [ "string" ] }
                }
            },
            "required": [
                "id",
                "title",
                "tags"
            ]
        }
        """);
    }

    public PageData Parse(string input)
    {
        var sections = ParseIntoSections(input);
        VerifySections(sections);
        return ExtractDataFromSections(sections);
    }

    private PageData ExtractDataFromSections(IDictionary<string, SectionInfo> sections)
    {
        JObject metadata;
        try
        {
            metadata = JObject.Parse(sections["Metadata"].Content);
        } catch (JsonReaderException)
        {
            throw new ParserException(sections["Metadata"].Line, "failed to parse metadata");
        }
        if (!metadata.IsValid(_metadataSchema))
        {
            throw new ParserException(sections["Metadata"].Line, "failed to validate metadata");
        }

        string summary = sections["Summary"].Content;

        string? details = null;
        if (sections.TryGetValue("Details", out var detailsSection))
        {
            details = detailsSection.Content;
        }

        return new PageData
        {
            Id = metadata.Value<long>("id"),
            Title = metadata.Value<string>("title")!,
            Summary = summary,
            Details = details,
            Tags = metadata.Value<List<string>>("tags")!,
        };
    }

    private void VerifySections(IDictionary<string, SectionInfo> sections)
    {
        var requiredSectionNames = new string[] { "Metadata", "Summary" };
        foreach (var requiredSectionName in requiredSectionNames)
        {
            if (!sections.ContainsKey(requiredSectionName))
            {
                throw new ParserException(1, $"missing required section '{requiredSectionName}'");
            }
        }

        var allowedSectionNames = new string[] { "Metadata", "Comment", "Summary", "Details" };
        foreach (var kvp in sections)
        {
            if (!allowedSectionNames.Contains(kvp.Key))
            {
                throw new ParserException(kvp.Value.Line, $"unknown section '{kvp.Key}'");
            }
        }
    }

    private record SectionInfo
    {
        public required string Name { get; init; }
        public required int Line { get; init; }
        public required string Content { get; init; }
    }
    private enum ParserState {
        Initial,
        InsideContent,
    }
    private IDictionary<string, SectionInfo> ParseIntoSections(string input)
    {
        var sections = new Dictionary<string, SectionInfo>();

        var state = ParserState.Initial;
        LineInfo? currentSectionHeaderLine = null;
        string? currentSectionName = null;
        string? currentSectionContent = null;

        foreach (var lineInfo in EnumerateLines(input))
        {
            if (lineInfo.Text.StartsWith("---"))
            {
                var nextSectionHeaderInfo = ParseSectionHeader(lineInfo);

                if (sections.ContainsKey(nextSectionHeaderInfo.Name))
                {
                    throw new ParserException(lineInfo.Line, $"section '{nextSectionHeaderInfo.Name}' already defined");
                }

                switch (state)
                {
                    case ParserState.Initial:
                        break;
                    case ParserState.InsideContent:
                        var successful_1 = sections.TryAdd(
                            currentSectionName!,
                            new SectionInfo {
                                Name = currentSectionName!,
                                Content = currentSectionContent!,
                                Line = currentSectionHeaderLine!.Line,
                            }
                        );
                        Debug.Assert(successful_1);
                        break;
                }

                currentSectionName = nextSectionHeaderInfo.Name;
                currentSectionContent = "";
                currentSectionHeaderLine = lineInfo;
                state = ParserState.InsideContent;
            }
            else
            {
                switch (state)
                {
                    case ParserState.Initial:
                        throw new ParserException(lineInfo.Line, "expected initial marker");
                    case ParserState.InsideContent:
                        currentSectionContent += lineInfo.Text;
                        break;
                }
            }
        }

        if (state == ParserState.Initial)
        {
            throw new ParserException(1, "expected intial marker");
        }

        var successful_2 = sections.TryAdd(
            currentSectionName!,
            new SectionInfo {
                Name = currentSectionName!,
                Content = currentSectionContent!,
                Line = currentSectionHeaderLine!.Line,
            }
        );
        Debug.Assert(successful_2);

        return sections;
    }

    private record LineInfo
    {
        public required int Line { get; init; }
        public required string Text { get; init; }
    }
    private IEnumerable<LineInfo> EnumerateLines(string input)
    {
        using (var reader = new StringReader(input))
        {
            int line = 1;

            while (true)
            {
                var text = reader.ReadLine();
                if (text == null)
                {
                    yield break;
                }

                yield return new LineInfo { Line = line, Text = text };

                line += 1;
            }
        }
    }

    private record SectionHeaderInfo
    {
        public required string Name { get; init; }
    }
    private SectionHeaderInfo ParseSectionHeader(LineInfo lineInfo)
    {
        var lexer = new Lexer(lineInfo.Text);

        var successful_1 = lexer.TryConsumeString("---");
        Debug.Assert(successful_1);

        lexer.ConsumeInlineWhitespace();

        string? sectionName = lexer.ConsumeRegex(@"^[a-zA-Z-]+");
        if (sectionName == null)
        {
            throw new ParserException(lineInfo.Line, "expected section name after '---' marker");
        }

        return new SectionHeaderInfo { Name = sectionName };
    }
}
