using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace WebPageCategorizer.Services
{
    public class PageConfidenceRawService : IPageConfidenceRawService
    {
        private readonly IPageDocumentDataService pageDocumentDataService;
        private readonly IPageConfidenceDataService pageConfidenceDataService;
        public const double EPS = 1e-10;

        public PageConfidenceRawService(IPageDocumentDataService pageDocumentDataService,
            IPageConfidenceDataService pageConfidenceDataService)
        {
            this.pageDocumentDataService = pageDocumentDataService;
            this.pageConfidenceDataService = pageConfidenceDataService;
        }

        public async Task<PageConfidence> Get(string url)
        {
            var categories = await pageDocumentDataService.GetCategories();
            foreach(var category in categories)
            {
                await GetPageScore(url, category);
            }

            return new PageConfidence(url, await pageConfidenceDataService.GetPage(url));
        }

        public async Task<double> GetPageScore(string url, string category, double weight = 1.0)
        {
            if (weight < EPS) return 0;

            var page = await pageConfidenceDataService.GetPage(url);
            
            var score = page.GetPageScore(category);
            if (score != null) return score.Value * weight;

            var links = page.GetLinks();

            var ans = (page.GetDocumentScore(category) ?? 0) * weight * 0.5;
            if(links.Count > 0)
            {
                double linkWeight = 0.5 / links.Count;
                foreach(var link in links)
                {
                    ans += await GetPageScore(link, category, weight * linkWeight);
                }
            }

            if(weight == 1.0) page.SetPageScore(category, ans/weight);

            return ans;
        }
    }
}
