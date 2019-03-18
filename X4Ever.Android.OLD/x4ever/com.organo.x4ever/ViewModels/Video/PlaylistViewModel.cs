using com.organo.x4ever.Controls;
using com.organo.x4ever.Localization;
using com.organo.x4ever.Models.Media;
using com.organo.x4ever.Pages;
using com.organo.x4ever.Services;
using com.organo.x4ever.Statics;
using com.organo.x4ever.ViewModels.Base;
using Plugin.MediaManager;
using Plugin.MediaManager.Abstractions;
using Plugin.MediaManager.Abstractions.Enums;
using Plugin.MediaManager.Abstractions.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using com.organo.x4ever.Globals;
using com.organo.x4ever.Helpers;
using com.organo.x4ever.Models;
using Xamarin.Forms;
using mediaFile = com.organo.x4ever.Models.Media;

namespace com.organo.x4ever.ViewModels.Video
{
    public class PlaylistViewModel : BaseViewModel
    {
        private readonly IMediaContentService _mediaContentService;
        private readonly IHelper helper;
        private IPlaybackController PlaybackController => CrossMediaManager.Current.PlaybackController;

        public PlaylistViewModel(INavigation navigation = null, Action postAction = null) : base(navigation)
        {
            _mediaContentService = DependencyService.Get<IMediaContentService>();
            helper = DependencyService.Get<IHelper>();
            MediaContents = new List<mediaFile.MediaContentDetail>();
            AccordionSources = new List<AccordionExtendedSource>();
            BindDataSourceAction = postAction;
            MessageText = string.Empty;
            MessageVisible = false;
            IsPlaying = false;
            ButtonSelectedPreviously = ButtonSelected.None;

            CrossMediaManager.Current.MediaFileChanged +=
                (object sender, Plugin.MediaManager.Abstractions.EventArguments.MediaFileChangedEventArgs e) =>
                {
                    if (IsPlaying)
                    {
                        ClosePopupAction();
                    }
                    else
                        IsPlaying = true;
                };
        }

        private async Task<List<AccordionExtendedSource>> GetData(ButtonSelected buttonSelected)
        {
            if (buttonSelected == ButtonSelected.None)
                return new List<AccordionExtendedSource>();

            var listWorkoutLevel = (await GetAsync()).FirstOrDefault(l =>
                (l.LevelDisplay.ToString().ToLower() ?? "") == buttonSelected.ToString().ToLower());
            var accordionSources = new List<AccordionExtendedSource>();
            var listWorkoutWeeks = listWorkoutLevel?.WorkoutWeeks ?? new List<mediaFile.WorkoutWeek>();

            var imageSize = App.Configuration.GetImageSizeByID(ImageIdentity.VIDEO_PLAYER_PAGE_COMMAND_IMAGE);
            //var imageSizeExpand = App.Configuration.GetImageSizeByID(ImageIdentity.WORKOUT_EXPAND_COLLAPSE_IMAGE);
            //var imageSizePlayIcon = App.Configuration.GetImageSizeByID(ImageIdentity.WORKOUT_PLAY_ICON);
            foreach (var week in listWorkoutWeeks)
            {
                var accordionSourceItems = new List<AccordionExtendedSourceItem>();
                foreach (var day in week.WorkoutDays)
                {
                    var list = new List<mediaFile.MediaContentDetail>();
                    foreach (var media in day.MediaContentDetails)
                    {
                        media.IsHeader = false;
                        media.TextStyle = (Style)App.CurrentApp.Resources["labelAccordionStyleItem"];
                        list.Add(media);
                    }

                    var mediaContentDetails = (from m in list
                                               orderby m.IsHeader descending, m.WorkoutWeek, m.WorkoutDay, m.ID
                                               select m).ToList();
                    var contentListView = new ListView()
                    {
                        // Source of data items.
                        ItemsSource = mediaContentDetails,
                        Header = new StackLayout
                        {
                            VerticalOptions = LayoutOptions.End,
                            HorizontalOptions = LayoutOptions.FillAndExpand,
                            Margin = new Thickness(5, 0, 5, 0),
                            Children =
                            {
                                new Grid()
                                {
                                    RowDefinitions = {new RowDefinition() {Height = GridLength.Star}},
                                    HorizontalOptions = LayoutOptions.FillAndExpand,
                                    ColumnDefinitions =
                                    {
                                        new ColumnDefinition() {Width = new GridLength(3, GridUnitType.Star)},
                                        new ColumnDefinition() {Width = GridLength.Auto}
                                    },
                                    Children =
                                    {
                                        new Label()
                                        {
                                            LineBreakMode = LineBreakMode.TailTruncation,
                                            VerticalOptions = LayoutOptions.End,
                                            HorizontalOptions = LayoutOptions.StartAndExpand,
                                            Style = (Style) App.CurrentApp.Resources["labelAccordionStyleItemHeader"],
                                            Text = TextResources.Header_Exercise
                                        },
                                        new StackLayout() {WidthRequest = 1},
                                        new Label()
                                        {
                                            LineBreakMode = LineBreakMode.TailTruncation,
                                            VerticalOptions = LayoutOptions.End,
                                            HorizontalOptions = LayoutOptions.EndAndExpand,
                                            Style = (Style) App.CurrentApp.Resources["labelAccordionStyleItemHeader"],
                                            Text = TextResources.Header_SetsAndRepeats,
                                            Margin=new Thickness(10,0,0,0)
                                        }
                                    }
                                }
                            }
                        },

                        // Define template for displaying each item. (Argument of DataTemplate
                        // constructor is called for each item; it must return a Cell derivative.)
                        ItemTemplate = new DataTemplate(() =>
                        {
                            // Create views with bindings for displaying each property.
                            Label titleLabel = new Label()
                            {
                                LineBreakMode = LineBreakMode.TailTruncation,
                                VerticalOptions = LayoutOptions.Center,
                                Style = (Style)App.CurrentApp.Resources["labelAccordionStyleItem"]
                            };
                            titleLabel.SetBinding(Label.TextProperty,
                                new Binding("MediaTitle", BindingMode.OneWay, null, null, "{0}"));

                            Image image = new Image()
                            {
                                Style = (Style)App.CurrentApp.Resources["imageIconMenuItem"],
                                Source = ImageResizer.ResizeImage(TextResources.icon_play, imageSize),
                                VerticalOptions = LayoutOptions.Center
                            };
                            image.SetBinding(VisualElement.IsVisibleProperty,
                                new Binding("IsHeaderReverse", BindingMode.OneWay, null, null, "{0}"));

                            Label setsAndRepeatsLabel = new Label()
                            {
                                LineBreakMode = LineBreakMode.TailTruncation,
                                Style = (Style)App.CurrentApp.Resources["labelAccordionStyleItem"]
                            };
                            setsAndRepeatsLabel.SetBinding(Label.TextProperty,
                                new Binding("SetsAndRepeats", BindingMode.OneWay, null, null, "{0}"));

                            var grid = new Grid()
                            {
                                RowDefinitions = { new RowDefinition() { Height = GridLength.Star } },
                                ColumnDefinitions =
                                {
                                    new ColumnDefinition() {Width = new GridLength(3, GridUnitType.Star)},
                                    new ColumnDefinition() {Width = GridLength.Auto}
                                },
                                HorizontalOptions = LayoutOptions.FillAndExpand,
                            };

                            var stackLayoutColumnTitle = new StackLayout
                            {
                                VerticalOptions = LayoutOptions.Center,
                                Orientation = StackOrientation.Vertical,
                                HorizontalOptions = LayoutOptions.FillAndExpand,
                                Children =
                                {
                                    new StackLayout()
                                    {
                                        HeightRequest = .5,
                                        BackgroundColor = Palette._FooterTexts,
                                        HorizontalOptions = LayoutOptions.FillAndExpand,
                                    },
                                    new StackLayout
                                    {
                                        VerticalOptions = LayoutOptions.Center,
                                        Orientation = StackOrientation.Horizontal,
                                        HorizontalOptions = LayoutOptions.StartAndExpand,
                                        Children =
                                        {
                                            image,
                                            titleLabel
                                        }
                                    }
                                }
                            };
                            var stackLayoutColumnSets = new StackLayout
                            {
                                VerticalOptions = LayoutOptions.Center,
                                Orientation = StackOrientation.Vertical,
                                HorizontalOptions = LayoutOptions.FillAndExpand,
                                Children =
                                {
                                    new StackLayout()
                                    {
                                        HeightRequest = .5,
                                        BackgroundColor = Palette._FooterTexts,
                                        HorizontalOptions = LayoutOptions.FillAndExpand,
                                    },
                                    setsAndRepeatsLabel
                                }
                            };

                            grid.Children.Add(stackLayoutColumnTitle, 0, 0);
                            grid.Children.Add(new StackLayout() { WidthRequest = 1 }, 1, 0);
                            grid.Children.Add(stackLayoutColumnSets, 2, 0);

                            // Return an assembled ViewCell.
                            return new ViewCell
                            {
                                View = new StackLayout
                                {
                                    Orientation = StackOrientation.Vertical,
                                    VerticalOptions = LayoutOptions.Center,
                                    HorizontalOptions = LayoutOptions.FillAndExpand,
                                    Padding = new Thickness(5, 0, 5, 20),
                                    Children =
                                    {
                                        grid
                                    },
                                },
                            };
                        }),
                        SeparatorVisibility = SeparatorVisibility.None,
                        Margin = new Thickness(0, 6, 0, 3),
                        VerticalOptions = LayoutOptions.Start,
                        RowHeight = 30,
                        BackgroundColor = Color.Transparent
                    };
                    contentListView.HeightRequest = (contentListView.RowHeight * (mediaContentDetails.Count)) + 20;
                    contentListView.ItemSelected += ContentListView_ItemSelected;

                    accordionSourceItems.Add(new AccordionExtendedSourceItem()
                    {
                        HeaderImage = TextResources.icon_plus_gray,
                        HeaderImageStyle = (Style)App.CurrentApp.Resources["imageExpandPlus"],
                        HeaderImageSelected = TextResources.icon_plus,

                        HeaderText = day.DayDisplay ?? "",
                        HeaderTextStyle = (Style)App.CurrentApp.Resources["labelStyleInfoHeading"],
                        HeaderTextSelectedStyle = (Style)App.CurrentApp.Resources["labelStyleInfoHighlight"],

                        HeaderNotePart1 = TextResources.NoteTapOn,
                        HeaderNotePart2 = TextResources.icon_play_gray,
                        HeaderNotePart3 = TextResources.NoteToWatchTheVideos,
                        HeaderNoteStyle = (Style)App.CurrentApp.Resources["labelStyleTextTitleIntern"],
                        HeaderNotePart2Style = (Style)App.CurrentApp.Resources["imageIconMenuItem"],

                        ContentItems = contentListView
                    });
                }

                accordionSources.Add(new AccordionExtendedSource()
                {
                    Title = week.WeekDisplay ?? "",
                    TitleStyle = (Style)App.CurrentApp.Resources["buttonStyleGray"],
                    TitleSelectedStyle = (Style)App.CurrentApp.Resources["buttonStyle"],
                    AccordionExtendedSourceItems = accordionSourceItems
                });
            }
            if (accordionSources.Count == 0)
                ShowMessage(buttonSelected);
            AccordionSources = accordionSources;
            return AccordionSources;
        }

        private async Task<List<mediaFile.WorkoutLevel>> GetAsync()
        {
            if (MediaContents.Count == 0)
            {
                var mediaContents = await _mediaContentService.GetDetailAsync();
                MediaContents = mediaContents.Where(c => c.Active == true).ToList();
            }

            await Task.Run(() =>
            {
                var workoutLevels = (from m in MediaContents
                                     group m by m.WorkoutLevel
                    into level
                                     select new mediaFile.WorkoutLevel()
                                     {
                                         LevelDisplay = level.Key,
                                         WorkoutWeeks = (from l in MediaContents
                                                         where l.WorkoutLevel == level.Key
                                                         group l by l.WorkoutWeek
                                             into week
                                                         select new mediaFile.WorkoutWeek()
                                                         {
                                                             WeekDisplay = TextResources.WeekCAPS + " " + week.Key,
                                                             WeekSequence = week.Key,
                                                             WorkoutDays = (from w in MediaContents
                                                                            where w.WorkoutLevel == level.Key && w.WorkoutWeek == week.Key
                                                                            group w by w.WorkoutDay
                                                                 into day
                                                                            select new mediaFile.WorkoutDay()
                                                                            {
                                                                                DayDisplay = TextResources.DayCAPS + " " + day.Key,
                                                                                DaySequence = day.Key,
                                                                                MediaContentDetails = MediaContents.Where(d =>
                                                                                    d.WorkoutLevel == level.Key && d.WorkoutWeek == week.Key &&
                                                                                    d.WorkoutDay == day.Key).ToList()
                                                                            }).ToList()
                                                         }).ToList()
                                     }).ToList();

                WorkoutLevels = workoutLevels;
            });

            return WorkoutLevels;
        }

        private void ContentListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            CurrentMediaContent = (mediaFile.MediaContentDetail)e.SelectedItem;
            if (!CurrentMediaContent.IsHeader)
                PopupActionCommand();
        }

        public async void UpdateCurrentMedia()
        {
            var queue = CrossMediaManager.Current.MediaQueue;
            queue.Clear();
            List<MediaFile> mediaFiles = new List<MediaFile>();
            IMediaFileMetadata metaData = new MediaFileMetadata();
            metaData = new MediaFileMetadata();
            metaData.DisplayTitle = CurrentMediaContent.MediaTitle;
            metaData.DisplayDescription = CurrentMediaContent.SetsAndRepeats;
            string source = helper.GetFilePath(CurrentMediaContent.MediaUrl, FileType.Video);
            //string source = "https://sandbox.oghq.ca/appcontent/welcome_video.mp4";
            // "https://archive.org/download/BigBuckBunny_328/BigBuckBunny_512kb.mp4";
            mediaFiles.Add(new MediaFile()
            {
                Url = source,
                Type = MediaFileType.Video,
                MetadataExtracted = false,
                Availability = ResourceAvailability.Remote,
                Metadata = metaData
            });
            CrossMediaManager.Current.MediaQueue.Repeat = RepeatType.RepeatAll;
            await CrossMediaManager.Current.Play(mediaFiles);
        }

        public async Task Play()
        {
            IsPlaying = false;
            await PlaybackController.Play();
        }

        public async void StopPlayer()
        {
            try
            {
                if (PlaybackController != null)
                    await PlaybackController.Stop();
            }
            catch (Exception)
            {
                // Commented
            }
        }

        private void ShowMessage(ButtonSelected buttonSelected)
        {
            MessageText = string.Format(TextResources.MessageNoWorkoutVideoFound,
                ButtonSelected.Beginner == buttonSelected ? TextResources.HealthyPlan :
                ButtonSelected.Moderate == buttonSelected ? TextResources.ActivePlan :
                ButtonSelected.Advanced == buttonSelected ? TextResources.ElitePlan : string.Empty);
            MessageVisible = MessageText != null && MessageText.Trim().Length > 0;
        }

        private ICommand _beginnerCommand;

        public ICommand BeginnerCommand
        {
            get
            {
                return _beginnerCommand ?? (_beginnerCommand = new Command(async (obj) =>
                {
                    await UpdateButtonSelected(ButtonSelected.Beginner);
                }));
            }
        }

        private ICommand _moderateCommand;

        public ICommand ModerateCommand
        {
            get
            {
                return _moderateCommand ?? (_moderateCommand = new Command(async (obj) =>
                {
                    await UpdateButtonSelected(ButtonSelected.Moderate);
                }));
            }
        }

        private ICommand _advancedCommand;

        public ICommand AdvancedCommand
        {
            get
            {
                return _advancedCommand ?? (_advancedCommand = new Command(async (obj) =>
                {
                    await UpdateButtonSelected(ButtonSelected.Advanced);
                }));
            }
        }

        public async Task UpdateButtonSelected(ButtonSelected buttonSelected = ButtonSelected.None)
        {
            if (ButtonSelectedPreviously != ButtonSelected.None && ButtonSelectedPreviously == buttonSelected)
                return;
            BeginnerStyle = LabelStyleDefault;
            ModerateStyle = LabelStyleDefault;
            AdvancedStyle = LabelStyleDefault;

            var imageSize = App.Configuration.GetImageSizeByID(ImageIdentity.WORKOUT_OPTIONS_IMAGE);
            if (imageSize == null)
                imageSize = new ImageSize();

            BeginnerImage = ImageResizer.ResizeImage(TextResources.icon_meal_healthy_dark, imageSize);
            ModerateImage = ImageResizer.ResizeImage(TextResources.icon_meal_active_dark, imageSize);
            AdvancedImage = ImageResizer.ResizeImage(TextResources.icon_meal_elite_dark, imageSize);

            switch (buttonSelected)
            {
                case ButtonSelected.Beginner:
                    BeginnerStyle = LabelStyleSelected;
                    BeginnerImage = ImageResizer.ResizeImage(TextResources.icon_meal_healthy, imageSize);
                    break;

                case ButtonSelected.Moderate:
                    ModerateStyle = LabelStyleSelected;
                    ModerateImage = ImageResizer.ResizeImage(TextResources.icon_meal_active, imageSize);
                    break;

                case ButtonSelected.Advanced:
                    AdvancedStyle = LabelStyleSelected;
                    AdvancedImage = ImageResizer.ResizeImage(TextResources.icon_meal_elite, imageSize);
                    break;
            }

            ButtonSelectedPreviously = buttonSelected;
            await GetData(buttonSelected);
        }

        private ButtonSelected ButtonSelectedPreviously { get; set; }

        public Action BindDataSourceAction { get; set; }

        private void ExecuteBind()
        {
            BindDataSourceAction?.Invoke();
        }

        public Action PopupAction { get; set; }

        private void PopupActionCommand()
        {
            PopupAction?.Invoke();
        }

        public Action ClosePopupAction { get; set; }

        public void ClosePopup()
        {
            ClosePopupAction?.Invoke();
        }

        private RootPage root;
        public const string RootPropertyName = "Root";

        public RootPage Root
        {
            get { return root; }
            set { SetProperty(ref root, value, RootPropertyName); }
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

        private List<AccordionExtendedSource> _accordionSources;
        public const string AccordionSourcesPropertyName = "AccordionSources";

        public List<AccordionExtendedSource> AccordionSources
        {
            get { return _accordionSources; }
            set { SetProperty(ref _accordionSources, value, AccordionSourcesPropertyName, ExecuteBind); }
        }

        private List<mediaFile.WorkoutLevel> _workoutLevels;
        public const string WorkoutLevelsPropertyName = "WorkoutLevels";

        public List<mediaFile.WorkoutLevel> WorkoutLevels
        {
            get { return _workoutLevels; }
            set { SetProperty(ref _workoutLevels, value, WorkoutLevelsPropertyName); }
        }

        private List<mediaFile.MediaContentDetail> _mediaContents;
        public const string MediaContentsPropertyName = "MediaContents";

        public List<mediaFile.MediaContentDetail> MediaContents
        {
            get { return _mediaContents; }
            set { SetProperty(ref _mediaContents, value, MediaContentsPropertyName); }
        }

        private mediaFile.MediaContentDetail _currentMediaContent;
        public const string CurrentMediaContentPropertyName = "CurrentMediaContent";

        public mediaFile.MediaContentDetail CurrentMediaContent
        {
            get { return _currentMediaContent; }
            set { SetProperty(ref _currentMediaContent, value, CurrentMediaContentPropertyName); }
        }

        public Style ButtonStyleDefault = (Style)App.CurrentApp.Resources["buttonAccordion"];
        public Style ButtonStyleSelected = (Style)App.CurrentApp.Resources["buttonAccordionHighlight"];

        public Style LabelStyleDefault = (Style)App.CurrentApp.Resources["labelStyleAccordion"];
        public Style LabelStyleSelected = (Style)App.CurrentApp.Resources["labelStyleAccordionHighlight"];

        private Style _beginnerStyle;
        public const string BeginnerStylePropertyName = "BeginnerStyle";

        public Style BeginnerStyle
        {
            get { return _beginnerStyle; }
            set { SetProperty(ref _beginnerStyle, value, BeginnerStylePropertyName); }
        }

        private Style _moderateStyle;
        public const string ModerateStylePropertyName = "ModerateStyle";

        public Style ModerateStyle
        {
            get { return _moderateStyle; }
            set { SetProperty(ref _moderateStyle, value, ModerateStylePropertyName); }
        }

        private Style _advancedStyle;
        public const string AdvancedStylePropertyName = "AdvancedStyle";

        public Style AdvancedStyle
        {
            get { return _advancedStyle; }
            set { SetProperty(ref _advancedStyle, value, AdvancedStylePropertyName); }
        }

        private ImageSource _beginnerImage;
        public const string BeginnerImagePropertyName = "BeginnerImage";

        public ImageSource BeginnerImage
        {
            get { return _beginnerImage; }
            set { SetProperty(ref _beginnerImage, value, BeginnerImagePropertyName); }
        }

        private ImageSource _moderateImage;
        public const string ModerateImagePropertyName = "ModerateImage";

        public ImageSource ModerateImage
        {
            get { return _moderateImage; }
            set { SetProperty(ref _moderateImage, value, ModerateImagePropertyName); }
        }

        private ImageSource _advancedImage;
        public const string AdvancedImagePropertyName = "AdvancedImage";

        public ImageSource AdvancedImage
        {
            get { return _advancedImage; }
            set { SetProperty(ref _advancedImage, value, AdvancedImagePropertyName); }
        }

        private Color _backgroundColor;
        public const string BackgroundColorPropertyName = "BackgroundColor";

        public Color BackgroundColor
        {
            get { return _backgroundColor; }
            set { SetProperty(ref _backgroundColor, value, BackgroundColorPropertyName); }
        }

        public bool IsPlaying { get; set; }

        private bool _messageVisible;
        public const string MessageVisiblePropertyName = "MessageVisible";

        public bool MessageVisible
        {
            get { return _messageVisible; }
            set { SetProperty(ref _messageVisible, value, AdvancedStylePropertyName); }
        }
    }

    public enum ButtonSelected
    {
        None,
        Beginner,
        Moderate,
        Advanced
    }

    public class ListDataViewCell : ViewCell
    {
        public ListDataViewCell()
        {
            var label = new Label();
            label.SetBinding(Label.TextProperty, new Binding("TextValue"));
            label.SetBinding(Label.ClassIdProperty, new Binding("DataValue"));
            View = new StackLayout()
            {
                Orientation = StackOrientation.Vertical,
                VerticalOptions = LayoutOptions.StartAndExpand,
                Padding = new Thickness(12, 8),
                Children = { label }
            };
        }
    }

    public class SimpleObject
    {
        public string TextValue { get; set; }
        public string DataValue { get; set; }
        public mediaFile.MediaContentDetail MediaDetail { get; set; }
    }
}