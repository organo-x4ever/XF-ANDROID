using com.organo.xchallenge.Globals;
using com.organo.xchallenge.Localization;
using com.organo.xchallenge.Models;
using com.organo.xchallenge.Models.User;
using com.organo.xchallenge.Pages;
using com.organo.xchallenge.Pages.ChangePassword;
using com.organo.xchallenge.Pages.Profile;
using com.organo.xchallenge.Pages.UserSettings;
using com.organo.xchallenge.Services;
using com.organo.xchallenge.Statics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using com.organo.xchallenge.Helpers;
using com.organo.xchallenge.ViewModels.Base;
using Xamarin.Forms;

namespace com.organo.xchallenge.ViewModels.Profile
{
    public class SettingsViewModel : BaseViewModel
    {
        private readonly IUserSettingService _settingService;
        public SettingsViewModel(INavigation navigation = null) : base(navigation)
        {
            SetPageImageSize();
            _settingService = DependencyService.Get<IUserSettingService>();
            TitleProfile = TextResources.EditProfile;
            TitlePassword = TextResources.ChangePassword;
            TitleUserSetting = TextResources.UserSettings;
            CountryProvinces = new List<CountryProvince>();
            CountryList = new List<string>();
            StateList = new List<string>();
            ApplicationLanguages = new List<ApplicationLanguage>();
            GetCountryList(async () => { await TabSelected(TabTitle.EditProfile); });
        }

        private async void GetCountryList(Action action)
        {
            CountryProvinces = await DependencyService.Get<IProvinceServices>().GetAsync();
            CountryList = CountryProvinces.Select(c => (string) c.CountryName).Distinct().ToList();
            action?.Invoke();
        }

        private void GetStateList()
        {
            var provinces = CountryProvinces.FirstOrDefault(p => p.CountryName == CountryName)?.Provinces;
            StateList = provinces?.Select(p => (string)p.ProvinceName).Distinct().ToList();
        }

        private  List<CountryProvince> CountryProvinces { get; set; }

        #region Settings Properties

        public async Task TabSelected(TabTitle tabTitle)
        {
            Selected = tabTitle;
            switch (tabTitle)
            {
                case TabTitle.EditProfile:
                    //Selected
                    UnderlineProfileStyle = BoxTabSelectedStyle;
                    TitleProfileStyle = LabelTabSelectedStyle;
                    ImageProfile = ImageProfileSelectedPath;

                    //Not Selected
                    UnderlinePasswordStyle = BoxTabStyle;
                    TitlePasswordStyle = LabelTabStyle;
                    ImagePassword = ImagePasswordPath;

                    UnderlineUserSettingStyle = BoxTabStyle;
                    TitleUserSettingStyle = LabelTabStyle;
                    ImageUserSetting = ImageUserSettingPath;

                    //Content
                    Content = new ProfileSetting(Root, this).Content;

                    CountryName = string.Empty;
                    CityName = string.Empty;
                    StateName = string.Empty;
                    Address = string.Empty;
                    PostalCode = string.Empty;
                    await LoadUserProfile();
                    UserEmail = App.CurrentUser.UserInfo.UserEmail;
                    break;

                case TabTitle.ChangePassword:
                    //Selected
                    UnderlinePasswordStyle = BoxTabSelectedStyle;
                    TitlePasswordStyle = LabelTabSelectedStyle;
                    ImagePassword = ImagePasswordSelectedPath;

                    //Not Selected
                    UnderlineProfileStyle = BoxTabStyle;
                    TitleProfileStyle = LabelTabStyle;
                    ImageProfile = ImageProfilePath;

                    UnderlineUserSettingStyle = BoxTabStyle;
                    TitleUserSettingStyle = LabelTabStyle;
                    ImageUserSetting = ImageUserSettingPath;

                    //Content
                    Content = new ChangePasswordPage(Root, this).Content;
                    break;

                case TabTitle.UserSettings:
                    //Selected
                    UnderlineUserSettingStyle = BoxTabSelectedStyle;
                    TitleUserSettingStyle = LabelTabSelectedStyle;
                    ImageUserSetting = ImageUserSettingSelectedPath;

                    //Not Selected
                    UnderlineProfileStyle = BoxTabStyle;
                    TitleProfileStyle = LabelTabStyle;
                    ImageProfile = ImageProfilePath;

                    UnderlinePasswordStyle = BoxTabStyle;
                    TitlePasswordStyle = LabelTabStyle;
                    ImagePassword = ImagePasswordPath;

                    SettingLanguageText = TextResources.Language;
                    SettingWeightVolumeText = TextResources.WeightVolumeType;

                    //Content
                    WeightVolumeClick_Action = null;
                    Content = new UserSettingPage(Root, this).Content;
                    break;
            }
        }

        private async Task LoadUserProfile()
        {
            if (UserMeta == null)
                UserMeta = await DependencyService.Get<IMetaPivotService>().GetMetaAsync();
            if (UserMeta != null)
            {
                Address = UserMeta.Address;
                CountryName = UserMeta.Country;
                CityName = UserMeta.City;
                PostalCode = UserMeta.PostalCode;
                StateName = UserMeta.State;
            }
        }

        public async Task LoadAppLanguages(Action action)
        {
            ApplicationLanguages =
                await DependencyService.Get<IApplicationLanguageService>().GetWithCountryAsync();
            foreach (var language in ApplicationLanguages)
            {
                if (language.LanguageCode == App.Configuration.AppConfig.DefaultLanguage)
                    language.IsSelected = true;
                else
                    language.IsSelected = false;
            }

            action?.Invoke();
        }

        public async void OnLanguageSelected(ApplicationLanguage selectedLanguage)
        {
            var requestModel = new ApplicationLanguageRequest()
            {
                LanguageCode = selectedLanguage.LanguageCode,
                LanguageName = selectedLanguage.LanguageName
            };
            //DisplayCountryLanguage
            var response = await _settingService.UpdateUserLanguageAsync(requestModel);
            if (response == HttpConstants.SUCCESS)
            {
                await App.Configuration.SetUserLanguage(requestModel.LanguageCode);
                App.GoToAccountPage(true);
            }
        }

        public async Task LoadWeightVolume(Action action)
        {
            WeightVolumeData = await DependencyService.Get<IWeightVolumeService>().GetAsync();
            if (WeightVolumeData.Count > 0)
            {
                var defaultVolume = App.Configuration.AppConfig.DefaultWeightVolume;
                WeightVolumeDataSelected =
                    WeightVolumeData.FirstOrDefault(w => w.VolumeCode.ToLower() == defaultVolume.ToLower());
            }

            action?.Invoke();
        }

        public void OnWeightVolumeChange(WeightVolume weightVolume)
        {
            WeightVolumeDataSelected = weightVolume;
            SaveWeightVolume(weightVolume.VolumeCode);
        }

        private async void SaveWeightVolume(string weightVolumeType)
        {
            var requestModel = new UserWeightVolume()
            {
                WeightVolume = weightVolumeType
            };
            var response = await _settingService.UpdateUserWeightVolumeAsync(requestModel);
            if (response == HttpConstants.SUCCESS)
            {
                await App.Configuration.SetWeightVolume(requestModel.WeightVolume);
                App.GoToAccountPage(true);
            }
        }

        private void OnWeightVolumeSelected()
        {
            WeightVolumeSelected = WeightVolumeDataSelected.DisplayVolume;
        }

        public List<ApplicationLanguage> ApplicationLanguages { get; set; }

        private string _settingLanguageText;
        public const string SettingLanguageTextPropertyName = "SettingLanguageText";

        public string SettingLanguageText
        {
            get { return _settingLanguageText; }
            set { SetProperty(ref _settingLanguageText, value, SettingLanguageTextPropertyName); }
        }

        private string _settingWeightVolumeText;
        public const string SettingWeightVolumeTextPropertyName = "SettingWeightVolumeText";

        public string SettingWeightVolumeText
        {
            get { return _settingWeightVolumeText; }
            set { SetProperty(ref _settingWeightVolumeText, value, SettingWeightVolumeTextPropertyName); }
        }

        private string _weightVolumeSelected;
        public const string WeightVolumeSelectedPropertyName = "WeightVolumeSelected";

        public string WeightVolumeSelected
        {
            get { return _weightVolumeSelected; }
            set { SetProperty(ref _weightVolumeSelected, value, WeightVolumeSelectedPropertyName); }
        }

        private List<WeightVolume> _weightVolumeData;
        public const string WeightVolumeDataPropertyName = "WeightVolumeData";

        public List<WeightVolume> WeightVolumeData
        {
            get { return _weightVolumeData; }
            set { SetProperty(ref _weightVolumeData, value, WeightVolumeDataPropertyName); }
        }

        private WeightVolume _weightVolumeDataSelected;
        public const string WeightVolumeDataSelectedPropertyName = "WeightVolumeDataSelected";

        public WeightVolume WeightVolumeDataSelected
        {
            get { return _weightVolumeDataSelected; }
            set
            {
                SetProperty(ref _weightVolumeDataSelected, value, WeightVolumeDataSelectedPropertyName,
                    OnWeightVolumeSelected);
            }
        }

        public Action WeightVolumeClick_Action;
        private ICommand _volumeSelectedCommand;

        public ICommand VolumeSelectedCommand
        {
            get
            {
                return _volumeSelectedCommand ?? (_volumeSelectedCommand = new Command(
                           (obj) => { WeightVolumeClick_Action(); }));
            }
        }

        public MetaPivot UserMeta { get; set; }
        public Style BoxTabStyle => (Style) App.CurrentApp.Resources["boxViewTabStyle"];
        public Style BoxTabSelectedStyle => (Style) App.CurrentApp.Resources["boxViewTabStyleSelected"];
        public Style LabelTabStyle => (Style) App.CurrentApp.Resources["labelTabStyle"];
        public Style LabelTabSelectedStyle => (Style) App.CurrentApp.Resources["labelTabStyleSelected"];

        public string ImageProfilePath => TextResources.icon_edit_profile;
        public string ImageProfileSelectedPath => TextResources.icon_edit_profile_color;
        public string ImagePasswordPath => TextResources.icon_password;
        public string ImagePasswordSelectedPath => TextResources.icon_password_color;
        public string ImageUserSettingPath => TextResources.icon_user_settings;
        public string ImageUserSettingSelectedPath => TextResources.icon_user_settings_color;

        //private RootPage root;
        //public const string RootPropertyName = "Root";

        //public RootPage Root
        //{
        //    get { return root; }
        //    set { SetProperty(ref root, value, RootPropertyName); }
        //}

        public TabTitle Selected { get; set; }

        private View content;
        public const string ContentPropertyName = "Content";

        public View Content
        {
            get { return content; }
            set { SetProperty(ref content, value, ContentPropertyName); }
        }

        private Style underlineProfileStyle;
        public const string UnderlineProfileStylePropertyName = "UnderlineProfileStyle";

        public Style UnderlineProfileStyle
        {
            get { return underlineProfileStyle; }
            set { SetProperty(ref underlineProfileStyle, value, UnderlineProfileStylePropertyName); }
        }

        private Style underlinePasswordStyle;
        public const string UnderlinePasswordStylePropertyName = "UnderlinePasswordStyle";

        public Style UnderlinePasswordStyle
        {
            get { return underlinePasswordStyle; }
            set { SetProperty(ref underlinePasswordStyle, value, UnderlinePasswordStylePropertyName); }
        }

        private Style underlineUserSettingStyle;
        public const string UnderlineUserSettingStylePropertyName = "UnderlineUserSettingStyle";

        public Style UnderlineUserSettingStyle
        {
            get { return underlineUserSettingStyle; }
            set { SetProperty(ref underlineUserSettingStyle, value, UnderlineUserSettingStylePropertyName); }
        }

        private Style titleProfileStyle;
        public const string TitleProfileStylePropertyName = "TitleProfileStyle";

        public Style TitleProfileStyle
        {
            get { return titleProfileStyle; }
            set { SetProperty(ref titleProfileStyle, value, TitleProfileStylePropertyName); }
        }

        private Style titlePasswordStyle;
        public const string TitlePasswordStylePropertyName = "TitlePasswordStyle";

        public Style TitlePasswordStyle
        {
            get { return titlePasswordStyle; }
            set { SetProperty(ref titlePasswordStyle, value, TitlePasswordStylePropertyName); }
        }

        private Style titleUserSettingStyle;
        public const string TitleUserSettingStylePropertyName = "TitleUserSettingStyle";

        public Style TitleUserSettingStyle
        {
            get { return titleUserSettingStyle; }
            set { SetProperty(ref titleUserSettingStyle, value, TitleUserSettingStylePropertyName); }
        }

        private string imageProfile;
        public const string ImageProfilePropertyName = "ImageProfile";

        public string ImageProfile
        {
            get { return imageProfile; }
            set { SetProperty(ref imageProfile, value, ImageProfilePropertyName, ChangeImageProfile); }
        }

        private void ChangeImageProfile()
        {
            ImageProfileSource = ImageResizer.ResizeImage(ImageProfile, SettingImageSize);
        }

        private ImageSource imageProfileSource;
        public const string ImageProfileSourcePropertyName = "ImageProfileSource";

        public ImageSource ImageProfileSource
        {
            get { return imageProfileSource; }
            set { SetProperty(ref imageProfileSource, value, ImageProfileSourcePropertyName); }
        }

        private string imagePassword;
        public const string ImagePasswordPropertyName = "ImagePassword";

        public string ImagePassword
        {
            get { return imagePassword; }
            set { SetProperty(ref imagePassword, value, ImagePasswordPropertyName, ChangeImagePassword); }
        }

        private void ChangeImagePassword()
        {
            ImagePasswordSource = ImageResizer.ResizeImage(ImagePassword, SettingImageSize);
        }

        private ImageSource imagePasswordSource;
        public const string ImagePasswordSourcePropertyName = "ImagePasswordSource";

        public ImageSource ImagePasswordSource
        {
            get { return imagePasswordSource; }
            set { SetProperty(ref imagePasswordSource, value, ImagePasswordSourcePropertyName); }
        }

        private string imageUserSetting;
        public const string ImageUserSettingPropertyName = "ImageUserSetting";

        public string ImageUserSetting
        {
            get { return imageUserSetting; }
            set { SetProperty(ref imageUserSetting, value, ImageUserSettingPropertyName, ChangeImageUserSetting); }
        }

        private void ChangeImageUserSetting()
        {
            ImageUserSettingSource = ImageResizer.ResizeImage(ImageUserSetting, SettingImageSize);
        }

        private ImageSource imageUserSettingSource;
        public const string ImageUserSettingSourcePropertyName = "ImageUserSettingSource";

        public ImageSource ImageUserSettingSource
        {
            get { return imageUserSettingSource; }
            set { SetProperty(ref imageUserSettingSource, value, ImageUserSettingSourcePropertyName); }
        }

        private string titleProfile;
        public const string TitleProfilePropertyName = "TitleProfile";

        public string TitleProfile
        {
            get { return titleProfile; }
            set { SetProperty(ref titleProfile, value, TitleProfilePropertyName); }
        }

        private string titlePassword;
        public const string TitlePasswordPropertyName = "TitlePassword";

        public string TitlePassword
        {
            get { return titlePassword; }
            set { SetProperty(ref titlePassword, value, TitlePasswordPropertyName); }
        }

        private string titleUserSetting;
        public const string TitleUserSettingPropertyName = "TitleUserSetting";

        public string TitleUserSetting
        {
            get { return titleUserSetting; }
            set { SetProperty(ref titleUserSetting, value, TitleUserSettingPropertyName); }
        }

        #endregion Settings Properties

        #region Change Password Properties

        private string currentPassword;
        public const string CurrentPasswordPropertyName = "CurrentPassword";

        public string CurrentPassword
        {
            get { return currentPassword; }
            set { SetProperty(ref currentPassword, value, CurrentPasswordPropertyName); }
        }

        private string newPassword;
        public const string NewPasswordPropertyName = "NewPassword";

        public string NewPassword
        {
            get { return newPassword; }
            set { SetProperty(ref newPassword, value, NewPasswordPropertyName); }
        }

        private string confirmNewPassword;
        public const string ConfirmNewPasswordPropertyName = "ConfirmNewPassword";

        public string ConfirmNewPassword
        {
            get { return confirmNewPassword; }
            set { SetProperty(ref confirmNewPassword, value, ConfirmNewPasswordPropertyName); }
        }

        #endregion Change Password Properties

        #region Edit Profile Properties

        private string userEmail;
        public const string UserEmailPropertyName = "UserEmail";

        public string UserEmail
        {
            get { return userEmail; }
            set { SetProperty(ref userEmail, value, UserEmailPropertyName); }
        }

        private string countryName;
        public const string CountryNamePropertyName = "CountryName";

        public string CountryName
        {
            get { return countryName; }
            set { SetProperty(ref countryName, value, CountryNamePropertyName, GetStateList); }
        }

        private string cityName;
        public const string CityNamePropertyName = "CityName";

        public string CityName
        {
            get { return cityName; }
            set { SetProperty(ref cityName, value, CityNamePropertyName); }
        }

        private string stateName;
        public const string StateNamePropertyName = "StateName";

        public string StateName
        {
            get { return stateName; }
            set { SetProperty(ref stateName, value, StateNamePropertyName); }
        }

        private string address;
        public const string AddressPropertyName = "Address";

        public string Address
        {
            get { return address; }
            set { SetProperty(ref address, value, AddressPropertyName); }
        }

        private string postalCode;
        public const string PostalCodePropertyName = "PostalCode";

        public string PostalCode
        {
            get { return postalCode; }
            set { SetProperty(ref postalCode, value, PostalCodePropertyName); }
        }

        private List<string> countryList;
        public const string CountryListPropertyName = "CountryList";

        public List<string> CountryList
        {
            get { return countryList; }
            set { SetProperty(ref countryList, value, CountryListPropertyName); }
        }

        private List<string> stateList;
        public const string StateListPropertyName = "StateList";

        public List<string> StateList
        {
            get { return stateList; }
            set { SetProperty(ref stateList, value, StateListPropertyName); }
        }

        #endregion Edit Profile Properties

        private ImageSize SettingImageSize { get; set; }

        private void SetPageImageSize()
        {
            SettingImageSize = App.Configuration.GetImageSizeByID(ImageIdentity.USER_SETTING_TAB_ICON);
            if (SettingImageSize != null)
            {
                SettingTabImageHeight = SettingImageSize.Height;
                SettingTabImageWidth = SettingImageSize.Width;
            }
        }

        private float settingTabImageHeight;
        public const string SettingTabImageHeightPropertyName = "SettingTabImageHeight";

        public float SettingTabImageHeight
        {
            get { return settingTabImageHeight; }
            set { SetProperty(ref settingTabImageHeight, value, SettingTabImageHeightPropertyName); }
        }

        private float settingTabImageWidth;
        public const string SettingTabImageWidthPropertyName = "SettingTabImageWidth";

        public float SettingTabImageWidth
        {
            get { return settingTabImageWidth; }
            set { SetProperty(ref settingTabImageWidth, value, SettingTabImageWidthPropertyName); }
        }

        //private ICommand _showSideMenuCommand;

        //public ICommand ShowSideMenuCommand
        //{
        //    get
        //    {
        //        return _showSideMenuCommand ?? (_showSideMenuCommand = new Command((obj) =>
        //        {
        //            Root.IsPresented = Root.IsPresented == false;
        //        }));
        //    }
        //}
    }

    public enum TabTitle
    {
        EditProfile,
        ChangePassword,
        UserSettings
    }
}