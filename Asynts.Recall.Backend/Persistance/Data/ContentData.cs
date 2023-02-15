using System.Collections.Generic;

namespace Asynts.Recall.Backend.Persistance.Data
{
    public record ContentData
    {
        public required int Id { get; set; }
        public required string Title { get; set; }
        public required string Contents { get; set; }
        public required IList<string> Tags { get; set; }
    }
}
