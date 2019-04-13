using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace WebPageCategorizer.Services
{
    public class Page
    {
        public Page(PageDocument document)
        {
            this.document = document;
            pageScores = new ConcurrentDictionary<string, double>();
        }

        private PageDocument document;
        private ConcurrentDictionary<string, double> pageScores;
        
        public double? GetDocumentScore(string category)
        {
            if(document?.Scores?.ContainsKey(category) ?? false)
            {
                return document.Scores[category];
            }
            return null;
        }

        public double? GetPageScore(string category)
        {
            if (pageScores?.ContainsKey(category) ?? false)
            {
                return pageScores[category];
            }
            return null;
        }

        public IEnumerable<KeyValuePair<string, double>> GetPageScores()
        {
            return pageScores;
        }

        public void SetPageScore(string category, double score)
        {
            pageScores[category] = score;
        }

        public ICollection<string> GetLinks()
        {
            return document?.Links ?? new string[0];
        }
    }
}
