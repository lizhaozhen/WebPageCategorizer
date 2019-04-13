using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WebPageCategorizer.Services
{
    public class PageConfidenceDataService : IPageConfidenceDataService
    {
        private readonly ConcurrentDictionary<string, Page> pages;
        private readonly ConcurrentDictionary<string, HashSet<string>> refers;

        private readonly IPageDocumentDataService pageDocumentDataService;

        public PageConfidenceDataService(IPageDocumentDataService pageDocumentDataService)
        {
            pages = new ConcurrentDictionary<string, Page>();
            refers = new ConcurrentDictionary<string, HashSet<string>>();
            this.pageDocumentDataService = pageDocumentDataService;
        }

        public void AddRefer(string url, string link)
        {
            if (!refers.ContainsKey(link)) refers[link] = new HashSet<string>();

            refers[link].Add(url);
        }

        public IEnumerable<string> GetRefers(string link)
        {
            if (refers.ContainsKey(link)) return refers[link];
            return new string[0];
        }

        public async Task<Page> GetPage(string url)
        {
            await FetchDocumentIfNone(url);
            return pages[url];
        }

        public async Task FetchDocumentIfNone(string url)
        {
            if (!pages.ContainsKey(url))
            {
                pages[url] = new Page(await pageDocumentDataService.GetDocument(url));
            }
        }
    }
}
