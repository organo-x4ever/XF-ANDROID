using com.organo.xchallenge.Globals;
using com.organo.xchallenge.Helpers;
using com.organo.xchallenge.Localization;
using com.organo.xchallenge.Models;
using com.organo.xchallenge.Services;
using com.organo.xchallenge.Statics;
using com.organo.xchallenge.ViewModels.Base;
using Xamarin.Forms;

namespace com.organo.xchallenge.ViewModels.Account
{
    public class UploadPhotoViewModel : BaseViewModel
    {
        private readonly IHelper _helper;
        private readonly IConstantServices _constantServices;
        public UploadPhotoViewModel(INavigation navigation = null) : base(navigation)
        {
            SkipPhotoOption = false;
            _helper = DependencyService.Get<IHelper>();
            this.SetPageImageSize();
            this.ImageFront = this.ImageDefault;
            this.ImageSide = this.ImageDefault;
            _constantServices = DependencyService.Get<IConstantServices>();
            SetSkipPhotoOption();
        }

        public async void SetSkipPhotoOption()
        {
            var _skip=await _constantServices.TrackerSkipPhotos();
            SkipPhotoOption = _skip;
        }

        public string ImageDefault => TextResources.icon_camera;

        private string imageFront;
        public const string ImageFrontPropertyName = "ImageFront";

        public string ImageFront
        {
            get { return imageFront; }
            set { SetProperty(ref imageFront, value, ImageFrontPropertyName, ChangeImageFront); }
        }

        private void ChangeImageFront()
        {
            this.ImageFrontSource = this.ImageFront == this.ImageDefault
                ? ImageResizer.ResizeImage(this.ImageFront, UploadImageSize)
                : _helper.GetFileUri(this.ImageFront, FileType.Upload);
        }

        private ImageSource imageFrontSource;
        public const string ImageFrontSourcePropertyName = "ImageFrontSource";

        public ImageSource ImageFrontSource
        {
            get { return imageFrontSource; }
            set { SetProperty(ref imageFrontSource, value, ImageFrontSourcePropertyName); }
        }

        private string imageSide;
        public const string ImageSidePropertyName = "ImageSide";

        public string ImageSide
        {
            get { return imageSide; }
            set { SetProperty(ref imageSide, value, ImageSidePropertyName, ChangeImageSide); }
        }

        private void ChangeImageSide()
        {
            this.ImageSideSource = this.ImageSide == this.ImageDefault
                ? ImageResizer.ResizeImage(this.ImageSide, UploadImageSize)
                : _helper.GetFileUri(this.ImageSide, FileType.Upload);
        }

        private ImageSource imageSideSource;
        public const string ImageSideSourcePropertyName = "ImageSideSource";

        public ImageSource ImageSideSource
        {
            get { return imageSideSource; }
            set { SetProperty(ref imageSideSource, value, ImageSideSourcePropertyName); }
        }

        public ImageSize UploadImageSize { get; set; }

        private void SetPageImageSize()
        {
            UploadImageSize = App.Configuration.GetImageSizeByID(ImageIdentity.UPLOAD_CAMERA_IMAGE);
            if (UploadImageSize != null)
            {
                this.CameraImageHeight = UploadImageSize.Height;
                this.CameraImageWidth = UploadImageSize.Width;
            }
        }

        private float cameraImageHeight;
        public const string CameraImageHeightPropertyName = "CameraImageHeight";

        public float CameraImageHeight
        {
            get { return cameraImageHeight; }
            set { SetProperty(ref cameraImageHeight, value, CameraImageHeightPropertyName); }
        }

        private float cameraImageWidth;
        public const string CameraImageWidthPropertyName = "CameraImageWidth";

        public float CameraImageWidth
        {
            get { return cameraImageWidth; }
            set { SetProperty(ref cameraImageWidth, value, CameraImageWidthPropertyName); }
        }
        
        private bool skipPhotoOption;
        public const string SkipPhotoOptionPropertyName = "SkipPhotoOption";
        public bool SkipPhotoOption
        {
            get => skipPhotoOption;
            set => SetProperty(ref skipPhotoOption, value, SkipPhotoOptionPropertyName);
        }
    }
}