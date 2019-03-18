using Xamarin.Forms;

namespace com.organo.x4ever.Extensions
{
    public static class StringExtensions
    {
        public static string CapitalizeForAndroid(this string str)
        {
            return Device.RuntimePlatform == Device.Android ? str.ToUpper() : str;
        }
    }
}