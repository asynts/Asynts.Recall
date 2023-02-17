using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Asynts.Recall.Backend.Persistance.Data;

namespace Asynts.Recall.Backend.Services;

public class SearchResultAvaliableEventArgs : EventArgs
{
    public IList<PageData> Pages { get; private set; }

    public SearchResultAvaliableEventArgs(IList<PageData> pages)
    {
        Pages = pages;
    }
};

public delegate void SearchResultAvaliableHandler(object sender, SearchResultAvaliableEventArgs eventArgs);

public interface ISearchService
{
    // Emitted in UI thread.
    public event SearchResultAvaliableHandler? ResultAvaliableEvent;

    IEnumerable<PageData> Search(PageSearchRouteData searchQuery);
    Task UpdateSearchQueryAsync(PageSearchRouteData searchQuery);
}
