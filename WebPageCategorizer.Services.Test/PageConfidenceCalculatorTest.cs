using System;
using System.Collections.Generic;
using System.Text;
using WebPageCategorizer.Services;
using Xunit;

namespace WebPageCategorizer.Services.Test
{
    public class PageConfidenceCalculatorTest
    {
        private readonly IPageConfidenceCalculator pageConfidenceCalculator;

        public PageConfidenceCalculatorTest()
        {
            pageConfidenceCalculator = new PageConfidenceCalculator();
        }

        [Theory]
        [InlineData(0.3)]
        public void CalculateNoLinksTest(double documentScore)
        {
            var score = pageConfidenceCalculator.Calculate(documentScore, new double[0]);

            Assert.Equal(documentScore, score);
        }

        [Fact]
        public void CalculateTest()
        {
            var documentScore = 0.3;
            var linkScores = new[] { 0.4, 0.7 };

            var score = pageConfidenceCalculator.Calculate(documentScore, linkScores);

            Assert.True(Math.Abs(0.425 - score) < 1e-6);
        }
    }
}
