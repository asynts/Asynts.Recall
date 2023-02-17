using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System.Diagnostics;

using CommunityToolkit.Mvvm.ComponentModel;

using Asynts.Recall.Backend.Persistance;
using Asynts.Recall.Backend.Persistance.Data;
using System;
using Microsoft.Extensions.DependencyInjection;

namespace Asynts.Recall.Frontend.ViewModels;

public partial class PageListViewModel : ObservableObject
{
    private readonly IServiceProvider _serviceProvider;
    readonly ISearchEngine _searchEngine;

    public PageListViewModel(IServiceProvider serviceProvider, ISearchEngine searchEngine)
    {
        _serviceProvider = serviceProvider;
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
            var pageVM = _serviceProvider.GetRequiredService<PageViewModel>();
            pageVM.Title = page.Title;
            pageVM.Contents = page.Contents;
            pageVM.Tags = page.Tags;
            Pages.Add(pageVM);
        }
    }

    [ObservableProperty]
    private ObservableCollection<PageViewModel> pages = new ObservableCollection<PageViewModel>();
}
