using Asynts.Recall.Backend.Persistance.Data;
using System;

namespace Asynts.Recall.Backend.Services;

public class ParserException : FormatException
{
    public ParserException(int line, string rawMessage)
        : base($"in line {line}: {rawMessage}")
    {
        Line = line;
        RawMessage = rawMessage;
    }

    public int Line { get; private set; }
    public string RawMessage { get; private set; }
}

public interface IPageParseService
{
    /// <exception cref="ParserException" />
    PageData Parse(string input);
}
