using com.organo.x4ever.Globals;
using com.organo.x4ever.Helpers;
using com.organo.x4ever.Localization;
using com.organo.x4ever.Models.Authentication;
using com.organo.x4ever.Pages;
using com.organo.x4ever.Statics;
using com.organo.x4ever.ViewModels.Base;
using Xamarin.Forms;

namespace com.organo.x4ever.ViewModels.Menu
{
    public class MenuPageViewModel : BaseViewModel
    {
        public MenuPageViewModel(INavigation navigation = null) : base(navigation)
        {
            ApplicationVersion =
                string.Format(TextResources.AppVersion, App.Configuration.AppConfig.ApplicationVersion);
            User = App.CurrentUser.UserInfo;
            ProfileImagePath = User.ProfileImage;
        }

        private RootPage root;
        public const string RootPropertyName = "Root";

        public RootPage Root
        {
            get { return root; }
            set { SetProperty(ref root, value, RootPropertyName); }
        }

        private string DefaultImage => TextResources.ImageNotAvailable;

        public UserInfo User { get; set; }

        private string profileImagePath;
        public const string ProfileImagePathPropertyName = "ProfileImagePath";

        public string ProfileImagePath
        {
            get { return profileImagePath; }
            set { SetProperty(ref profileImagePath, value, ProfileImagePathPropertyName, ChangeProfileImagePath); }
        }

        private void ChangeProfileImagePath()
        {
            var imageSize = App.Configuration.GetImageSizeByID(ImageIdentity.MENU_PAGE_USER_IMAGE);
            if (imageSize != null)
            {
                ProfileImageHeight = imageSize.Height;
                ProfileImageWidth = imageSize.Width;
            }

            ProfileImageSource = ProfileImagePath == DefaultImage
                ? ImageResizer.ResizeImage(ProfileImagePath, imageSize)
                : DependencyService.Get<IHelper>().GetFileUri(ProfileImagePath, FileType.User);
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
            set { SetProperty(ref _applicationVersion, value, ApplicationVersionPropertyName); }
        }
    }
}