﻿using Asynts.Recall.Backend.Persistance;
using Asynts.Recall.Backend.Persistance.Data;
using Asynts.Recall.Backend.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Windows.Threading;

namespace Asynts.Recall.Frontend.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    private readonly IRoutingService _routingService;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger _logger;
    private readonly IPageRepository _pageRepository;

    public MainWindowViewModel(
        IRoutingService routingService,
        IServiceProvider serviceProvider,
        ILogger<MainWindowViewModel> logger,
        IPageRepository pageRepository)
    {
        _routingService = routingService;
        _serviceProvider = serviceProvider;
        _logger = logger;
        _pageRepository = pageRepository;
        _routingService.RouteChangedEvent += _routingService_RouteChangedEvent;

        _routingService.Navigate(new PageSearchRouteData
        {
            RequiredTags = new List<string>(),
            InterestingTerms = new List<string>(),
            RawText = "",
        });
    }

    private void _routingService_RouteChangedEvent(object sender, RouteChangedEventArgs eventArgs)
    {
        _logger.LogDebug($"[_routingService_RouteChangedEvent] route={eventArgs.Route}");

        if (eventArgs.Route is PageSearchRouteData pageSearchRoute)
        {
            var pageSearchVM = _serviceProvider.GetRequiredService<PageSearchViewModel>();
            pageSearchVM.SetSearchQuery(pageSearchRoute);

            CurrentViewModel = pageSearchVM;
        }
        else if (eventArgs.Route is PageDetailsRouteData pageDetailsRoute)
        {
            var pageData = _pageRepository.GetById(pageDetailsRoute.PageId);
            var pageVM = ActivatorUtilities.CreateInstance<PageViewModel>(_serviceProvider, pageData);

            CurrentViewModel = pageVM;
        }

        NavigateBackEnabled = _routingService.NavigateBackPossible;
    }

    [RelayCommand]
    public void NavigateBack()
    {
        _routingService.TryBack();
    }

    [ObservableProperty]
    public ObservableObject? currentViewModel = null;

    [ObservableProperty]
    public bool navigateBackEnabled = false;
}
