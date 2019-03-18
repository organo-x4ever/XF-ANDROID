
using com.organo.x4ever.Extensions;
using com.organo.x4ever.Localization;
using com.organo.x4ever.Pages.Base;
using com.organo.x4ever.Permissions;
using com.organo.x4ever.Services;
using com.organo.x4ever.Statics;
using com.organo.x4ever.ViewModels.Menu;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using com.organo.x4ever.Globals;
using com.organo.x4ever.Handler;
using com.organo.x4ever.Helpers;
using Xamarin.Forms;

namespace com.organo.x4ever.Pages
{
    public partial class MenuPage : MenuPageXaml
    {
        private List<HomeMenuItem> _menuItems;
        private Globals.IMedia media;
        private IMetaService _metaService;
        private IDevicePermissionServices _devicePermissionServices;
        private MenuPageViewModel _model;
        private static readonly IHelper _helper = new Helper();

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
            _menuItems = new List<HomeMenuItem>();
            _devicePermissionServices = DependencyService.Get<IDevicePermissionServices>();
            _model = new MenuPageViewModel(Navigation)
            {
                Title = TextResources.XChallenge,
                Subtitle = TextResources.XChallenge,
                Icon = TextResources.icon_menu,
                Root = (RootPage) obj
            };
            BindingContext = this._model;
            float height = 30, width = 30;
            var iconSize = App.Configuration.GetImageSizeByID(ImageIdentity.MENU_ITEM_ICON);
            if (iconSize != null)
            {
                height = iconSize.Height;
                width = iconSize.Width;
            }

            _menuItems = new List<HomeMenuItem>();
            var menuItems = await DependencyService.Get<IMenuServices>().GetByApplicationAsync();
            foreach (var menuItem in menuItems)
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
            
            ListViewMenu.ItemsSource = _menuItems;
            ListViewMenu.SelectedItem = _menuItems[0];
            ListViewMenu.ItemSelected += async (sender, e) =>
            {
                if (ListViewMenu.SelectedItem == null)
                    return;
                await _model.Root.NavigateAsync(((HomeMenuItem) e.SelectedItem).MenuType);
            };
            media = DependencyService.Get<Globals.IMedia>();
            _metaService = DependencyService.Get<IMetaService>();
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
                    media.Refresh();
                    var mediaFile = await media.PickPhotoAsync();
                    if (mediaFile == null)
                    {
                        _model.SetActivityResource(showError: true, errorMessage: media.Message);
                        return;
                    }

                    await Task.Run(() => { _model.SetActivityResource(false, true); });
                    var response = await media.UploadPhotoAsync(mediaFile);
                    if (!response)
                    {
                        _model.SetActivityResource(true, false, showError: true, errorMessage: media.Message);
                        return;
                    }
                    _model.SetActivityResource();
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

                    media.Refresh();
                    var mediaFile = await media.TakePhotoAsync();
                    if (mediaFile == null)
                    {
                        _model.SetActivityResource(showError: true, errorMessage: media.Message);
                        return;
                    }

                    await Task.Run(() => { _model.SetActivityResource(false, true); });
                    var response = await media.UploadPhotoAsync(mediaFile);
                    if (!response)
                    {
                        _model.SetActivityResource(true, false, showError: true, errorMessage: media.Message);
                        return;
                    }
                    _model.SetActivityResource();
                }

                if (media.FileName != null && media.FileName.Trim().Length > 0)
                {
                    var profileImage = await _metaService.AddMeta(media.FileName,
                        MetaConstants.PROFILE_PHOTO.ToCapital(), MetaConstants.PROFILE_PHOTO,
                        MetaConstants.PROFILE_PHOTO);
                    var response = await _metaService.SaveMetaAsync(profileImage);
                    if (response != null && response.Contains(HttpConstants.SUCCESS))
                    {
                        _model.User.ProfileImage = media.FileName;
                        App.CurrentUser.UserInfo = _model.User;
                        _model.ProfileImagePath = _model.User.ProfileImage;
                    }
                }
                else
                    _model.SetActivityResource(showError: true, errorMessage: media.Message);
            }
        }
    }

    public abstract class MenuPageXaml : ModelBoundContentPage<MenuPageViewModel>
    {
    }
}