using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System.Diagnostics;

using CommunityToolkit.Mvvm.ComponentModel;
using Asynts.Recall.Backend.Persistance.Data;
using System;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.Serialization;
using Asynts.Recall.Backend.Services;

namespace Asynts.Recall.Frontend.ViewModels;

public partial class PageListViewModel : ObservableObject
{
    public long Id { get; private set; }

    private readonly IServiceProvider _serviceProvider;
    readonly ISearchService _searchService;

    public PageListViewModel(IServiceProvider serviceProvider, ISearchService searchService, ObjectIDGenerator idGenerator)
    {
        Id = idGenerator.GetId(this, out _);

        _serviceProvider = serviceProvider;
        _searchService = searchService;

        _searchService.ResultAvaliableEvent += _searchEngine_ResultAvaliableEvent;

        _searchService.UpdateSearchQueryAsync(new SearchQueryData
        {
            RequiredTags = new List<string>(),
            InterestingTerms = new List<string>(),
            RawTextQuery = "",
        });
    }

    private void _searchEngine_ResultAvaliableEvent(object sender, SearchResultAvaliableEventArgs eventArgs)
    {
        Pages.Clear();
        foreach (var page in eventArgs.Pages) {
            var pageVM = _serviceProvider.GetRequiredService<PageViewModel>();
            pageVM.Title = page.Title;
            pageVM.Contents = page.Contents;
            pageVM.Tags = page.Tags;
            Pages.Add(pageVM);

            Debug.WriteLine($"[PageListViewModel._searchEngine_ResultAvaliableEvent] adding page id={pageVM.Id}");
        }
    }

    [ObservableProperty]
    private ObservableCollection<PageViewModel> pages = new ObservableCollection<PageViewModel>();
}
