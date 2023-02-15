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

    // FIXME: Move this somewhere else.
    public void LoadExampleData()
    {
        contentList.Clear();

        Add(new ContentData
        {
            Id = 0,
            Title = "Hello, world!",
            Contents = "Hello to everyone!\nThis is an example.",
            Tags = new List<string>
            {
                "hello/",
                "notes/example/",
            },
        });
        Add(new ContentData
        {
            Id = 1,
            Title = "Another Example",
            Contents = "This is another example.",
            Tags = new List<string>
            {
                "notes/example/",
                "special/",
            },
        });
    }
}
