using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Threading;

using Asynts.Recall.Backend.Persistance.Data;

namespace Asynts.Recall.Backend.Services;

public class RoutingService : IRoutingService
{
    private readonly Dispatcher _dispatcher;

    private IList<RouteData> locations = new List<RouteData>();

    public RoutingService(Dispatcher dispatcher)
    {
        _dispatcher = dispatcher;
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
        Debug.WriteLine($"[RoutingService.Navigate] location={location}");

        locations.Add(location);

        NotifyRouteChanged();
    }

    private void NotifyRouteChanged()
    {
        _dispatcher.BeginInvoke(() => RouteChangedEvent?.Invoke(this, new RouteChangedEventArgs(Route))); 
    }
}
