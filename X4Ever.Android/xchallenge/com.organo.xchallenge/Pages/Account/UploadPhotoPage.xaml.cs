using com.organo.xchallenge.Extensions;
using com.organo.xchallenge.Globals;
using com.organo.xchallenge.Localization;
using com.organo.xchallenge.Models;
using com.organo.xchallenge.Models.Validation;
using com.organo.xchallenge.Pages.Base;
using com.organo.xchallenge.Permissions;
using com.organo.xchallenge.Services;
using com.organo.xchallenge.Statics;
using com.organo.xchallenge.ViewModels.Account;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using com.organo.xchallenge.Models.User;
using com.organo.xchallenge.Utilities;
using Xamarin.Forms;

namespace com.organo.xchallenge.Pages.Account
{
    public partial class UploadPhotoPage : UploadPhotoPageXaml
    {
        private readonly UploadPhotoViewModel _model;
        private readonly IMedia _media;
        private readonly IDevicePermissionServices _devicePermissionServices;
        private readonly UserFirstUpdate _user;
        private readonly ITrackerPivotService _trackerPivotService;
        private readonly IHelper _helper;
        private string _imageFrontName, _imageSideName;

        public UploadPhotoPage(UserFirstUpdate user, bool error = false, string message = "")
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
            _trackerPivotService = DependencyService.Get<ITrackerPivotService>();
            _helper = DependencyService.Get<IHelper>();
            if (error)
                _model.SetActivityResource(showError: true,
                    errorMessage: string.IsNullOrWhiteSpace(message.Trim())
                        ? message.Trim()
                        : _helper.ReturnMessage(message));
        }

        private void Initialization()
        {
            _imageFrontName = _imageSideName = string.Empty;
            if (_user.UserTrackers != null && _user.UserTrackers.Count > 0)
            {
                var frontPath = _user.UserTrackers.Get(TrackerEnum.frontimage);
                if (frontPath != null && frontPath.Trim().Length > 0)
                {
                    _model.ImageFront = frontPath;
                    _imageFrontName = frontPath;
                }

                var sidePath = _user.UserTrackers.Get(TrackerEnum.frontimage);
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
            buttonSkip.Clicked += (sender, e) =>
                 {
                     UpdateSkipOption();
                     App.GoToAccountPage(true);
                 };
        }

        private async void UpdateSkipOption()
        {
            await _trackerPivotService.PostSkipOptionAsync(App.CurrentUser.UserInfo.UserEmail, true);
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
                new string[] { TextResources.PickFromGallery, TextResources.TakeFromCamera });

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

                    _model.SetActivityResource(false, true);
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

                    _model.SetActivityResource(false, true);
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

        private List<Models.User.Tracker> _trackers;

        private async Task SubmitAsync()
        {
            _model.SetActivityResource(showEditable: false, showBusy: true, busyMessage: TextResources.ProcessingPleaseWait);
            if (Validate())
            {
                _trackers = new List<Models.User.Tracker>();
                _trackers.Add(_trackerPivotService.AddTracker(TrackerConstants.FRONT_IMAGE, _model.ImageFront));
                _trackers.Add(_trackerPivotService.AddTracker(TrackerConstants.SIDE_IMAGE, _model.ImageSide));
                foreach (var tracker in _trackers)
                {
                    tracker.RevisionNumber = "10000";
                    _user.UserTrackers.Add(tracker);
                }

                if (await _trackerPivotService.SaveTrackerStep3Async(_trackers, true))
                    App.GoToAccountPage(true);
                else
                    _model.SetActivityResource(showError: true,
                        errorMessage: _helper.ReturnMessage(_trackerPivotService.Message));
            }
        }

        private bool Validate()
        {
            ValidationErrors validationErrors = new ValidationErrors();
            if (_model.ImageFront == null || _model.ImageFront.Trim().Length == 0 ||
                _imageFrontName.Trim().Length == 0)
                validationErrors.Add(string.Format(TextResources.Required_IsMandatory, TextResources.FrontPhoto));
            if (_model.ImageSide == null || _model.ImageSide.Trim().Length == 0 ||
                _imageSideName.Trim().Length == 0)
                validationErrors.Add(string.Format(TextResources.Required_IsMandatory, TextResources.SidePhoto));
            if (validationErrors.Count() > 0)
                _model.SetActivityResource(showError: true, errorMessage: validationErrors.Show(CommonConstants.SPACE));
            return validationErrors.Count() == 0;
        }

        protected override bool OnBackButtonPressed()
        {
            App.LogoutAsync().GetAwaiter();
            App.GoToAccountPage();
            return true;
        }
    }

    public abstract class UploadPhotoPageXaml : ModelBoundContentPage<UploadPhotoViewModel>
    {
    }
}