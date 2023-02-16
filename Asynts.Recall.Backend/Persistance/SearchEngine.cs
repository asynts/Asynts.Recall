﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Asynts.Recall.Backend.Persistance.Data;

namespace Asynts.Recall.Backend.Persistance;

public class SearchEngine : ISearchEngine, IDisposable
{
    private readonly IContentRepository _contentRepository;
    private readonly TaskScheduler _criticalTaskScheduler;

    public SearchEngine(IContentRepository contentRepository, TaskScheduler criticalTaskScheduler)
    {
        _contentRepository = contentRepository;
        _criticalTaskScheduler = criticalTaskScheduler;
    }

    public void Dispose()
    {
        SearchQueryCancellationSource?.Dispose();
    }

    public event SearchEngineResultAvaliableHandler? ResultAvaliableEvent;

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
                // We allow the search to be cancelled in the middle.
                return Search(searchQuery)
                    .AsParallel()
                    .WithCancellation(cancellationToken)
                    .ToList();
            }, cancellationToken)
            // We allow the search to be cancelled after the results are ready.
            .ContinueWith(contentListTask =>
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return;
                }

                ResultAvaliableEvent?.Invoke(this, new SearchEngineResultAvaliableEventArgs(contentListTask.Result));
            }, _criticalTaskScheduler);
    }

    public IEnumerable<ContentData> Search(SearchQueryData query)
    {
        return _contentRepository.All()
            // Only include results that contain all the required tags.
            // We consider tags to be hierarchical and a matching prefix is sufficient. 
            .Where(content => query.RequiredTags.All(requiredTag => content.Tags.Any(tag => tag.StartsWith(requiredTag))))
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
