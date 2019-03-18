using com.organo.x4ever.Localization;
using Xamarin.Forms;

namespace com.organo.x4ever.ViewModels.ForgotPassword
{
    public class RequestPasswordViewModel : Base.BaseViewModel
    {
        public RequestPasswordViewModel(INavigation navigation = null) : base(navigation)
        {
            EmailAddress = string.Empty;
            UserName = string.Empty;
            SecretCode = string.Empty;
            Password = string.Empty;
            ConfirmPassword = string.Empty;
            LoginText = TextResources.Login;
        }

        private string emailAddress;
        public const string EmailAddressPropertyName = "EmailAddress";

        public string EmailAddress
        {
            get { return emailAddress; }
            set { SetProperty(ref emailAddress, value, EmailAddressPropertyName); }
        }

        private string userName;
        public const string UserNamePropertyName = "UserName";

        public string UserName
        {
            get { return userName; }
            set { SetProperty(ref userName, value, UserNamePropertyName); }
        }

        private string secretCode;
        public const string SecretCodePropertyName = "SecretCode";

        public string SecretCode
        {
            get { return secretCode; }
            set { SetProperty(ref secretCode, value, SecretCodePropertyName); }
        }

        private string password;
        public const string PasswordPropertyName = "Password";

        public string Password
        {
            get { return password; }
            set { SetProperty(ref password, value, PasswordPropertyName); }
        }

        private string confirmPassword;
        public const string ConfirmPasswordPropertyName = "ConfirmPassword";

        public string ConfirmPassword
        {
            get { return confirmPassword; }
            set { SetProperty(ref confirmPassword, value, ConfirmPasswordPropertyName); }
        }


        private string loginText;
        public const string LoginTextPropertyName = "LoginText";

        public string LoginText
        {
            get { return loginText; }
            set { SetProperty(ref loginText, value, LoginTextPropertyName); }
        }
    }
}