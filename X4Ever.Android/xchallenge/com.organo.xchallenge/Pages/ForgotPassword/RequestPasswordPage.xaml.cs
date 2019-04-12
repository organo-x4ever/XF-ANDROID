using com.organo.xchallenge.Globals;
using com.organo.xchallenge.Localization;
using com.organo.xchallenge.Pages.Base;
using com.organo.xchallenge.Services;
using com.organo.xchallenge.Statics;
using com.organo.xchallenge.ViewModels.ForgotPassword;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using com.organo.xchallenge.Models.Validation;
using Xamarin.Forms;

namespace com.organo.xchallenge.Pages.ForgotPassword
{
    public partial class RequestPasswordPage : RequestPasswordPageXaml
    {
        private readonly RequestPasswordViewModel _model;
        private readonly IUserPivotService _userPivotService;
        private readonly IHelper _helper;

        public RequestPasswordPage()
        {
            InitializeComponent();
            App.Configuration.InitialAsync(this);
            NavigationPage.SetHasNavigationBar(this, false);
            _model = new RequestPasswordViewModel();
            BindingContext = _model;
            buttonSubmit.Clicked += ButtonSubmit_Clicked;
            _userPivotService = DependencyService.Get<IUserPivotService>();
            _helper = DependencyService.Get<IHelper>();
            var tapGotPassword = new TapGestureRecognizer()
            {
                Command = new Command(GoToLogin)
            };
            linkGotPassword.GestureRecognizers.Add(tapGotPassword);
        }

        private void GoToLogin() => App.GoToAccountPage();

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
            _model.SetActivityResource(false, true);
            if (Validate())
            {
                _model.UserName = _model.EmailAddress.Trim();
                var response = await _userPivotService.RequestForgotPasswordAsync(
                    _model.EmailAddress.Trim(),
                    _model.EmailAddress.Trim());
                if (response != null)
                {
                    _model.SetActivityResource();
                    if (response.Contains(HttpConstants.SUCCESS))
                        App.CurrentApp.MainPage = new NewPassword(_model);
                    else
                        _model.SetActivityResource(true, showError: true,
                            errorMessage: _helper.ReturnMessage(response));
                }
                else
                    _model.SetActivityResource(true, showError: true,
                        errorMessage: TextResources.EmailIDNotFound);
            }
        }

        private bool Validate()
        {
            var validationErrors = new ValidationErrors();
            if (_model.EmailAddress == null || _model.EmailAddress.Trim().Length == 0)
                validationErrors.Add(string.Format(TextResources.Required_IsMandatory, TextResources.EmailAddress));
            else if (!Regex.IsMatch(_model.EmailAddress.Trim(), CommonConstants.EMAIL_VALIDATION_REGEX))
                validationErrors.Add(string.Format(TextResources.Validation_IsInvalid, TextResources.EmailAddress));
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

    public abstract class RequestPasswordPageXaml : ModelBoundContentPage<RequestPasswordViewModel>
    {
    }
}