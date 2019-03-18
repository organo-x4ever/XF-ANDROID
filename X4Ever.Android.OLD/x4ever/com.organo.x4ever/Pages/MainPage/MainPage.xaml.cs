
using com.organo.x4ever.Localization;
using com.organo.x4ever.Models;
using com.organo.x4ever.Models.Validation;
using com.organo.x4ever.Pages.Base;
using com.organo.x4ever.Services;
using com.organo.x4ever.Statics;
using com.organo.x4ever.ViewModels.Login;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using com.organo.x4ever.Handler;
using com.organo.x4ever.Pages.Account;
using Xamarin.Forms;

namespace com.organo.x4ever.Pages.MainPage
{
    public partial class MainPage : MainPageXaml
    {
        private static IAuthenticationService _authenticationService;
        private LoginViewModel _model;
        private string Message { get; set; }
        private MessageTypeParam MessageType { get; set; }

        public MainPage(string message = "", MessageTypeParam messageType = MessageTypeParam.MESSAGE)
        {
            try
            {
                InitializeComponent();
                Message = message;
                MessageType = messageType;
                _authenticationService = DependencyService.Get<IAuthenticationService>();
                _model = new LoginViewModel();
                BindingContext = _model;
                Init();
            }
            catch (Exception ex)
            {
                new ExceptionHandler(TAG, ex);
            }
        }

        private async void Init()
        {
            await App.Configuration.InitialAsync(this);
            NavigationPage.SetHasNavigationBar(this, false);
            await Task.Run(() => { _model.SetActivityResource(false, true); });
            var response = await DependencyService.Get<IUserService>().GetAuthenticationAsync();
            if (!response)
            {
                _model.SetActivityResource();
                ButtonSignIn.Clicked += async (sender, e) => { await LoginCommand(); };
                Initialization(Message);
            }
        }

        private void Initialization(string message = "")
        {
            if (message != null && message.Trim().Length > 0)
                _model.SetActivityResource(showMessage: true, message: message);
            EntryPassword.Text = string.Empty;
            EntryPassword.Focused += (sender, e) => { _model.BoxHeight = 2; };
            EntryPassword.Unfocused += (sender, e) => { _model.BoxHeight = 1; };
        }

        private async void entry_Completed(object sender, EventArgs e)
        {
            await LoginCommand();
        }

        private async Task LoginCommand()
        {
            await Task.Run(() =>
            {
                _model.SetActivityResource(false, true, busyMessage: TextResources.ValidatingCredentials);
            });
            if (await Validate())
            {
                if (!await _authenticationService.AuthenticationAsync(EntryUsername.Text.Trim(),
                    EntryPassword.Text.Trim()))
                {
                    EntryPassword.Text = string.Empty;
                    _model.SetActivityResource(true, showError: true,
                        errorMessage: string.IsNullOrEmpty(_authenticationService.Message)
                            ? TextResources.Message_LoginFailed
                            : _authenticationService.Message);
                    return;
                }

                await UserRedirect();
            }
        }

        private async Task UserRedirect()
        {
            await Task.Run(() =>
            {
                _model.SetActivityResource(false, true, busyMessage: TextResources.LoggedInLoadingAccount);
            });
            if (App.CurrentUser != null)
            {
                if (App.CurrentUser.UserInfo != null &&
                    App.CurrentUser.UserInfo.UserFirstName != null &&
                    App.CurrentUser.UserInfo.UserFirstName.Trim().Length > 0)
                    App.GoToAccountPage(true);
                else
                {
                    await Task.Delay(1);
                    App.CurrentApp.MainPage = new BasicInfoPage(new UserFirstUpdate
                    {
                        UserEmail = App.CurrentUser.UserInfo?.UserEmail
                    });
                }
            }
        }

        private async Task<bool> Validate()
        {
            var validationErrors = new ValidationErrors();
            await Task.Run(() =>
            {

                if (this.EntryUsername == null || this.EntryUsername.Text == null ||
                    this.EntryUsername.Text.Trim().Length == 0)
                    validationErrors.Add(string.Format(TextResources.Required_IsMandatory, TextResources.Username));
                else if (!Regex.IsMatch(this.EntryUsername.Text.Trim(), CommonConstants.EMAIL_VALIDATION_REGEX))
                    validationErrors.Add(string.Format(TextResources.Validation_IsInvalid, TextResources.Username));

                if (EntryPassword == null || EntryPassword.Text == null ||
                    EntryPassword.Text.Trim().Length == 0)
                    validationErrors.Add(string.Format(TextResources.Required_IsMandatory, TextResources.Password));
                else if (string.IsNullOrWhiteSpace(EntryPassword.Text))
                    validationErrors.Add(string.Format(TextResources.Validation_IsInvalid, TextResources.Password));
                else if (EntryPassword.Text.Trim().Length < 5)
                    validationErrors.Add(string.Format(TextResources.Validation_LengthMustBeMoreThan,
                        TextResources.Password, 5));
                else if (EntryPassword.Text.Trim().Length > 100)
                    validationErrors.Add(string.Format(TextResources.Validation_LengthMustBeLessThan,
                        TextResources.Password, 100));
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
            return DependencyService.Get<IBackButtonPress>().Exit();
        }
    }

    public abstract class MainPageXaml : ModelBoundContentPage<LoginViewModel>
    {
    }
}