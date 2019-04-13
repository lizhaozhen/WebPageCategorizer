using System;
using System.Collections.Generic;
using System.Text;

namespace WebPageCategorizer.Services
{
    public class PageDocument
    {
        public string Url { get; set; }
        public Dictionary<string, double> Scores { get; set; }
        public string[] Links { get; set; }
    }
}
