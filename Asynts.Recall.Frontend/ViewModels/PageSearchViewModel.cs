﻿using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.Serialization;
using System;
using Microsoft.Extensions.DependencyInjection;
using CommunityToolkit.Mvvm.ComponentModel;
using Asynts.Recall.Backend.Persistance.Data;
using Asynts.Recall.Backend.Services;
using CommunityToolkit.Mvvm.Input;
using System.Threading;

namespace Asynts.Recall.Frontend.ViewModels;

public partial class PageSearchViewModel : ObservableObject
{
    public long DebugId { get; private set; }

    private readonly IServiceProvider _serviceProvider;
    private readonly ISearchService _searchService;
    private readonly IRoutingService _routingService;

    public PageSearchViewModel(
        IServiceProvider serviceProvider,
        ISearchService searchService,
        ObjectIDGenerator idGenerator,
        IRoutingService routingService)
    {
        DebugId = idGenerator.GetId(this, out _);

        _serviceProvider = serviceProvider;
        _searchService = searchService;
        _routingService = routingService;

        _routingService.RouteChangedEvent += _routingService_RouteChangedEvent;
    }

    private void _routingService_RouteChangedEvent(object sender, RouteChangedEventArgs eventArgs)
    {
        if (eventArgs.Route is PageSearchRouteData route)
        {
            SetSearchQuery(route);
        }
    }

    private CancellationTokenSource? searchServiceCancellationSource = null;
    [RelayCommand]
    public async void SetSearchQuery(PageSearchRouteData searchQuery)
    {
        // Clear the search results before doing any asynchronous operation.
        LoadPages(new List<PageData>());

        // Cancel any previous request.
        // This can happen in parallel since the backend will ensure that 'OperationCancelledException' is thrown.
        searchServiceCancellationSource?.Cancel();
        searchServiceCancellationSource?.Dispose();
        searchServiceCancellationSource = new CancellationTokenSource();

        IList<PageData> pages;
        try
        {
            pages = await _searchService.SearchAsync(searchQuery, searchServiceCancellationSource.Token);
        }
        catch (OperationCanceledException)
        {
            return;
        }

        LoadPages(pages);
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
