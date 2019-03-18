using System;
using System.Collections.Generic;
using com.organo.x4ever.Helpers;
using com.organo.x4ever.Models;
using com.organo.x4ever.Statics;
using Xamarin.Forms;

namespace com.organo.x4ever.Controls
{
    public class AccordionExtended : ContentView
    {
        #region Private Variables

        private bool firstExpaned = false;
        private StackLayout mainLayout;

        #endregion Private Variables

        public AccordionExtended()
        {
            mainLayout = new StackLayout();
            Content = mainLayout;
        }

        #region Properties

        public const string DataSourcePropertyName = "DataSource";

        public static BindableProperty DataSourceProperty = BindableProperty.Create(
            propertyName: DataSourcePropertyName,
            returnType: typeof(List<AccordionExtendedSource>),
            declaringType: typeof(AccordionExtended),
            defaultValue: default(List<AccordionExtendedSource>));

        public List<AccordionExtendedSource> DataSource
        {
            get { return (List<AccordionExtendedSource>) GetValue(DataSourceProperty); }
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
            var firstOuter = FirstExpaned;
            if (DataSource != null)
            {
                foreach (var singleItem in DataSource)
                {
                    var titleButton = new AccordionExtendedMainButton()
                    {
                        Text = singleItem.Title ?? "",
                        ButtonStyle = singleItem.TitleStyle,
                        ButtonDefaultStyle = singleItem.TitleStyle,
                        ButtonSelectedStyle = singleItem.TitleSelectedStyle,
                        HorizontalOptions = LayoutOptions.FillAndExpand
                    };
                    titleButton.Expand = firstOuter;
                    titleButton.Clicked += OnAccordionExtendedMainButtonClicked;
                    layout.Children.Add(titleButton);
                    foreach (var accordionExtendedSourceItem in singleItem.AccordionExtendedSourceItems)
                    {
                        var headerButton = new AccordionExtendedContentButton()
                        {
                            Text = accordionExtendedSourceItem.HeaderText,
                            TextStyle = accordionExtendedSourceItem.HeaderTextStyle,
                            TextDefaultStyle = accordionExtendedSourceItem.HeaderTextStyle,
                            TextSelectedStyle = accordionExtendedSourceItem.HeaderTextSelectedStyle,

                            ImageSource = accordionExtendedSourceItem.HeaderImage,
                            ImageDefaultSource = accordionExtendedSourceItem.HeaderImage,
                            ImageSelectedSource = accordionExtendedSourceItem.HeaderImageSelected,
                            ImageStyle = accordionExtendedSourceItem.HeaderImageStyle,

                            NotePart1 = accordionExtendedSourceItem.HeaderNotePart1,
                            NotePart2 = accordionExtendedSourceItem.HeaderNotePart2,
                            NotePart3 = accordionExtendedSourceItem.HeaderNotePart3,
                            NoteStyle = accordionExtendedSourceItem.HeaderNoteStyle,
                            NotePart2Style = accordionExtendedSourceItem.HeaderNotePart2Style
                        };
                        headerButton.IsVisible = firstOuter;
                        var accordionExtendedContent = new ContentView()
                        {
                            Content = accordionExtendedSourceItem.ContentItems,
                            IsVisible = false
                        };
                        if (first)
                        {
                            headerButton.IsVisible = firstExpaned;
                            headerButton.Expand = firstExpaned;
                            accordionExtendedContent.IsVisible = firstExpaned;
                            first = false;
                        }

                        headerButton.AssosiatedContent = accordionExtendedContent;
                        headerButton.GestureRecognizers.Add(new TapGestureRecognizer()
                        {
                            Command = new Command(() => { OnAccordionExtendedContentButtonClicked(headerButton); })
                        });
                        titleButton.AccordionExtendedButtons.Add(headerButton);
                        layout.Children.Add(headerButton);
                        layout.Children.Add(accordionExtendedContent);
                    }

                    firstOuter = false;
                }
            }

            mainLayout = layout;
            Content = mainLayout;
        }

        private void OnAccordionExtendedMainButtonClicked(object sender, EventArgs args)
        {
            var senderButton = (AccordionExtendedMainButton) sender;
            if (!senderButton.Expand)
            {
                foreach (var childItem in mainLayout.Children)
                {
                    if (childItem.GetType() == typeof(ContentView))
                        childItem.IsVisible = false;
                    if (childItem.GetType() == typeof(AccordionExtendedContentButton))
                    {
                        var button = (AccordionExtendedContentButton) childItem;
                        button.Expand = false;
                        button.IsVisible = false;
                    }

                    if (childItem.GetType() == typeof(AccordionExtendedMainButton))
                    {
                        var button = (AccordionExtendedMainButton) childItem;
                        button.Expand = false;
                    }
                }

                if (senderButton.Expand)
                    senderButton.Expand = false;
                else senderButton.Expand = true;
                bool first = true;
                foreach (var button in senderButton.AccordionExtendedButtons)
                {
                    button.IsVisible = senderButton.Expand;
                    if (first)
                        button.AssosiatedContent.IsVisible = button.Expand = senderButton.Expand;
                    else button.AssosiatedContent.IsVisible = false;

                    first = false;
                }
            }
        }

        private void OnAccordionExtendedButtonClicked(object sender, EventArgs args)
        {
            var senderButton = (AccordionExtendedButton) sender;
            if (!senderButton.Expand)
            {
                foreach (var childItem in mainLayout.Children)
                {
                    if (childItem.GetType() == typeof(ContentView))
                        childItem.IsVisible = false;
                    if (childItem.GetType() == typeof(AccordionExtendedButton))
                    {
                        var button = (AccordionExtendedButton) childItem;
                        button.Expand = false;
                    }
                }

                if (senderButton.Expand)
                    senderButton.Expand = false;
                else senderButton.Expand = true;
                senderButton.AssosiatedContent.IsVisible = senderButton.Expand;
            }
        }

        private void OnAccordionExtendedContentButtonClicked(AccordionExtendedContentButton sender)
        {
            var senderButton = (AccordionExtendedContentButton) sender;
            if (!senderButton.Expand)
            {
                foreach (var childItem in mainLayout.Children)
                {
                    if (childItem.GetType() == typeof(ContentView))
                        childItem.IsVisible = false;
                    if (childItem.GetType() == typeof(AccordionExtendedContentButton))
                    {
                        var button = (AccordionExtendedContentButton) childItem;
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

    public class AccordionExtendedSource
    {
        public string Title { get; set; }
        public Style TitleStyle { get; set; }
        public Style TitleSelectedStyle { get; set; }
        public List<AccordionExtendedSourceItem> AccordionExtendedSourceItems { get; set; }
    }

    public class AccordionExtendedSourceItem
    {
        public string HeaderImage { get; set; }
        public string HeaderImageSelected { get; set; }
        public Style HeaderImageStyle { get; set; }

        public string HeaderText { get; set; }
        public Style HeaderTextStyle { get; set; }
        public Style HeaderTextSelectedStyle { get; set; }

        public string HeaderNotePart1 { get; set; }
        public string HeaderNotePart2 { get; set; }
        public string HeaderNotePart3 { get; set; }
        public Style HeaderNoteStyle { get; set; }
        public Style HeaderNotePart2Style { get; set; }

        public View ContentItems { get; set; }
    }

    public class AccordionExtendedMainButton : Button
    {
        #region Private Variables

        private bool expand = false;

        #endregion Private Variables

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

        public AccordionExtendedMainButton()
        {
            HorizontalOptions = LayoutOptions.FillAndExpand;
            AccordionExtendedButtons = new List<AccordionExtendedContentButton>();
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

        public List<AccordionExtendedContentButton> AccordionExtendedButtons { get; set; }

        #endregion Properties
    }

    public class AccordionExtendedButton : Button
    {
        #region Private Variables

        private bool expand = false;

        #endregion Private Variables

        public AccordionExtendedButton()
        {
            HorizontalOptions = LayoutOptions.FillAndExpand;
        }

        #region Properties

        public bool Expand
        {
            get { return expand; }
            set { expand = value; }
        }

        public ContentView AssosiatedContent { get; set; }

        #endregion Properties
    }

    public class AccordionExtendedContentButton : ContentView
    {
        #region Private Variables

        private bool expand = false;

        private ImageSize ImageSize { get; set; }

        private ImageSize ImageSizePlus { get; set; }

        private Image ImageIcon { get; set; }
        private Label LabelText { get; set; }
        private Label LabelNotePart1 { get; set; }
        private Image ImageNotePart2 { get; set; }
        private Label LabelNotePart3 { get; set; }

        private string _imageSource;

        public string ImageSource
        {
            get { return _imageSource; }
            set
            {
                _imageSource = value;
                ImageIcon.Source = ImageResizer.ResizeImage(_imageSource, ImageSizePlus);
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

        private string _notePart1;

        public string NotePart1
        {
            get { return _notePart1; }
            set
            {
                _notePart1 = value;
                LabelNotePart1.Text = _notePart1;
            }
        }

        private string _notePart2;

        public string NotePart2
        {
            get { return _notePart2; }
            set
            {
                _notePart2 = value;
                ImageNotePart2.Source = ImageResizer.ResizeImage(_notePart2, ImageSize);
            }
        }

        private string _notePart3;

        public string NotePart3
        {
            get { return _notePart3; }
            set
            {
                _notePart3 = value;
                LabelNotePart3.Text = _notePart3;
            }
        }

        private Style _noteStyle;

        public Style NoteStyle
        {
            get { return _noteStyle; }
            set
            {
                _noteStyle = value;
                LabelNotePart1.Style = _noteStyle;
                LabelNotePart3.Style = _noteStyle;
            }
        }

        private Style _notePart2Style;

        public Style NotePart2Style
        {
            get { return _notePart2Style; }
            set
            {
                _notePart2Style = value;
                ImageNotePart2.Style = _notePart2Style;
            }
        }

        private bool _noteVisible;

        public bool NoteVisible
        {
            get { return _noteVisible; }
            set
            {
                _noteVisible = value;
                LabelNotePart1.IsVisible = _noteVisible;
                ImageNotePart2.IsVisible = _noteVisible;
                LabelNotePart3.IsVisible = _noteVisible;
            }
        }

        private StackLayout stackLayout { get; set; }

        #endregion Private Variables

        public AccordionExtendedContentButton()
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

            ImageSizePlus = App.Configuration.GetImageSizeByID(ImageIdentity.WORKOUT_EXPAND_COLLAPSE_IMAGE);
            ImageIcon = new Image()
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Start
            };
            if (ImageSizePlus != null)
            {
                ImageIcon.HeightRequest = ImageSizePlus.Height;
                ImageIcon.WidthRequest = ImageSizePlus.Width;
            }

            stackLayoutInner.Children.Add(ImageIcon);

            LabelText = new Label()
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Start
            };
            stackLayoutInner.Children.Add(LabelText);
            stackLayout.Children.Add(stackLayoutInner);

            var stackLayoutNote = new StackLayout()
            {
                Orientation = StackOrientation.Horizontal,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.EndAndExpand
            };
            LabelNotePart1 = new Label()
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.End,
                IsVisible = false
            };

            ImageSize = App.Configuration.GetImageSizeByID(ImageIdentity.VIDEO_PLAYER_PAGE_NOTE_PLAY_IMAGE);
            ImageNotePart2 = new Image()
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                Margin = new Thickness(-2, 0, -2, 0)
            };
            if (ImageSize != null)
            {
                ImageNotePart2.HeightRequest = ImageSize.Height;
                ImageNotePart2.WidthRequest = ImageSize.Width;
            }

            LabelNotePart3 = new Label()
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Start,
                IsVisible = false
            };
            stackLayoutNote.Children.Add(LabelNotePart1);
            stackLayoutNote.Children.Add(ImageNotePart2);
            stackLayoutNote.Children.Add(LabelNotePart3);
            stackLayout.Children.Add(stackLayoutNote);
            Content = stackLayout;
            NoteVisible = false;
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
                    NoteVisible = true;
                }
                else
                {
                    ImageSource = ImageDefaultSource;
                    TextStyle = TextDefaultStyle;
                    NoteVisible = false;
                }
            }
        }

        public ContentView AssosiatedContent { get; set; }

        #endregion Properties
    }
}