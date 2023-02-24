﻿using Asynts.Recall.Backend.Persistance.Data;
using Asynts.Recall.Backend.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
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

    public MainWindowViewModel(IRoutingService routingService, IServiceProvider serviceProvider)
    {
        _routingService = routingService;
        _serviceProvider = serviceProvider;

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
        Debug.WriteLine($"[MainWindowViewModel._routingService_RouteChangedEvent] route={eventArgs.Route}");

        if (eventArgs.Route is PageSearchRouteData pageSearchRoute)
        {
            var pageSearchVM = _serviceProvider.GetRequiredService<PageSearchViewModel>();
            pageSearchVM.SetSearchQuery(pageSearchRoute);

            CurrentViewModel = pageSearchVM;
        }
        else if (eventArgs.Route is PageDetailsRouteData pageDetailsRoute)
        {
            var pageVM = _serviceProvider.GetRequiredService<PageViewModel>();
            pageVM.Id = pageDetailsRoute.PageId;

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
