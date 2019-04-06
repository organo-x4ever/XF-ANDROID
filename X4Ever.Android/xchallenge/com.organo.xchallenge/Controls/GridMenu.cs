using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.organo.xchallenge.Pages;
using com.organo.xchallenge.Statics;
using Xamarin.Forms;

namespace com.organo.xchallenge.Controls
{
    public class GridMenu : Grid
    {
        public string ApplicationVersion
        {
            get => (string) GetValue(ApplicationVersionProperty);
            set => SetValue(ApplicationVersionProperty, value);
        }

        public static readonly BindableProperty ApplicationVersionProperty = BindableProperty.Create(
            nameof(ApplicationVersion),
            typeof(string), typeof(GridMenu), null, BindingMode.OneWay, null);

        public RootPage RootPage
        {
            get => (RootPage) GetValue(RootPageProperty);
            set => SetValue(RootPageProperty, value);
        }

        public static readonly BindableProperty RootPageProperty = BindableProperty.Create(
            nameof(RootPage),
            typeof(RootPage), typeof(GridMenu), null, BindingMode.OneWay, null);

        public List<HomeMenuItem> Source
        {
            get => (List<HomeMenuItem>) GetValue(SourceProperty);
            set => SetValue(SourceProperty, value);
        }

        public static readonly BindableProperty SourceProperty = BindableProperty.Create(nameof(Source),
            typeof(List<HomeMenuItem>), typeof(GridMenu), null, BindingMode.TwoWay, null, OnGridMenuChanged);
        
        private Style DefaultStyle => (Style) App.CurrentApp.Resources["labelStyleMenuItem"];
        private Style SelectedStyle => (Style) App.CurrentApp.Resources["labelStyleMenuItemHighlight"];

        private static void OnGridMenuChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (GridMenu) bindable;
            if (control != null)
            {
                if (newValue is List<HomeMenuItem> menuItems)
                {
                    var rowNumber = -1;

                    #region Comments

                    //menuItems.Select(m =>
                    //{
                    //    rowNumber++;
                    //    var image = new Image()
                    //    {
                    //        Source = m.IconSource,
                    //        Style = (Style) App.CurrentApp.Resources["imageIconMenuItem"],
                    //        VerticalOptions = LayoutOptions.Center,
                    //        IsVisible = m.IsIconVisible,
                    //        BackgroundColor = Palette._Transparent
                    //    };
                    //    var label = new Label()
                    //    {
                    //        Text = m.MenuTitle,
                    //        Style = (Style) App.CurrentApp.Resources["labelStyleMenuItem"],
                    //        VerticalOptions = LayoutOptions.Center,
                    //        BackgroundColor = Palette._Transparent
                    //    };

                    //    var gestureRecognizer = new TapGestureRecognizer()
                    //    {
                    //        Command = new Command(async () => { await _rootPage.NavigateAsync(m.MenuType); })
                    //    };
                    //    image.GestureRecognizers.Add(gestureRecognizer);
                    //    label.GestureRecognizers.Add(gestureRecognizer);

                    //    control.RowDefinitions.Add(new RowDefinition {Height = GridLength.Auto});
                    //    control.Children.Add(new StackLayout()
                    //    {
                    //        Padding = new Thickness(15, 5, 0, 5),
                    //        Orientation = StackOrientation.Horizontal,
                    //        VerticalOptions = LayoutOptions.StartAndExpand,
                    //        BackgroundColor = Palette._Transparent,
                    //        Children =
                    //        {
                    //            image,
                    //            label
                    //        }
                    //    }, 0, rowNumber);

                    //    return m;
                    //});

                    #endregion Comments

                    foreach (var menuItem in menuItems)
                    {
                        var image = new Image()
                        {
                            Source = menuItem.IconSource,
                            Style = (Style) App.CurrentApp.Resources["imageIconMenuItem"],
                            VerticalOptions = LayoutOptions.Center,
                            IsVisible = menuItem.IsIconVisible,
                            BackgroundColor = Palette._Transparent
                        };
                        var label = new Label()
                        {
                            Text = menuItem.MenuTitle,
                            Style = menuItem.IsSelected ? control.SelectedStyle : control.DefaultStyle,
                            VerticalOptions = LayoutOptions.Center,
                            BackgroundColor = Palette._Transparent
                        };

                        var gestureRecognizer = new TapGestureRecognizer()
                        {
                            Command = new Command(async () =>
                            {
                                if (menuItem.IsSelected) return;
                                await control.RootPage.NavigateAsync(menuItem.MenuType);
                                foreach (var mi in menuItems)
                                {
                                    mi.IsSelected = false;
                                    mi.TextStyle = control.DefaultStyle;
                                }

                                var menu = menuItems.Find(t => t.MenuType == menuItem.MenuType);
                                menu.IsSelected = true;
                                menu.TextStyle = control.SelectedStyle;
                                OnGridMenuChanged(control, control.Source, menuItems);
                            })
                        };
                        image.GestureRecognizers.Add(gestureRecognizer);
                        label.GestureRecognizers.Add(gestureRecognizer);

                        var stackLayout = new StackLayout()
                        {
                            Padding = new Thickness(15, 5, 0, 5),
                            Orientation = StackOrientation.Horizontal,
                            VerticalOptions = LayoutOptions.StartAndExpand,
                            BackgroundColor = Palette._Transparent,
                            Children =
                            {
                                image, label
                            }
                        };

                        rowNumber++;
                        control.RowDefinitions.Add(new RowDefinition {Height = GridLength.Auto});
                        control.Children.Add(stackLayout, 0, rowNumber);
                    }

                    var stackLayoutVersion = new StackLayout()
                    {
                        HorizontalOptions = LayoutOptions.Center,
                        VerticalOptions = LayoutOptions.End,
                        Margin = new Thickness(0, 10, 0, 0),
                        BackgroundColor = Palette._Transparent,
                        Children =
                        {
                            new Label()
                            {
                                Text = control.ApplicationVersion,
                                Style = (Style) App.CurrentApp.Resources["labelAccordionStyleHeaderIntern"]
                            }
                        }
                    };

                    rowNumber++;
                    control.RowDefinitions.Add(new RowDefinition {Height = GridLength.Auto});
                    control.Children.Add(stackLayoutVersion, 0, rowNumber);
                }
            }
        }
    }
}

#region Comments

//    totalWeightLost += menu.WeightLost;

//    // MODIFY DATE
//    var labelModifyDate = new Label()
//    {
//        Style = (Style) App.CurrentApp.Resources["labelStyleTextTitle"]
//    };
//    var formattedModifyDate = new FormattedString();
//    formattedModifyDate.Spans.Add(new Span
//    {
//        Text = menu.ModifyDateDisplay,
//        FontAttributes = FontAttributes.Bold,
//    });
//    labelModifyDate.FormattedText = formattedModifyDate;

//    rowNumber++;
//    control.RowDefinitions.Add(new RowDefinition {Height = GridLength.Auto});
//    control.Children.Add(labelModifyDate, 0, rowNumber);


//    // CURRENT WEIGHT
//    var labelCurrentWeight = new Label()
//    {
//        Style = (Style) App.CurrentApp.Resources["labelStyleText"]
//    };
//    var formattedCurrentWeight = new FormattedString();
//    formattedCurrentWeight.Spans.Add(new Span
//    {
//        Text = TextResources.YourWeightIs + " ",
//        FontAttributes = FontAttributes.None
//    });
//    formattedCurrentWeight.Spans.Add(new Span
//    {
//        Text = menu.CurrentWeightDisplay,
//        FontAttributes = FontAttributes.None,
//        ForegroundColor = Palette._MainAccent
//    });
//    labelCurrentWeight.FormattedText = formattedCurrentWeight;

//    rowNumber++;
//    control.RowDefinitions.Add(new RowDefinition {Height = GridLength.Auto});
//    control.Children.Add(labelCurrentWeight, 0, rowNumber);

//    // WEIGHT LOST
//    var labelWeightLost = new Label()
//    {
//        Style = (Style) App.CurrentApp.Resources["labelStyleText"]
//    };
//    var formattedWeightLost = new FormattedString();
//    formattedWeightLost.Spans.Add(new Span
//    {
//        Text = TextResources.YouLost + " ",
//        FontAttributes = FontAttributes.None
//    });
//    formattedWeightLost.Spans.Add(new Span
//    {
//        Text = menu.WeightLostDisplay,
//        FontAttributes = FontAttributes.None,
//        ForegroundColor = Palette._MainAccent
//    });
//    labelWeightLost.FormattedText = formattedWeightLost;

//    rowNumber++;
//    control.RowDefinitions.Add(new RowDefinition {Height = GridLength.Auto});
//    control.Children.Add(labelWeightLost, 0, rowNumber);

//    // SHIRT SIZE
//    var labelShirtSize = new Label()
//    {
//        IsVisible = menu.IsShirtSizeAvailable,
//        Style = (Style) App.CurrentApp.Resources["labelStyleText"]
//    };
//    var formattedShirtSize = new FormattedString();
//    formattedShirtSize.Spans.Add(new Span
//    {
//        Text = TextResources.TShirtSize + " ",
//        FontAttributes = FontAttributes.None
//    });
//    formattedShirtSize.Spans.Add(new Span
//    {
//        Text = menu.ShirtSize,
//        FontAttributes = FontAttributes.None,
//        ForegroundColor = Palette._MainAccent
//    });
//    labelShirtSize.FormattedText = formattedShirtSize;

//    rowNumber++;
//    control.RowDefinitions.Add(new RowDefinition {Height = GridLength.Auto});
//    control.Children.Add(labelShirtSize, 0, rowNumber);

//    // SHIRT SIZE
//    var labelAboutJourney = new Label()
//    {
//        IsVisible = menu.IsAboutJourneyAvailable,
//        Style = (Style) App.CurrentApp.Resources["labelStyleText"]
//    };
//    var formattedAboutJourney = new FormattedString();
//    formattedAboutJourney.Spans.Add(new Span
//    {
//        Text = TextResources.HeadingAboutYourJourney + " ",
//        FontAttributes = FontAttributes.None
//    });
//    formattedAboutJourney.Spans.Add(new Span
//    {
//        Text = menu.AboutJourney,
//        FontAttributes = FontAttributes.None,
//        ForegroundColor = Palette._MainAccent
//    });
//    labelAboutJourney.FormattedText = formattedAboutJourney;

//    rowNumber++;
//    control.RowDefinitions.Add(new RowDefinition {Height = GridLength.Auto});
//    control.Children.Add(labelAboutJourney, 0, rowNumber);

//    // FRONT IMAGE
//    var imageFront = new Image()
//    {
//        Source = menu.FrontImageSource,
//        HeightRequest = menu.PictureHeight,
//        WidthRequest = menu.PictureWidth,
//        HorizontalOptions = LayoutOptions.Start,
//        VerticalOptions = LayoutOptions.Start,
//    };
//    if (control.ProfileModel.UserDetail.IsDownloadAllowed)
//        imageFront.GestureRecognizers.Add(new TapGestureRecognizer()
//        {
//            Command = new Command(() => { Device.OpenUri(new Uri(menu.FrontImageWithUrl)); }),
//            NumberOfTapsRequired = 2
//        });

//    // SIDE IMAGE
//    var imageSide = new Image()
//    {
//        Source = menu.SideImageSource,
//        HeightRequest = menu.PictureHeight,
//        WidthRequest = menu.PictureWidth,
//        HorizontalOptions = LayoutOptions.Start,
//        VerticalOptions = LayoutOptions.Start
//    };
//    if (control.ProfileModel.UserDetail.IsDownloadAllowed)
//        imageSide.GestureRecognizers.Add(new TapGestureRecognizer()
//        {
//            Command = new Command(() => { Device.OpenUri(new Uri(menu.SideImageWithUrl)); }),
//            NumberOfTapsRequired = 2
//        });

//    // IMAGES (FRONT, SIDE)
//    var stackLayoutImage = new StackLayout()
//    {
//        Orientation = StackOrientation.Horizontal,
//        HorizontalOptions = LayoutOptions.Center,
//        VerticalOptions = LayoutOptions.Start,
//        IsVisible = menu.IsImageAvailable
//    };

//    // ADDING IMAGE
//    stackLayoutImage.Children.Add(imageFront);
//    stackLayoutImage.Children.Add(imageSide);

//    rowNumber++;
//    control.RowDefinitions.Add(new RowDefinition {Height = GridLength.Auto});
//    control.Children.Add(stackLayoutImage, 0, rowNumber);

//    var stackActions = new StackLayout()
//    {
//        HorizontalOptions = LayoutOptions.FillAndExpand,
//        VerticalOptions = LayoutOptions.Start,
//        IsVisible = menu.IsDeleteAllowed
//    };

//    var buttonDelete = new Button()
//    {
//        Style = (Style) App.CurrentApp.Resources["buttonStyle"],
//        Text = TextResources.Delete + " " + menu.ModifyDateDisplay,
//        Command = new Command(async () =>
//        {
//            var response =
//                await control._MenuService.DeleteMenuAsync(menu.RevisionNumber);
//            if (response != null && response.Contains(HttpConstants.SUCCESS))
//            {
//                control.CloseAction.Invoke();
//                var showMenu = control.ProfileModel.UserDetail.IsMenuRequiredAfterDelete;
//                await control.ProfileModel.GetUserAsync(showMenu);
//            }
//        })
//    };
//    stackActions.Children.Add(buttonDelete);

//    rowNumber++;
//    control.RowDefinitions.Add(new RowDefinition {Height = GridLength.Auto});
//    control.Children.Add(stackActions, 0, rowNumber);

//    if (rowCount + 1 >= menus.Count)
//    {
//        continue;
//    }

//    // SETTING UP GRID CONTROLS
//    var stackLine = new StackLayout()
//    {
//        HorizontalOptions = LayoutOptions.FillAndExpand,
//        VerticalOptions = LayoutOptions.FillAndExpand,
//        BackgroundColor = Palette._LightGrayE,
//        Margin = new Thickness(25)
//    };

//    // LINE IMAGE
//    var imageLine = new Image()
//    {
//        Source = ImageResizer.ResizeImage("line.png", 727, 18),
//        HeightRequest = 18,
//        WidthRequest = 727,
//        HorizontalOptions = LayoutOptions.CenterAndExpand,
//        VerticalOptions = LayoutOptions.Center,
//        BackgroundColor = App.Configuration.BackgroundColor
//    };
//    stackLine.Children.Add(imageLine);

//    rowNumber++;
//    control.RowDefinitions.Add(new RowDefinition {Height = GridLength.Auto});
//    control.Children.Add(stackLine, 0, rowNumber);

//    rowCount++;

#endregion