
using com.organo.xchallenge.Models;
using com.organo.xchallenge.Models.User;
using com.organo.xchallenge.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace com.organo.xchallenge.ViewModels.Profile
{
    public class ProfileSettingViewModel : Base.BaseViewModel
    {
        private readonly IUserSettingService _settingService;

        public ProfileSettingViewModel(INavigation navigation = null) : base(navigation)
        {
            _settingService = DependencyService.Get<IUserSettingService>();
            ProfileLoadingComplete = false;
            CountryProvinces = new List<CountryProvince>();
            CountryList = new List<string>();
            StateList = new List<string>();
            GetCountryList(async () => { await LoadUserProfile(); });
        }

        private async void GetCountryList(Action action)
        {
            CountryProvinces = await DependencyService.Get<IProvinceServices>().GetAsync();
            CountryList = CountryProvinces.Select(c => (string) c.CountryName).Distinct().ToList();
            action?.Invoke();
        }

        private void GetStateList()
        {
            if (!string.IsNullOrEmpty(CountryName))
            {
                var provinces = CountryProvinces.FirstOrDefault(p => p.CountryName == CountryName)?.Provinces;
                StateList = provinces?.Select(p => (string) p.ProvinceName).Distinct().ToList();
            }
        }

        private List<CountryProvince> CountryProvinces { get; set; }

        private async Task LoadUserProfile()
        {
            ProfileLoadingComplete = false;

            CountryName = string.Empty;
            CityName = string.Empty;
            StateName = string.Empty;
            Address = string.Empty;
            PostalCode = string.Empty;
            UserEmail = App.CurrentUser.UserInfo.UserEmail;

            if (UserMeta == null)
                UserMeta = await DependencyService.Get<IMetaPivotService>().GetMetaAsync();
            if (UserMeta != null)
            {
                Address = UserMeta.Address;
                CountryName = UserMeta.Country;
                CityName = UserMeta.City;
                PostalCode = UserMeta.PostalCode;
                await Task.Delay(TimeSpan.FromMilliseconds(100));
                StateName = UserMeta.State;
                ProfileLoadingComplete = true;
            }
        }

        public MetaPivot UserMeta { get; set; }

        #region Edit Profile Properties

        private string userEmail;
        public const string UserEmailPropertyName = "UserEmail";

        public string UserEmail
        {
            get { return userEmail; }
            set { SetProperty(ref userEmail, value, UserEmailPropertyName); }
        }

        private string countryName;
        public const string CountryNamePropertyName = "CountryName";

        public string CountryName
        {
            get { return countryName; }
            set { SetProperty(ref countryName, value, CountryNamePropertyName, GetStateList); }
        }

        private string cityName;
        public const string CityNamePropertyName = "CityName";

        public string CityName
        {
            get { return cityName; }
            set { SetProperty(ref cityName, value, CityNamePropertyName); }
        }

        private string stateName;
        public const string StateNamePropertyName = "StateName";

        public string StateName
        {
            get { return stateName; }
            set { SetProperty(ref stateName, value, StateNamePropertyName); }
        }

        private string address;
        public const string AddressPropertyName = "Address";

        public string Address
        {
            get { return address; }
            set { SetProperty(ref address, value, AddressPropertyName); }
        }

        private string postalCode;
        public const string PostalCodePropertyName = "PostalCode";

        public string PostalCode
        {
            get { return postalCode; }
            set { SetProperty(ref postalCode, value, PostalCodePropertyName); }
        }

        private List<string> countryList;
        public const string CountryListPropertyName = "CountryList";

        public List<string> CountryList
        {
            get { return countryList; }
            set { SetProperty(ref countryList, value, CountryListPropertyName); }
        }

        private List<string> stateList;
        public const string StateListPropertyName = "StateList";

        public List<string> StateList
        {
            get { return stateList; }
            set { SetProperty(ref stateList, value, StateListPropertyName); }
        }

        private bool _profileLoadingComplete;
        public const string ProfileLoadingCompletePropertyName = "ProfileLoadingComplete";

        public bool ProfileLoadingComplete
        {
            get { return _profileLoadingComplete; }
            set { SetProperty(ref _profileLoadingComplete, value, ProfileLoadingCompletePropertyName); }
        }

        #endregion Edit Profile Properties
    }
}