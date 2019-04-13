using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace WebPageCategorizer.Services
{
    public class PageDocumentDataService : IPageDocumentDataService
    {
        private ICollection<PageDocument> documents = new[]
        {
            new PageDocument
            {
                Url = "one.com",
                Links = new []{ "two.com" },
                Scores = new Dictionary<string, double>()
                {
                    {"Sport", 0.5 },
                    {"Art", 0.6 },
                    {"Politics", 0.7 },
                    {"Scientific", 0.8 },
                }
            },
            new PageDocument
            {
                Url = "two.com",
                Links = new []{ "one.com" },
                Scores = new Dictionary<string, double>()
                {
                    {"Sport", 0.5 },
                    {"Art", 0.4 },
                    {"Politics", 0.3 },
                    {"Scientific", 0.2 },
                }
            }
        };

        public async Task<PageDocument> GetDocument(string url)
        {
            return documents.FirstOrDefault(x => x.Url == url);
        }

        public async Task<IEnumerable<string>> GetCategories()
        {
            return new[]
            {
                "Sport",
                "Art",
                "Politics",
                "Scientific"
            };
        }
    }
}
