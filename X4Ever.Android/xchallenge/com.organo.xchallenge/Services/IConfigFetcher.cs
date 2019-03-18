using System.Collections.Generic;
using System.Threading.Tasks;
using com.organo.xchallenge.Models;

namespace com.organo.xchallenge.Services
{
    /// <summary>
    /// An interface that fetches settings from embedded resources in a platform.
    /// </summary>
    public interface IConfigFetcher
    {
        /// <summary>
        /// Gets AppConfig object from settings.json file.
        /// </summary>
        /// <returns></returns>
        Task<AppConfig> GetAsync();

        string GetPictureDirectory();
    }
}