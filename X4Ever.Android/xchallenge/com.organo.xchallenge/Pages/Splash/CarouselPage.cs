
using System;
using Xamarin.Forms;

namespace com.organo.xchallenge.Pages.Splash
{
    public class CarouselPage : ContentView, IExtendedCarouselPage
    {
        public event Action PageAppearing;
        public event Action PageDisappearing;
        public event Action PageAppeared;
        public event Action PageDisappeared;

        public CarouselPage() : base()
        {

        }

        #region ICarouselView implementation

        public void OnPageAppearing()
        {
            if (PageAppearing != null)
            {
                PageAppearing();
            }
        }
        public void OnPageDisappearing()
        {
            if (PageDisappearing != null)
            {
                PageDisappearing();
            }
        }
        public void OnPageAppeared()
        {
            if (PageAppeared != null)
            {
                PageAppeared();
            }
        }
        public void OnPageDisappeared()
        {
            if (PageDisappeared != null)
            {
                PageDisappeared();
            }
        }

        #endregion ICarouselView implementation
    }
}