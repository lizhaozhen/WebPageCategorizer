using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using System.Threading.Tasks;
using Xunit;
using System.Linq;

namespace WebPageCategorizer.Services.Test
{
    public class PageConfidenceServiceTests
    {
        private readonly IPageConfidenceCalculator pageConfidenceCalculator;

        private readonly IPageDocumentDataService randomDocumentService;

        public PageConfidenceServiceTests()
        {
            pageConfidenceCalculator = new PageConfidenceCalculator();
            randomDocumentService = NewRandomPageDocumentDataService();
        }

        [Fact]
        public async Task PageConfidenceServiceTest()
        {
            var documentService = NewPageDocumentDataService();
            IPageConfidenceService pageConfidenceService = new PageConfidenceService(
                new PageConfidenceDataService(documentService),
                documentService, pageConfidenceCalculator);

            Assert.True(DoubleEquals(0.404, (await pageConfidenceService.Get("one.com")).Scores["Sport"]));
            Assert.True(DoubleEquals(0.508, (await pageConfidenceService.Get("two.com")).Scores["Sport"]));
            Assert.True(DoubleEquals(0.428, (await pageConfidenceService.Get("three.com")).Scores["Sport"]));
        }

        [Fact]
        public async Task PageConfidenceRawServiceTest()
        {
            var documentService = NewPageDocumentDataService();
            IPageConfidenceRawService service = new PageConfidenceRawService(documentService,
                new PageConfidenceDataService(documentService));
            Assert.True(DoubleEquals(0.404, (await service.Get("one.com")).Scores["Sport"]));
            Assert.True(DoubleEquals(0.508, (await service.Get("two.com")).Scores["Sport"]));
            Assert.True(DoubleEquals(0.428, (await service.Get("three.com")).Scores["Sport"]));
        }

        [Fact]
        public async Task PageConfidenceService_Random()
        {
            IPageConfidenceService pageConfidenceService = new PageConfidenceService(
                new PageConfidenceDataService(randomDocumentService),
                randomDocumentService, pageConfidenceCalculator);

            IPageConfidenceRawService rawService = new PageConfidenceRawService(randomDocumentService,
                new PageConfidenceDataService(randomDocumentService));
            for(int i=0;i<10;i++)
            {
                Assert.True(DoubleEquals((await pageConfidenceService.Get($"{i}.com")).Scores["Sport"],
                    (await rawService.Get($"{i}.com")).Scores["Sport"]));
            }
        }

        public bool DoubleEquals(double x, double y)
        {
            return Math.Abs(x - y) <= PageConfidenceService.EPS;
        }
        
        public IPageDocumentDataService NewPageDocumentDataService()
        {
            var moq = new Mock<IPageDocumentDataService>();
            moq.Setup(x => x.GetCategories()).Returns(Task.FromResult((IEnumerable<string>)new[] { "Sport" }));
            moq.Setup(x => x.GetDocument(It.Is<string>(y => y == "one.com")))
                .Returns(Task.FromResult(new PageDocument()
                {
                    Url = "one.com",
                    Scores = new Dictionary<string, double>
                    {
                        { "Sport", 0.3}
                    },
                    Links = new[] { "two.com" }
                }));
            moq.Setup(x => x.GetDocument(It.Is<string>(y => y == "two.com")))
                .Returns(Task.FromResult(new PageDocument()
                {
                    Url = "two.com",
                    Scores = new Dictionary<string, double>
                    {
                        { "Sport", 0.6}
                    },
                    Links = new[] { "three.com", "one.com" }
                }));
            moq.Setup(x => x.GetDocument(It.Is<string>(y => y == "three.com")))
                .Returns(Task.FromResult(new PageDocument()
                {
                    Url = "three.com",
                    Scores = new Dictionary<string, double>
                    {
                        { "Sport", 0.4}
                    },
                    Links = new[] { "one.com", "two.com" }
                }));
            return moq.Object;
        }

        public IPageDocumentDataService NewRandomPageDocumentDataService(int count = 10)
        {
            var moq = new Mock<IPageDocumentDataService>();
            moq.Setup(x => x.GetCategories()).Returns(Task.FromResult((IEnumerable<string>)new[] { "Sport" }));

            var random = new Random();

            for(int i=0;i<count;i++)
            {
                moq.Setup(x => x.GetDocument(It.Is<string>(y => y == $"{i}.com")))
                .Returns(Task.FromResult(new PageDocument()
                {
                    Url = $"{i}.com",
                    Scores = new Dictionary<string, double>
                    {
                        { "Sport", random.NextDouble()}
                    },
                    Links = Enumerable.Range(0, count).Where(x => x != i).Where(x => (random.Next() & 1) == (i & 1)).Select(x => $"{i}.com").ToArray()
                }));
            }

            return moq.Object;
        }
    }
}
