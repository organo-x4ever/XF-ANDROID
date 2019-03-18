using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using com.organo.xchallenge.Globals;
using com.organo.xchallenge.Handler;
using com.organo.xchallenge.Helpers;
using com.organo.xchallenge.Localization;
using com.organo.xchallenge.Models.Authentication;
using com.organo.xchallenge.Pages;
using com.organo.xchallenge.Services;
using com.organo.xchallenge.Statics;
using com.organo.xchallenge.ViewModels.Base;
using Xamarin.Forms;

namespace com.organo.xchallenge.ViewModels.Menu
{
    public class MenuPageViewModel : BaseViewModel
    {
        public MenuPageViewModel(INavigation navigation = null) : base(navigation)
        {
            ApplicationVersion =
                string.Format(TextResources.AppVersion, App.Configuration.AppConfig.ApplicationVersion);
            User = App.CurrentUser.UserInfo;
        }

        public async void GetProfilePhoto()
        {
            try
            {
                if (string.IsNullOrEmpty(App.CurrentUser.UserInfo.ProfileImage))
                {
                    var response = await DependencyService.Get<IMetaPivotService>().GetMetaAsync();
                    App.CurrentUser.UserInfo.ProfileImage = response?.ProfilePhoto;
                    User = App.CurrentUser.UserInfo;
                }

                ProfileImagePath = User.ProfileImage;
            }
            catch (Exception ex)
            {
                new ExceptionHandler("MenuPageViewModel", ex);
            }
        }

        public async void GetMenuData()
        {
            MenuItems = await DependencyService.Get<IMenuServices>().GetByApplicationAsync();
        }

        private UserInfo _user;
        public const string UserPropertyName = "User";

        public UserInfo User
        {
            get { return _user; }
            set
            {
                SetProperty(ref _user, value, UserPropertyName);
            }
        }

        private List<Models.Menu> _menuItems;
        public const string MenuItemsPropertyName = "MenuItems";

        public List<Models.Menu> MenuItems
        {
            get { return _menuItems; }
            set { SetProperty(ref _menuItems, value, MenuItemsPropertyName, MenuBindCallback); }
        }

        public Action MenuBindCallback { get; set; }

        private string DefaultImage => TextResources.ImageNotAvailable;

        private string profileImagePath;
        public const string ProfileImagePathPropertyName = "ProfileImagePath";

        public string ProfileImagePath
        {
            private get => profileImagePath;
            set
            {
                var photo = string.IsNullOrEmpty(value) ? null : value;
                SetProperty(ref profileImagePath, photo ?? TextResources.ImageNotAvailable,
                    ProfileImagePathPropertyName, ChangeProfileImagePath);
            }
        }

        private void ChangeProfileImagePath()
        {
            var imageSize = App.Configuration.GetImageSizeByID(ImageIdentity.MENU_PAGE_USER_IMAGE);
            if (imageSize != null)
            {
                ProfileImageHeight = imageSize.Height;
                ProfileImageWidth = imageSize.Width;
            }

            ProfileImageSource = ProfileImagePath.Contains(DefaultImage)
                ? ImageResizer.ResizeImage(ProfileImagePath, imageSize)
                : DependencyService.Get<IHelper>().GetFileUri(ProfileImagePath,
                    ProfileImagePath.Contains("http") ? FileType.None : FileType.User);
        }

        private ImageSource profileImageSource;
        public const string ProfileImageSourcePropertyName = "ProfileImageSource";

        public ImageSource ProfileImageSource
        {
            get { return profileImageSource; }
            set { SetProperty(ref profileImageSource, value, ProfileImageSourcePropertyName); }
        }

        private float profileImageHeight;
        public const string ProfileImageHeightPropertyName = "ProfileImageHeight";

        public float ProfileImageHeight
        {
            get { return profileImageHeight; }
            set { SetProperty(ref profileImageHeight, value, ProfileImageHeightPropertyName); }
        }

        private float profileImageWidth;
        public const string ProfileImageWidthPropertyName = "ProfileImageWidth";

        public float ProfileImageWidth
        {
            get { return profileImageWidth; }
            set { SetProperty(ref profileImageWidth, value, ProfileImageWidthPropertyName); }
        }

        private string _applicationVersion;
        public const string ApplicationVersionPropertyName = "ApplicationVersion";

        public string ApplicationVersion
        {
            get { return _applicationVersion; }
            set { SetProperty(ref _applicationVersion, value, ApplicationVersionPropertyName); }
        }
    }
}