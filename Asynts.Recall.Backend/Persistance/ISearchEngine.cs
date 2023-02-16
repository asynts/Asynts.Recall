using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Asynts.Recall.Backend.Persistance.Data;

namespace Asynts.Recall.Backend.Persistance;

public class SearchEngineResultAvaliableEventArgs : EventArgs
{
    public IList<ContentData> ContentList { get; private set; }

    public SearchEngineResultAvaliableEventArgs(IList<ContentData> contentList)
    {
        ContentList = contentList;
    }
};

public delegate void SearchEngineResultAvaliableHandler(object sender, SearchEngineResultAvaliableEventArgs eventArgs);

public interface ISearchEngine
{
    public event SearchEngineResultAvaliableHandler? ResultAvaliableEvent;

    IEnumerable<ContentData> Search(SearchQueryData query);
    Task UpdateSearchQueryAsync(SearchQueryData searchQuery);
}
