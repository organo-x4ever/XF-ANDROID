using System.Globalization;

namespace com.organo.x4ever.Localization
{
    public interface ILocalize
    {
        CultureInfo GetCurrentCultureInfo();
        string GetLanguage();
        string GetLanguage(string langCode);
        CultureInfo GetCurrentCultureInfo(string langCode);
    }
}