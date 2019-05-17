﻿//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//using Android.App;
//using Android.Content;
//using Android.OS;
//using Android.Runtime;
//using Android.Views;
//using Android.Widget;


using System;
using Android.Content;
using Android.Content.Res;
using Android.Provider;
using Android.Runtime;
using Android.Views;

namespace com.organo.xchallenge.Droid.Helpers
{
    public static partial class DeviceDisplay
    {
        static OrientationEventListener orientationListener;

        static bool PlatformKeepScreenOn
        {
            get
            {
                var window = Platform.GetCurrentActivity(true)?.Window;
                var flags = window?.Attributes?.Flags ?? 0;
                return flags.HasFlag(WindowManagerFlags.KeepScreenOn);
            }

            set
            {
                var window = Platform.GetCurrentActivity(true)?.Window;
                if (value)
                    window?.AddFlags(WindowManagerFlags.KeepScreenOn);
                else
                    window?.ClearFlags(WindowManagerFlags.KeepScreenOn);
            }
        }

        public static DeviceDisplayInfo GetMainDisplayInfo()
        {
            var displayMetrics = Platform.AppContext.Resources?.DisplayMetrics;
            
            return new DeviceDisplayInfo(
                width: displayMetrics?.WidthPixels ?? 0,
                height: displayMetrics?.HeightPixels ?? 0,
                density: displayMetrics?.Density ?? 0,
                orientation: 0, //CalculateOrientation(),
                rotation: 0, //CalculateRotation()
                xdpi: displayMetrics?.Xdpi,
                ydpi: displayMetrics?.Ydpi
            );
        }

        static void StartScreenMetricsListeners()
        {
            orientationListener = new Listener(Platform.AppContext, OnScreenMetricsChanged);
            orientationListener.Enable();
        }

        static void StopScreenMetricsListeners()
        {
            orientationListener?.Disable();
            orientationListener?.Dispose();
            orientationListener = null;
        }

        static void OnScreenMetricsChanged()
        {
            var metrics = GetMainDisplayInfo();
        }

        //static DisplayRotation CalculateRotation()
        //{
        //    var service = Platform.AppContext.GetSystemService(Context.WindowService);
        //    var display = service?.JavaCast<IWindowManager>()?.DefaultDisplay;

        //    if (display != null)
        //    {
        //        switch (display.Rotation)
        //        {
        //            case SurfaceOrientation.Rotation270:
        //                return DisplayRotation.Rotation270;
        //            case SurfaceOrientation.Rotation180:
        //                return DisplayRotation.Rotation180;
        //            case SurfaceOrientation.Rotation90:
        //                return DisplayRotation.Rotation90;
        //            case SurfaceOrientation.Rotation0:
        //                return DisplayRotation.Rotation0;
        //        }
        //    }

        //    return DisplayRotation.Unknown;
        //}

        //static DisplayOrientation CalculateOrientation()
        //{
        //    var config = Platform.AppContext.Resources?.Configuration;

        //    if (config != null)
        //    {
        //        switch (config.Orientation)
        //        {
        //            case Orientation.Landscape:
        //                return DisplayOrientation.Landscape;
        //            case Orientation.Portrait:
        //            case Orientation.Square:
        //                return DisplayOrientation.Portrait;
        //        }
        //    }

        //    return DisplayOrientation.Unknown;
        //}

        static string GetSystemSetting(string name)
            => Settings.System.GetString(Platform.AppContext.ContentResolver, name);
    }

    class Listener : OrientationEventListener
    {
        readonly Action onChanged;

        internal Listener(Context context, Action handler)
            : base(context) => onChanged = handler;

        public override void OnOrientationChanged(int orientation) => onChanged();
    }
}