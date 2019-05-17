using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.organo.xchallenge.Helpers
{
    //public static partial class DeviceDisplay
    //{
    //    static bool PlatformKeepScreenOn
    //    {
    //        get => throw new NotImplementedInReferenceAssemblyException();
    //        set => throw new NotImplementedInReferenceAssemblyException();
    //    }

    //    static DisplayInfo GetMainDisplayInfo() => throw new NotImplementedInReferenceAssemblyException();

    //    static void StartScreenMetricsListeners() => throw new NotImplementedInReferenceAssemblyException();

    //    static void StopScreenMetricsListeners() => throw new NotImplementedInReferenceAssemblyException();
    //}

    public static partial class DeviceDisplay
    {
        static event EventHandler<DisplayInfoChangedEventArgs> MainDisplayInfoChangedInternal;

        static DeviceDisplayInfo currentMetrics;

        //public static bool KeepScreenOn
        //{
        //    get => PlatformKeepScreenOn;
        //    set => PlatformKeepScreenOn = value;
        //}

        //public static DisplayInfo MainDisplayInfo => GetMainDisplayInfo();

        static void SetCurrent(DeviceDisplayInfo metrics) =>
            currentMetrics = new DeviceDisplayInfo(metrics.Width, metrics.Height, metrics.Density, metrics.Orientation,
                metrics.Rotation, metrics.Xdpi, metrics.Ydpi);

        public static event EventHandler<DisplayInfoChangedEventArgs> MainDisplayInfoChanged
        {
            add
            {
                var wasRunning = MainDisplayInfoChangedInternal != null;

                MainDisplayInfoChangedInternal += value;

                if (!wasRunning && MainDisplayInfoChangedInternal != null)
                {
                    //SetCurrent(GetMainDisplayInfo());
                    //StartScreenMetricsListeners();
                }
            }

            remove
            {
                var wasRunning = MainDisplayInfoChangedInternal != null;

                MainDisplayInfoChangedInternal -= value;

                if (wasRunning && MainDisplayInfoChangedInternal == null)
                {
                    //StopScreenMetricsListeners();
                }
            }
        }

        static void OnMainDisplayInfoChanged(DeviceDisplayInfo metrics)
            => OnMainDisplayInfoChanged(new DisplayInfoChangedEventArgs(metrics));

        static void OnMainDisplayInfoChanged(DisplayInfoChangedEventArgs e)
        {
            if (!currentMetrics.Equals(e.DisplayInfo))
            {
                SetCurrent(e.DisplayInfo);
                MainDisplayInfoChangedInternal?.Invoke(null, e);
            }
        }
    }

    public class DisplayInfoChangedEventArgs : EventArgs
    {
        public DisplayInfoChangedEventArgs(DeviceDisplayInfo displayInfo) =>
            DisplayInfo = displayInfo;

        public DeviceDisplayInfo DisplayInfo { get; }
    }
}