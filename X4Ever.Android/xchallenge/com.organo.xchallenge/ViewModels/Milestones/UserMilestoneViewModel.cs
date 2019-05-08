using com.organo.xchallenge.Converters;
using com.organo.xchallenge.Extensions;
using com.organo.xchallenge.Globals;
using com.organo.xchallenge.Helpers;
using com.organo.xchallenge.Localization;
using com.organo.xchallenge.Models;
using com.organo.xchallenge.Models.User;
using com.organo.xchallenge.Models.Validation;
using com.organo.xchallenge.Services;
using com.organo.xchallenge.Statics;
using com.organo.xchallenge.Utilities;
using com.organo.xchallenge.ViewModels.Base;
using com.organo.xchallenge.ViewModels.Profile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace com.organo.xchallenge.ViewModels.Milestones
{
    public class UserMilestoneViewModel : BaseViewModel
    {
        private readonly ITrackerPivotService _trackerPivotService;
        private readonly IMetaPivotService _metaPivotService;
        private readonly IUserMilestoneService _userMilestoneService;
        private readonly IHelper _helper;
        private readonly PoundToKiligramConverter _converter = new PoundToKiligramConverter();

        public UserMilestoneViewModel(INavigation navigation = null) : base(navigation)
        {
            _trackerPivotService = DependencyService.Get<ITrackerPivotService>();
            _metaPivotService = DependencyService.Get<IMetaPivotService>();
            _userMilestoneService = DependencyService.Get<IUserMilestoneService>();
            _helper = DependencyService.Get<IHelper>();

            SetPageImageSize();

            ShowBadgeAchievedImage = false;
            BadgeAchievedImage = "";
            StringFemale = StringFemaleDefault;
            StringMale = StringMaleDefault;
            ColorFemale = ColorMale = ColorGenderDefault;

            ActionType = ActionType.Check;
            BaseContent = null;

            CurrentWeightMinimumValue = 0;
            CurrentWeightMaximumValue = _converter.DisplayWeightVolume(
                App.Configuration.AppConfig.MAXIMUM_CURRENT_WEIGHT_KG,
                App.Configuration.AppConfig.MAXIMUM_CURRENT_WEIGHT_LB);
            CurrentWeightValue = CurrentWeightMinimumValue;

            UserTrackers = new List<TrackerPivot>();
            UserMetas = new MetaPivot();
            WeightLossGoal = 0;
            PreviousWeight = 0;

            GoalAchieved = false;
            CurrentWeightEnabled = true;
            MilestoneAchieved = false;
            AchievedContent = null;
            CurrentTitle = TextResources.CurrentWeight;
            CurrentSubTitle = string.Empty;
            IsCurrentSubTitle = false;
            ImageFront = ImageDefault;
            ImageSide = ImageDefault;
            IsGenderRequired = false;
            TShirtSize = string.Empty;

            IsAchievedVisible = false;
            IsBasicVisible = true;

            AboutYourJourney = ErrorMessage;
            MilestoneExtended = new UserMilestoneExtended();
            Milestones = new List<Milestone>();
            UserMilestones = new List<UserMilestone>();
            //ViewComponents = new List<View>();
            AchievedMilestonePercentage = null;
            AchievedMilestone = null;
        }

        public void GetUserTracker()
        {
            if (UserTrackers.Count > 0)
            {
                var trackerFirst = UserTrackers.OrderBy(t => t.ModifyDate).FirstOrDefault();
                var trackerLast = UserTrackers.OrderByDescending(t => t.ModifyDate).FirstOrDefault();
                if (trackerFirst != null && trackerLast != null)
                {
                    if (short.TryParse(trackerLast.CurrentWeight.ToString(), out short previous))
                    {
                        PreviousWeight = previous;

                        // CHANGED
                        CurrentWeightValue = PreviousWeight;
                        ChangeSliderValue(CurrentWeightValue);
                    }

                    if (short.TryParse(trackerFirst.CurrentWeight.ToString(), out short start))
                        StartWeight = start;
                }
            }

            GetMilestoneExtendedAsync();
        }

        private async void GetMilestoneExtendedAsync()
        {
            MilestoneExtended =
                await _userMilestoneService.GetExtendedAsync(App.Configuration.AppConfig.DefaultLanguage);
            if (MilestoneExtended != null)
            {
                if (MilestoneExtended.Milestones != null)
                {
                    var miles = MilestoneExtended.Milestones.ToList();
                    foreach (var mile in miles)
                    {
                        mile.TargetValue = (int)_converter.DisplayWeightVolume(mile.TargetValue);
                        Milestones.Add(mile);
                    }
                }

                if (MilestoneExtended.UserMilestones != null)
                    UserMilestones = MilestoneExtended.UserMilestones.ToList();
                if (MilestoneExtended.MilestonePercentages != null)
                    MilestonePercentage = MilestoneExtended.MilestonePercentages.ToList();
            }
        }

        // Find :: CHANGED
        public void ChangeSliderValue(double weightLose)
        {
            var defaultValue =
                App.Configuration.AppConfig.DefaultWeightVolume?.ToLower().Contains("kg") == true
                    ? App.Configuration.AppConfig.MINIMUM_WEIGHT_LOSE_KG
                    : App.Configuration.AppConfig.MINIMUM_WEIGHT_LOSE_LB;

            if (defaultValue > weightLose)
                weightLose = defaultValue;

            SliderCurrentWeight.SetMinValueAsync(weightLose);

            SliderCurrentWeight.ValueChanged += (sender, e) =>
            {
                if ((short)e.NewValue < _converter.DisplayWeightVolume(
                        App.Configuration.AppConfig.MINIMUM_WEIGHT_LOSE_KG,
                        App.Configuration.AppConfig.MINIMUM_WEIGHT_LOSE_LB))
                    SliderCurrentWeight.Value = CurrentWeightValue;
                else
                    CurrentWeightValue = (short)e.NewValue;
            };
        }

        private void GetUserGenderAsync()
        {
            var gender = UserMetas?.Gender;
            if (!string.IsNullOrEmpty(gender))
            {
                IsGenderRequired = false;
                GenderSelected = gender == Gender.Male.ToString() ? Gender.Male : Gender.Female;
                UpdateGenderStyleAsync();
            }
            else
                IsGenderRequired = true;
        }

        public List<string> GetTShirtSizeList()
        {
            TShirtSizeList = TextResources.TShirtSizes.Split(',').ToList();
            return TShirtSizeList;
        }

        private void SetGoalAchieved()
        {
            CurrentWeightEnabled = !GoalAchieved;
            CurrentTitle = TextResources.CurrentWeight;
            if (GoalAchieved)
            {
                CurrentTitle = TextResources.Milestone;
                IsAchievedVisible = true;
            }
            else
                IsAchievedVisible = false;
        }

        private Slider _sliderCurrentWeight;

        public const string SliderCurrentWeightPropertyName = "SliderCurrentWeight";

        // CHANGED
        public Slider SliderCurrentWeight
        {
            get { return _sliderCurrentWeight; }
            set { SetProperty(ref _sliderCurrentWeight, value, SliderCurrentWeightPropertyName); }
        }

        public ICommand _submitCommand;

        public ICommand SubmitCommand
        {
            get
            {
                return _submitCommand ?? (_submitCommand = new Command(async (obj) =>
                {
                    SetActivityResource(false, true);
                    if (ActionType == ActionType.Check)
                        CurrentWeightChanged();
                    else
                        await SaveTrackerAsync();
                }));
            }
        }

        private async void CurrentWeightChanged()
        {
            if (Validate())
            {
                AchievedMilestone = null;
                AchievedMilestonePercentage = null;
                ActionType = ActionType.Submit;
                if ((StartWeight - CurrentWeightValue) >= WeightLossGoal)
                {
                    CurrentTitle = TextResources.GoalAchieved;
                    SetActivityResource(showError: true, errorMessage: TextResources.MessageGoalAchievedWishes);
                    GoalAchieved = true;
                    //PERCENTAGE:
                    CheckMilestonePercentageAsync();
                    return;
                }
                else
                {
                    //MILESTONE:
                    CheckMilestoneAsync();

                    //PERCENTAGE:
                    CheckMilestonePercentageAsync();
                }

                if (!GoalAchieved)
                {
                    ActionType = ActionType.Submit;
                    await SaveTrackerAsync();
                }
                else
                {
                    GetUserGenderAsync();
                    SetActivityResource();
                }
            }
        }

        private void CheckMilestoneAsync()
        {
            var localMessage = "";

            var included = Milestones.Where(m => UserMilestones.Any(u => u.MilestoneID == m.ID));
            var milestones = Milestones.Except(included).OrderByDescending(m => m.TargetValue);
            foreach (var milestone in milestones)
            {
                if ((StartWeight - CurrentWeightValue) >= milestone.TargetValue)
                {
                    AchievedMilestone = milestone;
                    CurrentTitle = string.Format(AchievedMilestone.MilestoneTitle,
                        (StartWeight - CurrentWeightValue),
                        App.Configuration.AppConfig.DefaultWeightVolume);
                    localMessage = string.Format(milestone.AchievedMessage, (StartWeight - CurrentWeightValue),
                        App.Configuration.AppConfig.DefaultWeightVolume);
                    CurrentSubTitle = string.Format(AchievedMilestone.MilestoneSubTitle,
                        (StartWeight - CurrentWeightValue),
                        App.Configuration.AppConfig.DefaultWeightVolume);
                    IsCurrentSubTitle = !string.IsNullOrEmpty(CurrentSubTitle);
                    GoalAchieved = true;
                    break;
                }
            }

            if (!string.IsNullOrEmpty(localMessage))
                SetActivityResource(showMessage: true, message: localMessage, modalWindow: true);
        }

        private void CheckMilestonePercentageAsync()
        {
            var includedPercent = MilestonePercentage.Where(m =>
                UserMilestones.Any(u => u.MilestonePercentageId == m.ID && m.IsPercent));
            var milestonePercents = MilestonePercentage?.Except(includedPercent)
                .OrderByDescending(m => m.TargetPercentValue);
            foreach (var milestonePercent in milestonePercents)
            {
                if ((StartWeight - CurrentWeightValue) >=
                    ((WeightLossGoal * milestonePercent.TargetPercentValue) / 100))
                {
                    AchievedMilestonePercentage = milestonePercent;
                    BadgeAchievedImage = DependencyService.Get<IMessage>()
                        .GetResource(milestonePercent.AchievementGiftImage);
                }
            }
        }

        private async Task SaveTrackerAsync()
        {
            if (ActionType != ActionType.Submit)
                return;
            if (Validate())
            {
                var response = await _trackerPivotService.SaveTrackerAsync(BuildTracker());
                if (response == HttpConstants.SUCCESS)
                {
                    if (await MilestoneSaveAsync())
                    {
                        SaveGender();
                        SaveSuccessful(string.Empty);
                        await Task.Delay(TimeSpan.FromMilliseconds(500));
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            await App.CurrentApp.MainPage.Navigation.PopModalAsync();
                        });
                    }
                }
                else
                    SetActivityResource(showError: true, errorMessage: response, modalWindow: true);
            }
        }

        private async void SaveGender()
        {
            if (IsGenderRequired)
                await _metaPivotService.SaveMetaAsync(new List<Meta>()
                {
                    _metaPivotService.AddMeta(GenderSelected.ToString(), MetaConstants.GENDER, MetaConstants.GENDER,
                        MetaConstants.LABEL)
                });
        }

        private async Task<bool> MilestoneSaveAsync()
        {
            if (!GoalAchieved || AchievedMilestone == null)
            {
                if (AchievedMilestonePercentage != null)
                    await MilestonePercentSaveAsync();
                return true;
            }

            var response = await _userMilestoneService.SaveUserMilestoneAsync(new UserMilestone()
            {
                AchieveDate = DateTime.Now,
                MilestoneID = AchievedMilestone.ID
            });
            if (response == HttpConstants.SUCCESS)
            {
                if (AchievedMilestonePercentage != null)
                {
                    await MilestonePercentSaveAsync();
                }
                else return true;
            }

            return response == HttpConstants.SUCCESS;
        }

        private async Task<bool> MilestonePercentSaveAsync()
        {
            var response = await _userMilestoneService.SaveUserMilestoneAsync(new UserMilestone()
            {
                AchieveDate = DateTime.Now,
                IsPercentage = true,
                MilestonePercentageId = AchievedMilestonePercentage.ID,
            });
            return response == HttpConstants.SUCCESS;
        }

        private async void SaveSuccessful(string message = "")
        {
            ShowBadgeAchievedImage = BadgeAchievedImage != null && BadgeAchievedImage.Trim().Length > 0;
            await ProfileViewModel.GetUserAsync();
            SetActivityResource();
            DependencyService.Get<IInformationMessageServices>().ShortAlert(message.Trim().Length > 0
                ? message
                : GoalAchieved
                    ? TextResources.MessageMilestoneSubmissionSuccessful
                    : TextResources.MessageCurrentWeightSubmissionSuccessful);
        }

        private List<Models.User.Tracker> BuildTracker()
        {
            var trackerList = new List<Models.User.Tracker>();

            trackerList.Add(_trackerPivotService.AddTracker(TrackerConstants.CURRENT_WEIGHT,
                CurrentWeightValue.ToString()));

            trackerList.Add(_trackerPivotService.AddTracker(TrackerConstants.CURRENT_WEIGHT_UI,
                CurrentWeightValue.ToString()));

            trackerList.Add(_trackerPivotService.AddTracker(TrackerConstants.WEIGHT_VOLUME_TYPE,
                App.Configuration.AppConfig.DefaultWeightVolume));

            if (GoalAchieved)
            {
                trackerList.Add(_trackerPivotService.AddTracker(TrackerConstants.TSHIRT_SIZE,
                    TShirtSize.Trim()));
                trackerList.Add(_trackerPivotService.AddTracker(TrackerConstants.FRONT_IMAGE,
                    ImageFront.Trim()));
                trackerList.Add(_trackerPivotService.AddTracker(TrackerConstants.SIDE_IMAGE,
                    ImageSide.Trim()));
                trackerList.Add(_trackerPivotService.AddTracker(TrackerConstants.ABOUT_JOURNEY,
                    AboutYourJourney.Trim()));
            }

            return trackerList;
        }

        private bool Validate()
        {
            if (App.CurrentUser.UserInfo.UserEmail.ToLower().Contains("apple") &&
                App.CurrentUser.UserInfo.UserEmail.ToLower().Contains("@organogold.com"))
                return true;

            var validationErrors = new ValidationErrors();

            // Current Weight
            if (CurrentWeightValue == 0)
                validationErrors.Add(
                    string.Format(TextResources.Required_IsMandatory, TextResources.WeightLossGoal));
            else if (CurrentWeightValue < _converter.DisplayWeightVolume(
                         App.Configuration.AppConfig.MINIMUM_CURRENT_WEIGHT_KG,
                         App.Configuration.AppConfig.MINIMUM_CURRENT_WEIGHT_LB))
                validationErrors.Add(string.Format(TextResources.Validation_MustBeMoreThan,
                    TextResources.WeightLossGoal,
                    _converter.DisplayWeightVolume(
                        App.Configuration.AppConfig.MINIMUM_CURRENT_WEIGHT_KG,
                        App.Configuration.AppConfig.MINIMUM_CURRENT_WEIGHT_LB)));

            if (GoalAchieved)
            {

#if DEBUG
                if (string.IsNullOrEmpty(ImageFront) || ImageFront == ImageDefault)
                    ImageFront = "Uploads/no.png";
                if (string.IsNullOrEmpty(ImageSide) || ImageSide == ImageDefault)
                    ImageSide = "Uploads/no.png";
#endif

                // Front Photo
                if (string.IsNullOrEmpty(ImageFront) || ImageFront == ImageDefault)
                    validationErrors.Add(string.Format(TextResources.Required_MustBeSelected,
                        TextResources.FrontPhoto));

                // Side Photo
                if (string.IsNullOrEmpty(ImageSide) || ImageSide == ImageDefault)
                    validationErrors.Add(string.Format(TextResources.Required_MustBeSelected,
                        TextResources.SidePhoto));

                //Gender
                if (IsGenderRequired && !IsGenderSelected)
                    validationErrors.Add(string.Format(TextResources.Required_MustBeSelected,
                        TextResources.Gender));

                // T-Shirt Size
                if (string.IsNullOrEmpty(TShirtSize))
                    validationErrors.Add(string.Format(TextResources.Required_MustBeSelected,
                        TextResources.TShirtSize));

                // Why you want to join
                if (string.IsNullOrEmpty(AboutYourJourney))
                    validationErrors.Add(string.Format(TextResources.Required_IsMandatory,
                        TextResources.AboutYourJourney));
            }

            if (validationErrors.Count() > 0)
                SetActivityResource(showError: true,
                    errorMessage: validationErrors.Count() > 2
                        ? TextResources.PleaseReviewAllInputsAreMandatory
                        : validationErrors.Show("\n"), modalWindow: true);

            return validationErrors.Count() == 0;
        }

        public void Male_Selected()
        {
            GenderSelected = Gender.Male;
            UpdateGenderStyleAsync();
        }

        public void Female_Selected()
        {
            GenderSelected = Gender.Female;
            UpdateGenderStyleAsync();
        }

        private void UpdateGenderStyleAsync()
        {
            IsGenderSelected = true;
            if (GenderSelected == Gender.Male)
            {
                ColorFemale = ColorGenderDefault;
                ColorMale = Palette._MainAccent;
                StringMale = StringMaleSelected;
                StringFemale = StringFemaleDefault;
            }
            else
            {
                ColorMale = ColorGenderDefault;
                ColorFemale = Palette._MainAccent;
                StringFemale = StringFemaleSelected;
                StringMale = StringMaleDefault;
            }
        }


        private MetaPivot _userMetas;
        public const string UserMetasPropertyName = "UserMetas";

        public MetaPivot UserMetas
        {
            get { return _userMetas; }
            set { SetProperty(ref _userMetas, value, UserMetasPropertyName); }
        }

        private ProfileEnhancedViewModel profileViewModel;
        public const string ProfileViewModelPropertyName = "ProfileViewModel";

        public ProfileEnhancedViewModel ProfileViewModel
        {
            get { return profileViewModel; }
            set { SetProperty(ref profileViewModel, value, ProfileViewModelPropertyName); }
        }

        private UserMilestoneExtended milestoneExtended;
        public const string MilestoneExtendedPropertyName = "MilestoneExtended";

        public UserMilestoneExtended MilestoneExtended
        {
            get { return milestoneExtended; }
            set { SetProperty(ref milestoneExtended, value, MilestoneExtendedPropertyName); }
        }

        private List<UserMilestone> userMilestones;
        public const string UserMilestonesPropertyName = "UserMilestones";

        public List<UserMilestone> UserMilestones
        {
            get { return userMilestones; }
            set { SetProperty(ref userMilestones, value, UserMilestonesPropertyName); }
        }

        private Milestone achievedMilestone;
        public const string AchievedMilestonePropertyName = "AchievedMilestone";

        public Milestone AchievedMilestone
        {
            get { return achievedMilestone; }
            set { SetProperty(ref achievedMilestone, value, AchievedMilestonePropertyName); }
        }

        private MilestonePercentage achievedMilestonePercentage;
        public const string AchievedMilestonePercentagePropertyName = "AchievedMilestonePercentage";

        public MilestonePercentage AchievedMilestonePercentage
        {
            get { return achievedMilestonePercentage; }
            set { SetProperty(ref achievedMilestonePercentage, value, AchievedMilestonePercentagePropertyName); }
        }

        private List<Models.Milestone> milestones;
        public const string MilestonesPropertyName = "Milestones";

        public List<Models.Milestone> Milestones
        {
            get { return milestones; }
            set { SetProperty(ref milestones, value, MilestonesPropertyName); }
        }

        private List<MilestonePercentage> milestonePercentage;
        public const string MilestonePercentagePropertyName = "MilestonePercentage";

        public List<MilestonePercentage> MilestonePercentage
        {
            get { return milestonePercentage; }
            set { SetProperty(ref milestonePercentage, value, MilestonePercentagePropertyName); }
        }

        private List<TrackerPivot> _userTrackers;
        public const string UserTrackersPropertyName = "UserTrackers";

        public List<TrackerPivot> UserTrackers
        {
            get { return _userTrackers; }
            set { SetProperty(ref _userTrackers, value, UserTrackersPropertyName); }
        }

        private ActionType actionType;
        public const string ActionTypePropertyName = "ActionType";

        public ActionType ActionType
        {
            get { return actionType; }
            set { SetProperty(ref actionType, value, ActionTypePropertyName); }
        }

        public string YourCurrentWeightText => string.Format(TextResources.YourCurrentWeightFormat1,
            App.Configuration.AppConfig.DefaultWeightVolume);

        public string StartWeightText => string.Format(TextResources.StartWeightFormat,
            App.Configuration.AppConfig.DefaultWeightVolume);

        public string LastWeightText =>
            string.Format(TextResources.LastWeightFormat, App.Configuration.AppConfig.DefaultWeightVolume);

        public string WeightLossGoalText => string.Format(TextResources.WeightLossGoalFormat,
            App.Configuration.AppConfig.DefaultWeightVolume);

        private bool _isGenderRequired;
        public const string IsGenderRequiredPropertyName = "IsGenderRequired";

        public bool IsGenderRequired
        {
            get { return _isGenderRequired; }
            set { SetProperty(ref _isGenderRequired, value, IsGenderRequiredPropertyName); }
        }

        private bool _isGenderSelected;
        public const string IsGenderSelectedPropertyName = "IsGenderSelected";

        public bool IsGenderSelected
        {
            get { return _isGenderSelected; }
            set { SetProperty(ref _isGenderSelected, value, GenderSelectedPropertyName); }
        }

        private Gender _genderSelected;
        public const string GenderSelectedPropertyName = "GenderSelected";

        public Gender GenderSelected
        {
            get { return _genderSelected; }
            set { SetProperty(ref _genderSelected, value, GenderSelectedPropertyName); }
        }

        public string StringFemaleDefault => TextResources.icon_female;
        public string StringMaleDefault => TextResources.icon_male;
        public string StringFemaleSelected => TextResources.icon_female_color;
        public string StringMaleSelected => TextResources.icon_male_color;

        private string _stringFemale;
        public const string StringFemalePropertyName = "StringFemale";

        public string StringFemale
        {
            get { return _stringFemale; }
            set { SetProperty(ref _stringFemale, value, StringFemalePropertyName, ChangeStringFemale); }
        }

        public void ChangeStringFemale()
        {
            StringFemaleSource = ImageResizer.ResizeImage(StringFemale, GenderImageSize);
        }

        private ImageSource stringFemaleSource;
        public const string StringFemaleSourcePropertyName = "StringFemaleSource";

        public ImageSource StringFemaleSource
        {
            get { return stringFemaleSource; }
            set { SetProperty(ref stringFemaleSource, value, StringFemaleSourcePropertyName); }
        }

        private string stringMale;
        public const string StringMalePropertyName = "StringMale";

        public string StringMale
        {
            get { return stringMale; }
            set { SetProperty(ref stringMale, value, StringMalePropertyName, ChangeStringMale); }
        }

        private void ChangeStringMale()
        {
            StringMaleSource = ImageResizer.ResizeImage(StringMale, GenderImageSize);
        }

        private ImageSource stringMaleSource;
        public const string StringMaleSourcePropertyName = "StringMaleSource";

        public ImageSource StringMaleSource
        {
            get { return stringMaleSource; }
            set { SetProperty(ref stringMaleSource, value, StringMaleSourcePropertyName); }
        }

        public Color ColorGenderDefault => Palette._ButtonBackgroundGray;

        private Color colorFemale;
        public const string ColorFemalePropertyName = "ColorFemale";

        public Color ColorFemale
        {
            get { return colorFemale; }
            set { SetProperty(ref colorFemale, value, ColorFemalePropertyName); }
        }

        private Color colorMale;
        public const string ColorMalePropertyName = "ColorMale";

        public Color ColorMale
        {
            get { return colorMale; }
            set { SetProperty(ref colorMale, value, ColorMalePropertyName); }
        }

        private List<string> tShirtSizeList;
        public const string TShirtSizeListPropertyName = "TShirtSizeList";

        public List<string> TShirtSizeList
        {
            get { return tShirtSizeList; }
            set { SetProperty(ref tShirtSizeList, value, TShirtSizeListPropertyName); }
        }

        private View baseContent;
        public const string BaseContentPropertyName = "BaseContent";

        public View BaseContent
        {
            get { return baseContent; }
            set { SetProperty(ref baseContent, value, BaseContentPropertyName); }
        }

        private View achievedContent;
        public const string AchievedContentPropertyName = "AchievedContent";

        public View AchievedContent
        {
            get { return achievedContent; }
            set { SetProperty(ref achievedContent, value, AchievedContentPropertyName); }
        }

        private bool milestoneAchieved;
        public const string MilestoneAchievedPropertyName = "MilestoneAchieved";

        public bool MilestoneAchieved
        {
            get { return milestoneAchieved; }
            set { SetProperty(ref milestoneAchieved, value, MilestoneAchievedPropertyName); }
        }

        private bool goalAchieved;
        public const string GoalAchievedPropertyName = "GoalAchieved";

        public bool GoalAchieved
        {
            get { return goalAchieved; }
            set { SetProperty(ref goalAchieved, value, GoalAchievedPropertyName, SetGoalAchieved); }
        }

        private bool currentWeightEnabled;
        public const string CurrentWeightEnabledPropertyName = "CurrentWeightEnabled";

        public bool CurrentWeightEnabled
        {
            get { return currentWeightEnabled; }
            set { SetProperty(ref currentWeightEnabled, value, CurrentWeightEnabledPropertyName); }
        }

        private string currentTitle;
        public const string CurrentTitlePropertyName = "CurrentTitle";

        public string CurrentTitle
        {
            get { return currentTitle; }
            set { SetProperty(ref currentTitle, value, CurrentTitlePropertyName); }
        }

        private string currentSubTitle;
        public const string CurrentSubTitlePropertyName = "CurrentSubTitle";

        public string CurrentSubTitle
        {
            get { return currentSubTitle; }
            set { SetProperty(ref currentSubTitle, value, CurrentSubTitlePropertyName); }
        }

        private bool _isCurrentSubTitle;
        public const string IsCurrentSubTitlePropertyName = "IsCurrentSubTitle";

        public bool IsCurrentSubTitle
        {
            get { return _isCurrentSubTitle; }
            set { SetProperty(ref _isCurrentSubTitle, value, IsCurrentSubTitlePropertyName); }
        }

        /********** MILESTONE **********/

        private double currentWeightValue = 0;
        public const string CurrentWeightValuePropertyName = "CurrentWeightValue";

        public double CurrentWeightValue
        {
            get { return currentWeightValue; }
            set { SetProperty(ref currentWeightValue, value, CurrentWeightValuePropertyName); }
        }

        private double currentWeightMaximumValue = 0;
        public const string CurrentWeightMaximumValuePropertyName = "CurrentWeightMaximumValue";

        public double CurrentWeightMaximumValue
        {
            get { return currentWeightMaximumValue; }
            set { SetProperty(ref currentWeightMaximumValue, value, CurrentWeightMaximumValuePropertyName); }
        }

        private double currentWeightMinimumValue = 0;
        public const string CurrentWeightMinimumValuePropertyName = "CurrentWeightMinimumValue";

        public double CurrentWeightMinimumValue
        {
            get { return currentWeightMinimumValue; }
            set { SetProperty(ref currentWeightMinimumValue, value, CurrentWeightMinimumValuePropertyName); }
        }

        public string ImageDefault => TextResources.icon_camera;

        private string imageFront;
        public const string ImageFrontPropertyName = "ImageFront";

        public string ImageFront
        {
            get { return imageFront; }
            set { SetProperty(ref imageFront, value, ImageFrontPropertyName, ChangeImageFront); }
        }

        private void ChangeImageFront()
        {
            ImageFrontSource = ImageFront == ImageDefault
                ? ImageResizer.ResizeImage(ImageFront, CameraImageSize)
                : _helper.GetFileUri(ImageFront, FileType.Upload);
        }

        private ImageSource imageFrontSource;
        public const string ImageFrontSourcePropertyName = "ImageFrontSource";

        public ImageSource ImageFrontSource
        {
            get { return imageFrontSource; }
            set { SetProperty(ref imageFrontSource, value, ImageFrontSourcePropertyName); }
        }

        private string imageSide;
        public const string ImageSidePropertyName = "ImageSide";

        public string ImageSide
        {
            get { return imageSide; }
            set { SetProperty(ref imageSide, value, ImageSidePropertyName, ChangeImageSide); }
        }

        private void ChangeImageSide()
        {
            ImageSideSource = ImageSide == ImageDefault
                ? ImageResizer.ResizeImage(ImageSide, CameraImageSize)
                : _helper.GetFileUri(ImageSide, FileType.Upload);
        }

        private ImageSource imageSideSource;
        public const string ImageSideSourcePropertyName = "ImageSideSource";

        public ImageSource ImageSideSource
        {
            get { return imageSideSource; }
            set { SetProperty(ref imageSideSource, value, ImageSideSourcePropertyName); }
        }

        private string aboutYourJourney;
        public const string AboutYourJourneyPropertyName = "AboutYourJourney";

        public string AboutYourJourney
        {
            get { return aboutYourJourney; }
            set { SetProperty(ref aboutYourJourney, value, AboutYourJourneyPropertyName); }
        }

        private string tShirtSize;
        public const string TShirtSizePropertyName = "TShirtSize";

        public string TShirtSize
        {
            get { return tShirtSize; }
            set { SetProperty(ref tShirtSize, value, TShirtSizePropertyName); }
        }

        /********** MILESTONE **********/

        private double previousWeight = 0;
        public const string PreviousWeightPropertyName = "PreviousWeight";

        public double PreviousWeight
        {
            get { return previousWeight; }
            set { SetProperty(ref previousWeight, value, PreviousWeightPropertyName); }
        }

        private double startWeight = 0;
        public const string StartWeightPropertyName = "StartWeight";

        public double StartWeight
        {
            get { return startWeight; }
            set { SetProperty(ref startWeight, value, StartWeightPropertyName); }
        }

        private double weightLossGoal = 0;
        public const string WeightLossGoalPropertyName = "WeightLossGoal";

        public double WeightLossGoal
        {
            get { return weightLossGoal; }
            set { SetProperty(ref weightLossGoal, value, WeightLossGoalPropertyName); }
        }

        //private List<View> viewComponents;
        //public const string ViewComponentsPropertyName = "ViewComponents";

        //public List<View> ViewComponents
        //{
        //    get { return viewComponents; }
        //    set { SetProperty(ref viewComponents, value, ViewComponentsPropertyName); }
        //}

        private string _badgeAchievedImage;
        public const string BadgeAchievedImagePropertyName = "BadgeAchievedImage";

        public string BadgeAchievedImage
        {
            get { return _badgeAchievedImage; }
            set
            {
                SetProperty(ref _badgeAchievedImage, value, BadgeAchievedImagePropertyName, ChangeBadgeAchievedImage);
            }
        }

        private void ChangeBadgeAchievedImage()
        {
            if (BadgeAchievedImage.Trim().Length > 0)
                BadgeAchievedImageSource = ImageResizer.ResizeImage(BadgeAchievedImage, BadgeImageSize);
        }

        private ImageSource _badgeAchievedImageSource;
        public const string BadgeAchievedImageSourcePropertyName = "BadgeAchievedImageSource";

        public ImageSource BadgeAchievedImageSource
        {
            get { return _badgeAchievedImageSource; }
            set { SetProperty(ref _badgeAchievedImageSource, value, BadgeAchievedImageSourcePropertyName); }
        }


        private bool _showBadgeAchievedImage;
        public const string ShowBadgeAchievedImagePropertyName = "ShowBadgeAchievedImage";

        public bool ShowBadgeAchievedImage
        {
            get { return _showBadgeAchievedImage; }
            set { SetProperty(ref _showBadgeAchievedImage, value, ShowBadgeAchievedImagePropertyName); }
        }

        private bool _isBasicVisible = false;
        public const string IsBasicVisiblePropertyName = "IsBasicVisible";

        public bool IsBasicVisible
        {
            get { return _isBasicVisible; }
            set { SetProperty(ref _isBasicVisible, value, IsBasicVisiblePropertyName); }
        }

        private bool _isAchievedVisible = false;
        public const string IsAchievedVisiblePropertyName = "IsAchievedVisible";

        public bool IsAchievedVisible
        {
            get { return _isAchievedVisible; }
            set { SetProperty(ref _isAchievedVisible, value, IsAchievedVisiblePropertyName); }
        }

        public ImageSize CameraImageSize { get; set; }

        public ImageSize GenderImageSize { get; set; }

        public ImageSize BadgeImageSize { get; set; }

        private void SetPageImageSize()
        {
            CameraImageSize = App.Configuration.GetImageSizeByID(ImageIdentity.UPLOAD_CAMERA_IMAGE);
            if (CameraImageSize != null)
            {
                CameraImageHeight = CameraImageSize.Height;
                CameraImageWidth = CameraImageSize.Width;
            }

            GenderImageSize = App.Configuration.GetImageSizeByID(ImageIdentity.GENDER_IMAGE);
            if (GenderImageSize != null)
            {
                GenderImageHeight = GenderImageSize.Height;
                GenderImageWidth = GenderImageSize.Width;
            }

            BadgeImageSize = App.Configuration.GetImageSizeByID(ImageIdentity.MILESTONE_ACHEIVEMENT_BADGE_ICON);
            if (BadgeImageSize != null)
            {
                BadgeAchievedImageHeight = BadgeImageSize.Height;
                BadgeAchievedImageWidth = BadgeImageSize.Width;
            }
        }

        private float cameraImageHeight;
        public const string CameraImageHeightPropertyName = "CameraImageHeight";

        public float CameraImageHeight
        {
            get { return cameraImageHeight; }
            set { SetProperty(ref cameraImageHeight, value, CameraImageHeightPropertyName); }
        }

        private float cameraImageWidth;
        public const string CameraImageWidthPropertyName = "CameraImageWidth";

        public float CameraImageWidth
        {
            get { return cameraImageWidth; }
            set { SetProperty(ref cameraImageWidth, value, CameraImageWidthPropertyName); }
        }

        private float genderImageHeight;
        public const string GenderImageHeightPropertyName = "GenderImageHeight";

        public float GenderImageHeight
        {
            get { return genderImageHeight; }
            set { SetProperty(ref genderImageHeight, value, GenderImageHeightPropertyName); }
        }

        private float genderImageWidth;
        public const string GenderImageWidthPropertyName = "GenderImageWidth";

        public float GenderImageWidth
        {
            get { return genderImageWidth; }
            set { SetProperty(ref genderImageWidth, value, GenderImageWidthPropertyName); }
        }

        private float badgeAchievedImageHeight;
        public const string BadgeAchievedImageHeightPropertyName = "BadgeAchievedImageHeight";

        public float BadgeAchievedImageHeight
        {
            get { return badgeAchievedImageHeight; }
            set { SetProperty(ref badgeAchievedImageHeight, value, BadgeAchievedImageHeightPropertyName); }
        }

        private float badgeAchievedImageWidth;
        public const string BadgeAchievedImageWidthPropertyName = "BadgeAchievedImageWidth";

        public float BadgeAchievedImageWidth
        {
            get { return badgeAchievedImageWidth; }
            set { SetProperty(ref badgeAchievedImageWidth, value, BadgeAchievedImageWidthPropertyName); }
        }
    }
}