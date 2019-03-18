using com.organo.xchallenge.Localization;
using com.organo.xchallenge.Models;
using com.organo.xchallenge.Models.Validation;
using com.organo.xchallenge.Pages.Base;
using com.organo.xchallenge.Statics;
using com.organo.xchallenge.ViewModels.Account;
using System;
using System.Threading.Tasks;
using com.organo.xchallenge.Globals;
using com.organo.xchallenge.Services;
using Xamarin.Forms;

namespace com.organo.xchallenge.Pages.Account
{
    public partial class BasicInfoPage : AccountCreateXaml
    {
        private AccountViewModel _model;
        private UserFirstUpdate _user;
        private readonly IUserPivotService _userPivotService;
        private IHelper _helper;

        public BasicInfoPage(UserFirstUpdate user)
        {
            InitializeComponent();
            _userPivotService = DependencyService.Get<IUserPivotService>();
            _user = user;
            _helper = DependencyService.Get<IHelper>();
            App.Configuration.InitialAsync(this);
            NavigationPage.SetHasNavigationBar(this, false);
            _model = new AccountViewModel();
            BindingContext = _model;
            Initialization();
        }

        private async void Initialization()
        {
            await _model.LoadAppLanguages(OnLanguageRetrieve);
            _model.FirstName = _user.UserFirstName;
            _model.LastName = _user.UserLastName;
            buttonSubmit.Clicked += async (sender, e) => { await NextStepAsync(); };
        }

        private void OnLanguageRetrieve()
        {
            languageOption.DataSource = _model.ApplicationLanguages;
            languageOption.ShowSelection = _model.ApplicationLanguages.Count > 1;
            languageOption.OnItemSelectedAction = OnLanguageSelected;
            languageOption.TextStyle = (Style) App.CurrentApp.Resources["labelStyleTableViewItem"];
            languageOption.FlagStyle = (Style) App.CurrentApp.Resources["imageEntryIcon"];
        }

        private void OnLanguageSelected()
        {
            _model.OnLanguageSelected(languageOption.DataSourceSelected);
        }

        private async void Entry_Completed(object sender, EventArgs e)
        {
            await NextStepAsync();
        }

        private async Task NextStepAsync()
        {
            _model.SetActivityResource(false, true, busyMessage: TextResources.ProcessingPleaseWait);
            if (Validate())
            {
                _user.UserFirstName = _model.FirstName;
                _user.UserLastName = _model.LastName;
                if (await _userPivotService.UpdateStep1Async(new UserStep1()
                {
                    UserEmail = _user.UserEmail,
                    UserType = _user.UserType,
                    UserFirstName = _user.UserFirstName,
                    UserLastName = _user.UserLastName
                }))
                    App.CurrentApp.MainPage = new PersonalInfoPage(_user);
                else
                    _model.SetActivityResource(showError: true,
                        errorMessage: _helper.ReturnMessage(_userPivotService.Message));
            }
        }

        private bool Validate()
        {
            ValidationErrors validationErrors = new ValidationErrors();
            if (_model.FirstName == null || _model.FirstName.Trim().Length == 0)
                validationErrors.Add(string.Format(TextResources.Required_IsMandatory, TextResources.FirstName));
            if (_model.LastName == null || _model.LastName.Trim().Length == 0)
                validationErrors.Add(string.Format(TextResources.Required_IsMandatory, TextResources.LastName));
            if (validationErrors.Count() > 0)
                _model.SetActivityResource(showError: true, errorMessage: validationErrors.Show(CommonConstants.SPACE));
            return validationErrors.Count() == 0;
        }

        protected override bool OnBackButtonPressed()
        {
            App.LogoutAsync().GetAwaiter();
            App.GoToAccountPage();
            return true;
        }
    }

    public abstract class AccountCreateXaml : ModelBoundContentPage<AccountViewModel>
    {
    }
}