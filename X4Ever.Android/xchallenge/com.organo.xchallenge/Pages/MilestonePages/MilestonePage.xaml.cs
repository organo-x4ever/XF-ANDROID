using com.organo.xchallenge.Pages.Base;
using com.organo.xchallenge.ViewModels.Milestones;
using com.organo.xchallenge.ViewModels.Profile;
using System;
using com.organo.xchallenge.Globals;
using com.organo.xchallenge.Localization;
using Xamarin.Forms;

namespace com.organo.xchallenge.Pages.MilestonePages
{
    public partial class MilestonePage : MilestonePageXaml
    {
        private readonly MilestoneViewModel _model;

        public MilestonePage(RootPage root, MyProfileViewModel profileViewModel)
        {
            try
            {
                InitializeComponent();
                App.Configuration.InitialAsync(this);
                NavigationPage.SetHasNavigationBar(this, false);
                _model = new MilestoneViewModel(App.CurrentApp.MainPage.Navigation)
                {
                    Root = root,
                    ProfileViewModel = profileViewModel
                };
                BindingContext = _model;

                //Device.BeginInvokeOnMainThread(async () =>
                //{
                //    // Works
                //    await DisplayAlert("Testing!", "Some text", "OK");

                //    // Does not work
                //    await DisplayActionSheet("Test", "Cancel", "Destroy", new[] {"1", "2"});
                //});
            }
            catch (Exception ex)
            {
                DependencyService.Get<IMessage>().AlertAsync(TextResources.Alert,
                    ex.InnerException != null ? ex.InnerException.Message : ex.Message, TextResources.Ok);
            }
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }

    public abstract class MilestonePageXaml : ModelBoundContentPage<MilestoneViewModel> { }
}