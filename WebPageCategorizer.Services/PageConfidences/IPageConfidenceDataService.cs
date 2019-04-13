using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebPageCategorizer.Services
{
    public interface IPageConfidenceDataService
    {
        void AddRefer(string url, string link);
        Task FetchDocumentIfNone(string url);
        Task<Page> GetPage(string url);
        IEnumerable<string> GetRefers(string link);
    }
}