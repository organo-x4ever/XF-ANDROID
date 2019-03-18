
using com.organo.xchallenge.Pages.Base;
using com.organo.xchallenge.ViewModels.Profile;
using Xamarin.Forms;

namespace com.organo.xchallenge.Pages.Tracker
{
    public partial class TrackerGalleryPage : TrackerGalleryXamlPage
    {
        private MyProfileViewModel _model;

        public TrackerGalleryPage(MyProfileViewModel profileViewModel)
        {
            InitializeComponent();
            App.Configuration.InitialAsync(this);
            NavigationPage.SetHasNavigationBar(this, false);
            _model = profileViewModel;
            _model.Navigation = App.CurrentApp.MainPage.Navigation;
            BindingContext = _model;
            SetContentAsync();
        }

        private void SetContentAsync()
        {
            var label = new Label()
            {
                Style = (Style) App.CurrentApp.Resources["labelStyleTextTitleIntern"],
                Margin = new Thickness(10, 10, 10, 0)
            };
            var stackLayoutPicture = new StackLayout()
            {
                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalOptions = LayoutOptions.StartAndExpand,
                Orientation = StackOrientation.Horizontal,
                Margin = new Thickness(10, 5, 10, 0)
            };
            var imageFront = new Image()
            {
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.Center,
            };
            var imageSide = new Image()
            {
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.Center,
            };
            foreach (var userTracker in _model.UserTrackersDescending)
            {
                // Group Title
                label.Text = userTracker.ModifyDateDisplay;
                stackLayout.Children.Add(label);

                // Image Front
                imageFront.HeightRequest = userTracker.PictureHeight;
                imageFront.WidthRequest = userTracker.PictureWidth;
                imageFront.Source = userTracker.FrontImageSource;
                stackLayoutPicture.Children.Add(imageFront);

                // Image Side
                imageSide.HeightRequest = userTracker.PictureHeight;
                imageSide.WidthRequest = userTracker.PictureWidth;
                imageSide.Source = userTracker.SideImageSource;
                stackLayoutPicture.Children.Add(imageSide);

                stackLayout.Children.Add(stackLayoutPicture);
            }
        }

        protected override bool OnBackButtonPressed()
        {
            _model.ShowGalleryDetail = false;
            return true;
        }
    }

    public abstract class TrackerGalleryXamlPage : ModelBoundContentPage<MyProfileViewModel>
    {
    }
}