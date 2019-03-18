using com.organo.xchallenge.Models.User;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace com.organo.xchallenge.Services
{
    public interface IMeta1Service : IBaseService
    {
        string Message { get; set; }
        Task<string> SaveMetaAsync(List<Meta> metas);
        Task<string> SaveMetaAsync(Meta metas);
        Task<bool> SaveMetaStep2Async(List<Meta> metas);
        Task<UserMeta> GetMetaAsync(string keys);

        Task<UserMeta> GetMetaAsync(string[] keys);

        Task<UserMeta> GetLastestMetaAsync();

        Task<List<Meta>> GetMetaAsync();

        Task<UserMeta> GetLastestMetaAsync(long userId);

        Task<List<Meta>> GetMetaAsync(long userId);

        Task<UserMeta> Convert(List<Meta> metas);

        Task<List<UserMeta>> ConvertToList(List<Meta> metas);

        Task<Meta> AddMeta(string metaValue, string description, string key, string type);
        //Task<List<Meta>> AddListMeta(string metaValue, string description, string key, string type);
    }
}