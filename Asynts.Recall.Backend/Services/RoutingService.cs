using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Threading;

using Asynts.Recall.Backend.Persistance.Data;
using Microsoft.Extensions.Logging;

namespace Asynts.Recall.Backend.Services;

public class RoutingService : IRoutingService
{
    private readonly Dispatcher _dispatcher;
    private readonly ILogger _logger;

    private IList<RouteData> locations = new List<RouteData>();

    public RoutingService(Dispatcher dispatcher, ILogger<RoutingService> logger)
    {
        _dispatcher = dispatcher;
        _logger = logger;
    }

    public RouteData Route => locations.Last();

    public bool NavigateBackPossible => locations.Count >= 2;


    public event RouteChangedHandler? RouteChangedEvent;

    public bool TryBack()
    {
        if (locations.Count >= 2)
        {
            locations.RemoveAt(locations.Count - 1);

            NotifyRouteChanged();
            return true;
        }

        return false;
    }

    public void Replace(RouteData location)
    {
        locations.Clear();
        locations.Add(location);

        NotifyRouteChanged();
    }

    public void Navigate(RouteData location)
    {
        _logger.LogDebug($"[Navigate] location={location}");

        locations.Add(location);

        NotifyRouteChanged();
    }

    private void NotifyRouteChanged()
    {
        _dispatcher.BeginInvoke(() => RouteChangedEvent?.Invoke(this, new RouteChangedEventArgs(Route))); 
    }
}
