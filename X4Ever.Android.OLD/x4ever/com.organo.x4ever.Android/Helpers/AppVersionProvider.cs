using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using com.organo.x4ever.Droid.Helpers;
using com.organo.x4ever.Helpers;
using Xamarin.Forms;

[assembly: Dependency(typeof(AppVersionProvider))]

namespace com.organo.xchallenge.Droid.Helpers
{
    public class AppVersionProvider : IAppVersionProvider
    {
        public string Version
        {
            get
            {
                var context = Android.App.Application.Context;
                var info = context.PackageManager.GetPackageInfo(context.PackageName, 0);
                return $"{info.VersionName}";
                //return $"{info.VersionName}.{info.VersionCode.ToString()}";
            }
        }

        public string Get(string key)
        {
            var context = Android.App.Application.Context;
            var info = context.PackageManager.GetPackageInfo(context.PackageName, 0);
            return $"{info.VersionName}";
            //return $"{info.VersionName}.{info.VersionCode.ToString()}";
        }

        public async Task<string> GetAsync(string key)
        {
            var context = Android.App.Application.Context;
            var info = context.PackageManager.GetPackageInfo(context.PackageName, 0);
            return $"{info.VersionName}";
            //return $"{info.VersionName}.{info.VersionCode.ToString()}";
        }
    }
}