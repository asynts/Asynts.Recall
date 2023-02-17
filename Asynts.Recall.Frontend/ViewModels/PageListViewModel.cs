using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System.Diagnostics;

using CommunityToolkit.Mvvm.ComponentModel;

using Asynts.Recall.Backend.Persistance;
using Asynts.Recall.Backend.Persistance.Data;

namespace Asynts.Recall.Frontend.ViewModels;

public partial class PageListViewModel : ObservableObject
{
    readonly ISearchEngine _searchEngine;

    public PageListViewModel(ISearchEngine searchEngine)
    {
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
        Pages.Clear();
        foreach (var page in eventArgs.Pages) {
            Pages.Add(page);
        }
    }

    [ObservableProperty]
    private ObservableCollection<PageData> pages = new ObservableCollection<PageData>();
}
