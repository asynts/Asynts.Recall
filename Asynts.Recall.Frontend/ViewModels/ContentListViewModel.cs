using System.Collections.Generic;
using System.Linq;

using CommunityToolkit.Mvvm.ComponentModel;

using Asynts.Recall.Backend.Persistance;
using Asynts.Recall.Backend.Persistance.Data;

namespace Asynts.Recall.Frontend.ViewModels;

public partial class ContentListViewModel : ObservableObject
{
    readonly IContentRepository _memoryContentRepository;
    readonly ISearchEngine _searchEngine;

    public ContentListViewModel(IContentRepository memoryContentRepository, ISearchEngine searchEngine)
    {
        _memoryContentRepository = memoryContentRepository;
        _searchEngine = searchEngine;

        SetSearchQuery(new SearchQueryData
        {
            RequiredTags = new List<string>(),
            InterestingTerms = new List<string>(),
            RawTextQuery = "",
        });
    }

    public void SetSearchQuery(SearchQueryData searchQueryData)
    {
        ContentList = _searchEngine.Search(searchQueryData).ToList();
    }

    [ObservableProperty]
    private IList<ContentData> contentList = new List<ContentData>();
}
