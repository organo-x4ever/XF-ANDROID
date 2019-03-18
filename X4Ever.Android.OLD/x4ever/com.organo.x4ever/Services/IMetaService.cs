using com.organo.x4ever.Models.User;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace com.organo.x4ever.Services
{
    public interface IMetaService
    {
        Task<string> SaveMetaAsync(List<Meta> metas);

        Task<string> SaveMetaAsync(Meta metas);

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