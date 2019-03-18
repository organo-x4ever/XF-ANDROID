using com.organo.x4ever.Droid.Globals;
using com.organo.x4ever.Globals;
using com.organo.xchallenge.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: Dependency(typeof(SetDeviceProperty))]

namespace com.organo.x4ever.Droid.Globals
{
    public class SetDeviceProperty : ISetDeviceProperty
    {
        public void SetFullScreen()
        {
            // Get the MainActivity instance
            MainActivity activity = Forms.Context as MainActivity;
            activity.Window.AddFlags(Android.Views.WindowManagerFlags.Fullscreen);
        }

        public void SetStatusBarColor(Color color)
        {
            // Get the MainActivity instance
            MainActivity activity = Forms.Context as MainActivity;
            activity.Window.SetStatusBarColor(color.ToAndroid());
        }

        public void SetStatusBarColor(Color color, bool fullScreenMode)
        {
            // Get the MainActivity instance
            MainActivity activity = Forms.Context as MainActivity;
            activity.Window.SetStatusBarColor(color.ToAndroid());

            if (fullScreenMode)
                activity.Window.AddFlags(Android.Views.WindowManagerFlags.Fullscreen);
        }

        //if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.LOLLIPOP) {
        //        Window window = getWindow();
        //window.addFlags(WindowManager.LayoutParams.FLAG_DRAWS_SYSTEM_BAR_BACKGROUNDS);
        //        window.clearFlags(WindowManager.LayoutParams.FLAG_TRANSLUCENT_STATUS);
        //        window.setStatusBarColor(getResources()
        //                .getColor(R.color.YOurColorname));
        //    }
    }
}