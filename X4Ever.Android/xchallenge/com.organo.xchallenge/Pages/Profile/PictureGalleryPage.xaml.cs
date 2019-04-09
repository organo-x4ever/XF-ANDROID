using com.organo.xchallenge.Pages.Base;
using com.organo.xchallenge.ViewModels.Profile;
using System;
using com.organo.xchallenge.Globals;
using com.organo.xchallenge.Localization;
using Xamarin.Forms;

namespace com.organo.xchallenge.Pages.Profile
{
    public partial class PictureGalleryPage : PictureGalleryPageXaml
    {
        private MyProfileViewModel _model;

        public PictureGalleryPage(MyProfileViewModel model)
        {
            try
            {
                InitializeComponent();
                this._model = model;
                Init();
            }
            catch (Exception ex)
            {
                DependencyService.Get<IMessage>().AlertAsync(TextResources.Alert,
                    ex.InnerException != null ? ex.InnerException.Message : ex.Message, TextResources.Ok);
            }
        }

        private async void Init()
        {
            await App.Configuration.InitialAsync(this);
            NavigationPage.SetHasNavigationBar(this, false);

            _model.Navigation = App.CurrentApp.MainPage.Navigation;
            BindingContext = _model;
            ListViewGallery.ItemSelected += (sender, e) => ListViewGallery.SelectedItem = null;
        }

        protected override bool OnBackButtonPressed()
        {
            _model.ShowGalleryDetail = false;
            return true;
        }
    }

    public abstract class PictureGalleryPageXaml : ModelBoundContentPage<MyProfileViewModel> { }
}