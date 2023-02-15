using System.Collections.Generic;

namespace Asynts.Recall.Backend.Persistance.Data;

public record SearchQueryData
{
    public required IList<string> RequiredTags { get; set; }
    public required IList<string> InterestingTerms { get; set; }
    public required string RawTextQuery { get; set; }
}
