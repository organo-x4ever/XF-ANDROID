using com.organo.xchallenge.Globals;
using com.organo.xchallenge.Localization;
using com.organo.xchallenge.Models;
using com.organo.xchallenge.Models.User;
using com.organo.xchallenge.Models.Validation;
using com.organo.xchallenge.Pages;
using com.organo.xchallenge.Services;
using com.organo.xchallenge.Statics;
using com.organo.xchallenge.ViewModels.Account;
using com.organo.xchallenge.ViewModels.Base;
using com.organo.xchallenge.ViewModels.Profile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using com.organo.xchallenge.Utilities;
using Xamarin.Forms;

namespace com.organo.xchallenge.ViewModels.Milestones
{
    public class MilestoneViewModel : BaseViewModel
    {
        private readonly ITrackerService trackerService;
        private readonly IMetaService metaService;
        private readonly IAuthenticationService authenticationService;
        private readonly IUserMilestoneService userMilestoneService;

        public MilestoneViewModel(INavigation navigation = null) : base(navigation)
        {
            trackerService = DependencyService.Get<ITrackerService>();
            metaService = DependencyService.Get<IMetaService>();
            authenticationService = DependencyService.Get<IAuthenticationService>();
            userMilestoneService = DependencyService.Get<IUserMilestoneService>();

            ShowBadgeAchievedImage = false;
            BadgeAchievedImage = string.Empty;
            StringFemale = StringFemaleDefault;
            StringMale = StringMaleDefault;
            ColorFemale = ColorMale = ColorGenderDefault;

            ActionType = ActionType.Check;
            BaseContent = null;

            CurrentWeightMinimumValue = 0;
            CurrentWeightMaximumValue = App.Configuration.AppConfig.MAXIMUM_WEIGHT_LOSE;
            CurrentWeightValue = CurrentWeightMinimumValue;

            UserTrackers = new List<UserTracker>();
            UserMetas = new UserMeta();
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
            TShirtSize = string.Empty;
            AboutYourJourney = ErrorMessage;
            MilestoneExtended = new UserMilestoneExtended();
            Milestones = new List<MilestonePercentage>();
            UserMilestones = new List<UserMilestone>();
            ViewComponents = new List<View>();
            GetUserTracker();
        }

        private async void GetUserTracker()
        {
            UserTrackers = await trackerService.GetUserTrackerAsync();
            if (UserTrackers.Count > 0)
            {
                var trackerFirst = UserTrackers.OrderBy(t => t.RevisionNumber).FirstOrDefault();
                var trackerLast = UserTrackers.OrderByDescending(t => t.RevisionNumber).FirstOrDefault();
                if (trackerFirst != null && trackerLast != null)
                {
                    if (short.TryParse(trackerLast.CurrentWeight.ToString(), out short previous))
                        PreviousWeight = previous;
                    if (short.TryParse(trackerFirst.CurrentWeight.ToString(), out short start))
                        StartWeight = start;
                }
            }

            UserMetas = await metaService.GetMetaAsync(new string[]
                {MetaConstants.WEIGHT_LOSS_GOAL, MetaConstants.GENDER});
            if (UserMetas != null)
            {
                if (short.TryParse(UserMetas.WeightGoal.ToString(), out short weightLossGoal))
                    WeightLossGoal = weightLossGoal;
                GetUserGenderAsync();
            }

            BaseContent = new Pages.MilestonePages.BasicMilestonePage(this).Content;

            await GetMilestoneExtendedAsync();
        }

        private async Task GetMilestoneExtendedAsync()
        {
            MilestoneExtended =
                await userMilestoneService.GetExtendedAsync(App.Configuration.AppConfig.DefaultLanguage);
            if (MilestoneExtended != null)
            {
                if (MilestoneExtended.MilestonePercentages != null)
                    Milestones = MilestoneExtended.MilestonePercentages.ToList();
                if (MilestoneExtended.UserMilestones != null)
                    UserMilestones = MilestoneExtended.UserMilestones.ToList();
            }
        }

        private void GetUserGenderAsync()
        {
            if (UserMetas == null || UserMetas.Gender == null || UserMetas.Gender.Trim().Length == 0)
                IsGenderRequired = true;
            else
            {
                var gender = UserMetas.Gender;
                if (gender != null && gender.Trim().Length > 0)
                {
                    GenderSelected = gender == Gender.Male.ToString() ? Gender.Male : Gender.Female;
                    UpdateGenderStyleAsync();
                }
            }
        }

        public async Task<List<string>> GetTShirtSizeList()
        {
            await Task.Run(() => { TShirtSizeList = TextResources.TShirtSizes.Split(',').ToList(); });
            return TShirtSizeList;
        }

        private void SetGoalAchieved()
        {
            CurrentWeightEnabled = !GoalAchieved;
            CurrentTitle = TextResources.CurrentWeight;
            if (GoalAchieved)
            {
                CurrentTitle = TextResources.Milestone;
                AchievedContent = new Pages.MilestonePages.AchievedMilestonePage(this).Content;
            }
            else
                AchievedContent = null;
        }

        public ICommand _submitCommand;

        public ICommand SubmitCommand
        {
            get
            {
                return _submitCommand ?? (_submitCommand = new Command(async (obj) =>
                {
                    if (ActionType == ActionType.Check)
                        await CurrentWeightChanged();
                    else
                        await SaveTrackerAsync();
                }));
            }
        }

        private async Task CurrentWeightChanged()
        {
            if (await Validate())
            {
                AchievedMilestone = null;
                var included = Milestones.Where(m => UserMilestones.Any(u => u.MilestoneID == m.ID));
                var milestones = Milestones.Except(included).OrderByDescending(m => m.TargetPercentValue);
                ActionType = ActionType.Submit;

                if ((StartWeight - CurrentWeightValue) >= WeightLossGoal)
                {
                    CurrentTitle = TextResources.GoalAchieved;
                    SetActivityResource(true, false, true, false, string.Empty,
                        TextResources.MessageGoalAchievedWishes, string.Empty);
                    GoalAchieved = true;
                    return;
                }
                else
                    foreach (var milestone in milestones)
                    {
                        if ((StartWeight - CurrentWeightValue) >=
                            ((WeightLossGoal * milestone.TargetPercentValue) / 100))
                        {
                            AchievedMilestone = milestone;
                            CurrentTitle = milestone.MilestoneTitle;
                            SetActivityResource(true, false, true, false, string.Empty,
                                string.Format(milestone.AchievedMessage, (StartWeight - CurrentWeightValue),
                                    App.Configuration.AppConfig.DefaultWeightVolume),
                                string.Empty);
                            BadgeAchievedImage = DependencyService.Get<IMessage>()
                                .GetResource(milestone.AchievementGiftImage);

                            CurrentTitle = AchievedMilestone.MilestoneTitle;
                            CurrentSubTitle = AchievedMilestone.MilestoneSubTitle;
                            IsCurrentSubTitle =
                                CurrentSubTitle != null && CurrentSubTitle.Trim().Length > 0;

                            GoalAchieved = true;
                            return;
                        }
                    }

                if (!GoalAchieved)
                {
                    ActionType = ActionType.Submit;
                    await SaveTrackerAsync();
                }
            }
        }

        private async Task SaveTrackerAsync()
        {
            if (ActionType != ActionType.Submit)
                return;
            ValidationErrors validationErrors = new ValidationErrors();
            SetActivityResource(false, true, false, false, TextResources.ProcessingDataPleaseWait, string.Empty,
                string.Empty);
            if (await Validate())
            {
                var trackerList = await BuildTracker();
                if (trackerList.Count > 0)
                {
                    var response = await trackerService.SaveTrackerAsync(trackerList);
                    if (response == HttpConstants.SUCCESS)
                    {
                        var result = await MilestoneSaveAsync();
                        if (result)
                        {
                            var userMeta = new List<Meta>()
                            {
                                await metaService.AddMeta(GenderSelected.ToString(), MetaConstants.GENDER,
                                    MetaConstants.GENDER, MetaConstants.LABEL)
                            };
                            if (IsGenderRequired)
                            {
                                var metaResult = await metaService.SaveMetaAsync(userMeta);
                                if (metaResult == HttpConstants.SUCCESS)
                                {
                                }
                            }

                            await SaveSuccessful(result
                                ? string.Empty
                                : TextResources.MessageMilestoneSubmissionFailed);
                        }
                    }
                    else if (response.ToString().Contains(HttpConstants.UNAUTHORIZED))
                    {
                        await authenticationService.LogoutAsync();
                        App.GoToAccountPage();
                    }
                    else
                        SetActivityResource(true, false, false, true, string.Empty, string.Empty, response);
                }
                else
                    SetActivityResource(true, false, false, true, string.Empty, string.Empty,
                        TextResources.NoRecordToProcess);
            }
        }

        private async Task<bool> MilestoneSaveAsync()
        {
            if (!GoalAchieved || AchievedMilestone == null)
                return true;
            var userMilestone = new UserMilestone()
            {
                AchieveDate = DateTime.Now,
                MilestoneID = AchievedMilestone.ID
            };
            var response = await userMilestoneService.SaveUserMilestoneAsync(userMilestone);
            return response == HttpConstants.SUCCESS;
        }

        private async Task SaveSuccessful(string message = "")
        {
            await Task.Run(() =>
            {
                ShowBadgeAchievedImage =
                    BadgeAchievedImage != null && BadgeAchievedImage.Trim().Length > 0;
                SetActivityResource(false, false, false, false, string.Empty, string.Empty, string.Empty);
                DependencyService.Get<IInformationMessageServices>().LongAlert(message.Trim().Length > 0
                    ? message
                    : GoalAchieved
                        ? TextResources.MessageMilestoneSubmissionSuccessful
                        : TextResources.MessageCurrentWeightSubmissionSuccessful);
            });
            await ProfileViewModel.GetUserAsync();
            await App.CurrentApp.MainPage.Navigation.PopModalAsync();
        }

        private async Task<List<Models.User.Tracker>> BuildTracker()
        {
            var trackerList = new List<Models.User.Tracker>();
            await Task.Run(async () =>
            {
                trackerList.Add(await trackerService.AddTracker(TrackerConstants.CURRENT_WEIGHT,
                    CurrentWeightValue.ToString()));
                if (GoalAchieved)
                {
                    trackerList.Add(await trackerService.AddTracker(TrackerConstants.TSHIRT_SIZE,
                        TShirtSize.Trim()));
                    trackerList.Add(await trackerService.AddTracker(TrackerConstants.FRONT_IMAGE,
                        ImageFront.Trim()));
                    trackerList.Add(await trackerService.AddTracker(TrackerConstants.SIDE_IMAGE,
                        ImageSide.Trim()));
                    trackerList.Add(await trackerService.AddTracker(TrackerConstants.ABOUT_JOURNEY,
                        AboutYourJourney.Trim()));
                }
            });
            return trackerList;
        }

        private async Task<bool> Validate()
        {
            var validationErrors = new ValidationErrors();
            await Task.Run(() =>
            {
                // Current Weight
                if (CurrentWeightValue == 0)
                    validationErrors.Add(
                        string.Format(TextResources.Required_IsMandatory, TextResources.WeightLossGoal));
                else if (CurrentWeightValue < App.Configuration.AppConfig.MINIMUM_WEIGHT_LOSE)
                    validationErrors.Add(string.Format(TextResources.Validation_MustBeMoreThan,
                        TextResources.WeightLossGoal, App.Configuration.AppConfig.MINIMUM_WEIGHT_LOSE));

                if (GoalAchieved)
                {
                    // Front Photo
                    if (ImageFront == null || ImageFront.Trim().Length == 0 ||
                        ImageFront == ImageDefault)
                        validationErrors.Add(string.Format(TextResources.Required_MustBeSelected,
                            TextResources.FrontPhoto));

                    // Side Photo
                    if (ImageSide == null || ImageSide.Trim().Length == 0 ||
                        ImageSide == ImageDefault)
                        validationErrors.Add(string.Format(TextResources.Required_MustBeSelected,
                            TextResources.SidePhoto));

                    //Gender
                    if (IsGenderRequired && !IsGenderSelected)
                    {
                        validationErrors.Add(string.Format(TextResources.Required_MustBeSelected,
                            TextResources.Gender));
                    }

                    // T-Shirt Size
                    if (TShirtSize == null || TShirtSize.Trim().Length == 0)
                        validationErrors.Add(string.Format(TextResources.Required_MustBeSelected,
                            TextResources.TShirtSize));

                    // Why you want to join
                    if (AboutYourJourney == null || AboutYourJourney.Trim().Length == 0)
                        validationErrors.Add(string.Format(TextResources.Required_IsMandatory,
                            TextResources.AboutYourJourney));
                }
            });
            if (validationErrors.Count() > 0)
                if (validationErrors.Count() > 2)
                    SetActivityResource(true, false, false, true, string.Empty, string.Empty,
                        TextResources.SubmissionProblem + "\n" + TextResources.PleaseReviewAllInputsAreMandatory);
                else
                    SetActivityResource(true, false, false, true, string.Empty, string.Empty,
                        TextResources.SubmissionProblem + "\n" + validationErrors.Show("\n"));

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

        private RootPage root;
        public const string RootPropertyName = "Root";

        public RootPage Root
        {
            get { return root; }
            set { SetProperty(ref root, value, RootPropertyName); }
        }

        public UserMeta UserMetas { get; set; }

        private MyProfileViewModel profileViewModel;
        public const string ProfileViewModelPropertyName = "ProfileViewModel";

        public MyProfileViewModel ProfileViewModel
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

        private MilestonePercentage achievedMilestone;
        public const string AchievedMilestonePropertyName = "AchievedMilestone";

        public MilestonePercentage AchievedMilestone
        {
            get { return achievedMilestone; }
            set { SetProperty(ref achievedMilestone, value, AchievedMilestonePropertyName); }
        }

        private List<MilestonePercentage> milestones;
        public const string MilestonesPropertyName = "Milestones";

        public List<MilestonePercentage> Milestones
        {
            get { return milestones; }
            set { SetProperty(ref milestones, value, MilestonesPropertyName); }
        }

        private List<UserTracker> userTrackers;
        public const string UserTrackersPropertyName = "UserTrackers";

        public List<UserTracker> UserTrackers
        {
            get { return userTrackers; }
            set { SetProperty(ref userTrackers, value, UserTrackersPropertyName); }
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

        private Gender genderSelected;
        public const string GenderSelectedPropertyName = "GenderSelected";

        public Gender GenderSelected
        {
            get { return genderSelected; }
            set { SetProperty(ref genderSelected, value, GenderSelectedPropertyName); }
        }

        public string StringFemaleDefault => TextResources.icon_female;
        public string StringMaleDefault => TextResources.icon_male;
        public string StringFemaleSelected => TextResources.icon_female_color;
        public string StringMaleSelected => TextResources.icon_male_color;

        private string stringFemale;
        public const string StringFemalePropertyName = "StringFemale";

        public string StringFemale
        {
            get { return stringFemale; }
            set { SetProperty(ref stringFemale, value, StringFemalePropertyName); }
        }

        private string stringMale;
        public const string StringMalePropertyName = "StringMale";

        public string StringMale
        {
            get { return stringMale; }
            set { SetProperty(ref stringMale, value, StringMalePropertyName); }
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

        private Int16 currentWeightValue = 0;
        public const string CurrentWeightValuePropertyName = "CurrentWeightValue";

        public Int16 CurrentWeightValue
        {
            get { return currentWeightValue; }
            set { SetProperty(ref currentWeightValue, value, CurrentWeightValuePropertyName); }
        }

        private Int16 currentWeightMaximumValue = 0;
        public const string CurrentWeightMaximumValuePropertyName = "CurrentWeightMaximumValue";

        public Int16 CurrentWeightMaximumValue
        {
            get { return currentWeightMaximumValue; }
            set { SetProperty(ref currentWeightMaximumValue, value, CurrentWeightMaximumValuePropertyName); }
        }

        private Int16 currentWeightMinimumValue = 0;
        public const string CurrentWeightMinimumValuePropertyName = "CurrentWeightMinimumValue";

        public Int16 CurrentWeightMinimumValue
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
            set { SetProperty(ref imageFront, value, ImageFrontPropertyName); }
        }

        private string imageSide;
        public const string ImageSidePropertyName = "ImageSide";

        public string ImageSide
        {
            get { return imageSide; }
            set { SetProperty(ref imageSide, value, ImageSidePropertyName); }
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

        private Int16 previousWeight = 0;
        public const string PreviousWeightPropertyName = "PreviousWeight";

        public Int16 PreviousWeight
        {
            get { return previousWeight; }
            set { SetProperty(ref previousWeight, value, PreviousWeightPropertyName); }
        }

        private Int16 startWeight = 0;
        public const string StartWeightPropertyName = "StartWeight";

        public Int16 StartWeight
        {
            get { return startWeight; }
            set { SetProperty(ref startWeight, value, StartWeightPropertyName); }
        }

        private Int16 weightLossGoal = 0;
        public const string WeightLossGoalPropertyName = "WeightLossGoal";

        public Int16 WeightLossGoal
        {
            get { return weightLossGoal; }
            set { SetProperty(ref weightLossGoal, value, WeightLossGoalPropertyName); }
        }

        private List<View> viewComponents;
        public const string ViewComponentsPropertyName = "ViewComponents";

        public List<View> ViewComponents
        {
            get { return viewComponents; }
            set { SetProperty(ref viewComponents, value, ViewComponentsPropertyName); }
        }

        private string _badgeAchievedImage;
        public const string BadgeAchievedImagePropertyName = "BadgeAchievedImage";

        public string BadgeAchievedImage
        {
            get { return _badgeAchievedImage; }
            set { SetProperty(ref _badgeAchievedImage, value, BadgeAchievedImagePropertyName); }
        }

        private bool _showBadgeAchievedImage;
        public const string ShowBadgeAchievedImagePropertyName = "ShowBadgeAchievedImage";

        public bool ShowBadgeAchievedImage
        {
            get { return _showBadgeAchievedImage; }
            set { SetProperty(ref _showBadgeAchievedImage, value, ShowBadgeAchievedImagePropertyName); }
        }
    }
}