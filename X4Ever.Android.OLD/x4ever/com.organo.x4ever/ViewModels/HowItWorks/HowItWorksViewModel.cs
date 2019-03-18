using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using com.organo.x4ever.Helpers;
using com.organo.x4ever.Localization;
using com.organo.x4ever.Models;
using com.organo.x4ever.Pages;
using com.organo.x4ever.Statics;
using com.organo.x4ever.ViewModels.Base;
using Xamarin.Forms;

namespace com.organo.x4ever.ViewModels.HowItWorks
{
    public class HowItWorksViewModel : BaseViewModel
    {
        public HowItWorksViewModel(INavigation navigation = null) : base(navigation)
        {
            SetPageImageSize();
        }

        public async Task LoadAsync()
        {
            await Task.Run(() =>
            {
                SignUpSource = ImageResizer.ResizeImage(TextResources.page_sign_up_image, ImageSize_SignUp);
                ProductsSource = ImageResizer.ResizeImage(TextResources.page_product_image, ImageSize_Products);
                LoseWeightSource = ImageResizer.ResizeImage(TextResources.page_lose_weight_image, ImageSize_LoseWeight);
                EarnRewardsSource =
                    ImageResizer.ResizeImage(TextResources.page_earn_reward_image, ImageSize_EarnRewards);
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

        private ImageSource _signUpSource;
        public const string SignUpSourcePropertyName = "SignUpSource";

        public ImageSource SignUpSource
        {
            get { return _signUpSource; }
            set { SetProperty(ref _signUpSource, value, SignUpSourcePropertyName); }
        }

        private ImageSource _productsSource;
        public const string ProductsSourcePropertyName = "ProductsSource";

        public ImageSource ProductsSource
        {
            get { return _productsSource; }
            set { SetProperty(ref _productsSource, value, ProductsSourcePropertyName); }
        }

        private ImageSource _loseWeightSource;
        public const string LoseWeightSourcePropertyName = "LoseWeightSource";

        public ImageSource LoseWeightSource
        {
            get { return _loseWeightSource; }
            set { SetProperty(ref _loseWeightSource, value, LoseWeightSourcePropertyName); }
        }

        private ImageSource _earnRewardsSource;
        public const string EarnRewardsSourcePropertyName = "EarnRewardsSource";

        public ImageSource EarnRewardsSource
        {
            get { return _earnRewardsSource; }
            set { SetProperty(ref _earnRewardsSource, value, EarnRewardsSourcePropertyName); }
        }

        private ImageSize ImageSize_SignUp { get; set; }
        private ImageSize ImageSize_Products { get; set; }
        private ImageSize ImageSize_LoseWeight { get; set; }
        private ImageSize ImageSize_EarnRewards { get; set; }

        private void SetPageImageSize()
        {
            ImageSize_SignUp = App.Configuration.GetImageSizeByID(ImageIdentity.PAGE_IMAGE_SIGN_UP);
            if (ImageSize_SignUp != null)
            {
                ImageHeight_SignUp = ImageSize_SignUp.Height;
                ImageWidth_SignUp = ImageSize_SignUp.Width;
            }

            ImageSize_Products = App.Configuration.GetImageSizeByID(ImageIdentity.PAGE_IMAGE_PRODUCTS);
            if (ImageSize_Products != null)
            {
                ImageHeight_Products = ImageSize_Products.Height;
                ImageWidth_Products = ImageSize_Products.Width;
            }

            ImageSize_LoseWeight = App.Configuration.GetImageSizeByID(ImageIdentity.PAGE_IMAGE_LOSE_WEIGHT);
            if (ImageSize_LoseWeight != null)
            {
                ImageHeight_LoseWeight = ImageSize_LoseWeight.Height;
                ImageWidth_LoseWeight = ImageSize_LoseWeight.Width;
            }

            ImageSize_EarnRewards = App.Configuration.GetImageSizeByID(ImageIdentity.PAGE_IMAGE_EARN_REWARDS);
            if (ImageSize_EarnRewards != null)
            {
                ImageHeight_EarnRewards = ImageSize_EarnRewards.Height;
                ImageWidth_EarnRewards = ImageSize_EarnRewards.Width;
            }
        }

        private float imageHeight_SignUp;
        public const string ImageHeight_SignUpPropertyName = "ImageHeight_SignUp";

        public float ImageHeight_SignUp
        {
            get { return imageHeight_SignUp; }
            set { SetProperty(ref imageHeight_SignUp, value, ImageHeight_SignUpPropertyName); }
        }

        private float imageWidth_SignUp;
        public const string ImageWidth_SignUpPropertyName = "ImageWidth_SignUp";

        public float ImageWidth_SignUp
        {
            get { return imageWidth_SignUp; }
            set { SetProperty(ref imageWidth_SignUp, value, ImageWidth_SignUpPropertyName); }
        }

        private float imageHeight_Products;
        public const string ImageHeight_ProductsPropertyName = "ImageHeight_Products";

        public float ImageHeight_Products
        {
            get { return imageHeight_Products; }
            set { SetProperty(ref imageHeight_Products, value, ImageHeight_ProductsPropertyName); }
        }

        private float imageWidth_Products;
        public const string ImageWidth_ProductsPropertyName = "ImageWidth_Products";

        public float ImageWidth_Products
        {
            get { return imageWidth_Products; }
            set { SetProperty(ref imageWidth_Products, value, ImageWidth_ProductsPropertyName); }
        }
        
        private float imageHeight_LoseWeight;
        public const string ImageHeight_LoseWeightPropertyName = "ImageHeight_LoseWeight";

        public float ImageHeight_LoseWeight
        {
            get { return imageHeight_LoseWeight; }
            set { SetProperty(ref imageHeight_LoseWeight, value, ImageHeight_LoseWeightPropertyName); }
        }

        private float imageWidth_LoseWeight;
        public const string ImageWidth_LoseWeightPropertyName = "ImageWidth_LoseWeight";

        public float ImageWidth_LoseWeight
        {
            get { return imageWidth_LoseWeight; }
            set { SetProperty(ref imageWidth_LoseWeight, value, ImageWidth_LoseWeightPropertyName); }
        }

        private float imageHeight_EarnRewards;
        public const string ImageHeight_EarnRewardsPropertyName = "ImageHeight_EarnRewards";

        public float ImageHeight_EarnRewards
        {
            get { return imageHeight_EarnRewards; }
            set { SetProperty(ref imageHeight_EarnRewards, value, ImageHeight_EarnRewardsPropertyName); }
        }

        private float imageWidth_EarnRewards;
        public const string ImageWidth_EarnRewardsPropertyName = "ImageWidth_EarnRewards";

        public float ImageWidth_EarnRewards
        {
            get { return imageWidth_EarnRewards; }
            set { SetProperty(ref imageWidth_EarnRewards, value, ImageWidth_EarnRewardsPropertyName); }
        }
    }
}