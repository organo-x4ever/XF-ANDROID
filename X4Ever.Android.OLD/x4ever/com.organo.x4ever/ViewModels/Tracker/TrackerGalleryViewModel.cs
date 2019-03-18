using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using com.organo.x4ever.Models.User;
using com.organo.x4ever.Pages;
using com.organo.x4ever.ViewModels.Base;
using Xamarin.Forms;

namespace com.organo.x4ever.ViewModels.Tracker
{
    public class TrackerGalleryViewModel : BaseViewModel
    {
        public TrackerGalleryViewModel(INavigation navigation = null) : base(navigation)
        {

        }

        //public async Task SetContentAsync()
        //{
        //    var stackLayout = new StackLayout()
        //    {
        //        HorizontalOptions = LayoutOptions.StartAndExpand,
        //        VerticalOptions = LayoutOptions.StartAndExpand,
        //        Orientation = StackOrientation.Vertical
        //    };
        //    var label = new Label()
        //    {
        //        Style = (Style) App.CurrentApp.Resources["labelStyleTextTitleIntern"],
        //        Margin = new Thickness(10, 10, 10, 0)
        //    };
        //    var stackLayoutPicture = new StackLayout()
        //    {
        //        HorizontalOptions = LayoutOptions.StartAndExpand,
        //        VerticalOptions = LayoutOptions.StartAndExpand,
        //        Orientation = StackOrientation.Horizontal,
        //        Margin = new Thickness(10, 5, 10, 0)
        //    };
        //    var imageFront = new Image()
        //    {
        //        HorizontalOptions = LayoutOptions.Start,
        //        VerticalOptions = LayoutOptions.Center,
        //    };
        //    var imageSide = new Image()
        //    {
        //        HorizontalOptions = LayoutOptions.Start,
        //        VerticalOptions = LayoutOptions.Center,
        //    };
        //    foreach (var userTracker in UserTrackers)
        //    {
        //        // Group Title
        //        label.Text = userTracker.ModifyDateDisplay;
        //        stackLayout.Children.Add(label);

        //        // Image Front
        //        imageFront.HeightRequest = userTracker.PictureHeight;
        //        imageFront.WidthRequest = userTracker.PictureWidth;
        //        imageFront.Source = userTracker.FrontImageSource;
        //        stackLayoutPicture.Children.Add(imageFront);

        //        // Image Side
        //        imageSide.HeightRequest = userTracker.PictureHeight;
        //        imageSide.WidthRequest = userTracker.PictureWidth;
        //        imageSide.Source = userTracker.SideImageSource;
        //        stackLayoutPicture.Children.Add(imageSide);

        //        stackLayout.Children.Add(stackLayoutPicture);
        //    }

        //    Content = stackLayout;
        //}

        private List<UserTracker> userTrackers;
        public const string UserTrackersPropertyName = "UserTrackers";

        public List<UserTracker> UserTrackers
        {
            get { return userTrackers; }
            set { SetProperty(ref userTrackers, value, UserTrackersPropertyName); }
        }

        private View content;
        public const string ContentPropertyName = "Content";

        public View Content
        {
            get { return content; }
            set { SetProperty(ref content, value, ContentPropertyName); }
        }
    }
}