using System;
using System.Collections.Generic;
using com.organo.xchallenge.Helpers;
using com.organo.xchallenge.Localization;
using com.organo.xchallenge.Models.User;
using com.organo.xchallenge.Services;
using com.organo.xchallenge.Statics;
using com.organo.xchallenge.ViewModels.Profile;
using Xamarin.Forms;

namespace com.organo.xchallenge.Controls
{
    /// <include file='docs.xml' path='[@name="gridTracker"]/GridTracker/*'/>
    public class GridTracker : Grid, IDisposable
    {
        private ITrackerPivotService _trackerPivotService;

        private ImageSource ImageLineSource => ImageResizer.ResizeImage("line.png", 727, 18);

        /// <include file='docs.xml' path='[@name="gridTracker"]/Constructor/*'/>
        public GridTracker()
        {
            _trackerPivotService = DependencyService.Get<ITrackerPivotService>();
        }

        /// <include file='docs.xml' path='[@name="gridTracker"]/Source/*'/>
        public List<TrackerPivot> Source
        {
            get => (List<TrackerPivot>) GetValue(SourceProperty);
            set => SetValue(SourceProperty, value);
        }

        public static readonly BindableProperty SourceProperty = BindableProperty.Create(nameof(Source),
            typeof(List<TrackerPivot>), typeof(GridTracker), null, BindingMode.TwoWay, null, OnGridTrackerChanged);

        /// <include file='docs.xml' path='[@name="gridTracker"]/ProfileModel/*'/>
        public ProfileEnhancedViewModel ProfileModel
        {
            get => (ProfileEnhancedViewModel) GetValue(ProfileModelProperty);
            set => SetValue(ProfileModelProperty, value);
        }

        public static readonly BindableProperty ProfileModelProperty = BindableProperty.Create(nameof(ProfileModel),
            typeof(ProfileEnhancedViewModel), typeof(GridTracker), null, BindingMode.OneWay, null);

        /// <include file='docs.xml' path='[@name="gridTracker"]/CloseAction/*'/>
        public Action CloseAction
        {
            get => (Action) GetValue(CloseActionProperty);
            set => SetValue(CloseActionProperty, value);
        }

        public static readonly BindableProperty CloseActionProperty = BindableProperty.Create(nameof(CloseAction),
            typeof(Action), typeof(GridTracker), null, BindingMode.TwoWay, null);

        /// <include file='docs.xml' path='[@name="gridTracker"]/OnGridTrackerChanged/*'/>
        private static void OnGridTrackerChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (GridTracker) bindable;
            if (control != null)
            {
                if (newValue is List<TrackerPivot> trackers)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        var rowNumber = -1;
                        var rowCount = 0;
                        double totalWeightLost = 0;

                        foreach (var tracker in trackers)
                        {
                            totalWeightLost += tracker.WeightLost;

                            // MODIFY DATE
                            var labelModifyDate = new Label()
                            {
                                Style = (Style) App.CurrentApp.Resources["labelStyleTextTitle"]
                            };
                            var formattedModifyDate = new FormattedString();
                            formattedModifyDate.Spans.Add(new Span
                            {
                                Text = tracker.ModifyDateDisplay,
                                FontAttributes = FontAttributes.Bold,
                            });
                            labelModifyDate.FormattedText = formattedModifyDate;

                            rowNumber++;
                            control.RowDefinitions.Add(new RowDefinition {Height = GridLength.Auto});
                            control.Children.Add(labelModifyDate, 0, rowNumber);


                            // CURRENT WEIGHT
                            var labelCurrentWeight = new Label()
                            {
                                Style = (Style) App.CurrentApp.Resources["labelStyleText"]
                            };
                            var formattedCurrentWeight = new FormattedString();
                            formattedCurrentWeight.Spans.Add(new Span
                            {
                                Text = TextResources.YourWeightIs + " ",
                                FontAttributes = FontAttributes.None
                            });
                            formattedCurrentWeight.Spans.Add(new Span
                            {
                                Text = tracker.CurrentWeightDisplay,
                                FontAttributes = FontAttributes.None,
                                ForegroundColor = Palette._MainAccent
                            });
                            labelCurrentWeight.FormattedText = formattedCurrentWeight;

                            rowNumber++;
                            control.RowDefinitions.Add(new RowDefinition {Height = GridLength.Auto});
                            control.Children.Add(labelCurrentWeight, 0, rowNumber);

                            // WEIGHT LOST
                            var labelWeightLost = new Label()
                            {
                                Style = (Style) App.CurrentApp.Resources["labelStyleText"]
                            };
                            var formattedWeightLost = new FormattedString();
                            formattedWeightLost.Spans.Add(new Span
                            {
                                Text = TextResources.YouLost + " ",
                                FontAttributes = FontAttributes.None
                            });
                            formattedWeightLost.Spans.Add(new Span
                            {
                                Text = tracker.WeightLostDisplay,
                                FontAttributes = FontAttributes.None,
                                ForegroundColor = Palette._MainAccent
                            });
                            labelWeightLost.FormattedText = formattedWeightLost;

                            rowNumber++;
                            control.RowDefinitions.Add(new RowDefinition {Height = GridLength.Auto});
                            control.Children.Add(labelWeightLost, 0, rowNumber);

                            // SHIRT SIZE
                            var labelShirtSize = new Label()
                            {
                                IsVisible = tracker.IsShirtSizeAvailable,
                                Style = (Style) App.CurrentApp.Resources["labelStyleText"]
                            };
                            var formattedShirtSize = new FormattedString();
                            formattedShirtSize.Spans.Add(new Span
                            {
                                Text = TextResources.TShirtSize + " ",
                                FontAttributes = FontAttributes.None
                            });
                            formattedShirtSize.Spans.Add(new Span
                            {
                                Text = tracker.ShirtSize,
                                FontAttributes = FontAttributes.None,
                                ForegroundColor = Palette._MainAccent
                            });
                            labelShirtSize.FormattedText = formattedShirtSize;

                            rowNumber++;
                            control.RowDefinitions.Add(new RowDefinition {Height = GridLength.Auto});
                            control.Children.Add(labelShirtSize, 0, rowNumber);

                            // SHIRT SIZE
                            var labelAboutJourney = new Label()
                            {
                                IsVisible = tracker.IsAboutJourneyAvailable,
                                Style = (Style) App.CurrentApp.Resources["labelStyleText"]
                            };
                            var formattedAboutJourney = new FormattedString();
                            formattedAboutJourney.Spans.Add(new Span
                            {
                                Text = TextResources.HeadingAboutYourJourney + " ",
                                FontAttributes = FontAttributes.None
                            });
                            formattedAboutJourney.Spans.Add(new Span
                            {
                                Text = tracker.AboutJourney,
                                FontAttributes = FontAttributes.None,
                                ForegroundColor = Palette._MainAccent
                            });
                            labelAboutJourney.FormattedText = formattedAboutJourney;

                            rowNumber++;
                            control.RowDefinitions.Add(new RowDefinition {Height = GridLength.Auto});
                            control.Children.Add(labelAboutJourney, 0, rowNumber);

                            // FRONT IMAGE
                            var imageFront = new Image()
                            {
                                Source = tracker.FrontImageSource,
                                HeightRequest = tracker.PictureHeight,
                                WidthRequest = tracker.PictureWidth,
                                HorizontalOptions = LayoutOptions.Start,
                                VerticalOptions = LayoutOptions.Start,
                            };
                            if (control.ProfileModel.UserDetail.IsDownloadAllowed)
                                imageFront.GestureRecognizers.Add(new TapGestureRecognizer()
                                {
                                    Command = new Command(() =>
                                    {
                                        Device.OpenUri(new Uri(tracker.FrontImageWithUrl));
                                    }),
                                    NumberOfTapsRequired = 2
                                });

                            // SIDE IMAGE
                            var imageSide = new Image()
                            {
                                Source = tracker.SideImageSource,
                                HeightRequest = tracker.PictureHeight,
                                WidthRequest = tracker.PictureWidth,
                                HorizontalOptions = LayoutOptions.Start,
                                VerticalOptions = LayoutOptions.Start
                            };
                            if (control.ProfileModel.UserDetail.IsDownloadAllowed)
                                imageSide.GestureRecognizers.Add(new TapGestureRecognizer()
                                {
                                    Command = new Command(() => { Device.OpenUri(new Uri(tracker.SideImageWithUrl)); }),
                                    NumberOfTapsRequired = 2
                                });

                            // IMAGES (FRONT, SIDE)
                            var stackLayoutImage = new StackLayout()
                            {
                                Orientation = StackOrientation.Horizontal,
                                HorizontalOptions = LayoutOptions.Center,
                                VerticalOptions = LayoutOptions.Start,
                                IsVisible = tracker.IsImageAvailable
                            };

                            // ADDING IMAGE
                            stackLayoutImage.Children.Add(imageFront);
                            stackLayoutImage.Children.Add(imageSide);

                            rowNumber++;
                            control.RowDefinitions.Add(new RowDefinition {Height = GridLength.Auto});
                            control.Children.Add(stackLayoutImage, 0, rowNumber);

                            var stackActions = new StackLayout()
                            {
                                HorizontalOptions = LayoutOptions.FillAndExpand,
                                VerticalOptions = LayoutOptions.Start,
                                IsVisible = tracker.IsDeleteAllowed
                            };

                            var buttonDelete = new Button()
                            {
                                Style = (Style) App.CurrentApp.Resources["buttonStyle"],
                                Text = TextResources.Delete + " " + tracker.ModifyDateDisplay,
                                Command = new Command(async () =>
                                {
                                    var response =
                                        await control._trackerPivotService.DeleteTrackerAsync(tracker.RevisionNumber);
                                    if (response != null && response.Contains(HttpConstants.SUCCESS))
                                    {
                                        control.CloseAction.Invoke();
                                        //var showTracker = control.ProfileModel.UserDetail.IsTrackerRequiredAfterDelete;
                                        //await control.ProfileModel.GetUserAsync(showTracker);
                                    }
                                })
                            };
                            stackActions.Children.Add(buttonDelete);

                            rowNumber++;
                            control.RowDefinitions.Add(new RowDefinition {Height = GridLength.Auto});
                            control.Children.Add(stackActions, 0, rowNumber);

                            if (rowCount + 1 >= trackers.Count)
                            {
                                continue;
                            }

                            // SETTING UP GRID CONTROLS
                            var stackLine = new StackLayout()
                            {
                                HorizontalOptions = LayoutOptions.FillAndExpand,
                                VerticalOptions = LayoutOptions.FillAndExpand,
                                BackgroundColor = Palette._LightGrayE,
                                Margin = new Thickness(25)
                            };

                            // LINE IMAGE
                            var imageLine = new Image()
                            {
                                Source = control.ImageLineSource,
                                HeightRequest = 18,
                                WidthRequest = 727,
                                HorizontalOptions = LayoutOptions.CenterAndExpand,
                                VerticalOptions = LayoutOptions.Center,
                                BackgroundColor = App.Configuration.BackgroundColor
                            };
                            stackLine.Children.Add(imageLine);

                            rowNumber++;
                            control.RowDefinitions.Add(new RowDefinition {Height = GridLength.Auto});
                            control.Children.Add(stackLine, 0, rowNumber);

                            rowCount++;
                        }
                    });
                }
            }
        }

        public void Dispose()
        {
            if (!isDispose)
            {
                isDispose = true;
                GC.SuppressFinalize(this);
            }
        }

        private bool isDispose = false;
    }
}