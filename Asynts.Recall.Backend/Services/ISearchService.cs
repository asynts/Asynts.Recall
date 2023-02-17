using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Asynts.Recall.Backend.Persistance.Data;

namespace Asynts.Recall.Backend.Services;

public class SearchEngineResultAvaliableEventArgs : EventArgs
{
    public IList<PageData> Pages { get; private set; }

    public SearchEngineResultAvaliableEventArgs(IList<PageData> pages)
    {
        Pages = pages;
    }
};

public delegate void SearchEngineResultAvaliableHandler(object sender, SearchEngineResultAvaliableEventArgs eventArgs);

public interface ISearchService
{
    public event SearchEngineResultAvaliableHandler? ResultAvaliableEvent;

    IEnumerable<PageData> Search(SearchQueryData query);
    Task UpdateSearchQueryAsync(SearchQueryData searchQuery);
}
