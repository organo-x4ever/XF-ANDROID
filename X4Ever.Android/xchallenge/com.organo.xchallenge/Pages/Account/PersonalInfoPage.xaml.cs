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
using com.organo.xchallenge.Handler;

namespace com.organo.xchallenge.Pages.Account
{
    public partial class PersonalInfoPage : PersonalInfoPageXaml
    {
        private PersonalInfoViewModel _model;
        private readonly UserFirstUpdate _user;
        private readonly IMetaPivotService _metaPivotService;
        private readonly ITrackerPivotService _trackerPivotService;
        private readonly IHelper _helper;
        private readonly PoundToKiligramConverter _converter = new PoundToKiligramConverter();
        private const string revisionNumber = "10000";
        private bool IsRevised { get; set; }

        public PersonalInfoPage(UserFirstUpdate user)
        {
            try
            {
                InitializeComponent();
                _user = user;
                _helper = DependencyService.Get<IHelper>();
                _metaPivotService = DependencyService.Get<IMetaPivotService>();
                _trackerPivotService = DependencyService.Get<ITrackerPivotService>();
                IsRevised = false;
                Init();
            }
            catch (Exception ex)
            {
                new ExceptionHandler(TAG, ex);
            }
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
            BindingContext = _model;

            if (_user?.UserMetas?.Count > 0)
            {
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
            _model.GetWeightLoseWarning();
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
                tracker.RevisionNumber = revisionNumber;
                _user.UserTrackers.Add(tracker);

                tracker = _trackerPivotService.AddTracker(TrackerConstants.CURRENT_WEIGHT_UI,
                    _model.CurrentWeightValue.ToString());
                tracker.RevisionNumber = revisionNumber;
                _user.UserTrackers.Add(tracker);

                tracker = _trackerPivotService.AddTracker(TrackerConstants.WEIGHT_VOLUME_TYPE,
                    App.Configuration.AppConfig.DefaultWeightVolume);
                tracker.RevisionNumber = revisionNumber;
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
            _model.NextButtonText = TextResources.Next;
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
            else if(_model.WeightLossGoalValue>_model.CurrentWeightValue)
                validationErrors.Add(string.Format(TextResources.Validation_MustBeLessThan, TextResources.WeightLossGoal,TextResources.YourCurrentWeight));

            _model.ReviseHeaderText = _model.ReviseRequestText = string.Empty;
            labelReviseRequest.FormattedText = new FormattedString();
            if (!IsRevised && (_model.WeightLossGoalValue > ((_model.CurrentWeightValue / 100) * _model.WeightLoseWarningPercent)))
            {
                _model.ReviseRequestText = string.Format(TextResources.ReviseWeightText, (int)((_model.WeightLossGoalValue * 100) / _model.CurrentWeightValue));
                _model.ReviseHeaderText = TextResources.Warning;
                validationErrors.Add(_model.ReviseRequestText);
                _model.NextButtonText = TextResources.Yes + ", " + TextResources.Continue;

                try {
                    labelReviseRequest.FormattedText = new FormattedString() {
                        Spans = {
                            new Span() {
                                Text=_model.ReviseHeaderText,
                                ForegroundColor = Palette._Error,
                                FontAttributes = FontAttributes.Bold,
                            },
                            new Span() {
                                Text=": ",
                                ForegroundColor = Palette._Error,
                                FontAttributes = FontAttributes.Bold,
                            },
                            new Span() {
                                Text=_model.ReviseRequestText,
                                ForegroundColor = Palette._White,
                                FontAttributes = FontAttributes.None,
                            },
                        }
                    };
                }
                catch(Exception ex)
                {
                    new ExceptionHandler(TAG, ex);
                }
            }
            IsRevised = true;

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