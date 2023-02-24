using System;
using System.Collections.Generic;

using Asynts.Recall.Backend.Persistance.Data;

namespace Asynts.Recall.Backend.Services;

public class RouteChangedEventArgs : EventArgs
{
    public RouteData Route { get; private set; }

    public RouteChangedEventArgs(RouteData route)
    {
        Route = route;
    }
}

public delegate void RouteChangedHandler(object sender, RouteChangedEventArgs eventArgs);

public interface IRoutingService
{
    public RouteData Route { get; }

    public bool NavigateBackPossible { get; }

    // Emitted in UI thread.
    public event RouteChangedHandler? RouteChangedEvent;

    public void Navigate(RouteData route);
    public void Replace(RouteData route);
    public bool TryBack();
}
