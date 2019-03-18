using com.organo.x4ever.Globals;
using com.organo.x4ever.Localization;
using com.organo.x4ever.Models;
using com.organo.x4ever.Models.Validation;
using com.organo.x4ever.Pages.Base;
using com.organo.x4ever.Services;
using com.organo.x4ever.Statics;
using com.organo.x4ever.ViewModels.Registration;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using com.organo.x4ever.Handler;
using Xamarin.Forms;

namespace com.organo.x4ever.Pages.Registration
{
    public partial class RegisterPage : RegisterPageXaml
    {
        private RegisterViewModel _model;
        private IUserService _userService;
        private IHelper _helper;

        public RegisterPage()
        {
            try
            {
                InitializeComponent();
                Initialization();
            }
            catch (Exception ex)
            {
                new ExceptionHandler(TAG, ex);
            }
        }

        private async void Initialization()
        {
            await App.Configuration.InitialAsync(this);
            NavigationPage.SetHasNavigationBar(this, false);
            _model = new RegisterViewModel(App.CurrentApp.MainPage.Navigation);
            BindingContext = _model;
            _userService = DependencyService.Get<IUserService>();
            _helper = new Helper();
            _model.LoadApplicationList();
            var tapTermAndConditions = new TapGestureRecognizer()
            {
                Command = new Command(ShowTermAndConditions)
            };
            linkTermAndConditions.GestureRecognizers.Add(tapTermAndConditions);

            var tapHaveAnAccount = new TapGestureRecognizer()
            {
                Command = new Command(GoToLogin)
            };
            linkHaveAnAccount.GestureRecognizers.Add(tapHaveAnAccount);

            pickerApplication.Focused += (sender, ent) =>
            {
                pickerApplication.SetBinding(Picker.SelectedItemProperty, "ApplicationKey");
                pickerApplication.ItemDisplayBinding = new Binding("ApplicationName");
            };

            pickerApplication.SelectedIndexChanged += async (sender1, e1) =>
            {
                var selectedApplication = pickerApplication.SelectedItem;
                if (selectedApplication != null)
                {
                    _model.SelectedApplication = ((ApplicationUserSelection)selectedApplication).ApplicationKey;
                    entryEmail.Focus();
                }
            };

            buttonSubmit.Clicked += async (sender, e) =>
            {
                await Register();
            };
        }

        private async Task Register()
        {
            await Task.Run(() => { _model.SetActivityResource(false, true); });
            if (await Validate())
            {
                var guid = Guid.NewGuid();
                var user = new UserRegister()
                {
                    UserEmail = _model.EmailAddress,
                    UserPassword = _model.UserPassword,
                    UserStatus = MessagingServiceConstants.EMAIL_VERIFICATION_PENDING,
                    UserRegistered = DateTime.Now,
                    UserActivationKey = guid.ToString(),
                    UserMetas = null,
                    UserApplication = _model.SelectedApplication
                };
                var response = await _userService.RegisterAsync(user);
                _model.SetActivityResource();
                if (response != null)
                    if (response.Contains(HttpConstants.SUCCESS))
                    {
                        _model.SetActivityResource(showError: true,
                            errorMessage: TextResources.MessageRegistrationSuccessful);
                        App.CurrentApp.MainPage = new MainPage.MainPage();
                        return;
                    }

                _model.SetActivityResource(showError: true,
                    errorMessage: response != null
                        ? _helper.ReturnMessage(response)
                        : TextResources.MessageSomethingWentWrong);
            }
        }

        private async void ShowTermAndConditions()
        {
            if (TextResources.Culture.Name.Equals("en-US") || TextResources.Culture.Name.Equals("en"))
                // For en-US English
                await App.CurrentApp.MainPage.Navigation.PushModalAsync(new TermAndConditionsPage());
            else
                // For other languages
                await App.CurrentApp.MainPage.Navigation.PushModalAsync(new TermAndConditionsPage());
        }

        private void GoToLogin() => App.GoToAccountPage();

        private async Task<bool> Validate()
        {
            ValidationErrors validationErrors = new ValidationErrors();
            await Task.Run(() =>
            {
                if (string.IsNullOrEmpty(_model.SelectedApplication))
                    validationErrors.Add(string.Format(TextResources.Required_MustBeSelected, TextResources.Continent));
                if (_model.EmailAddress == null || _model.EmailAddress.Trim().Length == 0)
                    validationErrors.Add(string.Format(TextResources.Required_IsMandatory, TextResources.EmailAddress));
                else if (!Regex.IsMatch(_model.EmailAddress.Trim(), CommonConstants.EMAIL_VALIDATION_REGEX))
                    validationErrors.Add(string.Format(TextResources.Validation_IsInvalid, TextResources.EmailAddress));
                if (_model.UserPassword == null || _model.UserPassword.Trim().Length == 0)
                    validationErrors.Add(string.Format(TextResources.Required_IsMandatory, TextResources.Password));
                else if (string.IsNullOrWhiteSpace(_model.UserPassword))
                    validationErrors.Add(string.Format(TextResources.Validation_IsInvalid, TextResources.Password));
                else if (_model.UserPassword.Trim().Length < 5)
                    validationErrors.Add(string.Format(TextResources.Validation_LengthMustBeMoreThan,
                        TextResources.Password, 5));
                else if (_model.UserPassword.Trim().Length > 100)
                    validationErrors.Add(string.Format(TextResources.Validation_LengthMustBeLessThan,
                        TextResources.Password, 100));
                if (_model.UserConfirmPassword == null || _model.UserConfirmPassword.Trim().Length == 0)
                    validationErrors.Add(string.Format(TextResources.Required_IsMandatory,
                        TextResources.ConfirmPassword));
                else if (_model.UserPassword != _model.UserConfirmPassword)
                    validationErrors.Add(TextResources.MessagePasswordAndConfirmPasswordNotMatch);
                if (!_model.CheckedTermAndConditions)
                    validationErrors.Add(TextResources.MessageMustAgree);
            });
            if (validationErrors.Count() > 0)
                _model.SetActivityResource(showError: true, errorMessage: validationErrors.Show(CommonConstants.SPACE));
            return validationErrors.Count() == 0;
        }

        private async void Entry_Completed(object sender, EventArgs e)
        {
            await Register();
        }
        protected override bool OnBackButtonPressed()
        {
            App.GoToAccountPage();
            return true;
        }
    }

    public abstract class RegisterPageXaml : ModelBoundContentPage<RegisterViewModel> { }
}