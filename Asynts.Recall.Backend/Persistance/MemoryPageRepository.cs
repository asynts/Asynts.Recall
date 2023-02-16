using System.Collections.Generic;

using Asynts.Recall.Backend.Persistance.Data;

namespace Asynts.Recall.Backend.Persistance;

public class MemoryPageRepository : IPageRepository
{
    private List<PageData> pages = new List<PageData>();

    public void Add(PageData page)
    {
        pages.Add(page);
    }

    public IEnumerable<PageData> All()
    {
        return pages;
    }

    public void LoadExampleData()
    {
        pages.Clear();

        Add(new PageData
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
        Add(new PageData
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
