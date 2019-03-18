using com.organo.x4ever.Extensions;
using com.organo.x4ever.Localization;
using com.organo.x4ever.Models;
using com.organo.x4ever.Models.Validation;
using com.organo.x4ever.Pages.Base;
using com.organo.x4ever.Services;
using com.organo.x4ever.Statics;
using com.organo.x4ever.ViewModels.Account;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace com.organo.x4ever.Pages.Account
{
    public partial class AddressPage : AddressPageXaml
    {
        private AddressViewModel _model;
        private UserFirstUpdate _user;
        private IMetaService metaService;
        private ITrackerService trackerService;

        public AddressPage(UserFirstUpdate user)
        {
            InitializeComponent();
            App.Configuration.InitialAsync(this);
            NavigationPage.SetHasNavigationBar(this, false);
            _model = new AddressViewModel();
            _user = user;
            BindingContext = _model;
            Initialization();
            metaService = DependencyService.Get<IMetaService>();
            trackerService = DependencyService.Get<ITrackerService>();
        }

        private async void Initialization()
        {
            if (_user.UserMetas != null && _user.UserMetas.Count > 0)
            {
                _model.CountryName = await _user.UserMetas.ToList().Get(MetaEnum.country);
                _model.Address = await _user.UserMetas.ToList().Get(MetaEnum.address);
                _model.CityName = await _user.UserMetas.ToList().Get(MetaEnum.city);
                _model.StateName = await _user.UserMetas.ToList().Get(MetaEnum.state);
                _model.PostalCode = await _user.UserMetas.ToList().Get(MetaEnum.postalcode);
                if (_model.CountryName != null && _model.CountryName.Trim().Length > 0)
                    await _model.GetStateList(_model.CountryName);
            }

            pickerCountry.ItemsSource = await _model.GetCountryList();
            entryCountry.Focused += (sender, e) =>
            {
                entryCountry.Unfocus();
                pickerCountry.Focus();
                pickerCountry.SelectedIndexChanged += async (sender1, e1) =>
                {
                    var countrySelected = pickerCountry.SelectedItem;
                    if (countrySelected != null)
                    {
                        _model.CountryName = countrySelected.ToString();
                        await _model.GetStateList(_model.CountryName);
                        entryAddress.Focus();
                    }
                };
            };
            StateSetup();
            buttonNext.Clicked += async (sender, e) => { await NextStepAsync(); };
        }

        private void StateSetup()
        {
            entryState.Focused += (sender, e) =>
            {
                if (_model.StateList != null && _model.StateList.Count == 0)
                {
                    _model.SetActivityResource(showError: true,
                        errorMessage: string.Format(TextResources.Required_MustBeSelected, TextResources.Country));
                    return;
                }

                pickerState.ItemsSource = _model.StateList;
                entryState.Unfocus();
                pickerState.Focus();
                pickerState.SelectedIndexChanged += (sender1, e1) =>
                {
                    var stateSelected = pickerState.SelectedItem;
                    if (stateSelected != null)
                    {
                        _model.StateName = stateSelected.ToString();
                        entryPostalCode.Focus();
                    }
                };
            };
        }

        private async void entry_Completed(object sender, EventArgs e)
        {
            await NextStepAsync();
        }

        private async Task NextStepAsync()
        {
            if (await Validate())
            {
                _user.UserMetas.Add(await metaService.AddMeta(_model.CountryName, MetaConstants.COUNTRY,
                    MetaConstants.COUNTRY, MetaConstants.LABEL));
                _user.UserMetas.Add(await metaService.AddMeta(_model.Address, MetaConstants.ADDRESS,
                    MetaConstants.ADDRESS, MetaConstants.LABEL));
                _user.UserMetas.Add(await metaService.AddMeta(_model.CityName, MetaConstants.CITY, MetaConstants.CITY,
                    MetaConstants.LABEL));
                _user.UserMetas.Add(await metaService.AddMeta(_model.StateName, MetaConstants.STATE,
                    MetaConstants.STATE, MetaConstants.LABEL));
                _user.UserMetas.Add(await metaService.AddMeta(_model.PostalCode, MetaConstants.POSTAL_CODE,
                    MetaConstants.POSTAL_CODE, MetaConstants.LABEL));

                App.CurrentApp.MainPage = new UploadPhotoPage(_user);
            }
        }

        protected async Task<bool> Validate()
        {
            ValidationErrors validationErrors = new ValidationErrors();
            await Task.Run(() =>
            {
                if (_model.CountryName == null || _model.CountryName.Trim().Length == 0)
                {
                    validationErrors.Add(string.Format(TextResources.Required_IsMandatory, TextResources.Country));
                }

                if (_model.Address == null || _model.Address.Trim().Length == 0)
                {
                    validationErrors.Add(string.Format(TextResources.Required_IsMandatory, TextResources.Address));
                }

                if (_model.CityName == null || _model.CityName.Trim().Length == 0)
                {
                    validationErrors.Add(string.Format(TextResources.Required_IsMandatory, TextResources.City));
                }

                if (_model.StateName == null || _model.StateName.Trim().Length == 0)
                {
                    validationErrors.Add(string.Format(TextResources.Required_IsMandatory, TextResources.State));
                }

                if (_model.PostalCode == null || _model.PostalCode.Trim().Length == 0)
                {
                    validationErrors.Add(string.Format(TextResources.Required_IsMandatory, TextResources.PostalCode));
                }
            });
            if (validationErrors.Count() > 0)
            {
                _model.SetActivityResource(showError: true, errorMessage: validationErrors.Count() > 2
                    ? TextResources.Required_AllInputs
                    : validationErrors.Show(CommonConstants.SPACE));
            }

            return validationErrors.Count() == 0;
        }

        protected override bool OnBackButtonPressed()
        {
            return DependencyService.Get<IBackButtonPress>().Redirect(new PersonalInfoPage(_user));
        }
    }

    public abstract class AddressPageXaml : ModelBoundContentPage<AddressViewModel>
    {
    }
}