using System;
using System.Collections.Generic;
using System.Diagnostics;
using Asynts.Recall.Backend.Persistance.Data;

namespace Asynts.Recall.Backend.Services;

public class ParserException : FormatException
{
    public ParserException(int line, int column, string rawMessage)
        : base($"{line}:{column}: {rawMessage}")
    {
        Line = line;
        Column = column;
        RawMessage = rawMessage;
    }

    public int Line { get; private set; }
    public int Column { get; private set; }
    public string RawMessage { get; private set; }
}

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
public class PageParserService
{
    public PageParserService()
    {

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

        foreach (var line in EnumerateLines(input))
        {
            if (line.Text.StartsWith("---"))
            {
                var nextSection = ParseSectionHeader(line: line, existingSections: sections);

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

                currentSectionName = nextSection.Name;
                currentSectionContent = "";
                currentSectionHeaderLine = line;
                state = ParserState.InsideContent;
            }
            else
            {
                switch (state)
                {
                    case ParserState.Initial:
                        throw new ParserException(line.Line, 1, "expected initial marker");
                    case ParserState.InsideContent:
                        currentSectionContent += line.Text;
                        break;
                }
            }
        }

        if (state == ParserState.Initial)
        {
            throw new ParserException(1, 1, "expected intial marker");
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
        public required int Offset { get; init; }
        public required string Text { get; init; }
    }
    private IEnumerable<LineInfo> EnumerateLines(string input)
    {
        // FIXME
        throw new NotImplementedException();
    }

    private record SectionHeaderInfo
    {
        public required string Name { get; init; }
    }
    private SectionHeaderInfo ParseSectionHeader(LineInfo line, IDictionary<string, SectionInfo> existingSections)
    {
        // FIXME
        throw new NotImplementedException();
    }
}
