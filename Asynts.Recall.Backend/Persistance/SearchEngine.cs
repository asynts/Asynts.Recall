using System.Collections.Generic;

using Asynts.Recall.Backend.Persistance.Data;

namespace Asynts.Recall.Backend.Persistance
{
    public class SearchEngine : ISearchEngine
    {
        private readonly IContentRepository _contentRepository;

        public SearchEngine(IContentRepository contentRepository)
        {
            _contentRepository = contentRepository;
        }

        public IEnumerable<ContentData> Search(SearchQueryData query)
        {
            // FIXME: Actually implement search instead of mocking it.
            return _contentRepository.All();
        }
    }
}
