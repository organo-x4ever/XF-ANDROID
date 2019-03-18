using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.organo.x4ever.Helpers
{
    public interface IAppVersionProvider
    {
        string Version { get; }
        Task<string> GetAsync(string key);
        string Get(string key);
    }
}
