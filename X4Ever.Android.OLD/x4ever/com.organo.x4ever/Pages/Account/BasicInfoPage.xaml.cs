using com.organo.x4ever.Localization;
using com.organo.x4ever.Models;
using com.organo.x4ever.Models.Validation;
using com.organo.x4ever.Pages.Base;
using com.organo.x4ever.Statics;
using com.organo.x4ever.ViewModels.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using com.organo.x4ever.Services;
using Xamarin.Forms;

namespace com.organo.x4ever.Pages.Account
{
    public partial class BasicInfoPage : AccountCreateXaml
    {
        private AccountViewModel _model;
        private UserFirstUpdate _user;

        public BasicInfoPage(UserFirstUpdate user)
        {
            InitializeComponent();
            _user = user;
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
            if (await Validate())
            {
                _user.UserFirstName = _model.FirstName;
                _user.UserLastName = _model.LastName;
                App.CurrentApp.MainPage = new PersonalInfoPage(_user);
            }
        }

        private async Task<bool> Validate()
        {
            ValidationErrors validationErrors = new ValidationErrors();
            await Task.Run(() =>
            {
                if (_model.FirstName == null || _model.FirstName.Trim().Length == 0)
                    validationErrors.Add(string.Format(TextResources.Required_IsMandatory, TextResources.FirstName));
                if (_model.LastName == null || _model.LastName.Trim().Length == 0)
                    validationErrors.Add(string.Format(TextResources.Required_IsMandatory, TextResources.LastName));
            });
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