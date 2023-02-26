using System.Collections.Generic;

namespace Asynts.Recall.Backend.Persistance.Data;

public record PageData
{
    public required string Uuid { get; set; }
    public required string Title { get; set; }
    public required string Summary { get; set; }
    public string? Details { get; set; }
    public required IList<string> Tags { get; set; }
}
