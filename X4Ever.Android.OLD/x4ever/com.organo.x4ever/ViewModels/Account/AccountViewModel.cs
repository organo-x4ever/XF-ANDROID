using com.organo.x4ever.Models;
using com.organo.x4ever.Services;
using com.organo.x4ever.Statics;
using com.organo.x4ever.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace com.organo.x4ever.ViewModels.Account
{
    public class AccountViewModel : BaseViewModel
    {
        public AccountViewModel(INavigation navigation = null) : base(navigation)
        {
            FirstName = string.Empty;
            LastName = string.Empty;
        }

        public async Task LoadAppLanguages(Action action)
        {
            ApplicationLanguages = await DependencyService.Get<IApplicationLanguageService>().GetWithCountryAsync();
            foreach (var language in ApplicationLanguages)
            {
                if (language.LanguageCode == App.Configuration.AppConfig.DefaultLanguage)
                    language.IsSelected = true;
                else
                    language.IsSelected = false;
            }

            action?.Invoke();
        }

        public async void OnLanguageSelected(ApplicationLanguage selectedLanguage)
        {
            var requestModel = new ApplicationLanguageRequest()
            {
                LanguageCode = selectedLanguage.LanguageCode,
                LanguageName = selectedLanguage.LanguageName
            };
            //DisplayCountryLanguage
            var response = await DependencyService.Get<IUserSettingService>().UpdateUserLanguageAsync(requestModel);
            if (response == HttpConstants.SUCCESS)
            {
                await App.Configuration.SetUserLanguage(requestModel.LanguageCode);
                App.GoToAccountPage();
            }
        }
        
        public List<ApplicationLanguage> ApplicationLanguages { get; set; }

        private string firstName;
        public const string FirstNamePropertyName = "FirstName";

        public string FirstName
        {
            get { return firstName; }
            set { SetProperty(ref firstName, value, FirstNamePropertyName); }
        }

        private string lastName;
        public const string LastNamePropertyName = "LastName";

        public string LastName
        {
            get { return lastName; }
            set { SetProperty(ref lastName, value, LastNamePropertyName); }
        }
    }
}