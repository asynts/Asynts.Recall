using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Asynts.Recall.Backend.Utility;

internal class Lexer
{
    public Lexer(string input)
    {
        Input = input;
    }

    public string Input { get; private set; }
    public int Offset { get; private set; }

    public string RemainingInput => Input.Substring(Offset);

    public bool IsEnd()
    {
        return Offset >= Input.Length;
    }

    public char MustPeek()
    {
        return Input[Offset];
    }

    public char MustConsumeChar()
    {
        var @char = MustPeek();
        Offset += 1;
        return @char;
    }

    public bool TryConsumeString(string @string)
    {
        if (RemainingInput.StartsWith(@string))
        {
            Offset += @string.Length;
            return true;
        }

        return false;
    }

    public string? ConsumeInlineWhitespace()
    {
        int beginOffset = Offset;

        while (!IsEnd())
        {
            if (" \t".Contains(MustPeek()))
            {
                MustConsumeChar();
            }
            else
            {
                break;
            }
        }

        if (beginOffset == Offset)
        {
            return null;
        }

        return Input.Substring(beginOffset, Offset - beginOffset);
    }

    public string? ConsumeRegex(string @pattern)
    {
        var regex = new Regex(pattern, RegexOptions.Compiled);

        var match = regex.Match(RemainingInput);
        if (match.Success)
        {
            var result = match.Value;
            Offset += match.Length;
            return result;
        }

        return null;
    }
}
