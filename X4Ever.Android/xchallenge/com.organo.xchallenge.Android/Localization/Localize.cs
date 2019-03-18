using com.organo.xchallenge.Droid.Localization;
using com.organo.xchallenge.Localization;
using System.Globalization;
using Xamarin.Forms;

[assembly: Dependency(typeof(Localize))]

namespace com.organo.xchallenge.Droid.Localization
{
    public class Localize : ILocalize
    {
        public CultureInfo GetCurrentCultureInfo()
        {
            CultureInfo cultureInfo = new CultureInfo(GetLanguage());
            return cultureInfo;
        }

        public CultureInfo GetCurrentCultureInfo(string langCode)
        {
            return new CultureInfo(langCode);
        }

        public string GetLanguage()
        {
            var androidLocale = Java.Util.Locale.Default;
            var netLanguage = androidLocale.ToString().Replace("_", "-"); // turns pt_BR into pt-BR
            return netLanguage;
        }

        public string GetLanguage(string langCode)
        {
            var cultureInfo = new CultureInfo(langCode);
            return cultureInfo?.Name ?? langCode;
        }
    }
}