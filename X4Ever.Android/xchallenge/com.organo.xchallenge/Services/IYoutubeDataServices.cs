using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.organo.xchallenge.Models.Youtube;

namespace com.organo.xchallenge.Services
{
    public interface IYoutubeDataServices : IBaseService
    {
        Task<YoutubeConfiguration> GetAsync();
        Task<string> GetStringAsync(string requestUri);
    }
}