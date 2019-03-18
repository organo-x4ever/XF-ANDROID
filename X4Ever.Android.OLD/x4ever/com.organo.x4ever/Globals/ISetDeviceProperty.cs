using Xamarin.Forms;

namespace com.organo.x4ever.Globals
{
    public interface ISetDeviceProperty
    {
        void SetStatusBarColor(Color color);

        void SetStatusBarColor(Color color, bool fullScreenMode);

        void SetFullScreen();
    }
}