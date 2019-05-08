using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using com.organo.xchallenge.Localization;
using com.organo.xchallenge.Models;
using com.organo.xchallenge.Services;
using com.organo.xchallenge.Statics;
using com.organo.xchallenge.ViewModels.Base;
using Xamarin.Forms;

namespace com.organo.xchallenge.ViewModels.UserSettings
{
    public class UserSettingsViewModel : BaseViewModel
    {
        private readonly IUserSettingService _settingService;

        public UserSettingsViewModel(INavigation navigation = null) : base(navigation)
        {
            _settingService = DependencyService.Get<IUserSettingService>();

            ApplicationLanguages = new List<ApplicationLanguage>();
            SettingLanguageText = TextResources.Language;
            SettingWeightVolumeText = TextResources.WeightVolumeType;

            WeightVolumeClick_Action = null;
        }

        public async Task LoadAppLanguages(Action action)
        {
            ApplicationLanguages = await DependencyService.Get<IApplicationLanguageService>().GetWithCountryAsync();

            ApplicationLanguages = ApplicationLanguages.Select(l =>
            {
                l.IsSelected = l.LanguageCode == App.Configuration.AppConfig.DefaultLanguage;
                return l;
            }).ToList();

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
            var response = await _settingService.UpdateUserLanguageAsync(requestModel);
            if (response == HttpConstants.SUCCESS)
            {
                await App.Configuration.SetUserLanguageAsync(requestModel.LanguageCode);
                App.GoToAccountPage(true);
            }
        }

        public async Task LoadWeightVolume(Action action)
        {
            WeightVolumeData = await DependencyService.Get<IWeightVolumeService>().GetAsync();
            if (WeightVolumeData.Count > 0)
            {
                var defaultVolume = App.Configuration.AppConfig.DefaultWeightVolume;
                WeightVolumeDataSelected =
                    WeightVolumeData.FirstOrDefault(w => w.VolumeCode.ToLower() == defaultVolume.ToLower());
            }

            action?.Invoke();
        }

        public void OnWeightVolumeChange(WeightVolume weightVolume)
        {
            WeightVolumeDataSelected = weightVolume;
            SaveWeightVolume(weightVolume.VolumeCode);
        }

        private async void SaveWeightVolume(string weightVolumeType)
        {
            var requestModel = new UserWeightVolume()
            {
                WeightVolume = weightVolumeType
            };
            var response = await _settingService.UpdateUserWeightVolumeAsync(requestModel);
            if (response == HttpConstants.SUCCESS)
            {
                await App.Configuration.SetWeightVolumeAsync(requestModel.WeightVolume);
                App.GoToAccountPage(true);
            }
        }

        private void OnWeightVolumeSelected()
        {
            WeightVolumeSelected = WeightVolumeDataSelected.DisplayVolume;
        }

        public List<ApplicationLanguage> ApplicationLanguages { get; set; }

        private string _settingLanguageText;
        public const string SettingLanguageTextPropertyName = "SettingLanguageText";

        public string SettingLanguageText
        {
            get { return _settingLanguageText; }
            set { SetProperty(ref _settingLanguageText, value, SettingLanguageTextPropertyName); }
        }

        private string _settingWeightVolumeText;
        public const string SettingWeightVolumeTextPropertyName = "SettingWeightVolumeText";

        public string SettingWeightVolumeText
        {
            get { return _settingWeightVolumeText; }
            set { SetProperty(ref _settingWeightVolumeText, value, SettingWeightVolumeTextPropertyName); }
        }

        private string _weightVolumeSelected;
        public const string WeightVolumeSelectedPropertyName = "WeightVolumeSelected";

        public string WeightVolumeSelected
        {
            get { return _weightVolumeSelected; }
            set { SetProperty(ref _weightVolumeSelected, value, WeightVolumeSelectedPropertyName); }
        }

        private List<WeightVolume> _weightVolumeData;
        public const string WeightVolumeDataPropertyName = "WeightVolumeData";

        public List<WeightVolume> WeightVolumeData
        {
            get { return _weightVolumeData; }
            set { SetProperty(ref _weightVolumeData, value, WeightVolumeDataPropertyName); }
        }

        private WeightVolume _weightVolumeDataSelected;
        public const string WeightVolumeDataSelectedPropertyName = "WeightVolumeDataSelected";

        public WeightVolume WeightVolumeDataSelected
        {
            get { return _weightVolumeDataSelected; }
            set
            {
                SetProperty(ref _weightVolumeDataSelected, value, WeightVolumeDataSelectedPropertyName,
                    OnWeightVolumeSelected);
            }
        }

        public Action WeightVolumeClick_Action;
        private ICommand _volumeSelectedCommand;

        public ICommand VolumeSelectedCommand
        {
            get
            {
                return _volumeSelectedCommand ?? (_volumeSelectedCommand = new Command(
                           (obj) => { WeightVolumeClick_Action(); }));
            }
        }
    }
}