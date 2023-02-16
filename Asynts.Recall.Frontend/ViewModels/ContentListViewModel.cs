using System.Collections.Generic;
using System.Linq;

using CommunityToolkit.Mvvm.ComponentModel;

using Asynts.Recall.Backend.Persistance;
using Asynts.Recall.Backend.Persistance.Data;
using System.Diagnostics;

namespace Asynts.Recall.Frontend.ViewModels;

public partial class ContentListViewModel : ObservableObject
{
    readonly IContentRepository _memoryContentRepository;
    readonly ISearchEngine _searchEngine;

    public ContentListViewModel(IContentRepository memoryContentRepository, ISearchEngine searchEngine)
    {
        _memoryContentRepository = memoryContentRepository;
        _searchEngine = searchEngine;

        _searchEngine.ResultAvaliableEvent += _searchEngine_ResultAvaliableEvent;

        _searchEngine.UpdateSearchQueryAsync(new SearchQueryData
        {
            RequiredTags = new List<string>(),
            InterestingTerms = new List<string>(),
            RawTextQuery = "",
        });
    }

    private void _searchEngine_ResultAvaliableEvent(object sender, SearchEngineResultAvaliableEventArgs eventArgs)
    {
        ContentList = eventArgs.ContentList;
    }

    [ObservableProperty]
    private IList<ContentData> contentList = new List<ContentData>();
}
