using com.organo.xchallenge.Globals;
using com.organo.xchallenge.Localization;
using com.organo.xchallenge.ViewModels.Base;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using com.organo.xchallenge.Models;
using com.organo.xchallenge.Services;
using Xamarin.Forms;

namespace com.organo.xchallenge.ViewModels.Account
{
    public class AddressViewModel : BaseViewModel
    {
        public AddressViewModel(INavigation navigation = null) : base(navigation)
        {
            this.CountryName = string.Empty;
            this.CityName = string.Empty;
            this.StateName = string.Empty;
            this.Address = string.Empty;
            this.PostalCode = string.Empty;
            this.CountryList = new List<string>();
            this.StateList = new List<string>();
        }

        public async Task<List<string>> GetCountryList()
        {
            CountryProvinces = await DependencyService.Get<IProvinceServices>().GetAsync();
            CountryList = CountryProvinces.Select(c => (string)c.CountryName).Distinct().ToList();
            return CountryList;
        }

        public List<string> GetStateList(string country)
        {
            var provinces = CountryProvinces.FirstOrDefault(p => p.CountryName == country)?.Provinces;
            StateList = provinces?.Select(p => (string) p.ProvinceName).Distinct().ToList();
            return StateList;
        }

        private List<CountryProvince> CountryProvinces { get; set; }

        private string countryName;
        public const string CountryNamePropertyName = "CountryName";

        public string CountryName
        {
            get { return countryName; }
            set { SetProperty(ref countryName, value, CountryNamePropertyName); }
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
    }
}