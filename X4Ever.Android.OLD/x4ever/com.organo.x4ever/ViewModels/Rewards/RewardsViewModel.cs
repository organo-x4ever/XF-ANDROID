
using System.Threading.Tasks;
using System.Windows.Input;
using com.organo.x4ever.Helpers;
using com.organo.x4ever.Localization;
using com.organo.x4ever.Models;
using com.organo.x4ever.Pages;
using com.organo.x4ever.Statics;
using com.organo.x4ever.ViewModels.Base;
using Xamarin.Forms;

namespace com.organo.x4ever.ViewModels.Rewards
{
    public class RewardsViewModel : BaseViewModel
    {
        public RewardsViewModel(INavigation navigation = null) : base(navigation)
        {
            SetPageImageSize();
        }

        public async Task LoadAsync()
        {
            await Task.Run(() =>
            {
                TShirt10LBSource =
                    ImageResizer.ResizeImage(TextResources.page_tshirt_lose_10_image, ImageSize_TShirt10LB);
                TShirtsBundleSource =
                    ImageResizer.ResizeImage(TextResources.page_tshirts_bundle_image, ImageSize_TShirtsBundle);
            });
        }

        private RootPage root;
        public const string RootPropertyName = "Root";

        public RootPage Root
        {
            get { return root; }
            set { SetProperty(ref root, value, RootPropertyName); }
        }

        private ICommand _showSideMenuCommand;

        public ICommand ShowSideMenuCommand
        {
            get
            {
                return _showSideMenuCommand ?? (_showSideMenuCommand = new Command((obj) =>
                {
                    Root.IsPresented = Root.IsPresented == false;
                }));
            }
        }

        private ImageSource _tShirt10LBSource;
        public const string TShirt10LBSourcePropertyName = "TShirt10LBSource";

        public ImageSource TShirt10LBSource
        {
            get { return _tShirt10LBSource; }
            set { SetProperty(ref _tShirt10LBSource, value, TShirt10LBSourcePropertyName); }
        }

        private ImageSource _tShirtsBundleSource;
        public const string TShirtsBundleSourcePropertyName = "TShirtsBundleSource";

        public ImageSource TShirtsBundleSource
        {
            get { return _tShirtsBundleSource; }
            set { SetProperty(ref _tShirtsBundleSource, value, TShirtsBundleSourcePropertyName); }
        }

        private ImageSize ImageSize_TShirt10LB { get; set; }
        private ImageSize ImageSize_TShirtsBundle { get; set; }

        private void SetPageImageSize()
        {
            ImageSize_TShirt10LB = App.Configuration.GetImageSizeByID(ImageIdentity.PAGE_IMAGE_T_SHIRT);
            if (ImageSize_TShirt10LB != null)
            {
                ImageHeight_TShirt10LB = ImageSize_TShirt10LB.Height;
                ImageWidth_TShirt10LB = ImageSize_TShirt10LB.Width;
            }

            ImageSize_TShirtsBundle = App.Configuration.GetImageSizeByID(ImageIdentity.PAGE_IMAGE_T_SHIRTS_BUNDLE);
            if (ImageSize_TShirtsBundle != null)
            {
                ImageHeight_TShirtsBundle = ImageSize_TShirtsBundle.Height;
                ImageWidth_TShirtsBundle = ImageSize_TShirtsBundle.Width;
            }
        }

        private float imageHeight_TShirt10LB;
        public const string ImageHeight_TShirt10LBPropertyName = "ImageHeight_TShirt10LB";

        public float ImageHeight_TShirt10LB
        {
            get { return imageHeight_TShirt10LB; }
            set { SetProperty(ref imageHeight_TShirt10LB, value, ImageHeight_TShirt10LBPropertyName); }
        }

        private float imageWidth_TShirt10LB;
        public const string ImageWidth_TShirt10LBPropertyName = "ImageWidth_TShirt10LB";

        public float ImageWidth_TShirt10LB
        {
            get { return imageWidth_TShirt10LB; }
            set { SetProperty(ref imageWidth_TShirt10LB, value, ImageWidth_TShirt10LBPropertyName); }
        }

        private float imageHeight_TShirtsBundle;
        public const string ImageHeight_TShirtsBundlePropertyName = "ImageHeight_TShirtsBundle";

        public float ImageHeight_TShirtsBundle
        {
            get { return imageHeight_TShirtsBundle; }
            set { SetProperty(ref imageHeight_TShirtsBundle, value, ImageHeight_TShirtsBundlePropertyName); }
        }

        private float imageWidth_TShirtsBundle;
        public const string ImageWidth_TShirtsBundlePropertyName = "ImageWidth_TShirtsBundle";

        public float ImageWidth_TShirtsBundle
        {
            get { return imageWidth_TShirtsBundle; }
            set { SetProperty(ref imageWidth_TShirtsBundle, value, ImageWidth_TShirtsBundlePropertyName); }
        }
    }
}