using Asynts.Recall.Backend.Persistance.Data;

namespace Asynts.Recall.Backend.Persistance;

public class ResultAvaliableEventArgs : EventArgs
{
    public IList<ContentData> ContentList { get; private set; }

    public ResultAvaliableEventArgs(IList<ContentData> contentList)
    {
        ContentList = contentList;
    }
};

public delegate void ResultAvaliableHandler(object sender, ResultAvaliableEventArgs eventArgs);

public class SearchEngine : ISearchEngine
{
    private readonly IContentRepository _contentRepository;

    public SearchEngine(IContentRepository contentRepository)
    {
        _contentRepository = contentRepository;
    }

    public event ResultAvaliableHandler? ResultAvaliableEvent;

    // FIXME: Dispose?
    private CancellationTokenSource? _searchQueryCancellationToken = null;

    // FIXME: This function assumes that it runs in the UI thread.
    //        Instead we should take a synchronization context in the constructor and use that for the continuation.
    public Task UpdateSearchQueryAsync(SearchQueryData searchQuery)
    {
        if (_searchQueryCancellationToken != null)
        {
            _searchQueryCancellationToken.Cancel();
            _searchQueryCancellationToken.Dispose();
        }

        _searchQueryCancellationToken = new CancellationTokenSource();
        var cancellationToken = _searchQueryCancellationToken.Token;

        return Task
            // We allow the search to be cancelled, if the thread pool hasn't started processing it yet.
            .Run(() =>
            {
                // We allow the search to be cancelled in the middle.
                return Search(searchQuery)
                    .AsParallel()
                    .WithCancellation(cancellationToken)
                    .ToList();
            }, cancellationToken)
            // We allow the search to be cancelled after the results are ready.
            .ContinueWith(contentList =>
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return;
                }

                ResultAvaliableEvent?.Invoke(this, new ResultAvaliableEventArgs(contentList.Result));
            }, TaskScheduler.FromCurrentSynchronizationContext());
    }

    public IEnumerable<ContentData> Search(SearchQueryData query)
    {
        return _contentRepository.All()
            // Only include results that contain all the required tags.
            .Where(content => query.RequiredTags.All(requiredTag => content.Tags.Contains(requiredTag)))
            // Sort based on how well it matches the query.
            .OrderByDescending(content => ScoreResult(query, content));
    }

    private float ScoreResult(SearchQueryData query, ContentData content)
    {
        float score = 0;

        foreach (var interestingTerm in query.InterestingTerms)
        {
            if (content.Title.Contains(interestingTerm, StringComparison.InvariantCultureIgnoreCase))
            {
                score += 2;
            }
            if (content.Contents.Contains(interestingTerm, StringComparison.InvariantCultureIgnoreCase))
            {
                score += 1;
            }
            if (content.Title.Contains(query.RawTextQuery, StringComparison.Ordinal))
            {
                score += 20;
            }
            if (content.Contents.Contains(query.RawTextQuery, StringComparison.Ordinal))
            {
                score += 10;
            }
        }

        return score;
    }
}
