using Asynts.Recall.Backend.Persistance.Data;

namespace Asynts.Recall.Backend.Persistance;

public class SearchEngine : ISearchEngine
{
    private readonly IContentRepository _contentRepository;

    public SearchEngine(IContentRepository contentRepository)
    {
        _contentRepository = contentRepository;
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
