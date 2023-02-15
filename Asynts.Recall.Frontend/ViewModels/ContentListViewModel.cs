using System.Collections.Generic;
using System.Linq;
using Asynts.Recall.Backend.Persistance;
using Asynts.Recall.Backend.Persistance.Data;
using CommunityToolkit.Mvvm.Input;

namespace Asynts.Recall.Frontend.ViewModels;

// FIXME: Make this Observable.
public class ContentListViewModel
{
    readonly MemoryContentRepository _memoryContentRepository;
    readonly SearchEngine _searchEngine;

    public ContentListViewModel()
    {
        // FIXME: Use dependency injection here.

        _memoryContentRepository = new MemoryContentRepository();
        AddExampleData();

        _searchEngine = new SearchEngine(_memoryContentRepository);

        // FIXME: Get this from the user and use a better default.
        ContentList = _searchEngine.Search(new SearchQueryData
        {
            RequiredTags = new List<string>
            {
                "hello/",
            },
            InterestingTerms = new List<string>
            {
                "world/",
            },
            RawTextQuery = "",
        }).ToList();
    }

    public void SetSearchQuery(SearchQueryData searchQueryData)
    {
        ContentList = _searchEngine.Search(searchQueryData).ToList();
    }

    public IList<ContentData> ContentList { get; set; }

    private void AddExampleData()
    {
        _memoryContentRepository.Add(new ContentData
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
        _memoryContentRepository.Add(new ContentData
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
