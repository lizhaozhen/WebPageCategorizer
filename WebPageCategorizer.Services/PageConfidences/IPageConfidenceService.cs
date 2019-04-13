using System.Threading.Tasks;

namespace WebPageCategorizer.Services
{
    public interface IPageConfidenceService
    {
        Task Balance(string url, string category);
        Task<PageConfidence> Get(string url);
    }
}