using System.Collections.Generic;
using com.organo.xchallenge.Models;
using com.organo.xchallenge.Models.User;
using com.organo.xchallenge.Notification;
using System.Globalization;
using System.Threading.Tasks;
using com.organo.xchallenge.ViewModels.Profile;
using Xamarin.Forms;

namespace com.organo.xchallenge.Globals
{
    public interface IAppConfiguration
    {
        AppConfig AppConfig { get; set; }
        UserSetting UserSetting { get; set; }
        string BackgroundImage { get; set; }
        Color BackgroundColor { get; set; }
        Color StatusBarColor { get; set; }
        bool IsFullScreenMode { get; set; }
        ActivityType ActivityType { get; set; }
        CultureInfo LanguageInfo { get; set; }
        List<ImageSize> ImageSizes { get; set; }
        bool IsConnected { get; set; }
        bool IsAnimationAllowed { get; set; }
        bool IsMenuLoaded { get; set; }
        bool IsProfileEditAllowed { get; set; }
        string UserToken { get; }
        Task InitAsync();
        Task SetUserConfigurationAsync(string token, string languageCode, string weightVolume);
        Task SetUserLanguageAsync(string languageCode);
        Task SetWeightVolumeAsync(string weightVolume);
        Task SetUserTokenAsync(string token);
        string GetUserToken();
        Task<string> GetUserTokenAsync();
        Task<bool> IsUserTokenExistsAsync();
        Task DeleteUserTokenAsync();

        //Task SetNotificationStatusAsync(bool notificationAllowed);
        //bool GetNotificationStatus();
        //Task<bool> GetNotificationStatusAsync();
        //Task DeleteNotificationStatusAsync();

        void Initial(Page page, bool showBackgroundImage = false);
        Task InitialAsync(Page page, bool showBackgroundImage = false);
        void Initial(Page page, Color backgroundColor, bool showBackgroundImage = false);
        Task InitialAsync(Page page, Color backgroundColor, bool showBackgroundImage = false);
        Task GetActivityAsync(string action);
        Task GetConnectionInfoAsync();
        void GetConnectionInfo();
        Task SetImageAsync(string imageIdentity, string badgeImage);
        void SetImage(string imageIdentity, string badgeImage);
        ImageSize GetImageSizeByID(string imageIdentity);
        Task<ImageSize> GetImageSizeByIDAsync(string imageIdentity);
        string GetApplication();
        bool IsWelcomeVideoSkipped();
        void WelcomeVideoSkipped();
        void DeleteVideoSkipped();
        bool IsVersionPrompt();
        void VersionPrompted();
        void DeleteVersionPrompt();
        void SetUserKey();
        string GetUserKey();
        bool IsUserKeyExists();
        void DeleteUserKey();
        void SetUserGraph(ChartType chartType);
        ChartType GetUserGraph();
        void DeleteUserGraph();
    }
}