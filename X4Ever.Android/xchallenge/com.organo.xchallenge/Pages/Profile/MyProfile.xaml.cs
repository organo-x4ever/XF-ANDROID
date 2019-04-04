using com.organo.xchallenge.Controls;
using com.organo.xchallenge.Helpers;
using com.organo.xchallenge.Localization;
using com.organo.xchallenge.Pages.Base;
using com.organo.xchallenge.Services;
using com.organo.xchallenge.Statics;
using com.organo.xchallenge.Utilities;
using com.organo.xchallenge.ViewModels.Profile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using com.organo.xchallenge.Handler;
using Xamarin.Forms;

namespace com.organo.xchallenge.Pages.Profile
{
    public partial class MyProfile : MyProfileXaml
    {
        private MyProfileViewModel _model;
        private PopupLayout popupLayout;
        private readonly IAppVersionProvider _appVersionProvider = DependencyService.Get<IAppVersionProvider>();

        public MyProfile(RootPage root)
        {
            try
            {
                InitializeComponent();
                _model = new MyProfileViewModel(App.CurrentApp.MainPage.Navigation)
                {
                    Root = root,
                    PopupAction = OpenPopupWindow,
                    SliderGaugeModel = SliderGauge
                };
                BindingContext = _model;
                Init();
            }
            catch (Exception ex)
            {
                new ExceptionHandler(TAG, ex);
            }
        }

        private void Init()
        {
            App.Configuration.Initial(this);
            NavigationPage.SetHasNavigationBar(this, false);
            _model.GetPageData();
            Initialization();
        }

        public async void OpenPopupWindow()
        {
            var imageSize = App.Configuration.GetImageSizeByID(ImageIdentity.BADGE_HINT_WINDOW);
            Int16 height = 330, width = 306;
            if (imageSize != null)
            {
                height = (Int16) imageSize.Height;
                width = (Int16) imageSize.Width;
            }

            ClosePopupWindow();
            popupLayout = Content as PopupLayout;
            var stackLayout = new StackLayout()
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                BackgroundColor = Palette._Transparent,
                WidthRequest = width,
                HeightRequest = height,
                MinimumWidthRequest = width,
                MinimumHeightRequest = height,
            };

            var stackInner = new StackLayout()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Orientation = StackOrientation.Vertical,
                Margin = new Thickness(0)
            };
            stackLayout.Children.Add(stackInner);

            var closeImageSize = App.Configuration.GetImageSizeByID(ImageIdentity.BADGE_HINT_WINDOW_CLOSE);
            var closeImage = new Image()
            {
                Source = ImageResizer.ResizeImage(TextResources.icon_BadgeCloseCircle, closeImageSize),
                Style = (Style) App.CurrentApp.Resources["imageBadgeHintClose"],
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Margin = new Thickness(0, 0, 0, -10),
            };
            if (closeImageSize != null)
            {
                closeImage.WidthRequest = closeImageSize.Width;
                closeImage.HeightRequest = closeImageSize.Height;
                closeImage.MinimumWidthRequest = closeImageSize.Width;
                closeImage.MinimumHeightRequest = closeImageSize.Height;
            }

            var tapGestureRecognizer = new TapGestureRecognizer()
            {
                Command = new Command((obj) => { ClosePopupWindow(); })
            };
            closeImage.GestureRecognizers.Add(tapGestureRecognizer);
            stackInner.Children.Add(closeImage);

            var gridMain = new Grid()
            {
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                WidthRequest = width,
                HeightRequest = height,
                Margin = new Thickness(0),
                ColumnDefinitions =
                {
                    new ColumnDefinition() {Width = GridLength.Star}
                },
                RowDefinitions =
                {
                    new RowDefinition() {Height = 10},
                    new RowDefinition() {Height = GridLength.Auto},
                    new RowDefinition() {Height = GridLength.Auto},
                    new RowDefinition() {Height = GridLength.Star},
                }
            };

            var backgroundImage = new Image()
            {
                Source = ImageResizer.ResizeImage(TextResources.image_BadgeHintBackground, 300, 500),
                Aspect = Aspect.Fill
            };
            Grid.SetRowSpan(backgroundImage, 4);
            gridMain.Children.Add(backgroundImage);

            var titleLabel = new Label()
            {
                Text = TextResources.BadgesCAPS,
                Style = (Style) App.CurrentApp.Resources["labelStyleLargeMedium"],
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Margin = new Thickness(0, 15, 0, 0)
            };
            gridMain.Children.Add(titleLabel, 0, 1);

            var subTitleLabel = new Label()
            {
                Text = TextResources.BadgeHintSubTitle,
                Style = (Style) App.CurrentApp.Resources["labelStyleSmall"],
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Margin = new Thickness(0, -5, 0, 0)
            };
            gridMain.Children.Add(subTitleLabel, 0, 2);

            var gridRows = new Grid()
            {
                VerticalOptions = LayoutOptions.StartAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Margin = new Thickness(0, 10, 0, 0),
                ColumnDefinitions =
                {
                    new ColumnDefinition() {Width = GridLength.Auto},
                    new ColumnDefinition() {Width = GridLength.Star}
                },
                RowDefinitions =
                {
                    new RowDefinition() {Height = GridLength.Auto},
                    new RowDefinition() {Height = GridLength.Auto},
                    new RowDefinition() {Height = GridLength.Auto},
                }
            };

            gridMain.Children.Add(gridRows, 0, 3);

            var badgeImageSize = App.Configuration.GetImageSizeByID(ImageIdentity.BADGE_HINT_ICON);

            // Bronze hint description
            var bronzeImage = new Image()
            {
                Source = ImageResizer.ResizeImage(TextResources.Badge_Bronze, badgeImageSize),
                Style = (Style) App.CurrentApp.Resources["imageBadgeHintIcon"],
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };
            gridRows.Children.Add(bronzeImage, 0, 0);

            var bronzeLabel = new Label()
            {
                Text = TextResources.Badge_Bronze_Hint,
                Style = (Style) App.CurrentApp.Resources["labelStyleXSmall"],
                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalOptions = LayoutOptions.Center
            };
            gridRows.Children.Add(bronzeLabel, 1, 0);

            // Silver hint description
            var silverImage = new Image()
            {
                Source = ImageResizer.ResizeImage(TextResources.Badge_Silver, badgeImageSize),
                Style = (Style) App.CurrentApp.Resources["imageBadgeHintIcon"],
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.Center
            };
            gridRows.Children.Add(silverImage, 0, 1);

            var silverLabel = new Label()
            {
                Text = TextResources.Badge_Silver_Hint,
                Style = (Style) App.CurrentApp.Resources["labelStyleXSmall"],
                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalOptions = LayoutOptions.Center
            };
            gridRows.Children.Add(silverLabel, 1, 1);

            // Gold hint description
            var goldImage = new Image()
            {
                Source = ImageResizer.ResizeImage(TextResources.Badge_Gold, badgeImageSize),
                Style = (Style) App.CurrentApp.Resources["imageBadgeHintIcon"],
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };
            gridRows.Children.Add(goldImage, 0, 2);

            var goldLabel = new Label()
            {
                Text = TextResources.Badge_Gold_Hint,
                Style = (Style) App.CurrentApp.Resources["labelStyleXSmall"],
                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalOptions = LayoutOptions.Center
            };
            gridRows.Children.Add(goldLabel, 1, 2);

            if (badgeImageSize != null)
            {
                bronzeImage.WidthRequest = badgeImageSize.Width;
                bronzeImage.HeightRequest = badgeImageSize.Height;

                silverImage.WidthRequest = badgeImageSize.Width;
                silverImage.HeightRequest = badgeImageSize.Height;

                goldImage.WidthRequest = badgeImageSize.Width;
                goldImage.HeightRequest = badgeImageSize.Height;
            }

            stackInner.Children.Add(gridMain);
            popupLayout.ShowPopup(stackLayout);
            await Task.Delay(1);
        }

        public void ClosePopupWindow()
        {
            popupLayout = Content as PopupLayout;
            if (popupLayout != null && popupLayout.IsPopupActive)
            {
                popupLayout.DismissPopup();
            }
        }

        private void Initialization()
        {
            DependencyService.Get<IUserPushTokenServices>().SaveDeviceToken();
            VersionCheck();
        }

        async void VersionCheck()
        {
            await Task.Delay(TimeSpan.FromSeconds(3));
            if (!App.Configuration.IsVersionPrompt())
            {
                await _appVersionProvider.CheckAppVersionAsync(PromptUpdate);

                void PromptUpdate()
                {
                    try
                    {
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            var updateMessage = string.Format(TextResources.UpdateMessageArgs,
                                _appVersionProvider.AppName,
                                _appVersionProvider.UpdateVersion);
                            var response = await DisplayAlert(TextResources.UpdateTitle, updateMessage,
                                TextResources.Update,
                                TextResources.Later);
                            App.Configuration.VersionPrompted();
                            if (response)
                                _appVersionProvider.GotoGoogleAppStoreAsync();
                        });
                    }
                    catch (Exception e)
                    {
                        new ExceptionHandler(TAG, e);
                    }
                }
            }
        }

        private async Task ChartSetupAsync()
        {
            await Task.Run(() =>
            {
                var chartTypes = EnumUtil.GetValues<ChartType>();
                var list = new List<string>();
                foreach (var chartType in chartTypes)
                {
                    list.Add(chartType.ToString());
                }

                pickerGraphType.ItemsSource = list;
            });
            pickerGraphType.SelectedIndexChanged += (sender1, e1) =>
            {
                var chartTypeSelected = pickerGraphType.SelectedItem;
                if (chartTypeSelected != null)
                {
                    _model.SetChart((ChartType) Enum.Parse(typeof(ChartType), chartTypeSelected.ToString()));
                }
            };
        }

        private async void ChooseGraphType(object sender, EventArgs args)
        {
            //pickerGraphType.Focus();

            var chartTypes = EnumUtil.GetValues<ChartType>().ToArray();
            string[] list = new string[chartTypes.Count()];
            for (var i = 0; i < chartTypes.Count(); i++)
            {
                list[i] = ChartDisplay.Get(chartTypes[i]);
            }

            var result = await DisplayActionSheet("Choose Graph", TextResources.Cancel, null, list);
            if (result != null)
            {
                if (result != TextResources.Cancel)
                    _model.SetChart((ChartType) Enum.Parse(typeof(ChartType), result.Replace(" ", "")));
            }
        }

        private void ShowDetail(object sender, EventArgs args)
        {
			if (!_model.ShowTrackerDetail)
            {
				_model.ShowTrackerDetail = true;
				_model.ShowHideTrackerDetailAsync();
			}
        }

        private void ShowGallery(object sender, EventArgs args)
        {
            _model.ShowGalleryDetail = true;
        }

        protected override bool OnBackButtonPressed()
        {
            return DependencyService.Get<IBackButtonPress>().Exit();
        }
    }

    public abstract class MyProfileXaml : ModelBoundContentPage<MyProfileViewModel>
    {
    }
}