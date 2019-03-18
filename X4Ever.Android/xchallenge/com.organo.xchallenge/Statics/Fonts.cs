using Xamarin.Forms;

namespace com.organo.xchallenge.Statics
{
    public static class Fonts
    {
        public static Font SystemFontAt150PercentOfLarge = Font.SystemFontOfSize(Device.GetNamedSize(NamedSize.Large, typeof(Label)) * 1.5);
        public static Font SystemFontAt120PercentOfLarge = Font.SystemFontOfSize(Device.GetNamedSize(NamedSize.Large, typeof(Label)) * 1.2);
    }
}