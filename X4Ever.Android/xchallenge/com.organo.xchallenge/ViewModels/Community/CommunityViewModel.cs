using com.organo.xchallenge.Globals;
using com.organo.xchallenge.Localization;
using com.organo.xchallenge.Models;
using com.organo.xchallenge.Pages;
using com.organo.xchallenge.Services;
using com.organo.xchallenge.Statics;
using com.organo.xchallenge.ViewModels.Base;
using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace com.organo.xchallenge.ViewModels.Community
{
    public class CommunityViewModel : BaseViewModel
    {
        public CommunityViewModel(INavigation navigation = null) : base(navigation)
        {
            this.PageBackgroundImage = DependencyService.Get<IHelper>()
                .GetFileUri(TextResources.image_community_background, FileType.Image);
            LoadContent();
        }

        private async void LoadContent() =>
            ApplicationSetting = await DependencyService.Get<IApplicationSettingService>().GetAsync();

        private ImageSource _pageBackgroundImage;
        public const string PageBackgroundImagePropertyName = "PageBackgroundImage";

        public ImageSource PageBackgroundImage
        {
            get { return _pageBackgroundImage; }
            set { SetProperty(ref _pageBackgroundImage, value, PageBackgroundImagePropertyName); }
        }

        //private RootPage root;
        //public const string RootPropertyName = "Root";

        //public RootPage Root
        //{
        //    get { return root; }
        //    set { SetProperty(ref root, value, RootPropertyName); }
        //}

        private ApplicationSetting _applicationSetting;
        public const string ApplicationSettingPropertyName = "ApplicationSetting";

        public ApplicationSetting ApplicationSetting
        {
            get { return _applicationSetting; }
            set { SetProperty(ref _applicationSetting, value, ApplicationSettingPropertyName); }
        }

        private string FacebookUrl => ApplicationSetting?.CommunityFacebookUrl;
        private string InstagramUrl => ApplicationSetting?.CommunityInstagramUrl;

        //private ICommand _showSideMenuCommand;

        //public ICommand ShowSideMenuCommand
        //{
        //    get
        //    {
        //        return _showSideMenuCommand ?? (_showSideMenuCommand = new Command((obj) =>
        //        {
        //            this.Root.IsPresented = this.Root.IsPresented == false;
        //        }));
        //    }
        //}

        private ICommand _facebookCommand;

        public ICommand FacebookCommand
        {
            get
            {
                return _facebookCommand ?? (_facebookCommand = new Command((obj) =>
                {
                    Device.OpenUri(new Uri(FacebookUrl));
                }));
            }
        }

        private ICommand _instagramCommand;

        public ICommand InstagramCommand
        {
            get
            {
                return _instagramCommand ?? (_instagramCommand = new Command((obj) =>
                {
                    Device.OpenUri(new Uri(InstagramUrl));
                }));
            }
        }
    }
}