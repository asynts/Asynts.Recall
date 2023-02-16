using System.Collections.Generic;
using System.Linq;

using CommunityToolkit.Mvvm.ComponentModel;

using Asynts.Recall.Backend.Persistance;
using Asynts.Recall.Backend.Persistance.Data;
using System.Diagnostics;

namespace Asynts.Recall.Frontend.ViewModels;

public partial class PageListViewModel : ObservableObject
{
    readonly IPageRepository _memoryContentRepository;
    readonly ISearchEngine _searchEngine;

    public PageListViewModel(IPageRepository memoryContentRepository, ISearchEngine searchEngine)
    {
        _memoryContentRepository = memoryContentRepository;
        _searchEngine = searchEngine;

        _searchEngine.ResultAvaliableEvent += _searchEngine_ResultAvaliableEvent;

        _searchEngine.UpdateSearchQueryAsync(new SearchQueryData
        {
            RequiredTags = new List<string>(),
            InterestingTerms = new List<string>(),
            RawTextQuery = "",
        });
    }

    private void _searchEngine_ResultAvaliableEvent(object sender, SearchEngineResultAvaliableEventArgs eventArgs)
    {
        PageList = eventArgs.ContentList;
    }

    [ObservableProperty]
    private IList<PageData> pageList = new List<PageData>();
}
