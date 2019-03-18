using System.Collections.Generic;
using com.organo.x4ever.Models;
using com.organo.x4ever.Models.User;
using com.organo.x4ever.Notification;
using System.Globalization;
using System.Threading.Tasks;
using com.organo.x4ever.Statics;
using Xamarin.Forms;

namespace com.organo.x4ever.Globals
{
    public interface IAppConfiguration
    {
        AppConfig AppConfig { get; set; }
        UserSetting UserSetting { get; set; }
        string UserToken { get; set; }
        string BackgroundImage { get; set; }
        Color BackgroundColor { get; set; }
        Color StatusBarColor { get; set; }
        bool IsFullScreenMode { get; set; }
        ActivityType ActivityType { get; set; }
        CultureInfo LanguageInfo { get; set; }
        List<ImageSize> ImageSizes { get; set; }
        bool IsConnected { get; set; }
        bool IsAnimationAllowed { get; set; }
        void Init();
        Task InitAsync();

        Task SetUserLanguage(string languageCode);

        Task SetWeightVolume(string weightVolume);

        Task<string> GetUserToken();
        string GetToken();

        Task SetUserToken(string token);

        void Initial(Page page, bool showBackgroundImage = false);
        Task InitialAsync(Page page, bool showBackgroundImage = false);
        void Initial(Page page, Color backgroundColor, bool showBackgroundImage = false);
        Task InitialAsync(Page page, Color backgroundColor, bool showBackgroundImage = false);

        Task GetActivity(string action);

        Task GetConnectionInfoAsync();
        void GetConnectionInfo();
        Task SetImageAsync(string imageIdentity, string badgeImage);
        ImageSize GetImageSizeByID(string imageIdentity);
        Task<ImageSize> GetImageSizeByIDAsync(string imageIdentity);
        string GetApplication();
    }
}