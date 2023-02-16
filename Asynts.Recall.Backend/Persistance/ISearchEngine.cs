using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Asynts.Recall.Backend.Persistance.Data;

namespace Asynts.Recall.Backend.Persistance;

public class SearchEngineResultAvaliableEventArgs : EventArgs
{
    public IList<PageData> ContentList { get; private set; }

    public SearchEngineResultAvaliableEventArgs(IList<PageData> contentList)
    {
        ContentList = contentList;
    }
};

public delegate void SearchEngineResultAvaliableHandler(object sender, SearchEngineResultAvaliableEventArgs eventArgs);

public interface ISearchEngine
{
    public event SearchEngineResultAvaliableHandler? ResultAvaliableEvent;

    IEnumerable<PageData> Search(SearchQueryData query);
    Task UpdateSearchQueryAsync(SearchQueryData searchQuery);
}
