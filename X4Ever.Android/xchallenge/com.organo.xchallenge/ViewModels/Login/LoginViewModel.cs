using System;
using System.Windows.Input;
using com.organo.xchallenge.Helpers;
using com.organo.xchallenge.Localization;
using com.organo.xchallenge.Models;
using com.organo.xchallenge.Pages.ForgotPassword;
using com.organo.xchallenge.Pages.Registration;
using com.organo.xchallenge.Statics;
using com.organo.xchallenge.ViewModels.Base;
using Xamarin.Forms;

namespace com.organo.xchallenge.ViewModels.Login
{
    public class LoginViewModel : BaseViewModel
    {
        public LoginViewModel(INavigation navigation = null) : base(navigation)
        {
            EmailAddress = string.Empty;
            UserPassword = string.Empty;
            this.PageBackgroundImage = ImageResizer.ResizeImage(App.Configuration.BackgroundImage, 800, 1280);
            SetPageImageSize();
            BoxHeight = 1;
            ShowPasswordAction = () => { IsPassword = IsPassword == false; };
            ApplicationVersion = App.Configuration.AppConfig.ApplicationVersion;
        }

        private string _applicationVersion;
        public const string ApplicationVersionPropertyName = "ApplicationVersion";

        public string ApplicationVersion
        {
            get { return _applicationVersion; }
            set
            {
                var appVersions = value.Split('.');
                var appVersion = $"v{appVersions[0]}.{appVersions[1]}.{appVersions[2]}";
                SetProperty(ref _applicationVersion, appVersion, ApplicationVersionPropertyName);
            }
        }

        private string emailAddress;
        public const string EmailAddressPropertyName = "EmailAddress";

        public string EmailAddress
        {
            get { return emailAddress; }
            set { SetProperty(ref emailAddress, value, EmailAddressPropertyName); }
        }

        private string userPassword;
        public const string UserPasswordPropertyName = "UserPassword";

        public string UserPassword
        {
            get { return userPassword; }
            set { SetProperty(ref userPassword, value, UserPasswordPropertyName); }
        }
        
        private ImageSource _pageBackgroundImage;
        public const string PageBackgroundImagePropertyName = "PageBackgroundImage";

        public ImageSource PageBackgroundImage
        {
            get { return _pageBackgroundImage; }
            set { SetProperty(ref _pageBackgroundImage, value, PageBackgroundImagePropertyName); }
        }

        private bool _isPassword;
        public const string IsPasswordPropertyName = "IsPassword";

        public bool IsPassword
        {
            get { return _isPassword; }
            set { SetProperty(ref _isPassword, value, IsPasswordPropertyName, ShowPassword); }
        }

        private void ShowPassword()
        {
            EyeSource = ImageResizer.ResizeImage(IsPassword ? TextResources.icon_eye_hide : TextResources.icon_eye_show,
                EyeImageSize);
        }

        private ImageSource _eyeSource;
        public const string EyeSourcePropertyName = "EyeSource";

        public ImageSource EyeSource
        {
            get { return _eyeSource; }
            set { SetProperty(ref _eyeSource, value, EyeSourcePropertyName); }
        }

        private float eyeImageHeight;
        public const string EyeImageHeightPropertyName = "EyeImageHeight";

        public float EyeImageHeight
        {
            get { return eyeImageHeight; }
            set { SetProperty(ref eyeImageHeight, value, EyeImageHeightPropertyName); }
        }

        private float eyeImageWidth;
        public const string EyeImageWidthPropertyName = "EyeImageWidth";

        public float EyeImageWidth
        {
            get { return eyeImageWidth; }
            set { SetProperty(ref eyeImageWidth, value, EyeImageWidthPropertyName); }
        }

        private ImageSize EyeImageSize { get; set; }

        private void SetPageImageSize()
        {
            EyeImageSize = App.Configuration.GetImageSizeByID(ImageIdentity.IMAGE_EYE_PASSWORD);
            if (EyeImageSize != null)
            {
                EyeImageHeight = EyeImageSize.Height;
                EyeImageWidth = EyeImageSize.Width;
            }

            var controlImageSize = App.Configuration.GetImageSizeByID(ImageIdentity.ENTRY_EMAIL_ICON);
            if (controlImageSize != null)
            {
                ControlImageHeight = controlImageSize.Height;
                ControlImageWidth = controlImageSize.Width;
            }
        }

        private float controlImageHeight;
        public const string ControlImageHeightPropertyName = "ControlImageHeight";

        public float ControlImageHeight
        {
            get { return controlImageHeight; }
            set { SetProperty(ref controlImageHeight, value, ControlImageHeightPropertyName); }
        }

        private float controlImageWidth;
        public const string ControlImageWidthPropertyName = "ControlImageWidth";

        public float ControlImageWidth
        {
            get { return controlImageWidth; }
            set { SetProperty(ref controlImageWidth, value, ControlImageWidthPropertyName); }
        }

        private float boxHeight;
        public const string BoxHeightPropertyName = "BoxHeight";

        public float BoxHeight
        {
            get { return boxHeight; }
            set { SetProperty(ref boxHeight, value, BoxHeightPropertyName); }
        }

        public void ShowPasswordMethod()
        {
            ShowPasswordAction?.Invoke();
        }

        private Action _showPasswordAction;

        public Action ShowPasswordAction
        {
            get { return _showPasswordAction; }
            set
            {
                _showPasswordAction = value;
                ShowPasswordMethod();
            }
        }

        private ICommand _showPasswordCommand;

        public ICommand ShowPasswordCommand
        {
            get
            {
                return _showPasswordCommand ??
                       (_showPasswordCommand = new Command(() => { IsPassword = IsPassword == false; }));
            }
        }

        private ICommand _registerCommand;

        public ICommand RegisterCommand
        {
            get
            {
                return _registerCommand ?? (_registerCommand = new Command(() =>
                {
                    App.CurrentApp.MainPage = new RegisterPage();
                }));
            }
        }

        private ICommand _forgotPasswordCommand;

        public ICommand ForgotPasswordCommand
        {
            get
            {
                return _forgotPasswordCommand ?? (_forgotPasswordCommand = new Command(() =>
                {
                    App.CurrentApp.MainPage = new RequestPasswordPage();
                }));
            }
        }
    }
}