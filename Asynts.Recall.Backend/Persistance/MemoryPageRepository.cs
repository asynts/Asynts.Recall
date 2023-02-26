using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Asynts.Recall.Backend.Persistance.Data;

namespace Asynts.Recall.Backend.Persistance;

public class MemoryPageRepository : IPageRepository
{
    private Dictionary<string, PageData> pages = new Dictionary<string, PageData>();

    public void Add(PageData page)
    {
        var successful = pages.TryAdd(page.Uuid, page);
        Debug.Assert(successful);
    }

    public IEnumerable<PageData> All()
    {
        return pages.Select(kvp => kvp.Value);
    }

    public PageData GetByUuid(string uuid)
    {
        var successful = pages.TryGetValue(uuid, out var page);
        Debug.Assert(successful);

        return page!;
    }

    public void LoadExampleData()
    {
        pages.Clear();

        Add(new PageData
        {
            Uuid = "e6d61d1b-2ba2-4f22-a257-68b2d1d42b38",
            Title = "Hello, world!",
            Summary = "Hello to everyone!\nThis is an example.",
            Details = null,
            Tags = new List<string>
            {
                "hello/",
                "notes/example/",
            },
        });
        Add(new PageData
        {
            Uuid = "b52cb001-5cfa-4ace-8e4e-027329ce97d3",
            Title = "Another Example",
            Summary = "This is another example.",
            Details = "Here are some details:\n 1. Detail 1\n 2. Detail 2",
            Tags = new List<string>
            {
                "notes/example/",
                "special/",
            },
        });
    }
}
