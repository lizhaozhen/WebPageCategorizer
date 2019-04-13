using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebPageCategorizer.Services
{
    public interface IPageDocumentDataService
    {
        Task<IEnumerable<string>> GetCategories();
        Task<PageDocument> GetDocument(string url);
    }
}