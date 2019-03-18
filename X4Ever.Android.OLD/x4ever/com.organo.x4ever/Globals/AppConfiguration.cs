using com.organo.x4ever.Connection;
using com.organo.x4ever.Helpers;
using com.organo.x4ever.Localization;
using com.organo.x4ever.Models;
using com.organo.x4ever.Models.User;
using com.organo.x4ever.Notification;
using com.organo.x4ever.Services;
using com.organo.x4ever.Statics;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace com.organo.x4ever.Globals
{
    public class AppConfiguration : IAppConfiguration
    {
        private readonly IConfigFetcher _configFetcher;
        public string BackgroundImage { get; set; }
        public Color BackgroundColor { get; set; }
        public Color StatusBarColor { get; set; }
        public bool IsFullScreenMode { get; set; }
        public ActivityType ActivityType { get; set; }
        public CultureInfo LanguageInfo { get; set; }
        public bool IsConnected { get; set; }
        public bool IsAnimationAllowed { get; set; }
        public UserSetting UserSetting { get; set; }
        public AppConfig AppConfig { get; set; }
        public string UserToken { get; set; }
        public List<ImageSize> ImageSizes { get; set; }
        private readonly ISecureStorage _secureStorage;
        private readonly ILocalize _localize;
        private readonly ISetDeviceProperty _deviceProperty;

        public AppConfiguration()
        {
            UserToken = string.Empty;
            _configFetcher = DependencyService.Get<IConfigFetcher>();
            _secureStorage = DependencyService.Get<ISecureStorage>();
            _localize = DependencyService.Get<ILocalize>();
            _deviceProperty = DependencyService.Get<ISetDeviceProperty>();
            ImageSizes = new List<ImageSize>();
            BackgroundColor = Palette._MainBackground;
            StatusBarColor = Palette._MainAccent;
            BackgroundImage = TextResources.icon_background_blur;
            IsFullScreenMode = false;
            ActivityType = ActivityType.NONE;
            IsConnected = false;
            DependencyService.Get<ISetDeviceProperty>().SetStatusBarColor(StatusBarColor, IsFullScreenMode);
            InitialTasks();
        }

        private async void InitialTasks()
        {
            await GetConfigAsync();
            await GetConnectionInfoAsync();
            var applicationMode = "LIVE";
#if DEBUG
            applicationMode = "DEBUG";
#endif
            WriteLog.Write("Application Mode: " + applicationMode, true);
        }

        public async Task InitAsync()
        {
            await GetUserToken();
            await GetLanguageAsync();
            await GetWeightVolumeAsync();
        }

        public void Init()
        {
            GetUserToken().GetAwaiter();
            GetLanguageAsync().GetAwaiter();
            GetWeightVolumeAsync().GetAwaiter();
        }

        public void Initial(Page page, bool showBackgroundImage = false)
        {
            DependencyService.Get<ISetDeviceProperty>()
                .SetStatusBarColor(StatusBarColor, IsFullScreenMode);
            page.BackgroundColor = BackgroundColor;
            if (showBackgroundImage)
                page.BackgroundImage = BackgroundImage;
        }

        public async Task InitialAsync(Page page, bool showBackgroundImage = false)
        {
            await Task.Run(() =>
            {
                DependencyService.Get<ISetDeviceProperty>()
                    .SetStatusBarColor(StatusBarColor, IsFullScreenMode);
                page.BackgroundColor = BackgroundColor;
                if (showBackgroundImage)
                    page.BackgroundImage = BackgroundImage;
            });
        }

        public void Initial(Page page, Color backgroundColor, bool showBackgroundImage = false)
        {
            DependencyService.Get<ISetDeviceProperty>()
                .SetStatusBarColor(StatusBarColor, IsFullScreenMode);
            page.BackgroundColor = backgroundColor;
            if (showBackgroundImage)
                page.BackgroundImage = BackgroundImage;
        }
        public async Task InitialAsync(Page page, Color backgroundColor,
            bool showBackgroundImage = false)
        {
            await Task.Run(() =>
            {
                DependencyService.Get<ISetDeviceProperty>()
                    .SetStatusBarColor(StatusBarColor, IsFullScreenMode);
                page.BackgroundColor = backgroundColor;
                if (showBackgroundImage)
                    page.BackgroundImage = BackgroundImage;
            });
        }

        private void SetImageSizes(string id, string dimensions, string path = null, bool isDynamic = false)
        {
            var imageSize = new ImageSize();
            if (dimensions != null && dimensions.Contains("*"))
            {
                var splits = dimensions.Split('*');
                if (splits != null && splits.Length > 1)
                {
                    int width = 99, height = 99;
                    int.TryParse(splits[0], out width);
                    int.TryParse(splits[1], out height);
                    imageSize = new ImageSize()
                    {
                        ImageID = id,
                        ImageName = path,
                        Height = height,
                        Width = width,
                        IsDynamic = isDynamic
                    };
                    ImageSizes.Add(imageSize);
                    if (width == 0 || width == 99 || height == 0 || height == 99)
                        ShowImageMessage(id);
                }
                else
                    ShowImageMessage(id);
            }
            else
                ShowImageMessage(id);
        }

        private async void ShowImageMessage(string id)
        {
            await DependencyService.Get<IMessage>().AlertAsync(TextResources.Alert,
                "Image ID '" + id + "' has an error", TextResources.Ok, TextResources.Cancel);
        }

        public async Task GetActivity(string action = "")
        {
            await Task.Run(() =>
            {
                if (action != null && action.Trim().Length > 0)
                    if (action == ActivityType.WEIGHT_SUBMISSION_REQUIRED.ToString())
                        ActivityType = ActivityType.WEIGHT_SUBMISSION_REQUIRED;
            });
        }

        public async Task SetUserLanguage(string languageCode)
        {
                _localize.GetCurrentCultureInfo(AppConfig.DefaultLanguage);
                if (!string.IsNullOrEmpty(languageCode))
                    _secureStorage
                        .Store(StorageConstants.KEY_USER_LANGUAGE, Encoding.UTF8.GetBytes(languageCode));
                await GetLanguageAsync();
        }

        private async Task GetLanguageAsync()
        {
            var language = "";
            await Task.Run(() =>
            {
                var data = _secureStorage.Retrieve(StorageConstants.KEY_USER_LANGUAGE);
                if (data != null)
                    language = Encoding.UTF8.GetString(data, 0, data.Length);
                if (string.IsNullOrEmpty(language))
                {
                    if (!string.IsNullOrEmpty(AppConfig.DefaultLanguage))
                        AppConfig.DefaultLanguage = _localize.GetLanguage(AppConfig?.DefaultLanguage);
                    else
                        AppConfig.DefaultLanguage = _localize.GetLanguage();
                }
                else
                    AppConfig.DefaultLanguage = language;

                LanguageInfo = _localize.GetCurrentCultureInfo(AppConfig.DefaultLanguage);
                TextResources.Culture = LanguageInfo;
            });
        }

        public async Task SetWeightVolume(string weightVolume)
        {
            if (weightVolume != null)
                _secureStorage.Store(StorageConstants.KEY_USER_WEIGHT_VOLUME,Encoding.UTF8.GetBytes(weightVolume));
            await GetWeightVolumeAsync();
        }

        private async Task GetWeightVolumeAsync()
        {
            var weightVolume = "";
            await Task.Run(() =>
            {
                var data = _secureStorage.Retrieve(StorageConstants.KEY_USER_WEIGHT_VOLUME);
                if (data != null)
                    weightVolume = Encoding.UTF8.GetString(data, 0, data.Length);
                if (weightVolume != null && weightVolume.Trim().Length > 0)
                    AppConfig.DefaultWeightVolume = weightVolume;
            });
        }

        public async Task GetConnectionInfoAsync()
        {
            await Task.Run(() => { IsConnected = DependencyService.Get<IInternetConnection>().Check(); });
        }

        public void GetConnectionInfo()
        {
            IsConnected = DependencyService.Get<IInternetConnection>().Check();
        }

        public async Task SetUserToken(string token)
        {
            _secureStorage.Store(StorageConstants.KEY_VAULT_TOKEN_ID, Encoding.UTF8.GetBytes(token));
            await GetUserToken();
        }

        public async Task<string> GetUserToken()
        {
            UserToken = "";
            await Task.Run(() => { UserToken = GetToken(); });
            return UserToken;
        }

        public string GetToken()
        {
            var data = _secureStorage.Retrieve(StorageConstants.KEY_VAULT_TOKEN_ID);
            if (data != null)
                return Encoding.UTF8.GetString(data, 0, data.Length);
            return "";
        }

        private async Task GetConfigAsync()
        {
            AppConfig = new AppConfig();
            //Sensitive Data

#if DEBUG
            AppConfig.BaseUrl = await _configFetcher.GetAsync(AppConfigTags.BASE_URL_Dev, true);
#else
            AppConfig.BaseUrl = await _configFetcher.GetAsync(AppConfigTags.BASE_URL, true);
#endif

            AppConfig.WelcomeVideoUrl = await _configFetcher.GetAsync(AppConfigTags.WELCOME_VIDEO_URL, true);

            AppConfig.AuthenticationUrl = await _configFetcher.GetAsync(AppConfigTags.AUTHENTICATION_URL, true);
            AppConfig.AcceptedTokenName = await _configFetcher.GetAsync(AppConfigTags.ACCEPTED_TOKEN_NAME, true);
            AppConfig.AccessTokenType = await _configFetcher.GetAsync(AppConfigTags.ACCESS_TOKEN_TYPE, true);
            AppConfig.ApplicationRequestHeader =
                await _configFetcher.GetAsync(AppConfigTags.APPLICATION_REQUEST_HEADER, true);

            AppConfig.ApplicationVersion = DependencyService.Get<IAppVersionProvider>().Version;
            //Non-Sensitive Data
            AppConfig.TokenHeaderName = await _configFetcher.GetAsync(AppConfigTags.TOKEN_HEADER_NAME);
            AppConfig.TokenExpiryHeaderName = await _configFetcher.GetAsync(AppConfigTags.TOKEN_EXPIRY_HEADER_NAME);
            AppConfig.VideoUrl = await _configFetcher.GetAsync(AppConfigTags.VIDEO_URL);
            AppConfig.AudioUrl = await _configFetcher.GetAsync(AppConfigTags.AUDIO_URL);

            // TESTIMONIAL URLs
            AppConfig.TestimonialPhotoUrl = await _configFetcher.GetAsync(AppConfigTags.TESTIMONIAL_PHOTO_URL);
            AppConfig.TestimonialVideoUrl = await _configFetcher.GetAsync(AppConfigTags.TESTIMONIAL_VIDEO_URL);

            AppConfig.FileUploadUrl = await _configFetcher.GetAsync(AppConfigTags.FILE_UPLOAD_URL);
            AppConfig.UserImageUrl = await _configFetcher.GetAsync(AppConfigTags.USER_IMAGE_URL);
            AppConfig.DocumentUrl = await _configFetcher.GetAsync(AppConfigTags.DOCUMENT_URL);
            AppConfig.DefaultLanguage = await _configFetcher.GetAsync(AppConfigTags.DEFAULT_LANGUAGE);
            AppConfig.DefaultWeightVolume = await _configFetcher.GetAsync(AppConfigTags.DEFAULT_WEIGHT_VOLUME);
            var interval = await _configFetcher.GetAsync(AppConfigTags.WEIGHT_SUBMIT_INTERVAL);
            if (!int.TryParse(interval as string, out int intervalPeriod))
                intervalPeriod = 7;
            AppConfig.WeightSubmitInterval = intervalPeriod;
            AppConfig.WeightSubmitIntervalType =
                GetIntervalPeriodType(await _configFetcher.GetAsync(AppConfigTags.WEIGHT_SUBMIT_INTERVAL_TYPE));

            var targetDate = await _configFetcher.GetAsync(AppConfigTags.TARGET_DATE_CALCULATION);
            if (!float.TryParse(targetDate as string, out float targetDateValue))
                targetDateValue = 1;
            AppConfig.TargetDateCalculation = targetDateValue;

            var minCurrentWeight = await _configFetcher.GetAsync(AppConfigTags.MINIMUM_CURRENT_WEIGHT);
            if (!double.TryParse(minCurrentWeight, out double minCurrentWeightValue))
                minCurrentWeightValue = 1;
            AppConfig.MINIMUM_CURRENT_WEIGHT = minCurrentWeightValue;
            var maxCurrentWeight = await _configFetcher.GetAsync(AppConfigTags.MAXIMUM_CURRENT_WEIGHT);
            if (!double.TryParse(maxCurrentWeight, out double maxCurrentWeightValue))
                maxCurrentWeightValue = 1;
            AppConfig.MAXIMUM_CURRENT_WEIGHT = maxCurrentWeightValue;
            var minWeightLose = await _configFetcher.GetAsync(AppConfigTags.MINIMUM_WEIGHT_LOSE);
            if (!double.TryParse(minWeightLose, out double minWeightLoseValue))
                minWeightLoseValue = 1;
            AppConfig.MINIMUM_WEIGHT_LOSE = minWeightLoseValue;
            var maxWeightLose = await _configFetcher.GetAsync(AppConfigTags.MAXIMUM_WEIGHT_LOSE);
            if (!double.TryParse(maxWeightLose, out double maxWeightLoseValue))
                maxWeightLoseValue = 1;
            AppConfig.MAXIMUM_WEIGHT_LOSE = maxWeightLoseValue;
            var minAge = await _configFetcher.GetAsync(AppConfigTags.MINIMUM_AGE);
            if (!short.TryParse(minAge, out short minAgeValue))
                minAgeValue = 1;
            AppConfig.MINIMUM_AGE = minAgeValue;
            var maxAge = await _configFetcher.GetAsync(AppConfigTags.MAXIMUM_AGE);
            if (!short.TryParse(maxAge, out short maxAgeValue))
                maxAgeValue = 1;
            AppConfig.MAXIMUM_AGE = maxAgeValue;

            SetImageSizes(ImageIdentity.TOP_BAR_LOGO,
                await _configFetcher.GetAsync(ImageIdentity.TOP_BAR_LOGO),
                TextResources.logo_transparent);
            SetImageSizes(ImageIdentity.TOP_BAR_MENU,
                await _configFetcher.GetAsync(ImageIdentity.TOP_BAR_MENU),
                TextResources.icon_menu);
            SetImageSizes(ImageIdentity.TOP_BAR_CLOSE,
                await _configFetcher.GetAsync(ImageIdentity.TOP_BAR_CLOSE),
                TextResources.icon_close);
            SetImageSizes(ImageIdentity.MAIN_PAGE_LOGO,
                await _configFetcher.GetAsync(ImageIdentity.MAIN_PAGE_LOGO), TextResources.logo_page);
            SetImageSizes(ImageIdentity.MAIN_PAGE_XCHALLENGE_LOGO,
                await _configFetcher.GetAsync(ImageIdentity.MAIN_PAGE_XCHALLENGE_LOGO), TextResources.logo_challenge);
            SetImageSizes(ImageIdentity.MENU_PAGE_USER_IMAGE,
                await _configFetcher.GetAsync(ImageIdentity.MENU_PAGE_USER_IMAGE), null, true);

            SetImageSizes(ImageIdentity.USER_PROFILE_BADGE_ICON,
                await _configFetcher.GetAsync(ImageIdentity.USER_PROFILE_BADGE_ICON), null, true);

            SetImageSizes(ImageIdentity.MILESTONE_ACHEIVEMENT_BADGE_ICON,
                await _configFetcher.GetAsync(ImageIdentity.MILESTONE_ACHEIVEMENT_BADGE_ICON), null, true);

            SetImageSizes(ImageIdentity.BADGE_HINT_WINDOW,
                await _configFetcher.GetAsync(ImageIdentity.BADGE_HINT_WINDOW));

            SetImageSizes(ImageIdentity.BADGE_HINT_WINDOW_CLOSE,
                await _configFetcher.GetAsync(ImageIdentity.BADGE_HINT_WINDOW_CLOSE));

            SetImageSizes(ImageIdentity.BADGE_HINT_ICON,
                await _configFetcher.GetAsync(ImageIdentity.BADGE_HINT_ICON));

            SetImageSizes(ImageIdentity.ENTRY_EMAIL_ICON,
                await _configFetcher.GetAsync(ImageIdentity.ENTRY_EMAIL_ICON), TextResources.icon_email);
            SetImageSizes(ImageIdentity.ENTRY_LOCK_ICON,
                await _configFetcher.GetAsync(ImageIdentity.ENTRY_LOCK_ICON), TextResources.icon_lock);
            SetImageSizes(ImageIdentity.COUNTRY_FLAG_ICON,
                await _configFetcher.GetAsync(ImageIdentity.COUNTRY_FLAG_ICON));
            SetImageSizes(ImageIdentity.MENU_ITEM_ICON,
                await _configFetcher.GetAsync(ImageIdentity.MENU_ITEM_ICON));
            SetImageSizes(ImageIdentity.GENDER_IMAGE,
                await _configFetcher.GetAsync(ImageIdentity.GENDER_IMAGE));
            SetImageSizes(ImageIdentity.UPLOAD_CAMERA_IMAGE,
                await _configFetcher.GetAsync(ImageIdentity.UPLOAD_CAMERA_IMAGE));
            SetImageSizes(ImageIdentity.MEAL_PLAN_PAGE_MEAL_IMAGE,
                await _configFetcher.GetAsync(ImageIdentity.MEAL_PLAN_PAGE_MEAL_IMAGE));
            SetImageSizes(ImageIdentity.MEAL_PLAN_PAGE_MEAL_HEADER,
                await _configFetcher.GetAsync(ImageIdentity.MEAL_PLAN_PAGE_MEAL_HEADER));
            SetImageSizes(ImageIdentity.AUDIO_PLAYER_PAGE_COMMAND_IMAGE,
                await _configFetcher.GetAsync(ImageIdentity.AUDIO_PLAYER_PAGE_COMMAND_IMAGE));
            SetImageSizes(ImageIdentity.VIDEO_PLAYER_PAGE_COMMAND_IMAGE,
                await _configFetcher.GetAsync(ImageIdentity.VIDEO_PLAYER_PAGE_COMMAND_IMAGE));
            SetImageSizes(ImageIdentity.VIDEO_PLAYER_PAGE_EXPAND_LIST_IMAGE,
                await _configFetcher.GetAsync(ImageIdentity.VIDEO_PLAYER_PAGE_EXPAND_LIST_IMAGE));
            SetImageSizes(ImageIdentity.VIDEO_PLAYER_PAGE_NOTE_PLAY_IMAGE,
                await _configFetcher.GetAsync(ImageIdentity.VIDEO_PLAYER_PAGE_NOTE_PLAY_IMAGE));
            SetImageSizes(ImageIdentity.PICTURE_GALLERY_IMAGE,
                await _configFetcher.GetAsync(ImageIdentity.PICTURE_GALLERY_IMAGE));
            SetImageSizes(ImageIdentity.USER_SETTING_TAB_ICON,
                await _configFetcher.GetAsync(ImageIdentity.USER_SETTING_TAB_ICON));
            SetImageSizes(ImageIdentity.CHECKBOX_ICON,
                await _configFetcher.GetAsync(ImageIdentity.CHECKBOX_ICON));
            SetImageSizes(ImageIdentity.TESTIMONIAL_PERSON_IMAGE,
                await _configFetcher.GetAsync(ImageIdentity.TESTIMONIAL_PERSON_IMAGE));
            SetImageSizes(ImageIdentity.WORKOUT_OPTIONS_IMAGE,
                await _configFetcher.GetAsync(ImageIdentity.WORKOUT_OPTIONS_IMAGE));
            SetImageSizes(ImageIdentity.WORKOUT_EXPAND_COLLAPSE_IMAGE,
                await _configFetcher.GetAsync(ImageIdentity.WORKOUT_EXPAND_COLLAPSE_IMAGE));
            SetImageSizes(ImageIdentity.WORKOUT_PLAY_ICON,
                await _configFetcher.GetAsync(ImageIdentity.WORKOUT_PLAY_ICON));
            SetImageSizes(ImageIdentity.WORKOUT_VIDEO_WINDOW,
                await _configFetcher.GetAsync(ImageIdentity.WORKOUT_VIDEO_WINDOW));
            SetImageSizes(ImageIdentity.PAGE_IMAGE_SIGN_UP,
                await _configFetcher.GetAsync(ImageIdentity.PAGE_IMAGE_SIGN_UP));
            SetImageSizes(ImageIdentity.PAGE_IMAGE_PRODUCTS,
                await _configFetcher.GetAsync(ImageIdentity.PAGE_IMAGE_PRODUCTS));
            SetImageSizes(ImageIdentity.PAGE_IMAGE_EARN_REWARDS,
                await _configFetcher.GetAsync(ImageIdentity.PAGE_IMAGE_EARN_REWARDS));
            SetImageSizes(ImageIdentity.PAGE_IMAGE_LOSE_WEIGHT,
                await _configFetcher.GetAsync(ImageIdentity.PAGE_IMAGE_LOSE_WEIGHT));

            SetImageSizes(ImageIdentity.PAGE_IMAGE_T_SHIRT,
                await _configFetcher.GetAsync(ImageIdentity.PAGE_IMAGE_T_SHIRT));
            SetImageSizes(ImageIdentity.PAGE_IMAGE_T_SHIRTS_BUNDLE,
                await _configFetcher.GetAsync(ImageIdentity.PAGE_IMAGE_T_SHIRTS_BUNDLE));
            SetImageSizes(ImageIdentity.PAGE_IMAGE_LINE,
                await _configFetcher.GetAsync(ImageIdentity.PAGE_IMAGE_LINE));
            SetImageSizes(ImageIdentity.PAGE_IMAGE_BULLET,
                await _configFetcher.GetAsync(ImageIdentity.PAGE_IMAGE_BULLET));

            SetImageSizes(ImageIdentity.IMAGE_EYE_PASSWORD,
                await _configFetcher.GetAsync(ImageIdentity.IMAGE_EYE_PASSWORD));
        }

        private IntervalPeriodType GetIntervalPeriodType(string intervalType)
        {
            if (intervalType.ToLower().Contains("year"))
                return IntervalPeriodType.Years;
            else if (intervalType.ToLower().Contains("month"))
                return IntervalPeriodType.Months;
            else if (intervalType.ToLower().Contains("day"))
                return IntervalPeriodType.Days;
            else if (intervalType.ToLower().Contains("hour"))
                return IntervalPeriodType.Hours;
            else if (intervalType.ToLower().Contains("minute"))
                return IntervalPeriodType.Minutes;
            return IntervalPeriodType.Days;
        }

        public async Task<ImageSize> GetImageSizeByIDAsync(string imageIdentity)
        {
            var imageSize = new ImageSize();
            await Task.Run(() =>
            {
                imageSize = App.Configuration.ImageSizes.FirstOrDefault(s =>
                    s.ImageID.ToLower() == imageIdentity.ToLower());
            });
            return imageSize;
        }

        public ImageSize GetImageSizeByID(string imageIdentity)
        {
            var imageSize = App.Configuration.ImageSizes.FirstOrDefault(s =>
                s.ImageID.ToLower() == imageIdentity.ToLower());
            return imageSize;
        }

        public async Task SetImageAsync(string imageIdentity, string badgeImage)
        {
            await Task.Run(async () =>
            {
                var imageSize = await GetImageSizeByIDAsync(imageIdentity);
                if (imageSize != null)
                {
                    imageSize.ImageName = badgeImage;
                }
            });
        }

        public string GetApplication()
        {
            return App.CurrentUser?.UserInfo?.UserApplication ?? "";
        }
    }
}