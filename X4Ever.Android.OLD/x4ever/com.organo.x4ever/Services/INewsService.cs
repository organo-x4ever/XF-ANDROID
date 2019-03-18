using com.organo.x4ever.Models.News;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace com.organo.x4ever.Services
{
    public interface INewsService : IBaseService
    {
        long ExecutionTime { get; set; }
        Task<List<NewsModel>> GetByActive(bool active);
        Task<List<NewsModel>> GetByLanguage(string languageCode, bool active);
    }
}