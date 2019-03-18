using Xamarin.Forms;

namespace com.organo.x4ever.Statics
{
    public static class FontSizes
    {
        public readonly static double _75PercentOfSmall = Device.GetNamedSize(NamedSize.Default, typeof(Label)) * .75;
        public readonly static double _90PercentOfSmall = Device.GetNamedSize(NamedSize.Default, typeof(Label)) * .9;
        public readonly static double _100PercentOfDefault = Device.GetNamedSize(NamedSize.Default, typeof(Label)) * 1;
        public readonly static double _125PercentOfLarge = Device.GetNamedSize(NamedSize.Default, typeof(Label)) * 1.25;
        public readonly static double _150PercentOfLarge = Device.GetNamedSize(NamedSize.Default, typeof(Label)) * 1.5;

        public static double Default => 1.1;
        public static double Small => 1;
        public static double Medium => 1.3;
        public static double Large => 1.5;
        public static double XLarge => 2;
    }
}