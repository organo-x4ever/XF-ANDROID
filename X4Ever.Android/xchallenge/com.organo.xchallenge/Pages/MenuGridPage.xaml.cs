using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.organo.xchallenge.Controls;
using com.organo.xchallenge.Extensions;
using com.organo.xchallenge.Globals;
using com.organo.xchallenge.Handler;
using com.organo.xchallenge.Localization;
using com.organo.xchallenge.Pages.Base;
using com.organo.xchallenge.Permissions;
using com.organo.xchallenge.Services;
using com.organo.xchallenge.Statics;
using com.organo.xchallenge.ViewModels.Menu;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace com.organo.xchallenge.Pages
{
    public partial class MenuGridPage : MenuGridPageXaml
    {
        private Globals.IMedia _media;
        private IMetaPivotService _metaPivotService;
        private IDevicePermissionServices _devicePermissionServices;
        private MenuGridViewModel _model;
        private IHelper _helper;

        public MenuGridPage(RootPage root)
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
            _model = new MenuGridViewModel(Navigation)
            {
                Title = TextResources.XChallenge,
                Subtitle = TextResources.XChallenge,
                Icon = TextResources.icon_menu,
                Root = (RootPage) obj
            };
            BindingContext = this._model;
            _helper = DependencyService.Get<IHelper>();
            GridMenu.ItemSelectedHandler += GridMenu_ItemSelectedHandler;
            await _model.BindMenuData();
            await _model.GetProfilePhoto();
            App.Configuration.IsMenuLoaded = true;
            _metaPivotService = DependencyService.Get<IMetaPivotService>();
            _media = DependencyService.Get<Globals.IMedia>();
        }

        private async void GridMenu_ItemSelectedHandler(object sender, EventArgs e)
        {
            if (GridMenu.SelectedItem != null)
            {
                if (GridMenu.SelectedItem.IsSelected) return;
                Redirect(GridMenu.SelectedItem.MenuType);
                foreach (var mi in _model.MenuItems)
                {
                    mi.IsSelected = false;
                    mi.TextStyle = _model.DefaultStyle;
                }

                var menu = _model.MenuItems.Find(t => t.MenuType == GridMenu.SelectedItem.MenuType);
                menu.IsSelected = true;
                menu.TextStyle = _model.SelectedStyle;
                GridMenu.Source = _model.MenuItems;
                await GridMenu.Rebind(sender, _model.MenuItems);
            }
        }

        private async void Redirect(MenuType menuType) => await _model.Root.NavigateAsync(menuType);

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

    
    public abstract class MenuGridPageXaml : ModelBoundContentPage<MenuGridViewModel>
    {
    }
}