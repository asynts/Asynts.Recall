using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.Serialization;
using System;
using Microsoft.Extensions.DependencyInjection;
using CommunityToolkit.Mvvm.ComponentModel;
using Asynts.Recall.Backend.Persistance.Data;
using Asynts.Recall.Backend.Services;

namespace Asynts.Recall.Frontend.ViewModels;

public partial class PageSearchViewModel : ObservableObject
{
    public long DebugId { get; private set; }

    private readonly IServiceProvider _serviceProvider;
    readonly ISearchService _searchService;

    public PageSearchViewModel(IServiceProvider serviceProvider, ISearchService searchService, ObjectIDGenerator idGenerator)
    {
        DebugId = idGenerator.GetId(this, out _);

        _serviceProvider = serviceProvider;
        _searchService = searchService;

        // Maybe, the result is already avaliable, otherwise, we will get an event when it becomes avaliable.
        if (_searchService.LastSearchResult != null)
        {
            LoadPages(_searchService.LastSearchResult!);
        }
        _searchService.ResultAvaliableEvent += _searchEngine_ResultAvaliableEvent;
    }

    private void _searchEngine_ResultAvaliableEvent(object sender, SearchResultAvaliableEventArgs eventArgs)
    {
        LoadPages(eventArgs.Pages);
    }

    private void LoadPages(IList<PageData> pages)
    {
        Pages.Clear();
        foreach (var page in pages)
        {
            var pageVM = _serviceProvider.GetRequiredService<PageViewModel>();
            pageVM.Title = page.Title;
            pageVM.Contents = page.Contents;
            pageVM.Tags = page.Tags;
            pageVM.Id = page.Id;
            Pages.Add(pageVM);
        }
    }

    [ObservableProperty]
    private ObservableCollection<PageViewModel> pages = new ObservableCollection<PageViewModel>();
}
