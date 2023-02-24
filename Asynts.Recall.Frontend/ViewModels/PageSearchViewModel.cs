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
using CommunityToolkit.Mvvm.Input;
using System.Threading;
using System.Windows.Threading;
using Microsoft.Extensions.Logging;

namespace Asynts.Recall.Frontend.ViewModels;

public partial class PageSearchViewModel : ObservableObject
{
    public long DebugId { get; private set; }

    private readonly IServiceProvider _serviceProvider;
    private readonly ISearchService _searchService;
    private readonly IRoutingService _routingService;
    private readonly ILogger _logger;

    public PageSearchViewModel(
        IServiceProvider serviceProvider,
        ISearchService searchService,
        ObjectIDGenerator idGenerator,
        IRoutingService routingService,
        ILogger<PageSearchViewModel> logger)
    {
        DebugId = idGenerator.GetId(this, out _);

        _serviceProvider = serviceProvider;
        _searchService = searchService;
        _routingService = routingService;
        _logger = logger;

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

    private void LoadPages(IList<PageData> pages)
    {
        Pages.Clear();
        foreach (var pageData in pages)
        {
            var pageVM = ActivatorUtilities.CreateInstance<PageViewModel>(_serviceProvider, pageData);
            Pages.Add(pageVM);
        }
    }

    [ObservableProperty]
    private ObservableCollection<PageViewModel> pages = new ObservableCollection<PageViewModel>();
}
