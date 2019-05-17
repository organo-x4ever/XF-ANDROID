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
using com.organo.xchallenge.Droid.Helpers;
using com.organo.xchallenge.Helpers;
using Xamarin.Forms;
using Android.Util;

[assembly:Dependency(typeof(DeviceInfo))]

namespace com.organo.xchallenge.Droid.Helpers
{
    public class DeviceInfo : IDeviceInfo
    {
        public string GetModel => Build.Model;
        public string GetManufacturer => Build.Manufacturer;
        public string GetVersionString => Build.VERSION.Release;
        public string GetPlatform => "Android";
        public string GetAppName => "X4Ever";

        public int WidthPixels => DeviceDisplay.GetMainDisplayInfo().Width;
        public int HeightPixels => DeviceDisplay.GetMainDisplayInfo().Height;

        public float Ydpi => DeviceDisplay.GetMainDisplayInfo().Ydpi;

        public float Xdpi => DeviceDisplay.GetMainDisplayInfo().Xdpi;
        public float ScaledDensity => DeviceDisplay.GetMainDisplayInfo().Density;

        //private int GetWidth()
        //{

        //    var deviceInfo = DeviceDisplay.GetMainDisplayInfo();
        //    return deviceInfo.Width;


        //    //DisplayMetrics displayMetrics = new DisplayMetrics();
        //    //WindowManager.DefaultDisplay.GetRealMetrics(displayMetrics);

        //    //"displayMetrics.WidthPixels" get real screen width dimension
        //    //"displayMetrics.HeightPixels" get real screen height dimension




        //    //#region For screen Height & Width  
        //    //var pixels = Resources.DisplayMetrics.WidthPixels;  
        //    //var scale = Resources.DisplayMetrics.Density;  
        //    //var dps = (double)((pixels - 0.5f) / scale);  
        //    //var ScreenWidth = (int)dps;  
        //    //App.screenWidth = ScreenWidth;  
        //    //pixels = Resources.DisplayMetrics.HeightPixels;  
        //    //dps = (double)((pixels - 0.5f) / scale);  
        //    //var ScreenHeight = (int)dps;  
        //    //App.screenHeight = ScreenHeight;  
        //    //#endregion  

        //    //return 0;
        //}
    }
}