using System.Collections.Generic;

namespace WebPageCategorizer.Services
{
    public interface IPageConfidenceCalculator
    {
        double Calculate(double documentScore, IEnumerable<double> linkScores);
    }
}