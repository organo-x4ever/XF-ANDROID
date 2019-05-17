using com.organo.xchallenge.Controls;
using com.organo.xchallenge.Localization;
using com.organo.xchallenge.Pages.Base;
using com.organo.xchallenge.Statics;
using com.organo.xchallenge.ViewModels.Video;
using Plugin.MediaManager.Forms;
using System;
using System.Threading.Tasks;
using com.organo.xchallenge.Handler;
using com.organo.xchallenge.Helpers;
using com.organo.xchallenge.Models;
using com.organo.xchallenge.Services;
using Xamarin.Forms;

namespace com.organo.xchallenge.Pages.Video
{
    public partial class PlaylistPage : PlaylistPageXaml
    {
        private PlaylistViewModel _model;
        private PopupLayout _popupLayout;
        private readonly IDeviceInfo _deviceInfo;

        public PlaylistPage(RootPage root)
        {
            try
            {
                InitializeComponent();
                _deviceInfo = DependencyService.Get<IDeviceInfo>();
                _model = new PlaylistViewModel(App.CurrentApp.MainPage.Navigation)
                {
                    Root = root,
                    BindDataSourceAction = () =>
                    {
                        AccordionMain.DataSource = _model.AccordionSources;
                        AccordionMain.DataBind();
                    },
                    PopupAction = OpenPopupWindow,
                    ClosePopupAction = CloseWindow
                };
                Init();
            }
            catch (Exception ex)
            {
                var exceptionHandler = new ExceptionHandler(TAG, ex);
            }
        }

        private async void Init()
        {
            await App.Configuration.InitialAsync(this);
            NavigationPage.SetHasNavigationBar(this, false);

            BindingContext = _model;
            AccordionMain.FirstExpaned = true;
            await Page_Load();
        }

        private async Task Page_Load()
        {
            await _model.UpdateButtonSelected(ButtonSelected.Beginner);
        }

        public async void OpenPopupWindow()
        {
            var imageSizeWindow = await App.Configuration.GetImageSizeByIDAsync(ImageIdentity.WORKOUT_VIDEO_WINDOW);
            int height = 340, width = 360;
            if (imageSizeWindow != null)
            {
                height = (int) imageSizeWindow.Height;
                width = (int) imageSizeWindow.Width;
            }

            if (_deviceInfo.WidthPixels != 0)
            {
                var d = _deviceInfo.ScaledDensity;
                width = _deviceInfo.WidthPixels / (int)d;
                width -= 60;
                height = width - 10;
            }

            CloseWindow();
            _popupLayout = Content as PopupLayout;
            var stackLayout = new StackLayout()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.Center,
                BackgroundColor = Palette._Transparent,
                Orientation = StackOrientation.Vertical,
                HeightRequest = height,
                WidthRequest = width
            };
            StackLayout stackLayoutTitle = new StackLayout()
            {
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.End,
                Orientation = StackOrientation.Horizontal,
                BackgroundColor = Palette._Transparent,
            };
            //Label labelTitle = new Label()
            //{
            //    Text = _model.CurrentMediaContent.MediaTitle,
            //    LineBreakMode = LineBreakMode.TailTruncation,
            //    Style = (Style) App.CurrentApp.Resources["labelStyleInfoHeading"],
            //    HorizontalOptions = LayoutOptions.Start,
            //    Margin = new Thickness(3, 0, 0, 0)
            //};
            //Label labelSets = new Label()
            //{
            //    Text = (_model.CurrentMediaContent.SetsAndRepeats != null &&
            //            _model.CurrentMediaContent.SetsAndRepeats.Trim().Length > 0
            //        ? " [" + _model.CurrentMediaContent.SetsAndRepeats + "]"
            //        : ""),
            //    LineBreakMode = LineBreakMode.TailTruncation,
            //    Style = (Style) App.CurrentApp.Resources["labelStyleInfoHeading"],
            //    HorizontalOptions = LayoutOptions.StartAndExpand,
            //};
            //var imageSize = App.Configuration.GetImageSizeByID(ImageIdentity.TOP_BAR_CLOSE);
            //Image imageClose = new Image()
            //{
            //    Source = ImageResizer.ResizeImage(TextResources.icon_close, imageSize),
            //    Style = (Style) App.CurrentApp.Resources["imagePopupClose"],
            //    Margin = new Thickness(0, 2, 5, 2)
            //};
            
            var closeImageSize = App.Configuration.GetImageSizeByID(ImageIdentity.BADGE_HINT_WINDOW_CLOSE);
            var closeImage = new Image()
            {
                Source = ImageResizer.ResizeImage(TextResources.icon_BadgeCloseCircle, closeImageSize),
                Style = (Style) App.CurrentApp.Resources["imageBadgeHintClose"],
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Margin = new Thickness(0),
                WidthRequest = 60,
                HeightRequest = 60
            };

            GestureRecognizer gestureRecognizer = new TapGestureRecognizer()
            {
                Command = new Command(CloseWindow)
            };
            closeImage.GestureRecognizers.Add(gestureRecognizer);
            //if (imageSize != null)
            //{
            //    imageClose.HeightRequest = imageSize.Height;
            //    imageClose.WidthRequest = imageSize.Width;
            //}

            //stackLayoutTitle.Children.Add(labelTitle);
            //stackLayoutTitle.Children.Add(labelSets);
            stackLayoutTitle.Children.Add(closeImage);

            var videoView = new VideoView()
            {
                HeightRequest = height,
                WidthRequest = width,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                BackgroundColor = Palette._Transparent
            };
            videoView.SetBinding(VideoView.SourceProperty,
                new Binding("Source", BindingMode.OneWay, null, null, "{0}"));

            stackLayout.Children.Add(stackLayoutTitle);
            stackLayout.Children.Add(videoView);
            _popupLayout.ShowPopup(stackLayout);
            _model.UpdateCurrentMedia();
        }

        public void CloseWindow()
        {
            _popupLayout = Content as PopupLayout;
            if (_popupLayout.IsPopupActive)
            {
                _model.StopPlayer();
                _model.IsPlaying = false;
                _popupLayout.DismissPopup();
            }
        }

        protected override void OnDisappearing()
        {
            _model.StopPlayer();
            base.OnDisappearing();
        }

        protected override bool OnBackButtonPressed()
        {
            return DependencyService.Get<IBackButtonPress>().Redirect(_model.Root);
        }
    }

    public abstract class PlaylistPageXaml : ModelBoundContentPage<PlaylistViewModel>
    {
    }
}