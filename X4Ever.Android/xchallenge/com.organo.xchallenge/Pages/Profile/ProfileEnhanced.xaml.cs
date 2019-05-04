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
using com.organo.xchallenge.Extensions;
using com.organo.xchallenge.Handler;
using Xamarin.Forms;
using com.organo.xchallenge.Triggers;
using com.organo.xchallenge.Views;
using com.organo.xchallenge.Animated;
using com.organo.xchallenge.Pages.ChangePassword;
using com.organo.xchallenge.Pages.Notification;
using com.organo.xchallenge.Pages.UserSettings;

namespace com.organo.xchallenge.Pages.Profile
{
    public partial class ProfileEnhanced : ProfileEnhancedXaml
    {
        private ProfileEnhancedViewModel _model;
        private PopupLayout popupLayout;
        private readonly IAppVersionProvider _appVersionProvider = DependencyService.Get<IAppVersionProvider>();

        public ProfileEnhanced(RootPage root)
        {
            try
            {
                InitializeComponent();
                _model = new ProfileEnhancedViewModel(App.CurrentApp.MainPage.Navigation)
                {
                    Root = root,
                    PopupAction = OpenPopupWindow,
                    SliderGaugeModel = SliderGauge,
                    UserSettingAction = AnimatePanel
                };
                BindingContext = _model;
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

            _model.GetPageData();
            await VersionCheck();

            await Task.Delay(TimeSpan.FromMilliseconds(2));
            UserSettingLayout();
        }

        //private RelativeLayout _layout;
        private StackLayout _panel;
        
        private bool _PanelShowing = false;
        /// <summary>
        /// Gets a value to determine if the panel is showing or not
        /// </summary>
        /// <value><c>true</c> if panel showing; otherwise, <c>false</c>.</value>
        public bool PanelShowing
        {
            get => _PanelShowing;
            set => _PanelShowing = value;
        }

        private void UserSettingLayout()
        {
            _layout.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command((obj) =>
                {
                    if (PanelShowing)
                        AnimatePanel();
                })
            });
            CreatePanel();
        }
        
        private double _panelWidth = -1;


        /// <summary>
        /// Creates the right side menu panel
        /// </summary>
        private void CreatePanel()
        {
            if (_panel == null)
            {
                _panel = new StackLayout
                {
                    Children =
                    {
                        new AnimatedImage(new FloatingActionButtonView
                        {
                            ColorNormal = Palette._MainAccent,
                            ColorRipple = Palette._ButtonBackground,
                            ColorPressed = Palette._LightGrayD,
                            HasShadow = false,
                            ImageName = ImageConstants.ICON_PROFILE_EDIT,
                            Size = FloatingActionButtonSize.NormalMini,
                            Clicked = async (sender, e) =>
                            {
                                AnimatePanel();
                                await Navigation.PushAsync(new ProfileSetting(), true);
                            }
                        }),
                        new AnimatedImage(new FloatingActionButtonView
                        {
                            ColorNormal = Palette._MainAccent,
                            ColorRipple = Palette._ButtonBackground,
                            ColorPressed = Palette._LightGrayD,
                            HasShadow = false,
                            ImageName = ImageConstants.ICON_PROFILE_PASSWORD,
                            Size = FloatingActionButtonSize.NormalMini,
                            Clicked = async (sender, e) =>
                            {
                                AnimatePanel();
                                await Navigation.PushAsync(new ChangePasswordPage(), true);
                            }
                        }),
                        new AnimatedImage(new FloatingActionButtonView
                        {
                            ColorNormal = Palette._MainAccent,
                            ColorRipple = Palette._ButtonBackground,
                            ColorPressed = Palette._LightGrayD,
                            HasShadow = false,
                            ImageName = ImageConstants.ICON_PROFILE_LANGUAGE,
                            Size = FloatingActionButtonSize.NormalMini,
                            Clicked = async (sender, e) =>
                            {
                                AnimatePanel();
                                await Navigation.PushAsync(new UserSettingPage(),true);
                                //ChangeBackgroundColor();
                            }
                        }),
                        new AnimatedImage(new FloatingActionButtonView
                        {
                            ColorNormal = Palette._MainAccent,
                            ColorRipple = Palette._ButtonBackground,
                            ColorPressed = Palette._LightGrayD,
                            HasShadow = false,
                            ImageName = ImageConstants.ICON_PROFILE_NOTIFICATION,
                            Size = FloatingActionButtonSize.NormalMini,
                            Clicked = async (sender, e) =>
                            {
                                AnimatePanel();
                                await Navigation.PushAsync(new NotificationSettingPage(),true);
                            }
                        })
                    },
                    Padding = new Thickness(1, 1),
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    HorizontalOptions = LayoutOptions.EndAndExpand,
                    BackgroundColor = Palette._Transparent //Color.FromRgba(0, 0, 0, 180)
                };

                // add to layout
                _layout.Children.Add(_panel
                    ,
                    Constraint.RelativeToParent((p) => { return _layout.Width - (PanelShowing ? _panelWidth : 0); }),
                    Constraint.RelativeToParent((p) => { return 0; }),
                    Constraint.RelativeToParent((p) =>
                    {
                        if (_panelWidth == -1)
                            _panelWidth = p.Width / 3;
                        return _panelWidth;
                    }),
                    Constraint.RelativeToParent((p) => { return p.Height; })
                );
            }

            _model.IsUserSettingVisible = App.Configuration.IsProfileEditAllowed && _panel.Children.Count > 0;
        }

        /// <summary>
        /// Animates the panel in our out depending on the state
        /// </summary>
        private async void AnimatePanel()
        {
            if (_panel == null)
                CreatePanel();

            // swap the state
            PanelShowing = !PanelShowing;

            // show or hide the panel
            if (PanelShowing)
            {
                // hide all children
                foreach (var child in _panel.Children)
                {
                    child.Scale = 0;
                }

                // layout the panel to slide out
                var rect = new Rectangle(_layout.Width - _panel.Width, _panel.Y, _panel.Width, _panel.Height);
                await _panel.LayoutTo(rect, 250, Easing.CubicIn);

                // scale in the children for the panel
                foreach (var child in _panel.Children)
                {
                    await child.ScaleTo(1.2, 50, Easing.CubicIn);
                    await child.ScaleTo(1, 50, Easing.CubicOut);
                }
            }
            else
            {

                // layout the panel to slide in
                var rect = new Rectangle(_layout.Width, _panel.Y, _panel.Width, _panel.Height);
                await _panel.LayoutTo(rect, 200, Easing.CubicOut);

                // hide all children
                foreach (var child in _panel.Children)
                {
                    child.Scale = 0;
                }
            }
        }

        /// <summary>
        /// Changes the background color of the relative layout
        /// </summary>
        private void ChangeBackgroundColor()
        {
            var repeatCount = 0;
            _layout.Animate(
                // set the name of the animation
                name: "changeBG",

                // create the animation object and callback
                animation: new Xamarin.Forms.Animation((val) =>
                {
                    // val will be a from 0 - 1 and can use that to set a BG color
                    if (repeatCount == 0)
                        _layout.BackgroundColor = Color.FromRgb(1 - val, 1 - val, 1 - val);
                    else
                        _layout.BackgroundColor = Color.FromRgb(val, val, val);
                }),

                // set the length
                length: 750,

                // set the repeat action to update the repeatCount
                finished: (val, b) =>
                {
                    repeatCount++;
                    if (repeatCount > 1)
                        _layout.BackgroundColor = App.Configuration.BackgroundColor;
                },

                // determine if we should repeat
                repeat: () =>
                {
                    return repeatCount < 1;
                }
            );
        }

        public async void OpenPopupWindow()
        {
            var imageSize = App.Configuration.GetImageSizeByID(ImageIdentity.BADGE_HINT_WINDOW);
            short height = 330, width = 306;
            if (imageSize != null)
            {
                height = (short) imageSize.Height;
                width = (short) imageSize.Width;
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
            await Task.Delay(TimeSpan.FromMilliseconds(1));
        }

        public void ClosePopupWindow()
        {
            popupLayout = Content as PopupLayout;
            if (popupLayout != null && popupLayout.IsPopupActive)
            {
                popupLayout.DismissPopup();
            }
        }

        //await DependencyService.Get<IUserPushTokenServices>().SaveDeviceToken();
        private async Task VersionCheck()
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
                        ExceptionHandler exceptionHandler = new ExceptionHandler(TAG, e);
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

        private async void ShowDetail(object sender, EventArgs args)
        {
            if (_model.TrackerPage == null)
                await _model.ProduceTrackerLog();
            await Navigation.PushAsync(_model.TrackerPage);
        }

        private string CurrentProfileID => ProfileModifyID;//_model.UpdateProfileRequired ? ProfileModifyID : TrackerModifyID;
        private string DefaultProfileID => "stackLayoutProfile"; //IdentityConstants.LAYOUT_PROFILE_ID;
        private string TrackerModifyID => "stackLayoutTracker"; //IdentityConstants.LAYOUT_TRACKER_ID;
        private string ProfileModifyID => "stackLayoutEditProfile"; //IdentityConstants.LAYOUT_EDIT_PROFILE_ID;

        bool ShowEditProfilePanel = false;
        protected void DisplayEditProfilePanel()
        {
            if (!ShowEditProfilePanel) // && !((View)buttonProfile.Parent.Parent).FindByName<View>(CurrentProfileID).IsVisible
                SwitchEditProfilePanel();
        }
        protected void HideEditProfilePanel()
        {
            if (ShowEditProfilePanel)// && ((View)buttonProfile.Parent.Parent).FindByName<View>(CurrentProfileID).IsVisible)
                SwitchEditProfilePanel();
        }

        protected void SwitchEditProfilePanel()
        {
            // swap the state
            ShowEditProfilePanel = !ShowEditProfilePanel;

            if (ShowEditProfilePanel)
            {
                SwitchView switchView = new SwitchView()
                {
                    Source = DefaultProfileID,
                    Target = CurrentProfileID,
                    Direction = SwitchView.eDirection.Left
                };
                Device.BeginInvokeOnMainThread(() =>
                {
                    switchView.Invoke(buttonProfile);
                    if (CurrentProfileID == ProfileModifyID)
                    {
                        //await EditProfile();
                    }
                    else
                    {
                        //await UpdateTracker();
                    }
                });
            }
            else
            {
                SwitchView switchView = new SwitchView()
                {
                    Source = CurrentProfileID,
                    Target = DefaultProfileID,
                    Direction = SwitchView.eDirection.Right
                };
                Device.BeginInvokeOnMainThread(() =>
                {
                    switchView.Invoke(buttonProfile);
                });
            }
        }


        protected override bool OnBackButtonPressed()
        {
            return DependencyService.Get<IBackButtonPress>().Exit();
        }
    }

    public abstract class ProfileEnhancedXaml : ModelBoundContentPage<ProfileEnhancedViewModel>
    {
    }
}