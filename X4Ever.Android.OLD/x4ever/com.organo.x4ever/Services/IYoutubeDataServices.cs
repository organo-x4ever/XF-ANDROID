using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.organo.x4ever.Models.Youtube;

namespace com.organo.x4ever.Services
{
    public interface IYoutubeDataServices : IBaseService
    {
        Task<YoutubeConfiguration> GetAsync();
        Task<string> GetStringAsync(string requestUri);
    }
}