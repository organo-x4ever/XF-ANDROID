using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.organo.xchallenge.Localization;
using com.organo.xchallenge.Models.News;
using com.organo.xchallenge.Statics;
using Xamarin.Forms;

namespace com.organo.xchallenge.Controls
{
    public class GridNews : Grid
    {
        public List<NewsModel> Source
        {
            get => (List<NewsModel>) GetValue(SourceProperty);
            set => SetValue(SourceProperty, value);
        }

        public static readonly BindableProperty SourceProperty = BindableProperty.Create(nameof(Source),
            typeof(List<NewsModel>), typeof(GridNews), null, BindingMode.TwoWay, null, OnGridNewsChanged);

        private static void OnGridNewsChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (GridNews) bindable;
            if (control != null)
            {
                var rowNumber = -1;

                var stackLayoutMain = new StackLayout
                {
                    Orientation = StackOrientation.Vertical,
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    HorizontalOptions = LayoutOptions.FillAndExpand
                };
                var labelPageHeader = new Label
                {
                    Text = TextResources.LatestNewsCAPS,
                    Style = (Style) App.CurrentApp.Resources["labelStyleXLargeHeader"],
                    HorizontalOptions = LayoutOptions.Center,
                    HorizontalTextAlignment = TextAlignment.Center
                };
                stackLayoutMain.Children.Add(labelPageHeader);

                rowNumber++;
                control.RowDefinitions.Add(new RowDefinition {Height = GridLength.Auto});
                control.Children.Add(stackLayoutMain, 0, rowNumber);
                if (newValue is List<NewsModel> newsModels)
                {
                    foreach (var newsModel in newsModels)
                    {
                        // NEWS HEADER
                        var formattedNewsHeader = new FormattedString
                        {
                            Spans =
                            {
                                new Span
                                {
                                    Text = newsModel.Header,
                                    FontAttributes = FontAttributes.None,
                                }
                            }
                        };
                        var labelHeader = new Label
                        {
                            Style = (Style) App.CurrentApp.Resources["labelStyleInfoHighlight"],
                            FormattedText = formattedNewsHeader
                        };

                        // POST DATE
                        var formattedPostDate = new FormattedString
                        {
                            Spans =
                            {
                                new Span
                                {
                                    Text = newsModel.PostDate.ToString("MMMM dd, yyyy"),
                                    FontAttributes = FontAttributes.None
                                }
                            }
                        };
                        var labelPostDate = new Label()
                        {
                            Style = (Style) App.CurrentApp.Resources["labelAccordionStyleHeaderIntern"],
                            Margin = new Thickness(0, -7, 0, 0),
                            HorizontalOptions = LayoutOptions.StartAndExpand,
                            FormattedText = formattedPostDate
                        };

                        // IMAGE
                        var image = new Image
                        {
                            Source = newsModel.NewsImageSource,
                            VerticalOptions = LayoutOptions.Start,
                            HorizontalOptions = LayoutOptions.Center
                        };

                        // NEWS BODY
                        var formattedNewsBody = new FormattedString
                        {
                            Spans =
                            {
                                new Span
                                {
                                    Text = newsModel.Body,
                                    FontAttributes = FontAttributes.None
                                }
                            }
                        };
                        var labelNewsBody = new Label
                        {
                            Style = (Style) App.CurrentApp.Resources["labelStyleInfoCheck"],
                            FormattedText = formattedNewsBody
                        };

                        // STACKLAYOUT BODY
                        var stackLayoutBody = new StackLayout
                        {
                            Orientation = StackOrientation.Vertical,
                            VerticalOptions = LayoutOptions.FillAndExpand,
                            HorizontalOptions = LayoutOptions.FillAndExpand,
                            Margin = new Thickness(5, 10, 5, 15)
                        };
                        stackLayoutBody.Children.Add(image);
                        stackLayoutBody.Children.Add(labelNewsBody);

                        // POSTED BY
                        var formattedPostedBy = new FormattedString
                        {
                            Spans =
                            {
                                new Span
                                {
                                    Text = newsModel.PostedBy,
                                    FontAttributes = FontAttributes.None
                                }
                            }
                        };
                        var labelPostedBy = new Label
                        {
                            IsVisible = !string.IsNullOrEmpty(newsModel.PostedBy),
                            Style = (Style) App.CurrentApp.Resources["labelAccordionStyleHeaderIntern"],
                            VerticalOptions = LayoutOptions.Center,
                            HorizontalOptions = LayoutOptions.End,
                            FormattedText = formattedPostedBy
                        };

                        var stackLayout = new StackLayout
                        {
                            Orientation = StackOrientation.Vertical,
                            VerticalOptions = LayoutOptions.FillAndExpand,
                            HorizontalOptions = LayoutOptions.FillAndExpand
                        };
                        stackLayout.Children.Add(labelHeader);
                        stackLayout.Children.Add(labelPostDate);
                        stackLayout.Children.Add(stackLayoutBody);
                        stackLayout.Children.Add(labelPostedBy);

                        var frame = new Frame
                        {
                            Style = (Style) App.CurrentApp.Resources["frameStyleCard"],
                            BackgroundColor = Palette._BoxesBackground,
                            HasShadow = false,
                            CornerRadius = 5,
                            Margin = new Thickness(0, 7, 0, 5),
                            Content = stackLayout
                        };

                        rowNumber++;
                        control.RowDefinitions.Add(new RowDefinition {Height = GridLength.Auto});
                        control.Children.Add(frame, 0, rowNumber);
                    }
                }

                rowNumber++;
                control.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                control.Children.Add(new FooterView(), 0, rowNumber);
            }
        }
    }
}