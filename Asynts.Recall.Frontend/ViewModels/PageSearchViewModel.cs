using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.Serialization;
using System;
using System.Threading;
using System.Windows.Threading;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Asynts.Recall.Backend.Persistance.Data;
using Asynts.Recall.Backend.Services;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Asynts.Recall.Frontend.ViewModels;

public partial class PageSearchViewModel : ObservableObject
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ISearchService _searchService;
    private readonly IRoutingService _routingService;
    private readonly ILogger _logger;
    private readonly PageViewModelFactory _pageViewModelFactory;

    public PageSearchViewModel()
    {
        _serviceProvider = null!;
        _searchService = null!;
        _routingService = null!;
        _logger = null!;
        _pageViewModelFactory = null!;

        LoadPages(new List<PageData> {
            new PageData
            {
                Id = 1,
                Title = "Title (Page #1)",
                Contents = "Contents (Page #1)",
                Details = "Details (Page #1)",
                Tags = new List<string> { "example/1/", "hello/" },
            },
            new PageData
            {
                Id = 2,
                Title = "Title (Page #2)",
                Contents = "Contents (Page #2)",
                Details = "Details (Page #2)",
                Tags = new List<string> { "example/2/", "hello/" },
            },
            new PageData
            {
                Id = 3,
                Title = "Title (Page #3)",
                Contents = "Contents (Page #3)",
                Details = "Details (Page #3)",
                Tags = new List<string> { "example/3/", "hello/" },
            },
        }, isDesignTime: true);
    }

    public PageSearchViewModel(
        IServiceProvider serviceProvider,
        ISearchService searchService,
        IRoutingService routingService,
        ILogger<PageSearchViewModel> logger,
        PageViewModelFactory pageViewModelFactory)
    {
        _serviceProvider = serviceProvider;
        _searchService = searchService;
        _routingService = routingService;
        _logger = logger;
        _pageViewModelFactory = pageViewModelFactory;

        _routingService.RouteChangedEvent += _routingService_RouteChangedEvent;
    }

    private void _routingService_RouteChangedEvent(object sender, RouteChangedEventArgs eventArgs)
    {
        _logger.LogDebug($"[_routingService_RouteChangedEvent] route={eventArgs.Route}");

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

        _logger.LogDebug($"[SetSearchQuery]: cancelling existing request");

        // Cancel any previous request.
        // This can happen in parallel since the backend will ensure that 'OperationCancelledException' is thrown.
        searchServiceCancellationSource?.Cancel();
        searchServiceCancellationSource?.Dispose();
        searchServiceCancellationSource = new CancellationTokenSource();

        IList<PageData> pages;
        try
        {
            pages = await _searchService.SearchAsync(searchQuery, searchServiceCancellationSource.Token);
            _logger.LogDebug($"[SetSearchQuery]: got result");
        }
        catch (OperationCanceledException)
        {
            _logger.LogDebug($"[SetSearchQuery]: aborted");
            return;
        }

        // FIXME: I hope this runs in the correct synchronization context!
        LoadPages(pages);
    }

    private void LoadPages(IList<PageData> pages, bool isDesignTime = false)
    {
        Pages.Clear();
        foreach (var pageData in pages)
        {
            PageViewModel pageVM;
            if (isDesignTime)
            {
                pageVM = new PageViewModel(pageData);
            }
            else
            {
                pageVM = _pageViewModelFactory.Create(pageData);
            }

            Pages.Add(pageVM);
        }
    }

    [ObservableProperty]
    private ObservableCollection<PageViewModel> pages = new ObservableCollection<PageViewModel>();
}
