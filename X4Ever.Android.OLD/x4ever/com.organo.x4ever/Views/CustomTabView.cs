using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;

namespace com.organo.x4ever.Views
{
    public class CustomTabView : ContentView, INotifyPropertyChanged
    {
        private Label labelText = new Label
        {
            Text = "Tab",
            HorizontalOptions = LayoutOptions.Fill,
            HorizontalTextAlignment = TextAlignment.Center
        };

        private Image imageIcon = new Image {Source = string.Empty, HorizontalOptions = LayoutOptions.Center};

        private BoxView boxView =
            new BoxView {Color = Color.Black, HeightRequest = 1, HorizontalOptions = LayoutOptions.Fill};

        private StackLayout stackLayout = new StackLayout()
        {
            Orientation = StackOrientation.Horizontal,
            HorizontalOptions = LayoutOptions.FillAndExpand,
        };

        public CustomTabView()
        {
            this.IsSelected = false;
            this.boxView.BackgroundColor = this.UnderlineColor;
            this.labelText.TextColor = this.TextColor;
            this.imageIcon.Source = ImageSource.FromFile(this.Icon);

            this.stackLayout.Orientation = this.Orientation;
            this.stackLayout.HorizontalOptions = this.HorizontalOption;
            this.stackLayout.VerticalOptions = this.VerticalOption;

            this.stackLayout.BackgroundColor = this.BackgroundColor;

            TapGestureRecognizer t = new TapGestureRecognizer();
            t.Tapped += OnTapped;
            stackLayout.Children.Add(imageIcon);
            stackLayout.Children.Add(labelText);
            stackLayout.Children.Add(boxView);
            stackLayout.GestureRecognizers.Add(t);
            Content = stackLayout;

            Clicked += (sender, e) => { };
        }

        //public static readonly BindableProperty VerticalOptionProperty = BindableProperty.Create<CustomTabView, LayoutOptions>(p => p.VerticalOption, LayoutOptions.End);
        public static readonly BindableProperty VerticalOptionProperty = BindableProperty.Create("VerticalOption",
            typeof(LayoutOptions), typeof(LayoutOptions), LayoutOptions.End);

        public LayoutOptions VerticalOption
        {
            get { return (LayoutOptions) GetValue(VerticalOptionProperty); }
            set { SetValue(VerticalOptionProperty, value); }
        }

        //public static readonly BindableProperty HorizontalOptionProperty = BindableProperty.Create<CustomTabView, LayoutOptions>(p => p.HorizontalOption, LayoutOptions.End);
        public static readonly BindableProperty HorizontalOptionProperty = BindableProperty.Create("HorizontalOption",
            typeof(LayoutOptions), typeof(LayoutOptions), LayoutOptions.End);

        public LayoutOptions HorizontalOption
        {
            get { return (LayoutOptions) GetValue(HorizontalOptionProperty); }
            set { SetValue(HorizontalOptionProperty, value); }
        }

        //public static readonly BindableProperty OrientationProperty = BindableProperty.Create<CustomTabView, StackOrientation>(p => p.Orientation, StackOrientation.Horizontal);
        public static readonly BindableProperty OrientationProperty = BindableProperty.Create("Orientation",
            typeof(StackOrientation), typeof(StackOrientation), StackOrientation.Horizontal);

        public StackOrientation Orientation
        {
            get { return (StackOrientation) GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        //public static readonly BindableProperty IDProperty = BindableProperty.Create<CustomTabView, string>(p => p.ID, string.Empty);
        public static readonly BindableProperty IDProperty =
            BindableProperty.Create("ID", typeof(string), typeof(string), string.Empty);

        public string ID
        {
            get { return (string) GetValue(IDProperty); }
            set { SetValue(IDProperty, value); }
        }

        //public static readonly BindableProperty TitleProperty = BindableProperty.Create<CustomTabView, string>(p => p.Title, string.Empty);
        public static readonly BindableProperty TitleProperty =
            BindableProperty.Create("Title", typeof(string), typeof(string), string.Empty);

        public string Title
        {
            get { return (string) GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        //public static readonly BindableProperty TextColorProperty = BindableProperty.Create<CustomTabView, Color>(p => p.TextColor, Color.Gray);
        public static readonly BindableProperty TextColorProperty =
            BindableProperty.Create("TextColor", typeof(Color), typeof(Color), Color.Gray);

        public Color TextColor
        {
            get { return (Color) GetValue(TextColorProperty); }
            set { SetValue(TextColorProperty, value); }
        }

        //public static readonly BindableProperty SelectedTextColorProperty = BindableProperty.Create<CustomTabView, Color>(p => p.SelectedTextColor, Color.White);
        public static readonly BindableProperty SelectedTextColorProperty =
            BindableProperty.Create("SelectedTextColor", typeof(Color), typeof(Color), Color.White);

        public Color SelectedTextColor
        {
            get { return (Color) GetValue(SelectedTextColorProperty); }
            set { SetValue(SelectedTextColorProperty, value); }
        }

        //public static readonly BindableProperty IconProperty = BindableProperty.Create<CustomTabView, string>(p => p.Icon, string.Empty);
        public static readonly BindableProperty IconProperty =
            BindableProperty.Create("Icon", typeof(string), typeof(string), string.Empty);

        public string Icon
        {
            get { return (string) GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        //public static readonly BindableProperty SelectedIconProperty = BindableProperty.Create<CustomTabView, string>(p => p.SelectedIcon, string.Empty);
        public static readonly BindableProperty SelectedIconProperty =
            BindableProperty.Create("SelectedIcon", typeof(string), typeof(string), string.Empty);

        public string SelectedIcon
        {
            get { return (string) GetValue(SelectedIconProperty); }
            set { SetValue(SelectedIconProperty, value); }
        }

        //public static readonly new BindableProperty BackgroundColorProperty = BindableProperty.Create<CustomTabView, Color>(p => p.BackgroundColor, Color.Black);
        public static readonly new BindableProperty BackgroundColorProperty =
            BindableProperty.Create("BackgroundColor", typeof(Color), typeof(Color), Color.Black);

        public new Color BackgroundColor
        {
            get { return (Color) GetValue(BackgroundColorProperty); }
            set { SetValue(BackgroundColorProperty, value); }
        }

        //public static readonly BindableProperty SelectedBackgroundColorProperty = BindableProperty.Create<CustomTabView, Color>(p => p.SelectedBackgroundColor, Color.Gray);
        public static readonly BindableProperty SelectedBackgroundColorProperty =
            BindableProperty.Create("SelectedBackgroundColor", typeof(Color), typeof(Color), Color.Gray);

        public Color SelectedBackgroundColor
        {
            get { return (Color) GetValue(SelectedBackgroundColorProperty); }
            set { SetValue(SelectedBackgroundColorProperty, value); }
        }

        //public static readonly BindableProperty UnderlineColorProperty = BindableProperty.Create<CustomTabView, Color>(p => p.UnderlineColor, Color.Gray);
        public static readonly BindableProperty UnderlineColorProperty =
            BindableProperty.Create("UnderlineColor", typeof(Color), typeof(Color), Color.Gray);

        public Color UnderlineColor
        {
            get { return (Color) GetValue(UnderlineColorProperty); }
            set { SetValue(UnderlineColorProperty, value); }
        }

        //public static readonly BindableProperty SelectedUnderlineColorProperty = BindableProperty.Create<CustomTabView, Color>(p => p.SelectedUnderlineColor, Color.White);
        public static readonly BindableProperty SelectedUnderlineColorProperty =
            BindableProperty.Create("SelectedUnderlineColor", typeof(Color), typeof(Color), Color.White);

        public Color SelectedUnderlineColor
        {
            get { return (Color) GetValue(SelectedUnderlineColorProperty); }
            set { SetValue(SelectedUnderlineColorProperty, value); }
        }

        //public static readonly BindableProperty IsSelectedProperty = BindableProperty.Create<CustomTabView, bool>(p => p.IsSelected, false);
        public static readonly BindableProperty IsSelectedProperty =
            BindableProperty.Create("IsSelected", typeof(bool), typeof(bool), false);

        private bool isSelected = false;
        public const string IsSelectedPropertyName = "IsSelected";

        public bool IsSelected
        {
            get { return (bool) GetValue(IsSelectedProperty); }
            set
            {
                SetValue(IsSelectedProperty, value);
                SetProperty(ref isSelected, value, IsSelectedPropertyName);
            }
        }

        //public static readonly BindableProperty TargetPageProperty = BindableProperty.Create<CustomTabView, Page>(p => p.TargetPage, null);
        public static readonly BindableProperty TargetPageProperty =
            BindableProperty.Create("TargetPage", typeof(Page), typeof(Page), null);

        public Page TargetPage
        {
            get { return (Page) GetValue(TargetPageProperty); }
            set { SetValue(TargetPageProperty, value); }
        }

        public Action<object, EventArgs> Clicked { get; set; }

        private void OnTapped(object sender, EventArgs e)
        {
            //lbl.Text = lbl.Text == "\u2611" ? "\u2610" : "\u2611"; // \u2611 Uni code will show checked Box
            //this. = this.Checked == false;
            this.IsSelected = true;
            this.boxView.BackgroundColor = this.SelectedUnderlineColor;
            this.labelText.TextColor = this.SelectedTextColor;
            this.imageIcon.Source = ImageSource.FromFile(this.SelectedIcon);
            this.stackLayout.BackgroundColor = this.SelectedBackgroundColor;
        }

        protected void SetProperty<U>(
            ref U backingStore, U value,
            string propertyName,
            Action onChanged = null,
            Action<U> onChanging = null)
        {
            if (EqualityComparer<U>.Default.Equals(backingStore, value))
                return;

            if (onChanging != null)
                onChanging(value);

            OnTabPropertyChanging(propertyName);

            backingStore = value;

            if (onChanged != null)
                onChanged();

            OnTabPropertyChanged(propertyName);
        }

        #region INotifyTabPropertyChanging implementation

        public event PropertyChangingEventHandler TabPropertyChanging;

        #endregion INotifyTabPropertyChanging implementation

        public void OnTabPropertyChanging(string propertyName)
        {
            if (TabPropertyChanging == null)
                return;

            TabPropertyChanging(this, new PropertyChangingEventArgs(propertyName));
        }

        #region INotifyTabPropertyChanged implementation

        public event PropertyChangedEventHandler TabPropertyChanged;

        #endregion INotifyTabPropertyChanged implementation

        public void OnTabPropertyChanged(string propertyName)
        {
            if (TabPropertyChanged == null)
                return;

            TabPropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}