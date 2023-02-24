using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Asynts.Recall.Backend.Persistance.Data;

namespace Asynts.Recall.Backend.Persistance;

public class MemoryPageRepository : IPageRepository
{
    private Dictionary<long, PageData> pages = new Dictionary<long, PageData>();

    public void Add(PageData page)
    {
        var successful = pages.TryAdd(page.Id, page);
        Debug.Assert(successful);
    }

    public IEnumerable<PageData> All()
    {
        return pages.Select(kvp => kvp.Value);
    }

    public PageData GetById(long id)
    {
        var successful = pages.TryGetValue(id, out var page);
        Debug.Assert(successful);

        return page!;
    }

    public void LoadExampleData()
    {
        pages.Clear();

        Add(new PageData
        {
            Id = 0,
            Title = "Hello, world!",
            Contents = "Hello to everyone!\nThis is an example.",
            Details = null,
            Tags = new List<string>
            {
                "hello/",
                "notes/example/",
            },
        });
        Add(new PageData
        {
            Id = 1,
            Title = "Another Example",
            Contents = "This is another example.",
            Details = "Here are some details:\n 1. Detail 1\n 2. Detail 2",
            Tags = new List<string>
            {
                "notes/example/",
                "special/",
            },
        });
    }
}
