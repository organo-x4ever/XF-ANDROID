using com.organo.xchallenge.Extensions;
using com.organo.xchallenge.Localization;
using com.organo.xchallenge.Models;
using com.organo.xchallenge.Models.Validation;
using com.organo.xchallenge.Pages.Base;
using com.organo.xchallenge.Services;
using com.organo.xchallenge.Statics;
using com.organo.xchallenge.ViewModels.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using com.organo.xchallenge.Globals;
using com.organo.xchallenge.Models.User;
using Xamarin.Forms;

namespace com.organo.xchallenge.Pages.Account
{
    public partial class AddressPage : AddressPageXaml
    {
        private AddressViewModel _model;
        private UserFirstUpdate _user;
        private IMetaPivotService _metaPivotService;
        private readonly IHelper _helper;

        public AddressPage(UserFirstUpdate user)
        {
            InitializeComponent();
            App.Configuration.InitialAsync(this);
            NavigationPage.SetHasNavigationBar(this, false);
            _model = new AddressViewModel();
            _helper = DependencyService.Get<IHelper>();
            _user = user;
            BindingContext = _model;
            Initialization();
            _metaPivotService = DependencyService.Get<IMetaPivotService>();
        }

        private async void Initialization()
        {
            try
            {
				await _model.GetCountryList();
                if (_user.UserMetas != null && _user.UserMetas.Count > 0)
                {
                    _model.CountryName = _user.UserMetas.ToList().Get(MetaEnum.country);
                    _model.Address = _user.UserMetas.ToList().Get(MetaEnum.address);
                    _model.CityName = _user.UserMetas.ToList().Get(MetaEnum.city);
                    _model.PostalCode = _user.UserMetas.ToList().Get(MetaEnum.postalcode);
                    if (_model.CountryName != null && _model.CountryName.Trim().Length > 0)
                        _model.GetStateList(_model.CountryName);
                    _model.StateName = _user.UserMetas.ToList().Get(MetaEnum.state);
                }

                pickerCountry.ItemsSource = _model.CountryList;
                entryCountry.Focused += (sender, e) =>
                {
                    entryCountry.Unfocus();
                    pickerCountry.Focus();
			
                    pickerCountry.SelectedIndexChanged += (sender1, e1) =>
                    {
                        var countrySelected = pickerCountry.SelectedItem;
                        if (countrySelected != null)
                        {
                            _model.CountryName = countrySelected.ToString();
                            _model.GetStateList(_model.CountryName);
                            entryAddress.Focus();
                        }
                    };
				};
                StateSetup();
                buttonNext.Clicked += async (sender, e) => { await NextStepAsync(); };
            }
            catch
            {
                //
            }
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
            var _metas = new List<Meta>();

            _model.SetActivityResource(false, true, busyMessage: TextResources.ProcessingPleaseWait);
            if (Validate())
            {
                _metas.Add(_metaPivotService.AddMeta(_model.CountryName, MetaConstants.COUNTRY, MetaConstants.COUNTRY, MetaConstants.LABEL));
                _metas.Add(_metaPivotService.AddMeta(_model.Address, MetaConstants.ADDRESS, MetaConstants.ADDRESS, MetaConstants.LABEL));
                _metas.Add(_metaPivotService.AddMeta(_model.CityName, MetaConstants.CITY, MetaConstants.CITY, MetaConstants.LABEL));
                _metas.Add(_metaPivotService.AddMeta(_model.StateName, MetaConstants.STATE, MetaConstants.STATE, MetaConstants.LABEL));
                _metas.Add(_metaPivotService.AddMeta(_model.PostalCode, MetaConstants.POSTAL_CODE, MetaConstants.POSTAL_CODE, MetaConstants.LABEL));

                foreach (var meta in _metas)
                {
                    _user.UserMetas.Add(meta);
                }

                if (await _metaPivotService.SaveMetaStep2Async(_metas))
                    App.CurrentApp.MainPage = new UploadPhotoPage(_user);
                else
                    _model.SetActivityResource(showError: true,
                        errorMessage: _helper.ReturnMessage(_metaPivotService.Message));
            }
        }

        private bool Validate()
        {
            ValidationErrors validationErrors = new ValidationErrors();
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
            App.LogoutAsync().GetAwaiter();
            App.GoToAccountPage();
            return true;
        }
    }

    public abstract class AddressPageXaml : ModelBoundContentPage<AddressViewModel>
    {
    }
}