
using com.organo.xchallenge.Localization;
using com.organo.xchallenge.Models;
using com.organo.xchallenge.Models.Validation;
using com.organo.xchallenge.Pages.Base;
using com.organo.xchallenge.Services;
using com.organo.xchallenge.Statics;
using com.organo.xchallenge.ViewModels.Login;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using com.organo.xchallenge.Handler;
using com.organo.xchallenge.Helpers;
using com.organo.xchallenge.Pages.Account;
using Xamarin.Forms;

namespace com.organo.xchallenge.Pages.MainPage
{
    public partial class MainPage : MainPageXaml
    {
        private static IAuthenticationService _authenticationService;
        private LoginViewModel _model;
        private string Message { get; set; }
        private readonly IAppVersionProvider _appVersionProvider = DependencyService.Get<IAppVersionProvider>();

        public MainPage(string message = "")
        {
            try
            {
                InitializeComponent();
                Message = message;
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
            await DependencyService.Get<IUserPivotService>().GetAuthenticationAsync(UserRedirect, Page_Load);
        }

        private void Page_Load()
        {
            _model.SetActivityResource();
            ButtonSignIn.Clicked += async (sender, e) => { await LoginCommand(); };
            Initialization(Message);
            VersionCheck();
        }
        
        private void Initialization(string message = "")
        {
            if (message != null && message.Trim().Length > 0)
                _model.SetActivityResource(showMessage: true, message: message);
            EntryPassword.Text = string.Empty;
            EntryPassword.Focused += (sender, e) => { _model.BoxHeight = 2; };
            EntryPassword.Unfocused += (sender, e) => { _model.BoxHeight = 1; };
        }

        async void VersionCheck()
        {
            await Task.Delay(TimeSpan.FromSeconds(3));
            if (!App.Configuration.IsVersionPrompt())
            {
                await _appVersionProvider.CheckAppVersionAsync(PromptUpdate);

                void PromptUpdate()
                {
                    try
                    {
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            var updateMessage = string.Format(TextResources.UpdateMessageArgs,
                                _appVersionProvider.AppName,
                                _appVersionProvider.UpdateVersion);
                            var response = await DisplayAlert(TextResources.UpdateTitle, updateMessage,
                                TextResources.Update,
                                TextResources.Later);
                            App.Configuration.VersionPrompted();
                            if (response)
                                _appVersionProvider.GotoGoogleAppStoreAsync();
                        });
                    }
                    catch (Exception e)
                    {
                        new ExceptionHandler(TAG, e);
                    }
                }
            }
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

                UserRedirect();
            }
        }

        private async void UserRedirect()
        {
            await Task.Run(() =>
            {
                _model.SetActivityResource(false, true, busyMessage: TextResources.LoggedInLoadingAccount);
            });
            if (App.CurrentUser?.UserInfo != null)
            {
                await Task.Delay(1);
                var userInfo = App.CurrentUser?.UserInfo;
                var user = new UserFirstUpdate
                {
                    UserEmail = userInfo?.UserEmail,
                    UserFirstName = userInfo?.UserFirstName ?? "",
                    UserLastName = userInfo?.UserLastName ?? "",
                };
                if (userInfo?.UserFirstName?.Trim().Length == 0)
                    App.CurrentApp.MainPage = new BasicInfoPage(user);
                else if (userInfo?.IsMetaExists == false)
                    App.CurrentApp.MainPage = new PersonalInfoPage(user);
                else if (userInfo?.IsAddressExists == false)
                    App.CurrentApp.MainPage = new AddressPage(user);
                else if (userInfo?.IsTrackerExists == false)
                    App.CurrentApp.MainPage = new UploadPhotoPage(user);
                else
                    App.GoToAccountPage(true);
            }
            else
                _model.SetActivityResource();
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

                if (string.IsNullOrEmpty(EntryPassword?.Text))
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