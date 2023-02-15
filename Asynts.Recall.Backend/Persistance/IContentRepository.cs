using System.Collections.Generic;

using Asynts.Recall.Backend.Persistance.Data;

namespace Asynts.Recall.Backend.Persistance
{
    public interface IContentRepository
    {
        IEnumerable<ContentData> All();
    }
}
