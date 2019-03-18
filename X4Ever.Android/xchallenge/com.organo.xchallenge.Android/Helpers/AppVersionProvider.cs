using Android.Content;
using Android.Content.PM;
using com.organo.xchallenge.Droid.Helpers;
using com.organo.xchallenge.Helpers;
using com.organo.xchallenge.Services;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(AppVersionProvider))]

namespace com.organo.xchallenge.Droid.Helpers
{
    public class AppVersionProvider : IAppVersionProvider
    {
        private Context _Context => Android.App.Application.Context;
        private PackageInfo Info => _Context.PackageManager.GetPackageInfo(_Context.PackageName, 0);
        public string PackageName => $"{Info.PackageName}";
        public string Version => $"{Info.VersionName}";
        public string VersionCode => $"{Info.VersionCode.ToString()}";
        public string StoreUri => $"https://play.google.com/store/apps/details?id={PackageName}&hl=en";
        public string AppVersionApi => $"https://mapp.oghq.ca/?id={PackageName}";
        public string MarketUri => $"market://details?id={PackageName}";
        private string _appName { get; set; }
        public string AppName => _appName;
        private string _updateVersion { get; set; }
        public string UpdateVersion => _updateVersion;

        public string Get(string key)
        {
            var info = _Context.PackageManager.GetPackageInfo(_Context.PackageName, 0);
            return $"{info.VersionName}";
        }

        public async Task<string> GetAsync(string key)
        {
            return await Task.Factory.StartNew(() =>
            {
                var info = _Context.PackageManager.GetPackageInfo(_Context.PackageName, 0);
                return $"{info.VersionName}";
            });
        }

        public async Task<bool> CheckAppVersionAsync(Action updateCallback)
        {
            await Task.Delay(TimeSpan.FromMilliseconds(1500));
            return await CheckAppVersionAPIAsync(updateCallback);
        }

        private async Task<bool> CheckAppVersionAPIAsync(Action updateCallback)
        {
            return await Task.Run<bool>(async () =>
            {
                double.TryParse(this.Version.Replace(".", ""), out double currentVersion);
                var appInfoService = DependencyService.Get<IAppInfoService>();
                var appInfoModel = await appInfoService.GetAsync();

                _updateVersion = appInfoModel?.Version ?? "0";
                double.TryParse(_updateVersion.Replace(".", ""), out double appVersion);

                System.Diagnostics.Debug.WriteLine($"{this.Version} :: {_updateVersion}");
                System.Diagnostics.Debug.WriteLine($"{appVersion > currentVersion}");

                if (appVersion > currentVersion)
                {
                    _appName = appInfoModel?.AppName;
                    updateCallback();
                    return true;
                }

                return false;
            });
        }

        public void GotoGoogleAppStoreAsync()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                Intent googleAppStoreIntent;
                try
                {
                    googleAppStoreIntent = new Intent(Intent.ActionView,
                        Android.Net.Uri.Parse(this.StoreUri));
                    googleAppStoreIntent.AddFlags(ActivityFlags.NewTask);
                    _Context.StartActivity(googleAppStoreIntent);
                }
                catch (ActivityNotFoundException)
                {
                    Device.OpenUri(new Uri(StoreUri));
                }
            });
        }
    }
}