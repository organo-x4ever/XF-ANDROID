using com.organo.x4ever.Globals;
using com.organo.x4ever.Localization;
using com.organo.x4ever.Models;
using com.organo.x4ever.Pages.Base;
using com.organo.x4ever.Services;
using com.organo.x4ever.ViewModels.Profile;
using System;
using Xamarin.Forms;

namespace com.organo.x4ever.Pages.UserSettings
{
    public partial class UserSettingPage : UserSettingPageXaml
    {
        private SettingsViewModel _model;

        public UserSettingPage(RootPage root, SettingsViewModel model)
        {
            InitializeComponent();
            App.Configuration.InitialAsync(this);
            NavigationPage.SetHasNavigationBar(this, false);
            _model = model;
            _model.Root = root;
            BindingContext = _model;
            _model.CurrentPassword = string.Empty;
            _model.NewPassword = string.Empty;
            _model.ConfirmNewPassword = string.Empty;
            Page_Load();
        }

        private async void Page_Load()
        {
            await _model.LoadAppLanguages(OnLanguageRetrieve);
            await _model.LoadWeightVolume(BindWeightVolume);
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

        private void BindWeightVolume()
        {
            PickerWeightVolume.Title = TextResources.SelectWeightVolumeType;
            PickerWeightVolume.ItemsSource = _model.WeightVolumeData;
            PickerWeightVolume.ItemDisplayBinding = new Binding("DisplayVolume");
            _model.WeightVolumeClick_Action = OnWeightVolumeClicked;
            PickerWeightVolume.SelectedIndexChanged += PickerWeightVolume_SelectedIndexChanged;
        }

        private void OnWeightVolumeClicked()
        {
            if (_model.WeightVolumeData.Count > 1)
            {
                PickerWeightVolume.Focus();
            }
        }

        private void PickerWeightVolume_SelectedIndexChanged(object sender, EventArgs e)
        {
            _model.OnWeightVolumeChange((WeightVolume) PickerWeightVolume.SelectedItem);
        }

        protected override bool OnBackButtonPressed()
        {
            return DependencyService.Get<IBackButtonPress>().Redirect(_model.Root);
        }
    }

    public abstract class UserSettingPageXaml : ModelBoundContentPage<SettingsViewModel>
    {
    }
}