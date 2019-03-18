using com.organo.x4ever.Localization;
using com.organo.x4ever.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Xamarin.Forms;

namespace com.organo.x4ever.Controls
{
    public class LanguageOption : ContentView, INotifyPropertyChanged
    {
        private static Image flagImage;

        private static Label nameLabel = new Label()
        {
            Text = "",
            VerticalOptions = LayoutOptions.Center,
            HorizontalOptions = LayoutOptions.Start
        };

        private static Picker pickerLanguage = new Picker() { IsVisible = false, Title = TextResources.SelectLanguage };

        public LanguageOption()
        {
            flagImage = new Image() { IsVisible = false };
            TapGestureRecognizer t = new TapGestureRecognizer();
            t.Tapped += OnTapped;

            StackLayout layout = new StackLayout()
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.StartAndExpand
            };
            //layout.Children.Add(flagImage);
            layout.Children.Add(nameLabel);
            layout.Children.Add(pickerLanguage);
            layout.GestureRecognizers.Add(t);
            Content = layout;
        }

        private Style _textStyle;
        public const string TextStylePropertyName = "TextStyle";

        public Style TextStyle
        {
            get { return _textStyle; }
            set { SetLanguageProperty(ref _textStyle, value, TextStylePropertyName, TextStyleChanged); }
        }

        protected void TextStyleChanged()
        {
            nameLabel.Style = TextStyle;
        }

        private Style _flagStyle;
        public const string FlagStylePropertyName = "FlagStyle";

        public Style FlagStyle
        {
            get { return _flagStyle; }
            set { SetLanguageProperty(ref _flagStyle, value, FlagStylePropertyName, FlagStyleChanged); }
        }

        protected void FlagStyleChanged()
        {
            //flagImage.Style = FlagStyle;
        }

        private bool _showSelection;
        public const string ShowSelectionPropertyName = "ShowSelection";

        public bool ShowSelection
        {
            get { return _showSelection; }
            set { SetLanguageProperty(ref _showSelection, value, ShowSelectionPropertyName); }
        }

        private List<ApplicationLanguage> _dataSource;
        public const string DataSourcePropertyName = "DataSource";

        public List<ApplicationLanguage> DataSource
        {
            get { return _dataSource; }
            set { SetLanguageProperty(ref _dataSource, value, DataSourcePropertyName, DataSourceChanged); }
        }

        protected void DataSourceChanged()
        {
            if (DataSource.Count > 0)
            {
                if (DataSource.Any(d => d.IsSelected))
                    DataSourceSelected = DataSource.FirstOrDefault(d => d.IsSelected);
                else
                    DataSourceSelected = DataSource[0];
            }
        }

        private ApplicationLanguage _dataSourceSelected;
        public const string DataSourceSelectedPropertyName = "DataSourceSelected";

        public ApplicationLanguage DataSourceSelected
        {
            get { return _dataSourceSelected; }
            set
            {
                SetLanguageProperty(ref _dataSourceSelected, value, DataSourceSelectedPropertyName,
                    OnDataSourceSelectedChanged);
            }
        }

        protected void OnDataSourceSelectedChanged()
        {
            //flagImage.Source = ImageSource.FromUri(new Uri(DataSourceSelected.CountryFlag));
            nameLabel.Text = DataSourceSelected.LanguageName;
        }

        public Action OnItemSelectedAction { get; set; }

        protected void SetLanguageProperty<U>(
            ref U backingStore, U value,
            string propertyName,
            Action onChanged = null,
            Action<U> onChanging = null)
        {
            if (EqualityComparer<U>.Default.Equals(backingStore, value))
                return;

            onChanging?.Invoke(value);

            OnLanguagePropertyChanging(propertyName);

            backingStore = value;

            onChanged?.Invoke();

            OnLanguagePropertyChanged(propertyName);
        }

        #region INotifyPropertyChanging implementation

        public event PropertyChangingEventHandler LangaugePropertyChanging;

        #endregion INotifyPropertyChanging implementation

        public void OnLanguagePropertyChanging(string propertyName)
        {
            if (LangaugePropertyChanging == null)
                return;

            LangaugePropertyChanging(this, new PropertyChangingEventArgs(propertyName));
        }

        #region INotifyPropertyChanged implementation

        public event PropertyChangedEventHandler LangaugePropertyChanged;

        #endregion INotifyPropertyChanged implementation

        public void OnLanguagePropertyChanged(string propertyName)
        {
            if (LangaugePropertyChanged == null)
                return;

            LangaugePropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private void OnTapped(object sender, EventArgs e)
        {
            pickerLanguage.ItemsSource = DataSource;
            pickerLanguage.ItemDisplayBinding = new Binding("LanguageName");
            pickerLanguage.SelectedIndexChanged += OnLanguageChanged;
            if (ShowSelection)
                pickerLanguage.Focus();
        }

        private void OnLanguageChanged(object sender, EventArgs e)
        {
            var langSelected = pickerLanguage.SelectedItem;
            if (langSelected != null)
                DataSourceSelected = (ApplicationLanguage)langSelected;
            pickerLanguage.SelectedIndexChanged -= OnLanguageChanged;
            OnItemSelectedAction();
        }
    }
}