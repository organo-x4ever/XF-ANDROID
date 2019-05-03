using com.organo.xchallenge.Globals;
using com.organo.xchallenge.Localization;
using com.organo.xchallenge.Models.Validation;
using com.organo.xchallenge.Pages.Base;
using com.organo.xchallenge.Services;
using com.organo.xchallenge.Statics;
using com.organo.xchallenge.ViewModels.ChangePassword;
using com.organo.xchallenge.ViewModels.Profile;
using System;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace com.organo.xchallenge.Pages.ChangePassword
{
    public partial class ChangePasswordPage : ChangePasswordPageXaml
    {
        private readonly ChangePasswordViewModel _model;
        private readonly IUserPivotService _userPivotService;
        private readonly IHelper _helper;

        public ChangePasswordPage()
        {
            InitializeComponent();

            _model = new ChangePasswordViewModel();
            BindingContext = _model;
            _userPivotService = DependencyService.Get<IUserPivotService>();
            _helper = DependencyService.Get<IHelper>();
            Load_Form();
        }

        private async void Load_Form()
        {
            await App.Configuration.InitialAsync(this);
            NavigationPage.SetHasNavigationBar(this, true);
            _model.SetActivityResource();
            buttonSubmit.Clicked += async (sender, e) => { await RequestChangeAsync(); };
        }

        private async void Entry_Completed(object sender, EventArgs e)
        {
            await RequestChangeAsync();
        }

        private async Task RequestChangeAsync()
        {
            _model.SetActivityResource(false, true);
            if (Validate())
            {
                var response = await _userPivotService.ChangePasswordAsync(_model.CurrentPassword.Trim(), _model.NewPassword.Trim());
                if (response != null && response.Contains(HttpConstants.SUCCESS))
                {
                    _model.CurrentPassword = string.Empty;
                    _model.NewPassword = string.Empty;
                    _model.ConfirmNewPassword = string.Empty;

                    await Navigation.PopAsync();
                }

                _model.SetActivityResource(showError: true, errorMessage: response.Contains(HttpConstants.SUCCESS)
                    ? TextResources.MessagePasswordChanged
                    : _helper.ReturnMessage(response));
            }
        }

        private bool Validate()
        {
            ValidationErrors validationErrors = new ValidationErrors();
            if (_model.CurrentPassword == null || _model.CurrentPassword.Trim().Length == 0)
                validationErrors.Add(string.Format(TextResources.Required_IsMandatory,
                    TextResources.CurrentPassword));
            if (_model.NewPassword == null || _model.NewPassword.Trim().Length == 0)
                validationErrors.Add(string.Format(TextResources.Required_IsMandatory, TextResources.NewPassword));
            else if (string.IsNullOrWhiteSpace(_model.NewPassword))
                validationErrors.Add(string.Format(TextResources.Validation_IsInvalid, TextResources.NewPassword));
            else if (_model.NewPassword.Trim().Length < 5)
                validationErrors.Add(string.Format(TextResources.Validation_LengthMustBeMoreThan,
                    TextResources.Password, 5));
            else if (_model.NewPassword.Trim().Length > 100)
                validationErrors.Add(string.Format(TextResources.Validation_LengthMustBeLessThan,
                    TextResources.Password, 100));
            if (_model.ConfirmNewPassword == null || _model.ConfirmNewPassword.Trim().Length == 0)
                validationErrors.Add(string.Format(TextResources.Required_IsMandatory,
                    TextResources.ConfirmNewPassword));
            else if (_model.NewPassword != _model.ConfirmNewPassword)
                validationErrors.Add(TextResources.MessagePasswordAndConfirmPasswordNotMatch);
            if (validationErrors.Count() > 0)
                _model.SetActivityResource(showError: true, errorMessage: validationErrors.Show("\n"));
            return validationErrors.Count() == 0;
        }

        protected override bool OnBackButtonPressed()
        {
            return DependencyService.Get<IBackButtonPress>().Redirect(_model.Root);
        }
    }

    public abstract class ChangePasswordPageXaml : ModelBoundContentPage<ChangePasswordViewModel>
    {
    }
}