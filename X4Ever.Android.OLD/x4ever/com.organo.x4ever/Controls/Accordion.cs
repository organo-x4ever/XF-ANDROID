using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace com.organo.x4ever.Controls
{
    public class Accordion : ContentView
    {
        #region Private Variables

        private bool firstExpaned = false;
        private StackLayout mainLayout;

        #endregion Private Variables

        public Accordion()
        {
            mainLayout = new StackLayout();
            Content = mainLayout;
        }

        #region Properties

        public const string DataSourcePropertyName = "DataSource";

        public static BindableProperty DataSourceProperty = BindableProperty.Create(
            propertyName: DataSourcePropertyName,
            returnType: typeof(List<AccordionSource>),
            declaringType: typeof(Accordion),
            defaultValue: default(List<AccordionSource>));

        public List<AccordionSource> DataSource
        {
            get { return (List<AccordionSource>)GetValue(DataSourceProperty); }
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
                    var headerButton = new AccordionButton()
                    {
                        Text = singleItem.HeaderText,
                        ButtonStyle = singleItem.HeaderStyle,
                        ButtonDefaultStyle = singleItem.HeaderStyle,
                        ButtonSelectedStyle = singleItem.HeaderSelectedStyle
                    };

                    var accordionContent = new ContentView()
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
                    headerButton.Clicked += OnAccordionButtonClicked;
                    layout.Children.Add(headerButton);
                    layout.Children.Add(accordionContent);
                }
            }

            mainLayout = layout;
            Content = mainLayout;
        }

        private void OnAccordionButtonClicked(object sender, EventArgs args)
        {
            var senderButton = (AccordionButton)sender;
            if (!senderButton.Expand)
            {
                foreach (var childItem in mainLayout.Children)
                {
                    if (childItem.GetType() == typeof(ContentView))
                        childItem.IsVisible = false;
                    if (childItem.GetType() == typeof(AccordionButton))
                    {
                        var button = (AccordionButton)childItem;
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

    public class AccordionSource
    {
        public string HeaderText { get; set; }
        public Style HeaderStyle { get; set; }
        public Style HeaderSelectedStyle { get; set; }
        public View ContentItems { get; set; }
    }

    public class AccordionButton : Button
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
            set
            {
                _buttonDefaultStyle = value;
            }
        }

        private Style _buttonSelectedStyle;

        public Style ButtonSelectedStyle
        {
            get { return _buttonSelectedStyle; }
            set
            {
                _buttonSelectedStyle = value;
            }
        }

        #endregion Public Properties

        #endregion Private Variables

        public AccordionButton()
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
}