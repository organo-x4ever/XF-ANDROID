
using com.organo.xchallenge.Extensions;
using com.organo.xchallenge.Localization;
using com.organo.xchallenge.Pages.Base;
using com.organo.xchallenge.Permissions;
using com.organo.xchallenge.Services;
using com.organo.xchallenge.Statics;
using com.organo.xchallenge.ViewModels.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using com.organo.xchallenge.Globals;
using com.organo.xchallenge.Handler;
using com.organo.xchallenge.Helpers;
using Xamarin.Forms;

namespace com.organo.xchallenge.Pages
{
    public partial class MenuPage : MenuPageXaml
    {
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
                var exceptionHandler = new ExceptionHandler(TAG, ex);
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

        private void MenuBind()
        {
            if (_model.MenuItems?.Count > 0)
            {
                ListViewMenu.SelectedItem = _model.MenuItems.FirstOrDefault(m => m.IsSelected);
                ListViewMenu.ItemSelected += async (sender, e) =>
                {
                    if (ListViewMenu.SelectedItem != null)
                    {
                        var menuItem = (HomeMenuItem) ListViewMenu.SelectedItem;
                        if (!menuItem.IsSelected)
                        {
                            await _model.Root.NavigateAsync(menuItem.MenuType);
                            foreach (var mi in _model.MenuItems)
                            {
                                mi.IsSelected = false;
                                mi.TextStyle = _model.DefaultStyle;
                            }

                            var menu = _model.MenuItems.Find(t => t.MenuType == menuItem.MenuType);
                            menu.IsSelected = true;
                            menu.TextStyle = _model.SelectedStyle;
                        }

                        ListViewMenu.SelectedItem = null;
                    }
                };
                App.Configuration.IsMenuLoaded = true;
            }
        }


        private async void ChangeProfilePhoto(object sender, EventArgs args)
        {
            var result = await DisplayActionSheet(TextResources.ChooseOption, TextResources.Cancel, null,
                new string[] {TextResources.PickFromGallery, TextResources.TakeFromCamera});

            if (!string.IsNullOrEmpty(result) && result?.ToLower() != "cancel")
            {
                if (!await _devicePermissionServices.RequestReadStoragePermission())
                    _model.SetActivityResource(showError: true, errorMessage: TextResources.MessagePermissionReadStorageRequired);
                else
                {
                    _media.Refresh();
                    if (result == TextResources.PickFromGallery)
                        UploadPhoto(await _media.PickPhotoAsync());
                    else if (result == TextResources.TakeFromCamera)
                    {
                        if (!await _devicePermissionServices.RequestCameraPermission())
                            _model.SetActivityResource(showError: true, errorMessage: TextResources.MessagePermissionCameraRequired);
                        else if (!await _devicePermissionServices.RequestWriteStoragePermission())
                            _model.SetActivityResource(showError: true, errorMessage: TextResources.MessagePermissionCameraRequired);
                        else
                            UploadPhoto(await _media.TakePhotoAsync());
                    }
                }
            }
        }

        private async void UploadPhoto(Plugin.Media.Abstractions.MediaFile mediaFile)
        {
            if (mediaFile != null)
            {
                await Task.Run(() => { _model.SetActivityResource(false, true); });

                if (await _media.UploadPhotoAsync(mediaFile))
                {
                    if (!string.IsNullOrEmpty(_media.FileName))
                    {
                        var profileImage = _metaPivotService.AddMeta(_media.FileName,
                            MetaConstants.PROFILE_PHOTO.ToCapital(),
                            MetaConstants.PROFILE_PHOTO,
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
                            return;
                        }
                    }
                }
            }
            _model.SetActivityResource(showError: true, errorMessage: _media.Message);
        }
    }

    public abstract class MenuPageXaml : ModelBoundContentPage<MenuPageViewModel>
    {
    }
}