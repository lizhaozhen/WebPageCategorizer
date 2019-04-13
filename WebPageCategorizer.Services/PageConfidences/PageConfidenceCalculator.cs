using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace WebPageCategorizer.Services
{
    public class PageConfidenceCalculator : IPageConfidenceCalculator
    {
        public double Calculate(double documentScore, IEnumerable<double> linkScores)
        {
            return linkScores.Any()
                ? (documentScore + linkScores.Sum() / linkScores.Count()) / 2
                : documentScore;
        }
    }
}
