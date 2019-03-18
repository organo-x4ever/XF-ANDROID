using com.organo.x4ever.Extensions;
using com.organo.x4ever.Globals;
using com.organo.x4ever.Localization;
using com.organo.x4ever.Models;
using com.organo.x4ever.Models.Validation;
using com.organo.x4ever.Pages.Base;
using com.organo.x4ever.Permissions;
using com.organo.x4ever.Services;
using com.organo.x4ever.Statics;
using com.organo.x4ever.ViewModels.Account;
using System;
using System.Threading.Tasks;
using com.organo.x4ever.Utilities;
using Xamarin.Forms;

namespace com.organo.x4ever.Pages.Account
{
    public partial class UploadPhotoPage : UploadPhotoPageXaml
    {
        private readonly UploadPhotoViewModel _model;
        private readonly IMedia _media;
        private readonly IDevicePermissionServices _devicePermissionServices;
        private readonly UserFirstUpdate _user;
        private readonly ITrackerService _trackerService;
        private readonly IUserService _userService;
        private readonly IHelper _helper;
        private string _imageFrontName, _imageSideName;

        public UploadPhotoPage(UserFirstUpdate user)
        {
            InitializeComponent();
            App.Configuration.InitialAsync(this);
            NavigationPage.SetHasNavigationBar(this, false);
            _media = DependencyService.Get<IMedia>();
            _devicePermissionServices = DependencyService.Get<IDevicePermissionServices>();
            _model = new UploadPhotoViewModel();
            _user = user;
            BindingContext = _model;
            Initialization();
            _trackerService = DependencyService.Get<ITrackerService>();
            _userService = DependencyService.Get<IUserService>();
            _helper = DependencyService.Get<IHelper>();
        }

        private async void Initialization()
        {
            _imageFrontName = _imageSideName = string.Empty;
            if (_user.UserTrackers != null && _user.UserTrackers.Count > 0)
            {
                var frontPath = await _user.UserTrackers.Get(TrackerEnum.frontimage);
                if (frontPath != null && frontPath.Trim().Length > 0)
                {
                    _model.ImageFront = frontPath;
                    _imageFrontName = frontPath;
                }

                var sidePath = await _user.UserTrackers.Get(TrackerEnum.frontimage);
                if (sidePath != null && sidePath.Trim().Length > 0)
                {
                    _model.ImageSide = sidePath;
                    _imageSideName = sidePath;
                }
            }

            var tapImageFront = new TapGestureRecognizer()
            {
                Command = new Command(async (obj) => { await UploadImageAsync(ImageSide.FRONT); })
            };
            imageFront.GestureRecognizers.Add(tapImageFront);

            var tapImageSide = new TapGestureRecognizer()
            {
                Command = new Command(async (obj) => { await UploadImageAsync(ImageSide.SIDE); })
            };
            imageSide.GestureRecognizers.Add(tapImageSide);

            buttonSubmit.Clicked += async (sender, e) => { await SubmitAsync(); };
        }

        private async Task UploadImageAsync(ImageSide side)
        {
            if (side == ImageSide.SIDE)
            {
                _imageSideName = string.Empty;
                _model.ImageSide = _model.ImageDefault;
            }
            else
            {
                _imageFrontName = string.Empty;
                _model.ImageFront = _model.ImageDefault;
            }

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

                _media.Refresh();
                if (result == TextResources.PickFromGallery)
                {
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

                    _model.SetActivityResource();
                    (side == ImageSide.SIDE ? imageSide : imageFront).Source =
                        ImageSource.FromStream(() => { return mediaFile.GetStream(); });
                    if (_media.FileName != null)
                    {
                        if (side == ImageSide.SIDE)
                            _imageSideName = _model.ImageSide = _media.FileName;
                        else
                            _imageFrontName = _model.ImageFront = _media.FileName;
                        (side == ImageSide.SIDE ? imageSide : imageFront).Source =
                            ImageSource.FromStream(() => { return mediaFile.GetStream(); });
                    }
                    else
                        _model.SetActivityResource(showError: true,
                            errorMessage: _media.Message);
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

                    _model.SetActivityResource();
                    if (_media.FileName != null && _media.FileName.Trim().Length > 0)
                    {
                        if (side == ImageSide.SIDE)
                            _imageSideName = _model.ImageSide = _media.FileName;
                        else
                            _imageFrontName = _model.ImageFront = _media.FileName;
                        (side == ImageSide.SIDE ? imageSide : imageFront).Source =
                            ImageSource.FromStream(() => { return mediaFile.GetStream(); });
                    }
                    else
                        _model.SetActivityResource(showError: true,
                            errorMessage: _media.Message);
                }
            }
        }

        private async Task SubmitAsync()
        {
            await Task.Run(() => { _model.SetActivityResource(false, true); });
            if (await Validate())
            {
                _user.UserTrackers.Add(await _trackerService.AddTracker(TrackerConstants.FRONT_IMAGE,
                    _model.ImageFront));
                _user.UserTrackers.Add(await _trackerService.AddTracker(TrackerConstants.SIDE_IMAGE,
                    _model.ImageSide));

                var response = await _userService.UpdateAsync(_user);
                if (response == HttpConstants.SUCCESS)
                {
                    var result = await _userService.GetAsync();
                    if (result != null)
                    {
                        result.ProfileImage = result.ProfileImage != null && result.ProfileImage.Trim().Length > 0
                            ? _helper.GetFilePath(result.ProfileImage, FileType.User)
                            : TextResources.ImageNotAvailable;
                        App.CurrentUser.UserInfo = result;
                    }

                    App.GoToAccountPage(true);
                }
                else
                    _model.SetActivityResource(showError: true, errorMessage: _helper.ReturnMessage(response));
            }
        }

        private async Task<bool> Validate()
        {
            ValidationErrors validationErrors = new ValidationErrors();
            await Task.Run(() =>
            {
                if (_model.ImageFront == null || _model.ImageFront.Trim().Length == 0 ||
                    _imageFrontName.Trim().Length == 0)
                    validationErrors.Add(string.Format(TextResources.Required_IsMandatory, TextResources.FrontPhoto));
                if (_model.ImageSide == null || _model.ImageSide.Trim().Length == 0 ||
                    _imageSideName.Trim().Length == 0)
                    validationErrors.Add(string.Format(TextResources.Required_IsMandatory, TextResources.SidePhoto));
            });
            if (validationErrors.Count() > 0)
                _model.SetActivityResource(showError: true, errorMessage: validationErrors.Show(CommonConstants.SPACE));
            return validationErrors.Count() == 0;
        }

        protected override bool OnBackButtonPressed()
        {
            return DependencyService.Get<IBackButtonPress>().Redirect(new AddressPage(_user));
        }
    }

    public abstract class UploadPhotoPageXaml : ModelBoundContentPage<UploadPhotoViewModel>
    {
    }
}