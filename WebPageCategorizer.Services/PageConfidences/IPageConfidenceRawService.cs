using System.Threading.Tasks;

namespace WebPageCategorizer.Services
{
    public interface IPageConfidenceRawService
    {
        Task<PageConfidence> Get(string url);
        Task<double> GetPageScore(string url, string category, double weight = 1);
    }
}