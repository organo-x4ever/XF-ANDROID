using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.organo.xchallenge.Helpers
{
    public interface IAppVersionProvider
    {
        string StoreUri { get; }
        string MarketUri { get; }
        string AppVersionApi { get; }
        string PackageName { get; }
        string Version { get; }
        string VersionCode { get; }
        string AppName { get; }
        string UpdateVersion { get; }

        Task<string> GetAsync(string key);
        string Get(string key);
        Task<bool> CheckAppVersionAsync(Action updateCallback);
        void GotoGoogleAppStoreAsync();
    }
}