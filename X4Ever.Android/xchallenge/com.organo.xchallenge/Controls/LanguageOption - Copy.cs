
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;

namespace com.android.xchallenge.Controls
{
    public class LanguageOption : ContentView, INotifyPropertyChanged
    {
        private static Image flagImage = new Image();
        private static Label nameLabel = new Label() {Text = ""};
        private static Picker pickerLanguage = new Picker() {IsVisible = false};

        public LanguageOption()
        {
            TapGestureRecognizer t = new TapGestureRecognizer();
            t.Tapped += OnTapped;

            this.TextChanged();
            this.SourceChanged();

            StackLayout layout = new StackLayout()
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.StartAndExpand
            };
            layout.Children.Add(flagImage);
            layout.Children.Add(nameLabel);
            layout.Children.Add(pickerLanguage);
            layout.GestureRecognizers.Add(t);
            Content = layout;
        }
        
        public static readonly BindableProperty TextProperty =
            BindableProperty.Create(
                "Text", typeof(string), typeof(string), null, propertyChanged: OnTextChanged,
                defaultBindingMode: BindingMode.TwoWay);

        static void OnTextChanged(BindableObject bindable, object oldValue, object newValue)
        {
            nameLabel.Text = newValue?.ToString();
        }

        private string _text;
        public const string TextPropertyName = "Text";

        public string Text
        {
            get { return _text; }
            set { SetProperty(ref _text, value, TextPropertyName, TextChanged); }
        }

        public void TextChanged()
        {
            nameLabel.Text = this.Text;
        }

        private Style _textStyle;
        public const string TextStylePropertyName = "TextStyle";

        public Style TextStyle
        {
            get { return _textStyle; }
            set { SetProperty(ref _textStyle, value, TextStylePropertyName, TextStyleChanged); }
        }

        public void TextStyleChanged()
        {
            nameLabel.Style = this.TextStyle;
        }

        private string _source;
        public const string SourcePropertyName = "Source";

        public string Source
        {
            get { return _source; }
            set { SetProperty(ref _source, value, SourcePropertyName, SourceChanged); }
        }

        public void SourceChanged()
        {
            flagImage.Source = ImageSource.FromFile(this.Source);
        }

        private Style _flagStyle;
        public const string FlagStylePropertyName = "FlagStyle";

        public Style FlagStyle
        {
            get { return _flagStyle; }
            set { SetProperty(ref _flagStyle, value, FlagStylePropertyName, FlagStyleChanged); }
        }

        //private string[] _dataSource;
        //public const string DataSourcePropertyName = "DataSource";

        //public string[] DataSource
        //{
        //    get { return _dataSource; }
        //    set { SetProperty(ref _dataSource, value, DataSourcePropertyName, DataSourceChanged); }
        //}

        private static string[] _dataSource;

        public static string[] DataSource
        {
            get { return _dataSource; }
            set { _dataSource = value; }
        }

        public static readonly BindableProperty DataSourceProperty =
            BindableProperty.Create(
                "DataSource", typeof(string[]), typeof(string[]), null, propertyChanged: OnDataSourceChanged);

        static void OnDataSourceChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (newValue == null) return;
            // Property changed implementation goes here
            DataSource = ((string[]) newValue);
            if (((string[]) newValue).Length > 0)
                nameLabel.Text = ((string[]) newValue)[0];
        }

        public void FlagStyleChanged()
        {
            flagImage.Style = this.FlagStyle;
        }

        protected void SetProperty<U>(
            ref U backingStore, U value,
            string propertyName,
            Action onChanged = null,
            Action<U> onChanging = null)
        {
            if (EqualityComparer<U>.Default.Equals(backingStore, value))
                return;

            onChanging?.Invoke(value);

            OnPropertyChanging(propertyName);

            backingStore = value;

            onChanged?.Invoke();

            OnPropertyChanged(propertyName);
        }

        #region INotifyPropertyChanging implementation

        public event PropertyChangingEventHandler PropertyChanging;

        #endregion

        public void OnPropertyChanging(string propertyName)
        {
            if (PropertyChanging == null)
                return;

            PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
        }

        #region INotifyPropertyChanged implementation

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged == null)
                return;

            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private void OnTapped(object sender, EventArgs e)
        {
            pickerLanguage.ItemsSource = DataSource;
            pickerLanguage.Focus();
            pickerLanguage.SelectedIndexChanged += (sender1, e1) =>
            {
                var langSelected = pickerLanguage.SelectedItem;
                if (langSelected != null)
                {
                    SelectedIndex = 0;
                    Text = langSelected.ToString();
                }
            };
        }

        private int SelectedIndex { get; set; }
    }
}