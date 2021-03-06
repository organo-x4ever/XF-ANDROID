﻿using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly IHelper _helper;

        private readonly string[] menuStrings = {"settings".ToLower(), "configuration".ToLower(), "config".ToLower()};

        public MenuPageViewModel(INavigation navigation = null) : base(navigation)
        {
            _helper = DependencyService.Get<IHelper>();
            ApplicationVersion = App.Configuration.AppConfig.ApplicationVersion;
            User = App.CurrentUser.UserInfo;
        }

        public async Task GetProfilePhoto()
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
                var exceptionHandler = new ExceptionHandler(typeof(MenuPageViewModel).FullName, ex);
            }
        }

        public async Task GetMenuData()
        {
            try
            {
                float height = 32, width = 32;
                var iconSize = App.Configuration.GetImageSizeByID(ImageIdentity.MENU_ITEM_ICON);
                if (iconSize != null)
                {
                    height = iconSize.Height;
                    width = iconSize.Width;
                }

                var menuItems = await DependencyService.Get<IMenuServices>().GetByApplicationAsync();

                App.Configuration.IsProfileEditAllowed = menuItems.Any(m =>
                    ((MenuType)Enum.Parse(typeof(MenuType), m.MenuType) == MenuType.Settings));
                MenuItems = (from m in menuItems
                             where !((MenuType)Enum.Parse(typeof(MenuType), m.MenuType) == MenuType.Settings)
                             select new HomeMenuItem
                             {
                                 MenuTitle = _helper.GetResource(m.MenuTitle),
                                 MenuType = (MenuType)Enum.Parse(typeof(MenuType), m.MenuType),
                                 MenuIcon = m.MenuIcon != null ? _helper.GetResource(m.MenuIcon) : "",
                                 IconStyle = IconStyle,
                                 IconSource = m.MenuIcon != null
                                     ? ImageResizer.ResizeImage(_helper.GetResource(m.MenuIcon), iconSize)
                                     : null,
                                 IconHeight = height / 2,
                                 IconWidth = width / 2,
                                 IsIconVisible = m.MenuIconVisible,
                                 TextStyle = (MenuType)Enum.Parse(typeof(MenuType), m.MenuType) == MenuType.MyProfile
                                     ? SelectedStyle
                                     : DefaultStyle,
                                 IsSelected = (MenuType)Enum.Parse(typeof(MenuType), m.MenuType) == MenuType.MyProfile,
                                 ItemPadding = new Thickness(15, 5, 0, 5),
                                 IsVisible = !((MenuType)Enum.Parse(typeof(MenuType), m.MenuType) == MenuType.Settings)
                             }).ToList();
            }
            catch (Exception ex)
            {
                SetActivityResource(showMessage: true, message: "Sorry, there is an unresolved issue. We are fixing it please have patience and try again later.");
                new ExceptionHandler(typeof(MenuPageViewModel).FullName, ex);
            }
        }

        public Style DefaultStyle => (Style) App.CurrentApp.Resources["labelStyleMenuItem"];
        public Style SelectedStyle => (Style) App.CurrentApp.Resources["labelStyleMenuItemHighlight"];
        public Style IconStyle => (Style) App.CurrentApp.Resources["imageIconMenuItem"];

        private UserInfo _user;
        public const string UserPropertyName = "User";

        public UserInfo User
        {
            get { return _user; }
            set { SetProperty(ref _user, value, UserPropertyName); }
        }

        private List<HomeMenuItem> _menuItems;
        public const string MenuItemsPropertyName = "MenuItems";

        public List<HomeMenuItem> MenuItems
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
            set
            {
                var appVersions = value.Split('.');
                var appVersion = string.Format(TextResources.AppVersion,
                    $"{appVersions[0]}.{appVersions[1]}.{appVersions[2]}");
                SetProperty(ref _applicationVersion, appVersion, ApplicationVersionPropertyName);
            }
        }
    }
}