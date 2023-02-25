using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Asynts.Recall.Backend.Persistance.Data;

namespace Asynts.Recall.Backend.Services;

public class PageParserServiceFactory
{
    public PageParserServiceFactory()
    {

    }

    public PageParserService Create(string input)
    {
        return new PageParserService(input);
    }
}

/// <summary>
/// Can be used to load <see cref="PageData" /> from files
/// </summary>
/// <remarks>
/// <para>
/// The input files are split into sections using an <code>---</code> indicator followed by the name of the section.
/// Some sections are optional but none may appear more than once.
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
/// <description>Summary of the contents is shown directly in search results.</description>
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
    public PageParserService(string input)
    {
        Input = input;
        Offset = 0;
    }

    public string Input { get; private set; }
    public int Offset { get; private set; }

    public string RemainingInput => Input.Substring(Offset);

    public PageData Parse(string input)
    {
        var sections = new Dictionary<string, Section>();

        while (!IsEnd())
        {
            TryConsumeWhitespace();

            var nextSection = ParseSection();
            if (nextSection == null)
            {
                Debug.Assert(IsEnd());
                break;
            }

            if (sections.ContainsKey(nextSection.Name))
            {
                // FIXME: Create a custom exception for this.
                throw new Exception($"Section '{nextSection.Name}' occurs multiple times.");
            }
            sections[nextSection.Name] = nextSection;
        }

        var allowedSectionNames = new string[] { "Metadata", "Comment", "Summary", "Details" };
        foreach (var kvp in sections)
        {
            if (!allowedSectionNames.Contains(kvp.Key))
            {
                // FIXME: Create a custom exception for this.
                throw new Exception($"Unknown section '{kvp.Key}'.");
            }
        }

        var mandatorySectionNames = new string[] { "Metadata", "Summary" };
        foreach (var mandatorySectionName in mandatorySectionNames)
        {
            if (!sections.ContainsKey(mandatorySectionName))
            {
                // FIXME: Create a custom exception for this.
                throw new Exception($"Missing section '{mandatorySectionName}'.");
            }
        }

        // FIXME
        throw new NotImplementedException();
    }

    private record Section
    {
        public required string Name { get; set; }
        public required string Contents { get; set; }
    }
    private Section? ParseSection()
    {
        if (!TryConsumeString("---"))
        {
            return null;
        }

        if (!TryConsumeInlineWhitespace())
        {
            // FIXME: Create a custom exception for this.
            throw new Exception("Expected whitespace before section header name.");
        }

        var sectionName = TryConsumeIdentifier();
        if (sectionName == null)
        {
            // FIXME: Create a custom exception for this.
            throw new Exception("Expected section name");
        }

        TryConsumeWhitespace();

        var contents = TryConsumeUntil("---");
        if (contents == null)
        {
            contents = ConsumeAll();
        }

        return new Section { Name = sectionName, Contents = contents };
    }

    private bool IsEnd()
    {
        return Offset >= Input.Length;
    }

    private char PeekChar()
    {
        return Input[Offset];
    }
    private char ConsumeChar()
    {
        char @char = PeekChar();
        ++Offset;
        return @char;
    }
    private bool TryConsumeInlineWhitespace()
    {
        return TryConsumeAnyOf(" \t") != null;
    }
    private bool TryConsumeWhitespace()
    {
        return TryConsumeAnyOf(" \t\r\n") != null;
    }
    private string? TryConsumeIdentifier()
    {
        return TryConsumeRegex(@"^[a-zA-Z]+");
    }
    private string? TryConsumeRegex(string pattern)
    {
        var regex = new Regex(pattern);
        var match = regex.Match(RemainingInput);

        if (!match.Success)
        {
            return null;
        }

        var result = match.ValueSpan.ToString();
        Offset += result.Length;
        return result;
    }
    private string? TryConsumeAnyOf(string chars)
    {
        int beginOffset = Offset;
        while (!IsEnd() && chars.Contains(PeekChar()))
        {
            ConsumeChar();
        }

        if (beginOffset == Offset)
        {
            return null;
        }

        return Input.Substring(beginOffset, Offset - beginOffset);
    }
    private bool TryConsumeString(string @string)
    {
        if (Input.StartsWith(@string))
        {
            Offset += @string.Length;
            return true;
        }

        return false;
    }
    private string? TryConsumeUntil(string @until)
    {
        int beginOffset = Offset;

        while (!RemainingInput.StartsWith(until))
        {
            if (IsEnd())
            {
                return null;
            }

            ConsumeChar();
        }

        return Input.Substring(beginOffset, Offset - beginOffset);
    }
    private string ConsumeAll()
    {
        var result = RemainingInput;
        Offset += result.Length;
        return result;
    }
}
