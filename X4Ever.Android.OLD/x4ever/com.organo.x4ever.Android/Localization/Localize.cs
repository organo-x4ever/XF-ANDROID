using com.organo.x4ever.Droid.Localization;
using com.organo.x4ever.Localization;
using System.Globalization;
using Xamarin.Forms;

[assembly: Dependency(typeof(Localize))]

namespace com.organo.xchallenge.Droid.Localization
{
    public class Localize : ILocalize
    {
        //private CultureInfo ci;


        public string GetLanguage()
        {
            var androidLocale = Java.Util.Locale.Default;
            var netLanguage = androidLocale.ToString().Replace("_", "-"); // turns pt_BR into pt-BR
            return netLanguage;
        }

        public CultureInfo GetCurrentCultureInfo(string langCode)
        {
            return new CultureInfo(langCode);
        }


        public CultureInfo GetCurrentCultureInfo()
        {
            CultureInfo cultureInfo = new CultureInfo(GetLanguage());
            return cultureInfo;
        }

        public string GetLanguage(string langCode)
        {
            if (string.IsNullOrEmpty(langCode))
                return GetLanguage();
            var cultureInfo = new CultureInfo(langCode);
            return cultureInfo?.Name ?? langCode;
        }
    }
}