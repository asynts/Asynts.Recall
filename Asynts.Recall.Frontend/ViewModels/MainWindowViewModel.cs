using Asynts.Recall.Backend.Persistance.Data;
using Asynts.Recall.Backend.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Asynts.Recall.Frontend.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    private readonly IRoutingService _routingService;
    private readonly IServiceProvider _serviceProvider;

    public MainWindowViewModel(IRoutingService routingService, IServiceProvider serviceProvider)
    {
        _routingService = routingService;
        _serviceProvider = serviceProvider;

        currentViewModel = _serviceProvider.GetRequiredService<PageListViewModel>();
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

        if (eventArgs.Route is PageSearchRouteData)
        {
            CurrentViewModel = _serviceProvider.GetRequiredService<PageListViewModel>();
        }
        else if (eventArgs.Route is PageDetailsRouteData)
        {
            CurrentViewModel = _serviceProvider.GetRequiredService<PageViewModel>();
        }
    }

    [ObservableProperty]
    public ObservableObject currentViewModel;
}
