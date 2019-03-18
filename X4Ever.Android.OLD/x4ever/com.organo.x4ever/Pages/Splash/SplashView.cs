using com.organo.x4ever.Statics;
using System;
using System.Threading.Tasks;
using com.organo.x4ever.Handler;
using com.organo.x4ever.Helpers;
using Xamarin.Forms;

namespace com.organo.x4ever.Pages.Splash
{
    public class SplashView : ContentPage
    {
        private Image splashImage;

        public SplashView()
        {
            try
            {
                App.Configuration.InitialAsync(this, false);
                NavigationPage.SetHasNavigationBar(this, false);
                var sub = new AbsoluteLayout();
                var imageSize = App.Configuration.GetImageSizeByID(ImageIdentity.MAIN_PAGE_LOGO);
                splashImage = new Image()
                {
                    Source = imageSize.ImageName,
                    WidthRequest = 130,
                    HeightRequest = 100
                };

                AbsoluteLayout.SetLayoutFlags(splashImage,
                    AbsoluteLayoutFlags.PositionProportional);

                AbsoluteLayout.SetLayoutBounds(splashImage,
                    new Rectangle(0.5, 0.5, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));

                sub.Children.Add(splashImage);
                this.Content = sub;
            }
            catch (Exception ex)
            {
                var m = ex;
            }
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            try
            {
                await splashImage.ScaleTo(1, 1000);
                await splashImage.ScaleTo(0.9, 850);
                await splashImage.ScaleTo(150, 700);
                await App.CurrentApp.MainPage.Navigation.PopModalAsync();
            }
            catch (Exception ex)
            {
                new ExceptionHandler("SplashView.cs", ex);
            }
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}