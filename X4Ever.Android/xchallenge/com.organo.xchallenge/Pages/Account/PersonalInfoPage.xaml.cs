using com.organo.xchallenge.Extensions;
using com.organo.xchallenge.Localization;
using com.organo.xchallenge.Models;
using com.organo.xchallenge.Models.Validation;
using com.organo.xchallenge.Pages.Base;
using com.organo.xchallenge.Services;
using com.organo.xchallenge.Statics;
using com.organo.xchallenge.ViewModels.Account;
using System;
using System.Linq;
using System.Threading.Tasks;
using com.organo.xchallenge.Converters;
using com.organo.xchallenge.Globals;
using Xamarin.Forms;

namespace com.organo.xchallenge.Pages.Account
{
    public partial class PersonalInfoPage : PersonalInfoPageXaml
    {
        private PersonalInfoViewModel _model;
        private readonly UserFirstUpdate _user;
        private IMetaPivotService _metaPivotService;
        private ITrackerPivotService _trackerPivotService;
        private IHelper _helper;
        private readonly PoundToKiligramConverter _converter = new PoundToKiligramConverter();

        public PersonalInfoPage(UserFirstUpdate user)
        {
            InitializeComponent();
            _user = user;
            Init();
        }

        private void Init()
        {
            App.Configuration.InitialAsync(this);
            NavigationPage.SetHasNavigationBar(this, false);
            _model = new PersonalInfoViewModel()
            {
                SliderAgeModel = sliderAge,
                SliderCurrentWeightModel = sliderCurrentWeight,
                SliderWeightLossGoalModel = sliderWeightLossGoal
            };
            _model.SetSliders();
            _helper = DependencyService.Get<IHelper>();
            _metaPivotService = DependencyService.Get<IMetaPivotService>();
            _trackerPivotService = DependencyService.Get<ITrackerPivotService>();
            BindingContext = _model;
            Initialization();
        }

        private void Initialization()
        {
            if (_user?.UserMetas?.Count > 0)
            {
                //var age = _model.AgeValue;
                //var weight = _converter.DisplayWeightVolume(_model.CurrentWeightValue);
                //var weightLossGoal = _converter.DisplayWeightVolume(_model.WeightLossGoalValue);
                if (short.TryParse(_user.UserMetas?.ToList().Get(MetaEnum.age), out short age) && age > 0)
                    _model.AgeValue = age;

                if (double.TryParse(_user.UserMetas?.ToList().Get(MetaEnum.weighttolose),
                        out double weightLossGoal) && weightLossGoal > 0)
                    _model.WeightLossGoalValue = _converter.DisplayWeightVolume(weightLossGoal);
            }

            if (_user?.UserTrackers?.Count > 0)
            {
                if (double.TryParse(_user.UserTrackers?.ToList().Get(TrackerEnum.currentweight),
                        out double weight) && weight > 0)
                    _model.CurrentWeightValue = _converter.DisplayWeightVolume(weight);
            }

            buttonNext.Clicked += async (sender, e) => { await NextStepAsync(); };
        }

        private async Task NextStepAsync()
        {
            _model.SetActivityResource(false, true, busyMessage: TextResources.ProcessingPleaseWait);
            if (Validate())
            {
                _user.UserMetas.Add(_metaPivotService.AddMeta(_model.AgeValue.ToString(), MetaConstants.AGE,
                    MetaConstants.AGE, MetaConstants.LABEL));
                var tracker = _trackerPivotService.AddTracker(TrackerConstants.CURRENT_WEIGHT,
                    _model.CurrentWeightValue.ToString());
                tracker.RevisionNumber = "10000";
                _user.UserTrackers.Add(tracker);

                tracker = _trackerPivotService.AddTracker(TrackerConstants.CURRENT_WEIGHT_UI,
                    _model.CurrentWeightValue.ToString());
                tracker.RevisionNumber = "10000";
                _user.UserTrackers.Add(tracker);

                tracker = _trackerPivotService.AddTracker(TrackerConstants.WEIGHT_VOLUME_TYPE,
                    App.Configuration.AppConfig.DefaultWeightVolume);
                tracker.RevisionNumber = "10000";
                _user.UserTrackers.Add(tracker);

                _user.UserMetas.Add(_metaPivotService.AddMeta(_model.WeightLossGoalValue.ToString(),
                    MetaConstants.WEIGHT_LOSS_GOAL, MetaConstants.WEIGHT_LOSS_GOAL, MetaConstants.LABEL));

                _user.UserMetas.Add(_metaPivotService.AddMeta(_model.WeightLossGoalValue.ToString(),
                    MetaConstants.WEIGHT_LOSS_GOAL_UI, MetaConstants.WEIGHT_LOSS_GOAL_UI, MetaConstants.LABEL));

                _user.UserMetas.Add(_metaPivotService.AddMeta(App.Configuration.AppConfig.DefaultWeightVolume,
                    MetaConstants.WEIGHT_VOLUME_TYPE, MetaConstants.WEIGHT_VOLUME_TYPE, MetaConstants.LABEL));

                var response = await _metaPivotService.SaveMetaStep2Async(_user.UserMetas);
                if (response)
                {
                    var result = await _trackerPivotService.SaveTrackerStep3Async(_user.UserTrackers);
                    if (result)
                        App.CurrentApp.MainPage = new AddressPage(_user);
                    else
                        _model.SetActivityResource(showError: true,
                            errorMessage: _helper.ReturnMessage(_trackerPivotService.Message));
                }
                else
                    _model.SetActivityResource(showError: true,
                        errorMessage: _helper.ReturnMessage(_metaPivotService.Message));
            }
        }

        private bool Validate()
        {
            ValidationErrors validationErrors = new ValidationErrors();
            if (_model.AgeValue == 0)
                validationErrors.Add(string.Format(TextResources.Required_IsMandatory, TextResources.YourAge));
            else if (_model.AgeValue < App.Configuration.AppConfig.MINIMUM_AGE)
                validationErrors.Add(string.Format(TextResources.Validation_MustBeMoreThan, TextResources.YourAge,
                    App.Configuration.AppConfig.MINIMUM_AGE));

            if (_model.CurrentWeightValue == 0)
                validationErrors.Add(string.Format(TextResources.Required_IsMandatory,
                    TextResources.YourCurrentWeight));
            else if (_model.CurrentWeightValue <
                     _converter.DisplayWeightVolume(App.Configuration.AppConfig.MINIMUM_CURRENT_WEIGHT_KG,
                         App.Configuration.AppConfig.MINIMUM_CURRENT_WEIGHT_LB))
                validationErrors.Add(string.Format(TextResources.Validation_MustBeMoreThan,
                    TextResources.YourCurrentWeight,
                    _converter.DisplayWeightVolume(App.Configuration.AppConfig.MINIMUM_CURRENT_WEIGHT_KG,
                        App.Configuration.AppConfig.MINIMUM_CURRENT_WEIGHT_LB)));

            if (_model.WeightLossGoalValue == 0)
                validationErrors.Add(
                    string.Format(TextResources.Required_IsMandatory, TextResources.WeightLossGoal));
            else if (_model.WeightLossGoalValue <
                     _converter.DisplayWeightVolume(App.Configuration.AppConfig.MINIMUM_WEIGHT_LOSE_KG,
                         App.Configuration.AppConfig.MINIMUM_WEIGHT_LOSE_LB))
                validationErrors.Add(string.Format(TextResources.Validation_MustBeMoreThan,
                    TextResources.WeightLossGoal,
                    _converter.DisplayWeightVolume(App.Configuration.AppConfig.MINIMUM_WEIGHT_LOSE_KG,
                        App.Configuration.AppConfig.MINIMUM_WEIGHT_LOSE_LB)));
            if (validationErrors.Count() > 0)
                _model.SetActivityResource(showError: true,
                    errorMessage: validationErrors.Count() > 2
                        ? TextResources.Required_AllInputs
                        : validationErrors.Show(CommonConstants.SPACE));
            return validationErrors.Count() == 0;
        }

        protected override bool OnBackButtonPressed()
        {
            App.LogoutAsync().GetAwaiter();
            App.GoToAccountPage();
            return true;
        }
    }

    public abstract class PersonalInfoPageXaml : ModelBoundContentPage<PersonalInfoViewModel>
    {
    }
}