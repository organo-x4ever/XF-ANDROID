using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace com.organo.x4ever.Controls
{
    public class AccordionMultiView : ContentView
    {
        #region Private Variables

        private bool firstExpaned = false;
        private StackLayout mainLayout;

        #endregion Private Variables

        public AccordionMultiView()
        {
            mainLayout = new StackLayout();
            Content = mainLayout;
        }

        #region Properties

        public const string DataSourcePropertyName = "DataSource";

        public static BindableProperty DataSourceProperty = BindableProperty.Create(
            propertyName: DataSourcePropertyName,
            returnType: typeof(List<AccordionMultiViewSource>),
            declaringType: typeof(AccordionMultiView),
            defaultValue: default(List<AccordionMultiViewSource>));

        public List<AccordionMultiViewSource> DataSource
        {
            get { return (List<AccordionMultiViewSource>)GetValue(DataSourceProperty); }
            set { SetValue(DataSourceProperty, value); }
        }

        public bool FirstExpaned
        {
            get { return firstExpaned; }
            set { firstExpaned = value; }
        }

        #endregion Properties

        public void DataBind()
        {
            mainLayout = new StackLayout();
            var layout = new StackLayout();
            var first = true;
            if (DataSource != null)
            {
                foreach (var singleItem in DataSource)
                {
                    var accordionContent = new ContentView();
                    switch (singleItem?.ViewType)
                    {
                        case "list":

                            var headerButton = new AccordionMultiViewButton()
                            {
                                Text = singleItem.HeaderText,
                                ButtonStyle = singleItem.HeaderStyle,
                                ButtonDefaultStyle = singleItem.HeaderStyle,
                                ButtonSelectedStyle = singleItem.HeaderSelectedStyle
                            };

                            accordionContent = new ContentView()
                            {
                                Content = singleItem.ContentItems,
                                IsVisible = false
                            };

                            if (first)
                            {
                                headerButton.Expand = firstExpaned;
                                accordionContent.IsVisible = firstExpaned;
                                first = false;
                            }

                            headerButton.AssosiatedContent = accordionContent;
                            headerButton.Clicked += OnAccordionMultiViewButtonClicked;
                            layout.Children.Add(headerButton);
                            layout.Children.Add(accordionContent);

                            break;

                        case "grid":

                            var headerContentButton = new AccordionMultiViewContentButton()
                            {
                                Text = singleItem.HeaderText,
                                TextStyle = singleItem.HeaderStyle,
                                TextDefaultStyle = singleItem.HeaderStyle,
                                TextSelectedStyle = singleItem.HeaderSelectedStyle,

                                ImageSource = singleItem.HeaderImage,
                                ImageDefaultSource = singleItem.HeaderImage,
                                ImageSelectedSource = singleItem.HeaderImageSelected,
                                ImageStyle = singleItem.HeaderImageStyle,
                            };

                            accordionContent = new ContentView()
                            {
                                Content = singleItem.ContentItems,
                                IsVisible = false
                            };

                            if (first)
                            {
                                headerContentButton.Expand = firstExpaned;
                                accordionContent.IsVisible = firstExpaned;
                                first = false;
                            }

                            headerContentButton.AssosiatedContent = accordionContent;
                            headerContentButton.GestureRecognizers.Add(new TapGestureRecognizer()
                            {
                                Command = new Command(() =>
                                {
                                    OnAccordionMultiViewContentButtonClicked(headerContentButton);
                                })
                            });

                            layout.Children.Add(headerContentButton);
                            layout.Children.Add(accordionContent);

                            break;
                    }
                }
            }

            mainLayout = layout;
            Content = mainLayout;
        }

        private void OnAccordionMultiViewButtonClicked(object sender, EventArgs args)
        {
            var senderButton = (AccordionMultiViewButton)sender;
            if (!senderButton.Expand)
            {
                foreach (var childItem in mainLayout.Children)
                {
                    if (childItem.GetType() == typeof(ContentView))
                        childItem.IsVisible = false;
                    if (childItem.GetType() == typeof(AccordionMultiViewButton))
                    {
                        var button = (AccordionMultiViewButton)childItem;
                        button.Expand = false;
                    }
                }

                if (senderButton.Expand)
                    senderButton.Expand = false;
                else senderButton.Expand = true;
                senderButton.AssosiatedContent.IsVisible = senderButton.Expand;
            }
        }

        private void OnAccordionMultiViewContentButtonClicked(AccordionMultiViewContentButton sender)
        {
            var senderButton = sender;
            if (!senderButton.Expand)
            {
                foreach (var childItem in mainLayout.Children)
                {
                    if (childItem.GetType() == typeof(ContentView))
                        childItem.IsVisible = false;
                    if (childItem.GetType() == typeof(AccordionMultiViewContentButton))
                    {
                        var button = (AccordionMultiViewContentButton)childItem;
                        button.Expand = false;
                    }
                }

                if (senderButton.Expand)
                    senderButton.Expand = false;
                else senderButton.Expand = true;

                senderButton.AssosiatedContent.IsVisible = senderButton.Expand;
            }
        }
    }

    public class AccordionMultiViewSource
    {
        public string HeaderText { get; set; }
        public Style HeaderStyle { get; set; }
        public Style HeaderSelectedStyle { get; set; }

        public string HeaderImage { get; set; }
        public string HeaderImageSelected { get; set; }
        public Style HeaderImageStyle { get; set; }

        public string ViewType { get; set; }
        public View ContentItems { get; set; }
    }

    public class AccordionMultiViewButton : Button
    {
        #region Private Variables

        private bool expand = false;

        #region Public Properties

        private Style _buttonStyle;

        public Style ButtonStyle
        {
            get { return _buttonStyle; }
            set
            {
                _buttonStyle = value;
                Style = _buttonStyle;
            }
        }

        private Style _buttonDefaultStyle;

        public Style ButtonDefaultStyle
        {
            get { return _buttonDefaultStyle; }
            set { _buttonDefaultStyle = value; }
        }

        private Style _buttonSelectedStyle;

        public Style ButtonSelectedStyle
        {
            get { return _buttonSelectedStyle; }
            set { _buttonSelectedStyle = value; }
        }

        #endregion Public Properties

        #endregion Private Variables

        public AccordionMultiViewButton()
        {
            HorizontalOptions = LayoutOptions.FillAndExpand;
        }

        #region Properties

        public bool Expand
        {
            get { return expand; }
            set
            {
                expand = value;
                if (expand)
                    ButtonStyle = ButtonSelectedStyle;
                else
                    ButtonStyle = ButtonDefaultStyle;
            }
        }

        public ContentView AssosiatedContent { get; set; }

        #endregion Properties
    }

    public class AccordionMultiViewContentButton : ContentView
    {
        #region Private Variables

        private bool expand = false;

        private Image ImageIcon { get; set; }
        private Label LabelText { get; set; }

        private string _imageSource;

        public string ImageSource
        {
            get { return _imageSource; }
            set
            {
                _imageSource = value;
                ImageIcon.Source = _imageSource;
            }
        }

        private string _imageDefaultSource;

        public string ImageDefaultSource
        {
            get { return _imageDefaultSource; }
            set { _imageDefaultSource = value; }
        }

        private string _imageSelectedSource;

        public string ImageSelectedSource
        {
            get { return _imageSelectedSource; }
            set { _imageSelectedSource = value; }
        }

        private Style _imageStyle;

        public Style ImageStyle
        {
            get { return _imageStyle; }
            set
            {
                _imageStyle = value;
                ImageIcon.Style = _imageStyle;
            }
        }

        private string _text;

        public string Text
        {
            get { return _text; }
            set
            {
                _text = value;
                LabelText.Text = _text;
            }
        }

        private Style _textStyle;

        public Style TextStyle
        {
            get { return _textStyle; }
            set
            {
                _textStyle = value;
                LabelText.Style = _textStyle;
            }
        }

        private Style _textDefaultStyle;

        public Style TextDefaultStyle
        {
            get { return _textDefaultStyle; }
            set { _textDefaultStyle = value; }
        }

        private Style _textSelectedStyle;

        public Style TextSelectedStyle
        {
            get { return _textSelectedStyle; }
            set { _textSelectedStyle = value; }
        }

        private StackLayout stackLayout { get; set; }

        #endregion Private Variables

        public AccordionMultiViewContentButton()
        {
            HorizontalOptions = LayoutOptions.FillAndExpand;
            stackLayout = new StackLayout()
            {
                Orientation = StackOrientation.Horizontal,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Margin = new Thickness(5, 0, 5, 0)
            };
            var stackLayoutInner = new StackLayout()
            {
                Orientation = StackOrientation.Horizontal,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Start
            };
            ImageIcon = new Image()
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Start
            };
            stackLayoutInner.Children.Add(ImageIcon);

            LabelText = new Label()
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Start
            };
            stackLayoutInner.Children.Add(LabelText);
            stackLayout.Children.Add(stackLayoutInner);
            Content = stackLayout;
        }

        #region Properties

        public bool Expand
        {
            get { return expand; }
            set
            {
                expand = value;

                if (expand)
                {
                    ImageSource = ImageSelectedSource;
                    TextStyle = TextSelectedStyle;
                }
                else
                {
                    ImageSource = ImageDefaultSource;
                    TextStyle = TextDefaultStyle;
                }
            }
        }

        public ContentView AssosiatedContent { get; set; }

        #endregion Properties
    }
}