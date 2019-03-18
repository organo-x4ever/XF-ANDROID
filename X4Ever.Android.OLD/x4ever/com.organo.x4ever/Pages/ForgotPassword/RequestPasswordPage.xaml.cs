using com.organo.x4ever.Globals;
using com.organo.x4ever.Localization;
using com.organo.x4ever.Pages.Base;
using com.organo.x4ever.Services;
using com.organo.x4ever.Statics;
using com.organo.x4ever.ViewModels.ForgotPassword;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using com.organo.x4ever.Models.Validation;
using Xamarin.Forms;

namespace com.organo.x4ever.Pages.ForgotPassword
{
    public partial class RequestPasswordPage : RequestPasswordPageXaml
    {
        private RequestPasswordViewModel _model;
        private IUserService userService;
        private IHelper helper;

        public RequestPasswordPage()
        {
            InitializeComponent();
            App.Configuration.InitialAsync(this);
            NavigationPage.SetHasNavigationBar(this, false);
            _model = new RequestPasswordViewModel();
            BindingContext = _model;
            buttonSubmit.Clicked += ButtonSubmit_Clicked;
            userService = DependencyService.Get<IUserService>();
            helper = DependencyService.Get<IHelper>();
            var tapGotPassword = new TapGestureRecognizer()
            {
                Command = new Command(GoToLogin)
            };
            linkGotPassword.GestureRecognizers.Add(tapGotPassword);
        }

        private void GoToLogin()
        {
            App.GoToAccountPage();
        }

        private async void ButtonSubmit_Clicked(object sender, EventArgs e)
        {
            await RequestPassword();
        }

        private async void Entry_Completed(object sender, EventArgs e)
        {
            await RequestPassword();
        }

        private async Task RequestPassword()
        {
            await Task.Run(() =>
            {
                _model.SetActivityResource(false, true);
            });
            if (await Validate())
            {
                _model.UserName = _model.EmailAddress.Trim();
                var response =
                    await userService.RequestForgotPasswordAsync(_model.EmailAddress.Trim(),
                        _model.EmailAddress.Trim());
                if (response != null)
                    if (response.Contains(HttpConstants.SUCCESS))
                        App.CurrentApp.MainPage = new NewPassword(_model);

                _model.SetActivityResource(true, showError: true,
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
                if (_model.EmailAddress == null || _model.EmailAddress.Trim().Length == 0)
                    validationErrors.Add(string.Format(TextResources.Required_IsMandatory, TextResources.EmailAddress));
                else if (!Regex.IsMatch(_model.EmailAddress.Trim(), CommonConstants.EMAIL_VALIDATION_REGEX))
                    validationErrors.Add(string.Format(TextResources.Validation_IsInvalid, TextResources.EmailAddress));
            });
            if (validationErrors.Count() > 0)
                _model.SetActivityResource(showError: true, errorMessage: validationErrors.Show(CommonConstants.SPACE));
            return validationErrors.Count() == 0;
        }

        protected override bool OnBackButtonPressed()
        {
            App.GoToAccountPage();
            return true;
        }
    }

    public abstract class RequestPasswordPageXaml : ModelBoundContentPage<RequestPasswordViewModel> { }
}