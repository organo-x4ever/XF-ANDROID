using com.organo.x4ever.Pages.Base;
using com.organo.x4ever.ViewModels.Profile;
using System;
using com.organo.x4ever.Globals;
using com.organo.x4ever.Localization;
using Xamarin.Forms;

namespace com.organo.x4ever.Pages.Profile
{
    public partial class TrackerDetailPage : TrackerDetailPageXaml
    {
        private MyProfileViewModel _model;

        public TrackerDetailPage(MyProfileViewModel model)
        {
            try
            {
                InitializeComponent();
                App.Configuration.InitialAsync(this);
                NavigationPage.SetHasNavigationBar(this, false);
                _model = model;
                _model.Navigation = App.CurrentApp.MainPage.Navigation;
                BindingContext = _model;
            }
            catch (Exception ex)
            {
                DependencyService.Get<IMessage>().AlertAsync(TextResources.Alert,
                    ex.InnerException != null ? ex.InnerException.Message : ex.Message, TextResources.Ok);
            }
        }

        protected override bool OnBackButtonPressed()
        {
            _model.ShowTrackerDetail = false;
            return true;
        }
    }

    public abstract class TrackerDetailPageXaml : ModelBoundContentPage<MyProfileViewModel> { }
}