using System.Collections.Generic;

using Asynts.Recall.Backend.Persistance.Data;

namespace Asynts.Recall.Backend.Persistance;

public class MemoryContentRepository : IContentRepository
{
    private List<ContentData> contentList = new List<ContentData>();

    public MemoryContentRepository()
    {

    }

    public void Add(ContentData data)
    {
        contentList.Add(data);
    }

    public IEnumerable<ContentData> All()
    {
        return contentList;
    }
}
