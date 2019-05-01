using com.organo.xchallenge.Animated;
using com.organo.xchallenge.Pages.Base;
using com.organo.xchallenge.ViewModels.Animation_Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.organo.xchallenge.Localization;
using com.organo.xchallenge.Pages.News;
using com.organo.xchallenge.Pages.Notification;
using com.organo.xchallenge.Statics;
using com.organo.xchallenge.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace com.organo.xchallenge.Pages.Animation_Test
{
    public partial class AnimatedButtonPage : AnimatedButtonPageXaml
    {
        public AnimatedButtonPage()
        {
            try
            {
                InitializeComponent();
                // create the layout
                _layout = new RelativeLayout
                {
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    HorizontalOptions = LayoutOptions.FillAndExpand
                };

                CreateButton();
                //CreatePanel();
                CreatePanelWithFloatingImage();

                // set the content
                Content = _layout;

                _layout.GestureRecognizers.Add(new TapGestureRecognizer()
                {
                    Command = new Command((obj) =>
                    {
                        if (PanelShowing)
                            AnimatePanel();
                    })
                });
            }
            catch (Exception ex)
            {
                var msg = ex;
            }
        }
        private RelativeLayout _layout;
        private StackLayout _panel;

        private void CreateButton()
        {
            // create the button
            _layout.Children.Add(
                new AnimatedButton("Show Panel", AnimatePanel)
                {
                    BackgroundColor = Color.Red,
                    TextColor = Color.White,
                    Padding = 20,
                },
                    Constraint.RelativeToParent((p) =>
                    {
                        return 10;
                    }),
                    Constraint.RelativeToParent((p) =>
                    {
                        return Device.OnPlatform(28, 0, 0);
                    }),
                    Constraint.RelativeToParent((p) =>
                    {
                        return p.Width - (10 * 2);
                    })
                );
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
                        new Label
                        {
                            Text = "Options",
                            HorizontalOptions = LayoutOptions.Start,
                            VerticalOptions = LayoutOptions.Start,
                            HorizontalTextAlignment = TextAlignment.Center,
                            TextColor = Color.White
                        },
                        new AnimatedButton("Option 1", () =>
                        {
                            AnimatePanel();
                            ChangeBackgroundColor();
                        }),
                        new AnimatedButton("Option 2", () => { AnimatePanel(); }),
                        new AnimatedButton("Option 3", () => { AnimatePanel(); }),
                    },
                    Padding = 15,
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    HorizontalOptions = LayoutOptions.EndAndExpand,
                    BackgroundColor = Palette._Transparent //Color.FromRgba(0, 0, 0, 180)
                };

                // add to layout
                _layout.Children.Add(_panel,
                    Constraint.RelativeToParent((p) =>
                    {
                        return _layout.Width - (PanelShowing ? _panelWidth : 0);
                    }),
                    Constraint.RelativeToParent((p) =>
                    {
                        return 0;
                    }),
                    Constraint.RelativeToParent((p) =>
                    {
                        if (_panelWidth == -1)
                            _panelWidth = p.Width / 3;
                        return _panelWidth;
                    }),
                    Constraint.RelativeToParent((p) =>
                    {
                        return p.Height;
                    })
                );
            }
        }

        /// <summary>
        /// Creates the right side menu panel
        /// </summary>
        private void CreatePanelWithFloatingImage()
        {
            if (_panel == null)
            {
                _panel = new StackLayout
                {
                    Children =
                    {
                        new Label
                        {
                            Text = "Options",
                            HorizontalOptions = LayoutOptions.Center,
                            VerticalOptions = LayoutOptions.Start,
                            HorizontalTextAlignment = TextAlignment.Center,
                            TextColor = Color.White
                        },
                        new AnimatedImage(new FloatingActionButtonView
                        {
                            ColorNormal = Palette._MainAccent,
                            ColorRipple = Palette._MainAccent,
                            ColorPressed = Palette._ButtonBackground,
                            HasShadow = false,
                            ImageName = TextResources.icon_edit_profile_ar,
                            Size = FloatingActionButtonSize.Normal,
                            Clicked = async (sender, e) =>
                            {
                                AnimatePanel();
                                //ChangeBackgroundColor();

                                await Navigation.PushAsync(new NotificationPage());
                                //new XNavigationPage(new NotificationPage());
                            }
                        }),
                        new AnimatedImage(new FloatingActionButtonView
                        {
                            ColorNormal = Palette._MainAccent,
                            ColorRipple = Palette._MainAccent,
                            ColorPressed = Palette._ButtonBackground,
                            HasShadow = false,
                            ImageName = TextResources.icon_password_ar,
                            Size = FloatingActionButtonSize.Normal,
                            Clicked = async (sender, e) =>
                            {
                                AnimatePanel();
                                //ChangeBackgroundColor();
                                await App.CurrentApp.MainPage.Navigation.PushAsync(new NotificationSettingPage());
                            }
                        }),
                        new AnimatedImage(new FloatingActionButtonView
                        {
                            ColorNormal = Palette._MainAccent,
                            ColorRipple = Palette._MainAccent,
                            ColorPressed = Palette._ButtonBackground,
                            HasShadow = false,
                            ImageName = TextResources.icon_user_settings_ar,
                            Size = FloatingActionButtonSize.Normal,
                            Clicked = (sender, e) =>
                            {
                                AnimatePanel();
                                ChangeBackgroundColor();
                            }
                        }),
                    },
                    Padding = 15,
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    HorizontalOptions = LayoutOptions.EndAndExpand,
                    BackgroundColor = Palette._Transparent //Color.FromRgba(0, 0, 0, 180)
                };

                // add to layout
                _layout.Children.Add(_panel,
                    Constraint.RelativeToParent((p) =>
                    {
                        return _layout.Width - (PanelShowing ? _panelWidth : 0);
                    }),
                    Constraint.RelativeToParent((p) =>
                    {
                        return 0;
                    }),
                    Constraint.RelativeToParent((p) =>
                    {
                        if (_panelWidth == -1)
                            _panelWidth = p.Width / 3;
                        return _panelWidth;
                    }),
                    Constraint.RelativeToParent((p) =>
                    {
                        return p.Height;
                    })
                );
            }
        }

        private bool _PanelShowing = false;
        /// <summary>
        /// Gets a value to determine if the panel is showing or not
        /// </summary>
        /// <value><c>true</c> if panel showing; otherwise, <c>false</c>.</value>
        private bool PanelShowing
        {
            get
            {
                return _PanelShowing;
            }
            set
            {
                _PanelShowing = value;
            }
        }

        /// <summary>
        /// Animates the panel in our out depending on the state
        /// </summary>
        private async void AnimatePanel()
        {

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
                },

                // determine if we should repeat
                repeat: () =>
                {
                    return repeatCount < 1;
                }
            );
        }
    }
    public abstract class AnimatedButtonPageXaml : ModelBoundContentPage<AnimatedButtonViewModel> { }
}