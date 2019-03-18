using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.Res;
//using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using Android.Provider;
using com.organo.x4ever.Droid.Helpers;
using com.organo.x4ever.Helpers;
using Xamarin.Forms;

[assembly:Dependency(typeof(DeviceInfo))]

namespace com.organo.xchallenge.Droid.Helpers
{
    public class DeviceInfo : IDeviceInfo
    {
        public string GetModel() => Build.Model;
        public string GetManufacturer() => Build.Manufacturer;
        public string GetVersionString() => Build.VERSION.Release;
        public string GetPlatform() => "Android";
    }
}