using System.Collections.Generic;

using Asynts.Recall.Backend.Persistance.Data;

namespace Asynts.Recall.Backend.Persistance
{
    public class ContentRepository : IContentRepository
    {
        public ContentRepository() { }

        IEnumerable<ContentData> All()
        {
            return new List<ContentData>
            {
                new ContentData
                {
                    Id = 0,
                    Title = "Hello, world!",
                    Contents = "Hello to everyone!\nThis is an example.\n",
                    Tags = new List<string>
                    {
                        "hello/",
                        "notes/example/",
                    },
                },
                new ContentData
                {
                    Id = 1,
                    Title = "Another Example",
                    Contents = "This is another example.\n",
                    Tags = new List<string>
                    {
                        "notes/example/",
                        "special/",
                    },
                },
            };
        }
    }
}
