using System.Collections.Generic;

namespace Asynts.Recall.Backend.Persistance.Data;

public abstract record RouteData
{
    public abstract string NormalizedQuery();
    public abstract string RawQuery();
}

public record PageDetailsRouteData : RouteData
{
    public required long PageId { get; set; }

    public override string NormalizedQuery()
    {
        return $"#{PageId} ";
    }
    public override string RawQuery()
    {
        return "";
    }
}

public record PageSearchRouteData : RouteData
{
    public required IList<string> RequiredTags { get; set; }
    public required IList<string> InterestingTerms { get; set; }
    public required string RawText { get; set; }

    public override string NormalizedQuery()
    {
        string result = string.Empty;

        foreach (var tag in RequiredTags)
        {
            result += $"[{tag}] ";
        }
        foreach (var interestingTerm in InterestingTerms)
        {
            result += $"{interestingTerm} ";
        }

        return result;
    }

    public override string RawQuery()
    {
        return RawText;
    }
}
