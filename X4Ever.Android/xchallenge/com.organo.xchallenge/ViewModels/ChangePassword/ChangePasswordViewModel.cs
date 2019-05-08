using com.organo.xchallenge.ViewModels.Base;
using Xamarin.Forms;

namespace com.organo.xchallenge.ViewModels.ChangePassword
{
    public class ChangePasswordViewModel : BaseViewModel
    {
        public ChangePasswordViewModel(INavigation navigation = null) : base(navigation)
        {
            CurrentPassword = string.Empty;
            NewPassword = string.Empty;
            ConfirmNewPassword = string.Empty;
        }

        #region Change Password Properties

        private string currentPassword;
        public const string CurrentPasswordPropertyName = "CurrentPassword";

        public string CurrentPassword
        {
            get { return currentPassword; }
            set { SetProperty(ref currentPassword, value, CurrentPasswordPropertyName); }
        }

        private string newPassword;
        public const string NewPasswordPropertyName = "NewPassword";

        public string NewPassword
        {
            get { return newPassword; }
            set { SetProperty(ref newPassword, value, NewPasswordPropertyName); }
        }

        private string confirmNewPassword;
        public const string ConfirmNewPasswordPropertyName = "ConfirmNewPassword";

        public string ConfirmNewPassword
        {
            get { return confirmNewPassword; }
            set { SetProperty(ref confirmNewPassword, value, ConfirmNewPasswordPropertyName); }
        }

        #endregion Change Password Properties

    }
}