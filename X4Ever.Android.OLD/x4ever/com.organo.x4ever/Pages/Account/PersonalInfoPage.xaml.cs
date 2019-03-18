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
using com.organo.x4ever.Converters;
using Xamarin.Forms;

namespace com.organo.x4ever.Pages.Account
{
    public partial class PersonalInfoPage : PersonalInfoPageXaml
    {
        private PersonalInfoViewModel _model;
        private UserFirstUpdate _user;
        private IMetaService metaService;
        private ITrackerService trackerService;
        private readonly PoundToKiligramConverter _converter = new PoundToKiligramConverter();

        public PersonalInfoPage(UserFirstUpdate user)
        {
            InitializeComponent();
            App.Configuration.InitialAsync(this);
            NavigationPage.SetHasNavigationBar(this, false);
            _model = new PersonalInfoViewModel();
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
                short age = _model.AgeValue;
                double weight = _converter.DisplayWeightVolume(_model.CurrentWeightValue),
                    weightLossGoal = _converter.DisplayWeightVolume(_model.WeightLossGoalValue);
                if (short.TryParse(await _user.UserMetas.ToList().Get(MetaEnum.age), out age))
                    _model.AgeValue = age;

                if (double.TryParse(await _user.UserMetas.ToList().Get(MetaEnum.weighttolose), out weightLossGoal))
                    _model.WeightLossGoalValue = _converter.DisplayWeightVolume(weightLossGoal);

                if (double.TryParse(await _user.UserTrackers.ToList().Get(TrackerEnum.currentweight), out weight))
                    _model.CurrentWeightValue = _converter.DisplayWeightVolume(weight);
            }

            sliderAge.ValueChanged += (sender, e) =>
            {
                if ((short)e.NewValue < App.Configuration.AppConfig.MINIMUM_AGE)
                    sliderAge.Value = _model.AgeValue;
                else
                    _model.AgeValue = (short)e.NewValue;
            };
            sliderCurrentWeight.ValueChanged += (sender, e) =>
            {
                if (e.NewValue <
                    _converter.DisplayWeightVolume(App.Configuration.AppConfig.MINIMUM_CURRENT_WEIGHT))
                    sliderCurrentWeight.Value = _model.CurrentWeightValue;
                else
                {
                    double.TryParse(e.NewValue.ToString("0"), out double v);
                    _model.CurrentWeightValue = v;
                }
            };
            sliderWeightLossGoal.ValueChanged += (sender, e) =>
            {
                if (e.NewValue < _converter.DisplayWeightVolume(App.Configuration.AppConfig.MINIMUM_WEIGHT_LOSE))
                    sliderWeightLossGoal.Value = _model.WeightLossGoalValue;
                else
                {
                    double.TryParse(e.NewValue.ToString("0"), out double v);
                    _model.WeightLossGoalValue = v;
                }
            };
            buttonNext.Clicked += async (sender, e) => { await NextStepAsync(); };

            sliderAge.SetMinValueAsync(App.Configuration.AppConfig.MINIMUM_AGE, delay: 0);
            sliderCurrentWeight.SetMinValueAsync(
                _converter.DisplayWeightVolume(App.Configuration.AppConfig.MINIMUM_CURRENT_WEIGHT), delay: 0);
            sliderWeightLossGoal.SetMinValueAsync(
                _converter.DisplayWeightVolume(App.Configuration.AppConfig.MINIMUM_WEIGHT_LOSE), delay: 0);
        }

        private async Task NextStepAsync()
        {
            if (await Validate())
            {
                _user.UserMetas.Add(await metaService.AddMeta(_model.AgeValue.ToString(), MetaConstants.AGE,
                    MetaConstants.AGE, MetaConstants.LABEL));
                _user.UserTrackers.Add(await trackerService.AddTracker(TrackerConstants.CURRENT_WEIGHT,
                    _model.CurrentWeightValue.ToString()));
                _user.UserMetas.Add(await metaService.AddMeta(_model.WeightLossGoalValue.ToString(),
                    MetaConstants.WEIGHT_LOSS_GOAL, MetaConstants.WEIGHT_LOSS_GOAL, MetaConstants.LABEL));

                App.CurrentApp.MainPage = new AddressPage(_user);
            }
        }

        private async Task<bool> Validate()
        {
            ValidationErrors validationErrors = new ValidationErrors();
            await Task.Run(() =>
            {
                if (_model.AgeValue == 0)
                    validationErrors.Add(string.Format(TextResources.Required_IsMandatory, TextResources.YourAge));
                else if (_model.AgeValue < App.Configuration.AppConfig.MINIMUM_AGE)
                    validationErrors.Add(string.Format(TextResources.Validation_MustBeMoreThan, TextResources.YourAge,
                        App.Configuration.AppConfig.MINIMUM_AGE));

                if (_model.CurrentWeightValue == 0)
                    validationErrors.Add(string.Format(TextResources.Required_IsMandatory,
                        TextResources.YourCurrentWeight));
                else if (_model.CurrentWeightValue <
                         _converter.DisplayWeightVolume(App.Configuration.AppConfig.MINIMUM_CURRENT_WEIGHT))
                    validationErrors.Add(string.Format(TextResources.Validation_MustBeMoreThan,
                        TextResources.YourCurrentWeight,
                        _converter.DisplayWeightVolume(App.Configuration.AppConfig.MINIMUM_CURRENT_WEIGHT)));

                if (_model.WeightLossGoalValue == 0)
                    validationErrors.Add(
                        string.Format(TextResources.Required_IsMandatory, TextResources.WeightLossGoal));
                else if (_model.WeightLossGoalValue <
                         _converter.DisplayWeightVolume(App.Configuration.AppConfig.MINIMUM_WEIGHT_LOSE))
                    validationErrors.Add(string.Format(TextResources.Validation_MustBeMoreThan,
                        TextResources.WeightLossGoal,
                        _converter.DisplayWeightVolume(App.Configuration.AppConfig.MINIMUM_WEIGHT_LOSE)));
            });
            if (validationErrors.Count() > 0)
                _model.SetActivityResource(showError: true,
                    errorMessage: validationErrors.Count() > 2
                        ? TextResources.Required_AllInputs
                        : validationErrors.Show(CommonConstants.SPACE));
            return validationErrors.Count() == 0;
        }

        protected override bool OnBackButtonPressed()
        {
            return DependencyService.Get<IBackButtonPress>().Redirect(new BasicInfoPage(_user));
        }
    }

    public abstract class PersonalInfoPageXaml : ModelBoundContentPage<PersonalInfoViewModel>
    {
    }
}