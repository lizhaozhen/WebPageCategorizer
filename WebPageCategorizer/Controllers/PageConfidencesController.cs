using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebPageCategorizer.Services;

namespace WebPageCategorizer.Controllers
{
    public class PageConfidencesController : BaseApiController
    {
        private readonly IPageConfidenceRawService pageConfidenceManager;
        private readonly IPageConfidenceService pageConfidenceService;

        public PageConfidencesController(IPageConfidenceRawService pageConfidenceManager,
            IPageConfidenceService pageConfidenceService)
        {
            this.pageConfidenceManager = pageConfidenceManager;
            this.pageConfidenceService = pageConfidenceService;
        }

        [HttpGet]
        [Route("{url}")]
        public async Task<PageConfidence> Get(string url)
        {
            return await pageConfidenceService.Get(url);
        }
    }
}
