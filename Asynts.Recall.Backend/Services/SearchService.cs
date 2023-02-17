using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using Asynts.Recall.Backend.Persistance;
using Asynts.Recall.Backend.Persistance.Data;

namespace Asynts.Recall.Backend.Services;

public class SearchService : ISearchService, IDisposable
{
    private readonly IPageRepository _pageRepository;
    private readonly Dispatcher _dispatcher;

    public SearchService(IPageRepository pageRepository, Dispatcher dispatcher)
    {
        _pageRepository = pageRepository;
        _dispatcher = dispatcher;
    }

    public void Dispose()
    {
        SearchQueryCancellationSource?.Dispose();
    }

    public event SearchResultAvaliableHandler? ResultAvaliableEvent;

    private CancellationTokenSource? SearchQueryCancellationSource = null;
    public Task UpdateSearchQueryAsync(SearchQueryData searchQuery)
    {
        if (SearchQueryCancellationSource != null)
        {
            SearchQueryCancellationSource.Cancel();
            SearchQueryCancellationSource.Dispose();
        }

        SearchQueryCancellationSource = new CancellationTokenSource();
        var cancellationToken = SearchQueryCancellationSource.Token;

        return Task
            // We allow the search to be cancelled, if the thread pool hasn't started processing it yet.
            .Run(() =>
            {
                try
                {
                    // We allow the search to be cancelled in the middle.
                    var pages = Search(searchQuery)
                        .AsParallel()
                        .WithCancellation(cancellationToken)
                        .ToList();

                    // To avoid race conditions, we dispatch to an event queue before checking for cancellation.
                    _dispatcher.BeginInvoke(() =>
                    {
                        // We allow cancellation even if the result is already ready.
                        if (cancellationToken.IsCancellationRequested)
                        {
                            return;
                        }

                        ResultAvaliableEvent?.Invoke(this, new SearchResultAvaliableEventArgs(pages));
                    });
                }
                catch (OperationCanceledException) { }
            }, cancellationToken);
    }

    public IEnumerable<PageData> Search(SearchQueryData query)
    {
        return _pageRepository.All()
            // Only include results that contain all the required tags.
            // We consider tags to be hierarchical and a matching prefix is sufficient. 
            .Where(page => query.RequiredTags.All(requiredTag => page.Tags.Any(tag => tag.StartsWith(requiredTag))))
            // Sort based on how well it matches the query.
            .OrderByDescending(page => ScoreSearchResult(query, page));
    }

    private float ScoreSearchResult(SearchQueryData query, PageData content)
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
        }

        if (content.Title.Contains(query.RawTextQuery, StringComparison.InvariantCulture))
        {
            score += 20;
        }
        if (content.Contents.Contains(query.RawTextQuery, StringComparison.InvariantCulture))
        {
            score += 10;
        }

        return score;
    }
}
