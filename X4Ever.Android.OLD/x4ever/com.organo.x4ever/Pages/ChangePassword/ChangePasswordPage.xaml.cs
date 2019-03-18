using com.organo.x4ever.Globals;
using com.organo.x4ever.Localization;
using com.organo.x4ever.Models.Validation;
using com.organo.x4ever.Pages.Base;
using com.organo.x4ever.Services;
using com.organo.x4ever.Statics;
using com.organo.x4ever.ViewModels.Profile;
using System;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace com.organo.x4ever.Pages.ChangePassword
{
    public partial class ChangePasswordPage : ChangePasswordPageXaml
    {
        private SettingsViewModel _model;
        private IUserService userService;
        private IHelper helper;

        public ChangePasswordPage(RootPage root, SettingsViewModel model)
        {
            InitializeComponent();
            App.Configuration.InitialAsync(this);
            _model = model;
            _model.Root = root;
            BindingContext = _model;
            Load_Form();
            userService = DependencyService.Get<IUserService>();
            helper = DependencyService.Get<IHelper>();
            _model.SetActivityResource();
            _model.CurrentPassword = string.Empty;
            _model.NewPassword = string.Empty;
            _model.ConfirmNewPassword = string.Empty;
        }

        private async void Load_Form()
        {
            await Task.Run(() => { buttonSubmit.Clicked += async (sender, e) => { await RequestChangeAsync(); }; });
        }

        private async void Entry_Completed(object sender, EventArgs e)
        {
            await RequestChangeAsync();
        }

        private async Task RequestChangeAsync()
        {
            await Task.Run(() =>
            {
                _model.IsMessage = false;
                _model.MessageText = string.Empty;
            });
            _model.SetActivityResource(false, true);
            if (await Validate())
            {
                var response =
                    await userService.ChangePasswordAsync(_model.CurrentPassword.Trim(), _model.NewPassword.Trim());
                _model.SetActivityResource();
                if (response != null)
                {
                    if (response.Contains(HttpConstants.SUCCESS))
                    {
                        _model.CurrentPassword = string.Empty;
                        _model.NewPassword = string.Empty;
                        _model.ConfirmNewPassword = string.Empty;
                    }
                }

                _model.SetActivityResource(showError: true, errorMessage: response.Contains(HttpConstants.SUCCESS)
                    ? TextResources.MessagePasswordChanged
                    : helper.ReturnMessage(response));
            }
        }

        private async Task<bool> Validate()
        {
            ValidationErrors validationErrors = new ValidationErrors();
            await Task.Run(() =>
            {
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
            });
            if (validationErrors.Count() > 0)
                _model.SetActivityResource(showError: true, errorMessage: validationErrors.Show("\n"));
            return validationErrors.Count() == 0;
        }

        protected override bool OnBackButtonPressed()
        {
            return DependencyService.Get<IBackButtonPress>().Redirect(_model.Root);
        }
    }

    public abstract class ChangePasswordPageXaml : ModelBoundContentPage<SettingsViewModel>
    {
    }
}