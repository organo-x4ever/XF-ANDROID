
using com.organo.xchallenge.Extensions;
using com.organo.xchallenge.Localization;
using com.organo.xchallenge.Pages.Base;
using com.organo.xchallenge.Permissions;
using com.organo.xchallenge.Services;
using com.organo.xchallenge.Statics;
using com.organo.xchallenge.ViewModels.Menu;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using com.organo.xchallenge.Globals;
using com.organo.xchallenge.Handler;
using com.organo.xchallenge.Helpers;
using Xamarin.Forms;

namespace com.organo.xchallenge.Pages
{
    public partial class MenuPage : MenuPageXaml
    {
        private List<HomeMenuItem> _menuItems;
        private Globals.IMedia _media;
        private IMetaPivotService _metaPivotService;
        private IDevicePermissionServices _devicePermissionServices;
        private MenuPageViewModel _model;
        private IHelper _helper;

        public MenuPage(RootPage root)
        {
            try
            {
                InitializeComponent();
                Init(root);
            }
            catch (Exception ex)
            {
                new ExceptionHandler(TAG, ex);
            }
        }

        private async void Init(object obj)
        {
            _devicePermissionServices = DependencyService.Get<IDevicePermissionServices>();
            _model = new MenuPageViewModel(Navigation)
            {
                Title = TextResources.XChallenge,
                Subtitle = TextResources.XChallenge,
                Icon = TextResources.icon_menu,
                Root = (RootPage) obj,
                MenuBindCallback = MenuBind
            };
            BindingContext = this._model;
            _helper = DependencyService.Get<IHelper>();
            await _model.GetMenuData();
            await _model.GetProfilePhoto();
            _metaPivotService = DependencyService.Get<IMetaPivotService>();
            _media = DependencyService.Get<Globals.IMedia>();
        }

        public void MenuBind()
        {
            if (_model.MenuItems?.Count > 0)
            {
                float height = 30, width = 30;
                var iconSize = App.Configuration.GetImageSizeByID(ImageIdentity.MENU_ITEM_ICON);
                if (iconSize != null)
                {
                    height = iconSize.Height;
                    width = iconSize.Width;
                }

                _menuItems = new List<HomeMenuItem>();
                foreach (var menuItem in _model.MenuItems)
                {
                    _menuItems.Add(new HomeMenuItem
                    {
                        MenuTitle = _helper.GetResource(menuItem.MenuTitle),
                        MenuType = (MenuType) Enum.Parse(typeof(MenuType), menuItem.MenuType),
                        MenuIcon = menuItem.MenuIcon != null ? _helper.GetResource(menuItem.MenuIcon) : "",
                        IconSource = menuItem.MenuIcon != null
                            ? ImageResizer.ResizeImage(_helper.GetResource(menuItem.MenuIcon), iconSize)
                            : null,
                        IconHeight = height,
                        IconWidth = width,
                        IsIconVisible = menuItem.MenuIconVisible
                    });
                }

                if (_menuItems?.Count > 0)
                {
                    ListViewMenu.ItemsSource = _menuItems;
                    ListViewMenu.SelectedItem = _menuItems[0];
                    ListViewMenu.ItemSelected += async (sender, e) =>
                    {
                        if (ListViewMenu.SelectedItem == null)
                            return;
                        await _model.Root.NavigateAsync(((HomeMenuItem) e.SelectedItem).MenuType);
                    };
                    App.Configuration.IsMenuLoaded = true;
                }
            }
        }

        private async void ChangeProfilePhoto(object sender, EventArgs args)
        {
            var result = await DisplayActionSheet(TextResources.ChooseOption, TextResources.Cancel, null,
                new string[] {TextResources.PickFromGallery, TextResources.TakeFromCamera});

            if (result != null)
            {
                if (result == "Cancel")
                    return;
                if (!await _devicePermissionServices.RequestReadStoragePermission())
                {
                    _model.SetActivityResource(showError: true,
                        errorMessage: TextResources.MessagePermissionReadStorageRequired);
                    return;
                }

                if (result == TextResources.PickFromGallery)
                {
                    _media.Refresh();
                    var mediaFile = await _media.PickPhotoAsync();
                    if (mediaFile == null)
                    {
                        _model.SetActivityResource(showError: true, errorMessage: _media.Message);
                        return;
                    }

                    await Task.Run(() => { _model.SetActivityResource(false, true); });
                    var response = await _media.UploadPhotoAsync(mediaFile);
                    if (!response)
                    {
                        _model.SetActivityResource(true, false, showError: true, errorMessage: _media.Message);
                        return;
                    }
                }
                else if (result == TextResources.TakeFromCamera)
                {
                    if (!await _devicePermissionServices.RequestCameraPermission())
                    {
                        _model.SetActivityResource(showError: true,
                            errorMessage: TextResources.MessagePermissionCameraRequired);
                        return;
                    }

                    if (!await _devicePermissionServices.RequestWriteStoragePermission())
                    {
                        _model.SetActivityResource(showError: true,
                            errorMessage: TextResources.MessagePermissionCameraRequired);
                        return;
                    }

                    _media.Refresh();
                    var mediaFile = await _media.TakePhotoAsync();
                    if (mediaFile == null)
                    {
                        _model.SetActivityResource(showError: true, errorMessage: _media.Message);
                        return;
                    }

                    await Task.Run(() => { _model.SetActivityResource(false, true); });
                    var response = await _media.UploadPhotoAsync(mediaFile);
                    if (!response)
                    {
                        _model.SetActivityResource(true, false, showError: true, errorMessage: _media.Message);
                        return;
                    }
                }
				
                _model.SetActivityResource();

                if (!string.IsNullOrEmpty(_media.FileName))
                {
                    var profileImage = await _metaPivotService.AddMeta(_media.FileName,
                        MetaConstants.PROFILE_PHOTO.ToCapital(), MetaConstants.PROFILE_PHOTO,
                        MetaConstants.PROFILE_PHOTO);
                    var response = await _metaPivotService.SaveMetaAsync(profileImage);
                    if (response != null && response.Contains(HttpConstants.SUCCESS))
                    {
                        _model.User.ProfileImage = _media.FileName;
                        App.CurrentUser.UserInfo = _model.User;
                        _model.ProfileImagePath = _model.User.ProfileImage;
						_model.SetActivityResource(showMessage: true,
                            message: TextResources.ChangeProfilePhoto + " " + TextResources.Change + " " +
                                     TextResources.Success);
                    }
                }
                else
                    _model.SetActivityResource(showError: true, errorMessage: _media.Message);
            }
        }
    }

    public abstract class MenuPageXaml : ModelBoundContentPage<MenuPageViewModel>
    {
    }
}