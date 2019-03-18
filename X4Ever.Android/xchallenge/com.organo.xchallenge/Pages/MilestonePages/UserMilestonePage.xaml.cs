using com.organo.xchallenge.Extensions;
using com.organo.xchallenge.Globals;
using com.organo.xchallenge.Localization;
using com.organo.xchallenge.Pages.Base;
using com.organo.xchallenge.Permissions;
using com.organo.xchallenge.Services;
using com.organo.xchallenge.Utilities;
using com.organo.xchallenge.ViewModels.Milestones;
using com.organo.xchallenge.ViewModels.Profile;
using System;
using System.Threading.Tasks;
using com.organo.xchallenge.Converters;
using Xamarin.Forms;

namespace com.organo.xchallenge.Pages.MilestonePages
{
    public partial class UserMilestonePage : UserMilestonePageXaml
    {
        private readonly UserMilestoneViewModel _model;
        private readonly IMedia _media;
        private readonly IDevicePermissionServices _devicePermissionServices;
        private readonly PoundToKiligramConverter _converter = new PoundToKiligramConverter();

        public UserMilestonePage(RootPage root, MyProfileViewModel profileViewModel)
        {
            try
            {
                InitializeComponent();
                App.Configuration.InitialAsync(this);
                NavigationPage.SetHasNavigationBar(this, false);
                _media = DependencyService.Get<IMedia>();
                _devicePermissionServices = DependencyService.Get<IDevicePermissionServices>();
                _model = new UserMilestoneViewModel(App.CurrentApp.MainPage.Navigation)
                {
                    Root = root,
                    ProfileViewModel = profileViewModel,
                    // CHANGED
                    SliderCurrentWeight = sliderCurrentWeight
                };
                BindingContext = _model;
                _model.GetUserTracker();
                _model.ChangeSliderValue(0);
                _model.ViewComponents.Add(sliderCurrentWeight);
                Initialization();
            }
            catch (Exception ex)
            {
                ClientService.WriteLog(null, ex, true).GetAwaiter();
            }
        }

        private async void Initialization()
        {
            try
            {
                pickerTShirtSize.ItemsSource = await _model.GetTShirtSizeList();
                entryTShirtSize.Focused += (sender, e) =>
                {
                    entryTShirtSize.Unfocus();
                    pickerTShirtSize.Focus();
                    pickerTShirtSize.SelectedIndexChanged += (sender1, e1) =>
                    {
                        var shirtSizeSelected = pickerTShirtSize.SelectedItem;
                        if (shirtSizeSelected != null)
                        {
                            _model.TShirtSize = shirtSizeSelected.ToString();
                            entryAboutJourney.Focus();
                        }
                    };
                };

                var tapMale = new TapGestureRecognizer()
                {
                    Command = new Command(_model.Male_Selected)
                };
                ImageMale.GestureRecognizers.Add(tapMale);
                LabelMale.GestureRecognizers.Add(tapMale);

                var tapFemale = new TapGestureRecognizer()
                {
                    Command = new Command(_model.Female_Selected)
                };
                ImageFemale.GestureRecognizers.Add(tapFemale);
                LabelFemale.GestureRecognizers.Add(tapFemale);

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

                _model.ViewComponents.Add(imageFront);
                _model.ViewComponents.Add(imageSide);
                _model.ViewComponents.Add(ImageMale);
                _model.ViewComponents.Add(LabelMale);
                _model.ViewComponents.Add(ImageFemale);
                _model.ViewComponents.Add(LabelFemale);
                _model.ViewComponents.Add(entryTShirtSize);
                _model.ViewComponents.Add(pickerTShirtSize);
                _model.ViewComponents.Add(entryAboutJourney);
            }
            catch (Exception ex)
            {
                await ClientService.WriteLog(null, ex, true);
            }
        }

        private async Task UploadImageAsync(ImageSide side)
        {
            if (side == ImageSide.SIDE)
                _model.ImageSide = _model.ImageDefault;
            else
                _model.ImageFront = _model.ImageDefault;
            Device.BeginInvokeOnMainThread(async () =>
            {
                var localMessage = "";
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
                    if (result.ToString() == TextResources.PickFromGallery)
                    {
                        var mediaFile = await _media.PickPhotoAsync();
                        if (mediaFile == null)
                            localMessage = _media.Message;
                        else
                        {
                            await Task.Run(() => { _model.SetActivityResource(false, true); });
                            var response = await _media.UploadPhotoAsync(mediaFile);
                            if (response)
                            {
                                if (side == ImageSide.SIDE)
                                    _model.ImageSide = _media.FileName;
                                else
                                    _model.ImageFront = _media.FileName;
                                (side == ImageSide.SIDE ? imageSide : imageFront).Source =
                                    ImageSource.FromStream(() => { return mediaFile.GetStream(); });
                            }
                            else
                                localMessage = _media.Message;

                            _model.SetActivityResource();
                        }
                    }
                    else if (result.ToString() == TextResources.TakeFromCamera)
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
                            localMessage = _media.Message;
                        else
                        {
                            await Task.Run(() => { _model.SetActivityResource(false, true); });
                            var response = await _media.UploadPhotoAsync(mediaFile);
                            if (response)
                            {
                                if (side == ImageSide.SIDE)
                                    _model.ImageSide = _media.FileName;
                                else
                                    _model.ImageFront = _media.FileName;
                                (side == ImageSide.SIDE ? imageSide : imageFront).Source =
                                    ImageSource.FromStream(() => { return mediaFile.GetStream(); });
                            }
                            else
                                localMessage = _media.Message;

                            _model.SetActivityResource();
                        }
                    }

                    if (localMessage != null && localMessage.Trim().Length > 0)
                        _model.SetActivityResource(showError: true, errorMessage: localMessage);
                }
            });
            await Task.Delay(1);
        }

        protected override bool OnBackButtonPressed()
        {
            return DependencyService.Get<IBackButtonPress>().Exit();
        }
    }

    public abstract class UserMilestonePageXaml : ModelBoundContentPage<UserMilestoneViewModel>
    {
    }
}