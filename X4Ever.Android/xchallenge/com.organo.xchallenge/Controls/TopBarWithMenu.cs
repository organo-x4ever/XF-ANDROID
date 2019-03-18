using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace com.organo.xchallenge.Controls
{
    public class TopBarWithMenu : ContentView, INotifyPropertyChanged
    {
        //    <Grid BackgroundColor = "{x:Static statics:Palette._MainAccent}" HorizontalOptions="FillAndExpand" VerticalOptions="Start">
        //<Grid.ColumnDefinitions>
        //<ColumnDefinition Width = "35" />
        //< ColumnDefinition Width="*" />
        //<ColumnDefinition Width = "35" />
        //</ Grid.ColumnDefinitions >
        //< Grid.RowDefinitions >
        //< RowDefinition Height="35" />
        //</Grid.RowDefinitions>
        //<StackLayout Grid.Column="0" Style= "{DynamicResource stackMenuBar}" >
        //< Image Source= "{translate:Translate icon_menu}" Style= "{DynamicResource imageLogoMenu}" >
        //</ Image >
        //< StackLayout.GestureRecognizers >
        //< TapGestureRecognizer Command= "{Binding ShowSideMenuCommand}" />
        //</ StackLayout.GestureRecognizers >
        //</ StackLayout >
        //< StackLayout Grid.Column= "1" HorizontalOptions= "Center" VerticalOptions= "Center" >
        //< Image Source= "{translate:Translate logo_transparent}" Style= "{DynamicResource imageLogoTransparent}" ></ Image >
        //</ StackLayout >
        //</ Grid >

        private Image LogoImage;
        private Image MenuImage;
        private Grid GridTopBar;

        public TopBarWithMenu()
        {
            LogoImage = new Image();
            MenuImage = new Image();

            var tapGestureRecognizers = new TapGestureRecognizer()
            {
                Command = new Command(MenuImageAction)
            };
            MenuImage.GestureRecognizers.Add(tapGestureRecognizers);
        }

        private Action _menuImageAction;
        public const string MenuImageActionPropertyName = "MenuImageAction";

        public Action MenuImageAction
        {
            get { return _menuImageAction; }
            set { SetProperty(ref _menuImageAction, value, MenuImageActionPropertyName); }
        }

        private void Initialize()
        {
            this.GridTopBar = new Grid()
            {
                RowDefinitions = new RowDefinitionCollection()
                {
                    new RowDefinition() {Height = this.Height}
                },
                ColumnDefinitions = new ColumnDefinitionCollection()
                {
                    new ColumnDefinition() {Width = 35},
                    new ColumnDefinition() {Width = GridLength.Star},
                    new ColumnDefinition() {Width = 35}
                }
            };
        }

        public void Bind()
        {
            this.Initialize();
            this.GridTopBar.Children.Add(this.MenuImage, 0, 0);
            this.GridTopBar.Children.Add(this.LogoImage, 1, 0);
        }

        private bool _isMenuVisible;
        public const string IsMenuVisiblePropertyName = "IsMenuVisible";

        public bool IsMenuVisible
        {
            get { return _isMenuVisible; }
            set
            {
                SetProperty(ref _isMenuVisible, value, IsMenuVisiblePropertyName,
                    () => { this.MenuImage.IsVisible = this.IsMenuVisible; });
            }
        }

        private Color _barColor;
        public const string BarColorPropertyName = "BarColor";

        public Color BarColor
        {
            get { return _barColor; }
            set
            {
                SetProperty(ref _barColor, value, BarColorPropertyName,
                    () => { this.GridTopBar.BackgroundColor = this.BarColor; });
            }
        }

        private double _barHeight;
        public const string BarHeightPropertyName = "BarHeight";

        public double BarHeight
        {
            get { return _barHeight; }
            set
            {
                SetProperty(ref _barHeight, value, BarHeightPropertyName,
                    () => { this.GridTopBar.HeightRequest = this.BarHeight; });
            }
        }

        private string _logoImageSource;
        public const string LogoImageSourcePropertyName = "LogoImageSource";

        public string LogoImageSource
        {
            get { return _logoImageSource; }
            set
            {
                SetProperty(ref _logoImageSource, value, LogoImageSourcePropertyName,
                    () => { this.LogoImage.Source = this.LogoImageSource; });
            }
        }

        private Style _logoImageStyle;
        public const string LogoImageStylePropertyName = "LogoImageStyle";

        public Style LogoImageStyle
        {
            get { return _logoImageStyle; }
            set
            {
                SetProperty(ref _logoImageStyle, value, LogoImageStylePropertyName,
                    () => { this.LogoImage.Style = this.LogoImageStyle; });
            }
        }

        private Thickness _logoImageMargin;
        public const string LogoImageMarginPropertyName = "LogoImageMargin";

        public Thickness LogoImageMargin
        {
            get { return _logoImageMargin; }
            set
            {
                SetProperty(ref _logoImageMargin, value, LogoImageMarginPropertyName,
                    () => { this.LogoImage.Margin = this.LogoImageMargin; });
            }
        }

        private string _menuImageSource;
        public const string MenuImageSourcePropertyName = "MenuImageSource";

        public string MenuImageSource
        {
            get { return _menuImageSource; }
            set
            {
                SetProperty(ref _menuImageSource, value, MenuImageSourcePropertyName,
                    () => { this.MenuImage.Source = this.MenuImageSource; });
            }
        }

        private Style _menuImageStyle;
        public const string MenuImageStylePropertyName = "MenuImageStyle";

        public Style MenuImageStyle
        {
            get { return _menuImageStyle; }
            set
            {
                SetProperty(ref _menuImageStyle, value, MenuImageStylePropertyName,
                    () => { this.MenuImage.Style = this.MenuImageStyle; });
            }
        }

        private Thickness _menuImageMargin;
        public const string MenuImageMarginPropertyName = "MenuImageMargin";

        public Thickness MenuImageMargin
        {
            get { return _menuImageMargin; }
            set
            {
                SetProperty(ref _menuImageMargin, value, MenuImageMarginPropertyName,
                    () => { this.MenuImage.Margin = this.MenuImageMargin; });
            }
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

            OnPropertyChanging(propertyName);

            backingStore = value;

            if (onChanged != null)
                onChanged();

            OnPropertyChanged(propertyName);
        }

        #region INotifyPropertyChanging implementation

        public event PropertyChangingEventHandler PropertyChanging;

        #endregion INotifyPropertyChanging implementation

        public void OnPropertyChanging(string propertyName)
        {
            if (PropertyChanging == null)
                return;

            PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
        }

        #region INotifyPropertyChanged implementation

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion INotifyPropertyChanged implementation

        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged == null)
                return;

            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}