using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using Asynts.Recall.Backend.Persistance.Data;
using Asynts.Recall.Backend.Services;

namespace Asynts.Recall.Backend.Persistance;

public class MemoryPageRepository : IPageRepository
{
    private readonly IPageParserService _pageParserService;
    private readonly Dispatcher _dispatcher;

    public MemoryPageRepository(IPageParserService pageParserService, Dispatcher dispatcher)
    {
        _pageParserService = pageParserService;
        _dispatcher = dispatcher;
    }

    private Dictionary<string, PageData> pages = new Dictionary<string, PageData>();

    public event EventHandler? LoadedEvent;

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

    // This should be part of 'System.IO.Directory'!
    private Task<string[]> GetFilesAsync(string path, CancellationToken cancellationToken = default)
    {
        return Task.Run(() =>
        {
            return Directory.GetFiles(path);
        }, cancellationToken: cancellationToken);
    }

    private void NotifyLoaded()
    {
        _dispatcher.BeginInvoke(() => LoadedEvent?.Invoke(this, new EventArgs()));
    }

    // FIXME: Move this somewhere else.
    public async Task LoadFromDiskAsync(string directoryPath, CancellationToken cancellationToken = default)
    {
        Debug.Assert(pages.Count == 0);

        foreach (var filePath in await GetFilesAsync(directoryPath, cancellationToken))
        {
            var text = await File.ReadAllTextAsync(filePath, cancellationToken);
            var pageData = _pageParserService.Parse(text);

            Add(pageData);
        }

        NotifyLoaded();
    }

    // FIXME: Move this somewhere else.
    public void LoadExampleData()
    {
        Debug.Assert(pages.Count == 0);

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

        NotifyLoaded();
    }
}
