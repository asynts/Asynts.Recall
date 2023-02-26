using System;
using System.Collections.Generic;
using System.Diagnostics;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Asynts.Recall.Backend.Persistance.Data;
using Asynts.Recall.Backend.Services;

namespace Asynts.Recall.Frontend.ViewModels;

public partial class QueryBoxViewModel : ObservableObject
{
    private readonly IRoutingService _routingService;

    public QueryBoxViewModel()
    {
        _routingService = null!;

        query = string.Empty;
        rawQuery = string.Empty;
    }

    public QueryBoxViewModel(IRoutingService routingService)
    {
        _routingService = routingService;

        query = string.Empty;
        rawQuery = string.Empty;

        _routingService.RouteChangedEvent += _routingService_RouteChangedEvent;
    }

    private void _routingService_RouteChangedEvent(object sender, RouteChangedEventArgs eventArgs)
    {
        Query = eventArgs.Route.NormalizedQuery();
        RawQuery = eventArgs.Route.RawQuery();
    }

    [ObservableProperty]
    private string query;

    [ObservableProperty]
    private string rawQuery;

    [RelayCommand]
    public void SubmitQuery()
    {
        var route = ParseQuery();
        _routingService.Navigate(route);
    }

    private RouteData ParseQuery()
    {
        var interestingTerms = new List<string>();
        var requiredTags = new List<string>();

        var queryParts = Query.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        foreach (var queryPart in queryParts)
        {
            if (queryPart.StartsWith("#"))
            {
                string pageUuid = queryPart.Substring(1);
                return new PageDetailsRouteData { PageUuid = pageUuid };
            }
            else if (queryPart.StartsWith('[') && queryPart.EndsWith(']')) {
                requiredTags.Add(queryPart.Substring(1, queryPart.Length - 2));
            }
            else
            {
                interestingTerms.Add(queryPart);
            }
        }

        return new PageSearchRouteData
        {
            InterestingTerms = interestingTerms,
            RequiredTags = requiredTags,
            RawText = RawQuery,
        };
    }
}
