using com.organo.x4ever.Globals;
using com.organo.x4ever.Localization;
using com.organo.x4ever.Pages.Base;
using com.organo.x4ever.Services;
using com.organo.x4ever.Statics;
using com.organo.x4ever.ViewModels.ForgotPassword;
using System.Threading.Tasks;
using com.organo.x4ever.Models.Validation;
using Xamarin.Forms;

namespace com.organo.x4ever.Pages.ForgotPassword
{
    public partial class NewPassword : NewPasswordXaml
    {
        private RequestPasswordViewModel _model;
        private IUserService userService;
        private IHelper helper;

        public NewPassword(RequestPasswordViewModel model)
        {
            InitializeComponent();
            App.Configuration.InitialAsync(this);
            NavigationPage.SetHasNavigationBar(this, false);
            _model = model;
            BindingContext = _model;
            userService = DependencyService.Get<IUserService>();
            helper = DependencyService.Get<IHelper>();
            buttonSubmit.Clicked += ButtonSubmit_Clicked;
        }

        private async void ButtonSubmit_Clicked(object sender, System.EventArgs e)
        {
            await ChangePassword();
        }

        private async void Entry_Completed(object sender, System.EventArgs e)
        {
            await ChangePassword();
        }

        private async Task ChangePassword()
        {
            await Task.Run(() =>
            {
                _model.LayoutOptions = LayoutOptions.Center;
                _model.SetActivityResource(false, true);
            });

            if (await Validate())
            {
                var response =
                    await userService.ChangeForgotPasswordAsync(_model.SecretCode.Trim(),
                        _model.Password.Trim());
                if (response != null)
                    if (response.Contains(HttpConstants.SUCCESS))
                        App.CurrentApp.MainPage = new MainPage.MainPage(TextResources.MessagePasswordChanged);

                _model.SetActivityResource(showError: true,
                    errorMessage: response != null
                        ? helper.ReturnMessage(response)
                        : TextResources.MessageSomethingWentWrong);
            }
        }

        private async Task<bool> Validate()
        {
            var validationErrors = new ValidationErrors();
            await Task.Run(() =>
            {
                if (_model.SecretCode == null || _model.SecretCode.Trim().Length == 0)
                    validationErrors.Add(string.Format(TextResources.Required_IsMandatory,
                        TextResources.AuthorizationCode));
                else if (_model.Password == null || _model.Password.Trim().Length == 0)
                    validationErrors.Add(string.Format(TextResources.Required_IsMandatory, TextResources.NewPassword));
                else if (_model.Password.Trim().Length < 5)
                    validationErrors.Add(string.Format(TextResources.Validation_LengthMustBeMoreThan,
                        TextResources.Password, 5));
                else if (_model.Password.Trim().Length > 100)
                    validationErrors.Add(string.Format(TextResources.Validation_LengthMustBeLessThan,
                        TextResources.Password, 100));
                else if (_model.ConfirmPassword == null || _model.ConfirmPassword.Trim().Length == 0)
                    validationErrors.Add(string.Format(TextResources.Required_IsMandatory,
                        TextResources.ConfirmPassword));
                else if (_model.Password.Trim() != _model.ConfirmPassword.Trim())
                    validationErrors.Add(TextResources.MessagePasswordAndConfirmPasswordNotMatch);
            });
            if (validationErrors.Count() > 0)
                _model.SetActivityResource(showError: true,
                    errorMessage: validationErrors.Count() > 2
                        ? TextResources.Required_AllInputs
                        : validationErrors.Show(CommonConstants.SPACE));
            return validationErrors.Count() == 0;
        }

        protected override bool OnBackButtonPressed()
        {
            return DependencyService.Get<IBackButtonPress>().Redirect(new RequestPasswordPage());
        }
    }

    public abstract class NewPasswordXaml : ModelBoundContentPage<RequestPasswordViewModel> { }
}