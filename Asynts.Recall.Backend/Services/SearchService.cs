using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Asynts.Recall.Backend.Persistance;
using Asynts.Recall.Backend.Persistance.Data;

namespace Asynts.Recall.Backend.Services;

public class SearchService : ISearchService
{
    private readonly IPageRepository _pageRepository;

    public SearchService(IPageRepository pageRepository)
    {
        _pageRepository = pageRepository;
    }

    public IEnumerable<PageData> Search(PageSearchRouteData query, CancellationToken cancellationToken = default)
    {
        // FIXME: What does 'AsParallel' do exactly?

        return _pageRepository.All()
            // Linq only allows parallel queries to be cancelled.
            .AsParallel()
            .WithCancellation(cancellationToken)
            // Only include results that contain all the required tags.
            // We consider tags to be hierarchical and a matching prefix is sufficient. 
            .Where(page => query.RequiredTags.All(requiredTag => page.Tags.Any(tag => tag.StartsWith(requiredTag))))
            // Sort based on how well it matches the query.
            .OrderByDescending(page => ScoreSearchResult(query, page));
    }

    private float ScoreSearchResult(PageSearchRouteData query, PageData content)
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

        if (content.Title.Contains(query.RawText, StringComparison.InvariantCulture))
        {
            score += 20;
        }
        if (content.Contents.Contains(query.RawText, StringComparison.InvariantCulture))
        {
            score += 10;
        }

        return score;
    }
}
