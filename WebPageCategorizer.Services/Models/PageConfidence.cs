using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace WebPageCategorizer.Services
{
    public class PageConfidence
    {
        public PageConfidence(string url, Page page)
        {
            Url = url;
            Scores = page.GetPageScores().ToDictionary(x => x.Key, x => x.Value);
        }

        public string Url { get; set; }
        // key is category, value is confidence
        public Dictionary<string, double> Scores { get; set; }
    }
}
