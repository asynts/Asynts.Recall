using System.Collections.Generic;

namespace Asynts.Recall.Backend.Persistance.Data;

public record RouteData { }

public record PageDetailsRouteData : RouteData
{
    public required long PageId { get; set; }
}

public record PageSearchRouteData : RouteData
{
    public required IList<string> RequiredTags { get; set; }
    public required IList<string> InterestingTerms { get; set; }
    public required string RawText { get; set; }
}
