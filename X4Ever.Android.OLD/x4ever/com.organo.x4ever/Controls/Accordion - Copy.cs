using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace com.android.xchallenge.Controls
{
    public class Accordion : ContentView
    {
        #region Private Variables

        bool firstExpaned = false;
        StackLayout mainLayout;

        #endregion

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
            get { return (List<AccordionSource>) GetValue(DataSourceProperty); }
            set { SetValue(DataSourceProperty, value); }
        }

        public bool FirstExpaned
        {
            get { return firstExpaned; }
            set { firstExpaned = value; }
        }

        #endregion

        public void DataBind()
        {
            mainLayout = new StackLayout();
            var layout = new StackLayout();
            var first = true;
            if (DataSource != null)
            {
                var title = "";
                var titleLabel = new Label();
                foreach (var singleItem in DataSource)
                {
                    singleItem.ShowTitle = (singleItem.Title ?? "") != title;
                    title = singleItem.Title ?? "";
                    if (singleItem.ShowTitle)
                        titleLabel = new Label()
                        {
                            Text = singleItem.Title ?? "",
                            IsVisible = singleItem.ShowTitle,
                            Style = singleItem.TitleStyle,
                            HorizontalOptions = LayoutOptions.Center
                        };
                    var headerButton = new AccordionButton()
                    {
                        Text = singleItem.HeaderText,
                        Style = singleItem.HeaderStyle
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
                    layout.Children.Add(titleLabel);
                    layout.Children.Add(headerButton);
                    layout.Children.Add(accordionContent);
                }
            }

            mainLayout = layout;
            Content = mainLayout;
        }

        void OnAccordionButtonClicked(object sender, EventArgs args)
        {
            foreach (var childItem in mainLayout.Children)
            {
                if (childItem.GetType() == typeof(ContentView))
                    childItem.IsVisible = false;
                if (childItem.GetType() == typeof(AccordionButton))
                {
                    var button = (AccordionButton) childItem;
                    button.Expand = false;
                }
            }

            var senderButton = (AccordionButton) sender;

            if (senderButton.Expand)
            {
                senderButton.Expand = false;
            }
            else senderButton.Expand = true;

            senderButton.AssosiatedContent.IsVisible = senderButton.Expand;
        }
    }

    public class AccordionSource
    {
        public string Title { get; set; }
        public bool ShowTitle { get; set; }
        public Style TitleStyle { get; set; }
        public string HeaderText { get; set; }
        public Style HeaderStyle { get; set; }
        public Style HeaderSelectedStyle { get; set; }
        public View ContentItems { get; set; }
    }

    public class AccordionButton : Button
    {
        #region Private Variables

        bool expand = false;

        #endregion

        public AccordionButton()
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

        #endregion
    }
}