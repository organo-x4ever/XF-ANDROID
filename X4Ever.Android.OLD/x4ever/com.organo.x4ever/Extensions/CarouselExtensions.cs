using Xamarin.Forms;

namespace com.organo.x4ever.Extensions
{
    public static class CarouselExtensions
    {
        public static void PageFirst(this CarouselPage carouselPage)
        {
            var pageCount = carouselPage.Children.Count;
            if (pageCount < 2)
                return;

            var index = 0;
            carouselPage.CurrentPage = carouselPage.Children[index];
        }

        public static void PageLast(this CarouselPage carouselPage)
        {
            var pageCount = carouselPage.Children.Count;
            if (pageCount < 2)
                return;

            var index = pageCount - 1;
            carouselPage.CurrentPage = carouselPage.Children[index];
        }

        public static void PageNext(this CarouselPage carouselPage)
        {
            var pageCount = carouselPage.Children.Count;
            if (pageCount < 2)
                return;

            var index = carouselPage.Children.IndexOf(carouselPage.CurrentPage);
            index++;
            if (index >= pageCount)
                index = 0;
            carouselPage.CurrentPage = carouselPage.Children[index];
        }

        public static void PagePrevious(this CarouselPage carouselPage)
        {
            var pageCount = carouselPage.Children.Count;
            if (pageCount < 2)
                return;

            var index = carouselPage.Children.IndexOf(carouselPage.CurrentPage);
            index--;
            if (index < 0)
                index = pageCount - 1;

            carouselPage.CurrentPage = carouselPage.Children[index];
        }
    }
}