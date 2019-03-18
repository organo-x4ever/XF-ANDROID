
using com.organo.x4ever.Globals;
using com.organo.x4ever.Handler;
using com.organo.x4ever.Helpers;
using com.organo.x4ever.Localization;
using com.organo.x4ever.Models;
using com.organo.x4ever.Models.User;
using com.organo.x4ever.Pages;
using com.organo.x4ever.Pages.Profile;
using com.organo.x4ever.Services;
using com.organo.x4ever.Statics;
using com.organo.x4ever.ViewModels.Base;
using Microcharts;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Entry = Microcharts.Entry;

namespace com.organo.x4ever.ViewModels.Profile
{
    public class MyProfileViewModel : BaseViewModel
    {
        private readonly IUserService _userService;
        private readonly ImageSize _imageSizeBadge;

        public MyProfileViewModel(Xamarin.Forms.INavigation navigation = null) : base(navigation)
        {
            _userService = DependencyService.Get<IUserService>();
            UserGreeting = string.Empty;
            JoiningDate = string.Empty;
            TargetDate = string.Empty;
            DisplayDetailLink = string.Empty;
            DisplayGalleryLink = string.Empty;
            Seperator = string.Empty;
            YourGoal = 0;
            YouLost = 0;
            ToLoose = 0;
            GaugeMin = 0;
            GaugeMax = 100;
            GaugeCurrent = 0;
            _imageSizeBadge = App.Configuration.GetImageSizeByID(ImageIdentity.USER_PROFILE_BADGE_ICON);
            if (_imageSizeBadge != null)
            {
                UserBadgeImageHeight = _imageSizeBadge.Height;
                UserBadgeImageWidth = _imageSizeBadge.Width;
            }

            BadgeAchievedImage = ImageResizer.ResizeImage(TextResources.Badge_Null, _imageSizeBadge);
            MilestoneRequired = false;
            ShowTrackerDetail = false;
            UserDetail = new UserWithDetail();
            UserTrackers = new List<UserTracker>();
            UserTrackersDescending = new List<UserTracker>();
            SetPageImageSize();
            Page_Load();
        }

        private async void Page_Load()
        {
            await GetUserAsync(GetTrackerInputAsync);
        }

        /********** Profile Content View : START **********/

        public async Task GetUserAsync(Action action = null)
        {
            UserDetail = await _userService.GetFullAsync();
            if (UserDetail == null)
            {
                await App.LogoutAsync();
                App.GoToAccountPage();
                return;
            }

            UserTrackers = UserDetail.UserDetailTrackers;
            UserGreeting = string.Format(TextResources.GreetingUser, UserDetail.DisplayName);
            if (UserDetail.Achievement != null && UserDetail.Achievement.AchievementIcon != null)
                BadgeAchievedImage = ImageResizer.ResizeImage(
                    DependencyService.Get<IMessage>().GetResource(UserDetail.Achievement.AchievementIcon),
                    _imageSizeBadge);
            JoiningDate = String.Format("{0:MMM d, yyyy}", UserDetail.UserRegistered);

            if (UserDetail.UserDetailMeta != null)
                YourGoal = UserDetail.UserDetailMeta.WeightGoal;

            //(the weight you want to lose / 0.33)/ 7
            var noOfDays = (YourGoal / App.Configuration.AppConfig.TargetDateCalculation);
            TargetDate =
                String.Format("{0:MMM d, yyyy}",
                    UserDetail.UserRegistered.AddDays(noOfDays)); // "Sunday, March 9, 2008"

            var trackerFirst = UserDetail.UserDetailTrackers.OrderBy(t => t.ModifyDate).FirstOrDefault();
            var trackerLast = UserDetail.UserDetailTrackers.OrderByDescending(t => t.ModifyDate).FirstOrDefault();
            MilestoneRequired = trackerFirst == null;
            if (trackerFirst != null && trackerLast != null)
            {
                StartWeight = trackerFirst.CurrentWeight;
                Weight = StartWeight;
                WeightLossGoal = YourGoal;

                YouLost = (short) (StartWeight - trackerLast.CurrentWeight);
                ToLoose = (short) (YourGoal - YouLost);
                ToLoose = (short) (ToLoose >= 0 ? ToLoose : 0);

                int.TryParse(trackerLast.RevisionNumber, out int revisionNumber);
                revisionNumber = (revisionNumber > 1 ? revisionNumber - 1 : 1);
                var trackerPrevious =
                    UserDetail.UserDetailTrackers.FirstOrDefault(t =>
                        t.RevisionNumber == revisionNumber.ToString());
                if (trackerPrevious != null)
                    YouLostThisWeek = (short) (trackerPrevious.CurrentWeight - trackerLast.CurrentWeight);

                // Milestone Requirement Check
                MilestoneRequired = UserDetail.IsWeightSubmissionRequired;
            }

            await LoadGauge();
            await GetTrackerData();
            action?.Invoke();
        }

        private async Task LoadGauge()
        {
            // Gauge Values
            await Task.Run(() =>
            {
                var trackerFirst = UserDetail.UserDetailTrackers.OrderBy(t => t.ModifyDate).FirstOrDefault();
                var trackerLast = UserDetail.UserDetailTrackers.OrderByDescending(t => t.ModifyDate).FirstOrDefault();
                if (trackerFirst != null && trackerLast != null)
                {
                    var gaugeCurrent = trackerFirst.CurrentWeight - trackerLast.CurrentWeight;
                    GaugeCurrentConstant = (gaugeCurrent * 100) / (double) YourGoal;
                    GaugeCurrent = GaugeCurrentConstant > 100 ? 100 : GaugeCurrentConstant;
                }
            });
            // Gauge Values
        }

        private async void GetTrackerInputAsync()
        {
            await Task.Delay(1500);
            if (MilestoneRequired)
                await App.CurrentApp.MainPage.Navigation.PushModalAsync(
                    new Pages.MilestonePages.UserMilestonePage(Root, this));
        }

        private RootPage root;
        public const string RootPropertyName = "Root";

        public RootPage Root
        {
            get { return root; }
            set { SetProperty(ref root, value, RootPropertyName); }
        }

        private UserWithDetail userDetail;
        public const string UserDetailPropertyName = "UserDetail";

        public UserWithDetail UserDetail
        {
            get { return userDetail; }
            set { SetProperty(ref userDetail, value, UserDetailPropertyName); }
        }

        private bool milestoneRequired;
        public const string MilestoneRequiredPropertyName = "MilestoneRequired";

        public bool MilestoneRequired
        {
            get { return milestoneRequired; }
            set { SetProperty(ref milestoneRequired, value, MilestoneRequiredPropertyName); }
        }

        private string userGreeting;
        public const string UserGreetingPropertyName = "UserGreeting";

        public string UserGreeting
        {
            get { return userGreeting; }
            set { SetProperty(ref userGreeting, value, UserGreetingPropertyName); }
        }

        private string joiningDate;
        public const string JoiningDatePropertyName = "JoiningDate";

        public string JoiningDate
        {
            get { return joiningDate; }
            set { SetProperty(ref joiningDate, value, JoiningDatePropertyName); }
        }

        private string targetDate;
        public const string TargetDatePropertyName = "TargetDate";

        public string TargetDate
        {
            get { return targetDate; }
            set { SetProperty(ref targetDate, value, TargetDatePropertyName); }
        }

        public string YourGoalText => TextResources.YourGoal;
        public string ThisWeekText => TextResources.ThisWeek;
        public string RemainingText => TextResources.Remaining;

        public string ToLooseText =>
            string.Format(TextResources.ToLooseFormat1, App.Configuration.AppConfig.DefaultWeightVolume);

        public string LostText =>
            string.Format(TextResources.LostFormat1, App.Configuration.AppConfig.DefaultWeightVolume);

        private double yourGoal;
        public const string YourGoalPropertyName = "YourGoal";

        public double YourGoal
        {
            get { return yourGoal; }
            set { SetProperty(ref yourGoal, value, YourGoalPropertyName); }
        }

        private double startWeight;
        public const string StartWeightPropertyName = "StartWeight";

        public double StartWeight
        {
            get { return startWeight; }
            set { SetProperty(ref startWeight, value, StartWeightPropertyName); }
        }

        private double youLost;
        public const string YouLostPropertyName = "YouLost";

        public double YouLost
        {
            get { return youLost; }
            set { SetProperty(ref youLost, value, YouLostPropertyName); }
        }

        private double youLostThisWeek;
        public const string YouLostThisWeekPropertyName = "YouLostThisWeek";

        public double YouLostThisWeek
        {
            get { return youLostThisWeek; }
            set { SetProperty(ref youLostThisWeek, value, YouLostThisWeekPropertyName); }
        }

        private double toLoose;
        public const string ToLoosePropertyName = "ToLoose";

        public double ToLoose
        {
            get { return toLoose; }
            set { SetProperty(ref toLoose, value, ToLoosePropertyName); }
        }

        /********** Profile Content View : END **********/

        /********** Tracker Chart View : START **********/

        private string displayDetailLink;
        public const string DisplayDetailLinkPropertyName = "DisplayDetailLink";

        public string DisplayDetailLink
        {
            get { return displayDetailLink; }
            set { SetProperty(ref displayDetailLink, value, DisplayDetailLinkPropertyName); }
        }

        Xamarin.Forms.Page TrackerPage => new TrackerDetailPage(this);
        private bool showTrackerDetail;
        public const string ShowTrackerDetailPropertyName = "ShowTrackerDetail";

        public bool ShowTrackerDetail
        {
            get { return showTrackerDetail; }
            set
            {
                SetProperty(ref showTrackerDetail, value, ShowTrackerDetailPropertyName, ShowHideTrackerDetailAsync);
            }
        }

        public async void ShowHideTrackerDetailAsync()
        {
            PopType = PopupType.None;
            if (ShowTrackerDetail)
                await ShowTrackerDetailAsync();
            else
                await HideTrackerDetailAsync();
        }

        public async Task ShowTrackerDetailAsync()
        {
            PopType = PopupType.Detail;
            UserTrackersDescending = new List<UserTracker>();
            UserTrackersDescending = UserTrackers.OrderByDescending(t => t.ModifyDate).ToList();
            await PushModalAsync(TrackerPage);
        }

        public async Task HideTrackerDetailAsync()
        {
            await PopModalAsync();
        }

        /********** Tracker Content View : END **********/

        /********** Tracker Gallery View : START **********/

        private string seperator;
        public const string SeperatorPropertyName = "Seperator";

        public string Seperator
        {
            get { return seperator; }
            set { SetProperty(ref seperator, value, SeperatorPropertyName); }
        }

        private string displayGalleryLink;
        public const string DisplayGalleryLinkPropertyName = "DisplayGalleryLink";

        public string DisplayGalleryLink
        {
            get { return displayGalleryLink; }
            set { SetProperty(ref displayGalleryLink, value, DisplayGalleryLinkPropertyName); }
        }

        Xamarin.Forms.Page GalleryPage => new PictureGalleryPage(this);
        private bool showGalleryDetail;
        public const string ShowGalleryDetailPropertyName = "ShowGalleryDetail";

        public bool ShowGalleryDetail
        {
            get { return showGalleryDetail; }
            set
            {
                SetProperty(ref showGalleryDetail, value, ShowGalleryDetailPropertyName, ShowHideGalleryDetailAsync);
            }
        }

        public async void ShowHideGalleryDetailAsync()
        {
            PopType = PopupType.None;
            if (ShowGalleryDetail)
                await ShowGalleryDetailAsync();
            else
                await HideGalleryDetailAsync();
        }

        public async Task ShowGalleryDetailAsync()
        {
            PopType = PopupType.Gallery;
            GalleryImageSize = App.Configuration.GetImageSizeByID(ImageIdentity.PICTURE_GALLERY_IMAGE);
            if (GalleryImageSize == null)
                GalleryImageSize = new ImageSize();
            UserTrackersDescending = new List<UserTracker>();
            UserTrackersDescending = UserTrackers
                .Where(g => !string.IsNullOrEmpty(g.FrontImage) && !string.IsNullOrEmpty(g.SideImage))
                .OrderByDescending(t => t.ModifyDate).ToList();

            foreach (var userTracker in UserTrackersDescending)
            {
                userTracker.FrontImageSource =
                    DependencyService.Get<IHelper>().GetFileUri(userTracker.FrontImage, FileType.User);
                userTracker.SideImageSource =
                    DependencyService.Get<IHelper>().GetFileUri(userTracker.SideImage, FileType.User);

                userTracker.PictureHeight = GalleryImageSize.Height;
                userTracker.PictureWidth = GalleryImageSize.Width;
            }

            await PushModalAsync(GalleryPage);
        }

        public async Task HideGalleryDetailAsync()
        {
            await PopModalAsync();
        }

        /********** Tracker Gallery View : END **********/

        /********** Profile Chart View : START **********/

        private async Task GetTrackerData()
        {
            Entries = new List<Entry>();
            UserTrackers = UserTrackers.OrderBy(t => t.ModifyDate).ToList();
            int index = 0;
            foreach (var tracker in UserTrackers)
            {
                index++;
                var barColor = ChartColor.Get(index);
                tracker.WeightLost = (double) (StartWeight - (int) tracker.CurrentWeight);
                tracker.BackgroundColor = Xamarin.Forms.Color.FromHex(ChartColor.GetString(index));
                Entries.Add(new Entry((float) tracker.WeightLost)
                {
                    Label =
                        tracker
                            .RevisionNumberDisplayShort, // tracker.ModifyDate.Day.ToString() + " " + tracker.ModifyDate.Month.ToMonthShortNameCapital(),
                    ValueLabel = tracker.WeightLostDisplay.ToString(),
                    Color = barColor,
                    TextColor = barColor
                });
            }

            await SetChart();
        }

        public async Task SetChart(ChartType chartType = ChartType.LineChart)
        {
            try
            {
                ChartTypeDisplay = ChartDisplay.Get(chartType);
                var maxValue = WeightLossGoal + ((WeightLossGoal * 25) / 100);
                await Task.Run(() =>
                {
                    Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                    {
                        try
                        {
                            switch (chartType)
                            {
                                case ChartType.DonutChart:
                                    SetDonutChart();
                                    break;

                                case ChartType.LineChart:
                                    SetLineChart(maxValue);
                                    break;

                                case ChartType.PieChart:
                                    SetPieChart();
                                    break;

                                case ChartType.PointChart:
                                    SetPointChart(maxValue);
                                    break;

                                case ChartType.RadarChart:
                                    SetRadarChart();
                                    break;

                                case ChartType.RadialGaugeChart:
                                    SetRadialGaugeChart();
                                    break;
                            }
                        }
                        catch (Exception exception)
                        {
                            new ExceptionHandler("MyProfileViewModel:SetChart()::" + chartType, exception);
                        }
                    });
                });
            }
            catch (Exception ex)
            {
                new ExceptionHandler("MyProfileViewModel:SetChart()", ex);
            }
        }

        private void SetDonutChart()
        {
            ChartData = new DonutChart()
            {
                Entries = Entries,
                AnimationDuration = new TimeSpan(AnimationTime),
                IsAnimated = true,
                BackgroundColor = BackgroundColor
            };
        }

        private void SetLineChart(double maxValue = 100)
        {
            ChartData = new LineChart()
            {
                Entries = Entries,
                AnimationDuration = new TimeSpan(AnimationTime),
                IsAnimated = true,
                LabelOrientation = Orientation.Horizontal,
                PointMode = PointMode.Circle,
                ValueLabelOrientation = Orientation.Vertical,
                MinValue = 0,
                MaxValue = (float) maxValue,
                BackgroundColor = BackgroundColor
            };
        }

        private void SetPieChart()
        {
            ChartData = new PieChart()
            {
                Entries = Entries,
                AnimationDuration = new TimeSpan(AnimationTime),
                IsAnimated = true,
                BackgroundColor = BackgroundColor
            };
        }

        private void SetPointChart(double maxValue = 100)
        {
            ChartData = new PointChart()
            {
                Entries = Entries,
                AnimationDuration = new TimeSpan(AnimationTime),
                IsAnimated = true,
                LabelOrientation = Orientation.Horizontal,
                PointMode = PointMode.Square,
                ValueLabelOrientation = Orientation.Vertical,
                MinValue = 0,
                MaxValue = (float) maxValue,
                BackgroundColor = BackgroundColor
            };
        }

        private void SetRadarChart()
        {
            ChartData = new RadarChart()
            {
                Entries = Entries,
                AnimationDuration = new TimeSpan(AnimationTime),
                IsAnimated = true,
                BackgroundColor = BackgroundColor
            };
        }

        private void SetRadialGaugeChart()
        {
            ChartData = new RadialGaugeChart()
            {
                Entries = Entries,
                AnimationDuration = new TimeSpan(AnimationTime),
                IsAnimated = true,
                BackgroundColor = BackgroundColor
            };
        }

        protected long AnimationTime => Milliseconds;
        protected long Tick => 10000;
        protected long Milliseconds => 1000;
        protected long Seconds => 2;

        private string chartTypeDisplay;
        public const string ChartTypeDisplayPropertyName = "ChartTypeDisplay";

        public string ChartTypeDisplay
        {
            get { return chartTypeDisplay; }
            set { SetProperty(ref chartTypeDisplay, value, ChartTypeDisplayPropertyName); }
        }

        public static IEnumerable<T> GetValues<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<T>();
        }

        private List<UserTracker> userTrackers;
        public const string UserTrackersPropertyName = "UserTrackers";

        public List<UserTracker> UserTrackers
        {
            get
            {
                foreach (var tracker in userTrackers)
                {
                    if (StartWeight == 0)
                        StartWeight = tracker.CurrentWeight;
                    tracker.WeightLost = (short) (StartWeight - (int) tracker.CurrentWeight);
                }

                return userTrackers;
            }
            set { SetProperty(ref userTrackers, value, UserTrackersPropertyName); }
        }

        private List<UserTracker> userTrackersDescending;
        public const string UserTrackersDescendingPropertyName = "UserTrackersDescending";

        public List<UserTracker> UserTrackersDescending
        {
            get { return userTrackersDescending; }
            set { SetProperty(ref userTrackersDescending, value, UserTrackersDescendingPropertyName); }
        }

        private View content;
        public const string ContentPropertyName = "Content";

        public View Content
        {
            get { return content; }
            set { SetProperty(ref content, value, ContentPropertyName); }
        }

        private object chartData;
        public const string ChartDataPropertyName = "ChartData";

        public object ChartData
        {
            get { return chartData; }
            set { SetProperty(ref chartData, value, ChartDataPropertyName); }
        }

        protected List<Entry> Entries { get; set; }
        protected double Weight { get; set; }
        protected double WeightLossGoal { get; set; }

        public SKColor BackgroundColor => ChartColor.Get(0);

        public double _gaugeMin;
        private const string GaugeMinPropertyName = "GaugeMin";

        public double GaugeMin
        {
            get { return _gaugeMin; }
            set { SetProperty(ref _gaugeMin, value, GaugeMinPropertyName); }
        }

        public double _gaugeMax;
        private const string GaugeMaxPropertyName = "GaugeMax";

        public double GaugeMax
        {
            get { return _gaugeMax; }
            set { SetProperty(ref _gaugeMax, value, GaugeMaxPropertyName); }
        }

        public double _gaugeCurrent;
        private const string GaugeCurrentPropertyName = "GaugeCurrent";

        public double GaugeCurrent
        {
            get { return _gaugeCurrent; }
            set { SetProperty(ref _gaugeCurrent, value, GaugeCurrentPropertyName); }
        }

        public double _gaugeCurrentPercentage;
        private const string GaugeCurrentPercentagePropertyName = "GaugeCurrentPercentage";

        public double GaugeCurrentPercentage
        {
            get { return _gaugeCurrentPercentage; }
            set { SetProperty(ref _gaugeCurrentPercentage, value, GaugeCurrentPercentagePropertyName); }
        }

        public double GaugeCurrentConstant { get; set; }

        private ImageSource _badgeAchievedImage;
        public const string BadgeAchievedImagePropertyName = "BadgeAchievedImage";

        public ImageSource BadgeAchievedImage
        {
            get { return _badgeAchievedImage; }
            set { SetProperty(ref _badgeAchievedImage, value, BadgeAchievedImagePropertyName); }
        }

        private ICommand _showSideMenuCommand;

        public ICommand ShowSideMenuCommand
        {
            get
            {
                return _showSideMenuCommand ?? (_showSideMenuCommand = new Command((obj) =>
                {
                    Root.IsPresented = Root.IsPresented == false;
                }));
            }
        }

        private ICommand _badgeHintShowCommand;

        public ICommand BadgeHintShowCommand
        {
            get
            {
                return _badgeHintShowCommand ??
                       (_badgeHintShowCommand = new Command((obj) => { PopupAction?.Invoke(); }));
            }
        }

        public Action PopupAction { get; set; }

        public PopupType PopType { get; set; }

        private ICommand _closeCommand;

        public ICommand CloseCommand
        {
            get
            {
                return _closeCommand ?? (_closeCommand = new Command(
                           (obj) =>
                           {
                               if (PopType == PopupType.Detail)
                                   ShowTrackerDetail = false;
                               else if (PopType == PopupType.Gallery)
                                   ShowGalleryDetail = false;
                               PopType = PopupType.None;
                           }));
            }
        }

        /********** Profile Chart View : END **********/

        private void SetPageImageSize()
        {
            var imageSize = App.Configuration.GetImageSizeByID(ImageIdentity.USER_PROFILE_BADGE_ICON);
            if (imageSize != null)
            {
                UserBadgeImageHeight = imageSize.Height;
                UserBadgeImageWidth = imageSize.Width;
            }
        }

        public ImageSize GalleryImageSize { get; set; }

        private float userBadgeImageHeight;
        public const string UserBadgeImageHeightPropertyName = "UserBadgeImageHeight";

        public float UserBadgeImageHeight
        {
            get { return userBadgeImageHeight; }
            set { SetProperty(ref userBadgeImageHeight, value, UserBadgeImageHeightPropertyName); }
        }

        private float userBadgeImageWidth;
        public const string UserBadgeImageWidthPropertyName = "UserBadgeImageWidth";

        public float UserBadgeImageWidth
        {
            get { return userBadgeImageWidth; }
            set { SetProperty(ref userBadgeImageWidth, value, UserBadgeImageWidthPropertyName); }
        }

        private float pictureGalleryImageHeight;
        public const string PictureGalleryImageHeightPropertyName = "PictureGalleryImageHeight";

        public float PictureGalleryImageHeight
        {
            get { return pictureGalleryImageHeight; }
            set { SetProperty(ref pictureGalleryImageHeight, value, PictureGalleryImageHeightPropertyName); }
        }

        private float pictureGalleryImageWidth;
        public const string PictureGalleryImageWidthPropertyName = "PictureGalleryImageWidth";

        public float PictureGalleryImageWidth
        {
            get { return pictureGalleryImageWidth; }
            set { SetProperty(ref pictureGalleryImageWidth, value, PictureGalleryImageWidthPropertyName); }
        }
    }
}