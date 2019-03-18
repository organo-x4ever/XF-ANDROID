using System.Collections.Generic;
using System.Windows.Input;
using com.organo.x4ever.Helpers;
using com.organo.x4ever.Localization;
using com.organo.x4ever.Models;
using com.organo.x4ever.Services;
using com.organo.x4ever.Statics;
using Xamarin.Forms;

namespace com.organo.x4ever.ViewModels.Registration
{
    public class RegisterViewModel : Base.BaseViewModel
    {
        public RegisterViewModel(INavigation navigation = null) : base(navigation)
        {
            ApplicationList = new List<ApplicationUserSelection>();
            SelectedApplication = string.Empty;
            EmailAddress = string.Empty;
            userPassword = string.Empty;
            UserConfirmPassword = string.Empty;
            CheckedTermAndConditions = false;
            LoginText = TextResources.Login;
            SetPageImageSize();
            ToggleCheckbox();
        }

        public async void LoadApplicationList()
        {
            ApplicationList = await DependencyService.Get<IApplicationServices>().GetAsync();
        }

        private List<ApplicationUserSelection> _applicationList;
        public const string ApplicationListPropertyName = "ApplicationList";

        public List<ApplicationUserSelection> ApplicationList
        {
            get { return _applicationList; }
            set { SetProperty(ref _applicationList, value, ApplicationListPropertyName); }
        }

        private string selectedApplication;
        public const string SelectedApplicationPropertyName = "SelectedApplication";

        public string SelectedApplication
        {
            get { return selectedApplication; }
            set { SetProperty(ref selectedApplication, value, SelectedApplicationPropertyName); }
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

        private string userConfirmPassword;
        public const string UserConfirmPasswordPropertyName = "UserConfirmPassword";

        public string UserConfirmPassword
        {
            get { return userConfirmPassword; }
            set { SetProperty(ref userConfirmPassword, value, UserConfirmPasswordPropertyName); }
        }

        private bool checkedTermAndConditions;
        public const string CheckedTermAndConditionsPropertyName = "CheckedTermAndConditions";

        public bool CheckedTermAndConditions
        {
            get { return checkedTermAndConditions; }
            set
            {
                SetProperty(ref checkedTermAndConditions, value, CheckedTermAndConditionsPropertyName, ToggleCheckbox);
            }
        }


        private string loginText;
        public const string LoginTextPropertyName = "LoginText";

        public string LoginText
        {
            get { return loginText; }
            set { SetProperty(ref loginText, value, LoginTextPropertyName); }
        }

        private void ToggleCheckbox()
        {
            CheckboxImageSource = CheckedTermAndConditions
                ? ImageResizer.ResizeImage(TextResources.icon_checked, IconImageSize)
                : ImageResizer.ResizeImage(TextResources.icon_unchecked, IconImageSize);
        }

        private ImageSource checkboxImageSource;
        public const string CheckboxImageSourcePropertyName = "CheckboxImageSource";

        public ImageSource CheckboxImageSource
        {
            get { return checkboxImageSource; }
            set { SetProperty(ref checkboxImageSource, value, CheckboxImageSourcePropertyName); }
        }

        private ICommand _termAndConditionsCheckCommand;

        public ICommand TermAndConditionsCheckCommand
        {
            get
            {
                return _termAndConditionsCheckCommand ?? (_termAndConditionsCheckCommand = new Command((obj) =>
                {
                    CheckedTermAndConditions = CheckedTermAndConditions ? false : true;
                }));
            }
        }


        private ICommand _showSlideMenuCommand;

        public ICommand ShowSlideMenuCommand
        {
            get { return _showSlideMenuCommand ?? (_showSlideMenuCommand = new Command((obj) => { })); }
        }

        private ImageSize IconImageSize { get; set; }

        private void SetPageImageSize()
        {
            IconImageSize = App.Configuration.GetImageSizeByID(ImageIdentity.CHECKBOX_ICON);
            if (IconImageSize != null)
            {
                CheckboxImageHeight = IconImageSize.Height;
                CheckboxImageWidth = IconImageSize.Width;
            }
        }

        private float checkboxImageHeight;
        public const string CheckboxImageHeightPropertyName = "CheckboxImageHeight";

        public float CheckboxImageHeight
        {
            get { return checkboxImageHeight; }
            set { SetProperty(ref checkboxImageHeight, value, CheckboxImageHeightPropertyName); }
        }

        private float checkboxImageWidth;
        public const string CheckboxImageWidthPropertyName = "CheckboxImageWidth";

        public float CheckboxImageWidth
        {
            get { return checkboxImageWidth; }
            set { SetProperty(ref checkboxImageWidth, value, CheckboxImageWidthPropertyName); }
        }
    }
}