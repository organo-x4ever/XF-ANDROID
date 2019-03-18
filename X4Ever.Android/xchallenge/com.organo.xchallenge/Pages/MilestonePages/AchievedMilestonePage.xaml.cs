using com.organo.xchallenge.Globals;
using com.organo.xchallenge.Localization;
using com.organo.xchallenge.Pages.Base;
using com.organo.xchallenge.Permissions;
using com.organo.xchallenge.ViewModels.Milestones;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using com.organo.xchallenge.Services;
using com.organo.xchallenge.Utilities;
using Xamarin.Forms;

namespace com.organo.xchallenge.Pages.MilestonePages
{
    public partial class AchievedMilestonePage : AchievedMilestonePageXaml
    {
        private MilestoneViewModel _model;
        private readonly IMedia media;

        private IDevicePermissionServices _devicePermissionServices;
        //private Plugin.Media.Abstractions.MediaFile _mediaFile;

        public AchievedMilestonePage(MilestoneViewModel model)
        {
            try
            {
                InitializeComponent();
                media = DependencyService.Get<IMedia>();
                _devicePermissionServices = DependencyService.Get<IDevicePermissionServices>();
                _model = model;
                BindingContext = _model;
                Initialization();
            }
            catch (Exception ex)
            {
                DependencyService.Get<IMessage>().AlertAsync(TextResources.Alert,
                    ex.InnerException != null ? ex.InnerException.Message : ex.Message, TextResources.Ok);
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
                await DependencyService.Get<IMessage>().AlertAsync(TextResources.Alert,
                    ex.InnerException != null ? ex.InnerException.Message : ex.Message, TextResources.Ok);
            }
        }

        protected async Task UploadImageAsync(ImageSide side)
        {
            if (side == ImageSide.SIDE)
                _model.ImageSide = _model.ImageDefault;
            else
                _model.ImageFront = _model.ImageDefault;

            //var result = await DisplayActionSheet(TextResources.ChooseOption, TextResources.Cancel, null,
            //    new string[] { TextResources.PickFromGallery, TextResources.TakeFromCamera });
            //var result = await DisplayAlert(TextResources.ChooseOption, string.Empty, TextResources.PickFromGallery, TextResources.TakeFromCamera );
            var result = TextResources.TakeFromCamera;
            if (result != null)
            {
                if (result == "Cancel")
                    return;
                if (!await _devicePermissionServices.RequestReadStoragePermission())
                {
                    await DisplayAlert(TextResources.Review, TextResources.MessagePermissionReadStorageRequired,
                        TextResources.Ok);
                    return;
                }

                media.Refresh();
                if (result.ToString() == TextResources.PickFromGallery)
                {
                    var _mediaFile = await media.PickPhotoAsync();
                    if (_mediaFile == null)
                    {
                        _model.SetActivityResource(showError: true, errorMessage: media.Message);
                        return;
                    }

                    (side == ImageSide.SIDE ? imageSide : imageFront).Source =
                        ImageSource.FromStream(() => { return _mediaFile.GetStream(); });
                    if (media.FileName != null)
                    {
                        if (side == ImageSide.SIDE)
                            _model.ImageSide = media.FileName;
                        else
                            _model.ImageFront = media.FileName;
                    }
                    else
                        _model.SetActivityResource(showError: true, errorMessage: media.Message);
                }
                else if (result.ToString() == TextResources.TakeFromCamera)
                {
                    if (!await _devicePermissionServices.RequestCameraPermission())
                    {
                        await DisplayAlert(TextResources.Review, TextResources.MessagePermissionCameraRequired,
                            TextResources.Ok);
                        return;
                    }

                    if (!await _devicePermissionServices.RequestWriteStoragePermission())
                    {
                        await DisplayAlert(TextResources.Review, TextResources.MessagePermissionCameraRequired,
                            TextResources.Ok);
                        return;
                    }

                    var mediaFile = await media.TakePhotoAsync();
                    if (mediaFile == null)
                    {
                        _model.SetActivityResource(showError: true, errorMessage: media.Message);
                        return;
                    }

                    (side == ImageSide.SIDE ? imageSide : imageFront).Source =
                        ImageSource.FromStream(() => { return mediaFile.GetStream(); });
                    if (media.FileName != null && media.FileName.Trim().Length > 0)
                    {
                        var index = media.FileName.LastIndexOf("\"");
                        var fileName = media.FileName.Remove(index, 1);
                        if (side == ImageSide.SIDE)
                            _model.ImageSide = fileName;
                        else
                            _model.ImageFront = fileName;
                    }
                    else
                        _model.SetActivityResource(showError: true, errorMessage: media.Message);
                }
            }
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }

    public abstract class AchievedMilestonePageXaml : ModelBoundContentPage<MilestoneViewModel>
    {
    }
}