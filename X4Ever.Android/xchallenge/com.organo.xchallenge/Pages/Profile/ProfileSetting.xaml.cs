using com.organo.xchallenge.Globals;
using com.organo.xchallenge.Localization;
using com.organo.xchallenge.Models.User;
using com.organo.xchallenge.Models.Validation;
using com.organo.xchallenge.Pages.Base;
using com.organo.xchallenge.Services;
using com.organo.xchallenge.Statics;
using com.organo.xchallenge.ViewModels.Profile;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace com.organo.xchallenge.Pages.Profile
{
    public partial class ProfileSetting : ProfileSettingXaml
    {
        private SettingsViewModel _model;
        private IMetaPivotService metaPivotService;

        public ProfileSetting(RootPage root, SettingsViewModel model)
        {
            try
            {
                InitializeComponent();
                App.Configuration.InitialAsync(this);
                NavigationPage.SetHasNavigationBar(this, false);
                _model = model;
                _model.Root = root;
                BindingContext = _model;
                _model.SetActivityResource();
                _model.CurrentPassword = string.Empty;
                _model.NewPassword = string.Empty;
                _model.ConfirmNewPassword = string.Empty;
                LoadForm();
                metaPivotService = DependencyService.Get<IMetaPivotService>();
            }
            catch (Exception ex)
            {
                DependencyService.Get<IMessage>().AlertAsync(TextResources.Alert,
                    ex.InnerException != null ? ex.InnerException.Message : ex.Message, TextResources.Ok);
            }
        }

        private async void LoadForm()
        {
            entryCountry.Focused += (sender, e) =>
            {
                entryCountry.Unfocus();
                if (pickerCountry.ItemsSource == null || pickerCountry.ItemsSource.Count == 0)
                {
                    pickerCountry.Unfocus();
                    DependencyService.Get<IInformationMessageServices>().LongAlert(TextResources.NoRecordToProcess);
                }
                else
                    pickerCountry.Focus();
            };
            pickerCountry.SelectedIndexChanged += (sender1, e1) =>
            {
                if (pickerCountry.SelectedItem != null)
                    entryAddress.Focus();
            };

            entryState.Focused += (sender, e) =>
            {
                if (_model.StateList != null && _model.StateList.Count == 0)
                {
                    DependencyService.Get<IInformationMessageServices>()
                        .LongAlert(string.Format(TextResources.Required_MustBeSelected, TextResources.Country));
                    return;
                }

                entryState.Unfocus();
                pickerState.Focus();
            };
            pickerState.SelectedIndexChanged += (sender1, e1) =>
            {
                if (pickerState.SelectedItem != null)
                    entryPostalCode.Focus();
            };

            await Task.Run(() => { buttonSubmit.Clicked += async (sender, e) => { await UpdateProfileAsync(); }; });
        }

        private async Task UpdateProfileAsync()
        {
            await Task.Run(() => { _model.SetActivityResource(false, true); });
            if (await Validate())
            {
                List<Meta> metaList = new List<Meta>();
                metaList.Add(await metaPivotService.AddMeta(_model.CountryName, MetaConstants.COUNTRY, MetaConstants.COUNTRY,
                    MetaConstants.LABEL));
                metaList.Add(await metaPivotService.AddMeta(_model.Address, MetaConstants.ADDRESS, MetaConstants.ADDRESS,
                    MetaConstants.LABEL));
                metaList.Add(await metaPivotService.AddMeta(_model.CityName, MetaConstants.CITY, MetaConstants.CITY,
                    MetaConstants.LABEL));
                metaList.Add(await metaPivotService.AddMeta(_model.StateName, MetaConstants.STATE, MetaConstants.STATE,
                    MetaConstants.LABEL));
                metaList.Add(await metaPivotService.AddMeta(_model.PostalCode, MetaConstants.POSTAL_CODE,
                    MetaConstants.POSTAL_CODE, MetaConstants.LABEL));
                var response = await metaPivotService.SaveMetaAsync(metaList);
                _model.SetActivityResource();
                if (response == HttpConstants.SUCCESS)
                    _model.UserMeta = null;
                _model.SetActivityResource(showError: true,
                    errorMessage: response == HttpConstants.SUCCESS
                        ? TextResources.MessageUserDetailSaveSuccessful
                        : TextResources.MessageSomethingWentWrong);
            }
        }

        private async Task<bool> Validate()
        {
            ValidationErrors validationErrors = new ValidationErrors();
            await Task.Run(() =>
            {
                if (_model.CountryName == null || _model.CountryName.Trim().Length == 0)
                    validationErrors.Add(string.Format(TextResources.Required_IsMandatory, TextResources.Country));
                if (_model.Address == null || _model.Address.Trim().Length == 0)
                    validationErrors.Add(string.Format(TextResources.Required_IsMandatory, TextResources.Address));
                if (_model.CityName == null || _model.CityName.Trim().Length == 0)
                    validationErrors.Add(string.Format(TextResources.Required_IsMandatory, TextResources.City));
                if (_model.StateName == null || _model.StateName.Trim().Length == 0)
                    validationErrors.Add(string.Format(TextResources.Required_IsMandatory, TextResources.State));
                if (_model.PostalCode == null || _model.PostalCode.Trim().Length == 0)
                    validationErrors.Add(string.Format(TextResources.Required_IsMandatory, TextResources.PostalCode));
            });
            if (validationErrors.Count() > 0)
                _model.SetActivityResource(showError: true,
                    errorMessage: validationErrors.Count() > 2
                        ? TextResources.Required_AllInputs
                        : validationErrors.Show(CommonConstants.SPACE));
            return validationErrors.Count() == 0;
        }

        private async void entry_Completed(object sender, EventArgs e)
        {
            await UpdateProfileAsync();
        }

        protected override bool OnBackButtonPressed()
        {
            return DependencyService.Get<IBackButtonPress>().Redirect(_model.Root);
        }
    }

    public abstract class ProfileSettingXaml : ModelBoundContentPage<SettingsViewModel>
    {
    }
}