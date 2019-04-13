using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebPageCategorizer.Services
{
    public class PageConfidenceService : IPageConfidenceService
    {
        private readonly IPageConfidenceDataService pageConfidenceDataService;
        private readonly IPageDocumentDataService pageDocumentDataService;
        private readonly IPageConfidenceCalculator pageConfidenceCalculator;
        public const double EPS = 1e-5;

        public PageConfidenceService(IPageConfidenceDataService pageConfidenceDataService,
            IPageDocumentDataService pageDocumentDataService,
            IPageConfidenceCalculator pageConfidenceCalculator)
        {
            this.pageConfidenceDataService = pageConfidenceDataService;
            this.pageDocumentDataService = pageDocumentDataService;
            this.pageConfidenceCalculator = pageConfidenceCalculator;
        }

        public async Task<PageConfidence> Get(string url)
        {
            var categories = await pageDocumentDataService.GetCategories();
            foreach (var category in categories)
            {
                await Balance(url, category);
            }

            return new PageConfidence(url, await pageConfidenceDataService.GetPage(url));
        }

        public async Task Balance(string url, string category)
        {
            var hashSet = new HashSet<string>();
            hashSet.Add(url);

            while(hashSet.Any())
            {
                var first = hashSet.First();
                hashSet.Remove(first);

                var page = await pageConfidenceDataService.GetPage(first);

                var links = page.GetLinks();
                var linkScores = new List<double>();
                foreach(var link in links)
                {
                    pageConfidenceDataService.AddRefer(first, link);

                    var linkPage = await pageConfidenceDataService.GetPage(link);
                    if(linkPage.GetPageScore(category) == null)
                    {
                        hashSet.Add(link);
                    }
                    linkScores.Add(linkPage.GetPageScore(category) ?? linkPage.GetDocumentScore(category) ?? 0);
                }

                // calculate score
                var score = pageConfidenceCalculator.Calculate(page.GetDocumentScore(category) ?? 0, linkScores);
                var pageScore = page.GetPageScore(category);
                if(pageScore == null || Math.Abs(pageScore.Value - score) > EPS)
                {
                    foreach(var refer in pageConfidenceDataService.GetRefers(first))
                    {
                        hashSet.Add(refer);
                    }
                }

                page.SetPageScore(category, score);
            }
        }
    }
}
