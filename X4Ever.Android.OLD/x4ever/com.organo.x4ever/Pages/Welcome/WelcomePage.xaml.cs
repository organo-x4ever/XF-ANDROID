using com.organo.x4ever.Pages.Base;
using com.organo.x4ever.Services;
using com.organo.x4ever.ViewModels.Welcome;
using System;
using com.organo.x4ever.Globals;
using com.organo.x4ever.Localization;
using com.organo.x4ever.Statics;
using Xamarin.Forms;

namespace com.organo.x4ever.Pages.Welcome
{
    public partial class WelcomePage : WelcomePageXaml
    {
        private WelcomeViewModel _model;
        private static IBackButtonPress _backButtonPress;
        public WelcomePage()
        {
            try
            {
                InitializeComponent();
                this.Initial();
            }
            catch (Exception ex)
            {
                DependencyService.Get<IMessage>().AlertAsync(TextResources.Alert,
                    ex.InnerException != null ? ex.InnerException.Message : ex.Message, TextResources.Ok);
            }
        }

        private async void Initial()
        {
            await App.Configuration.InitialAsync(this);
            NavigationPage.SetHasNavigationBar(this, false);
            _model = new WelcomeViewModel();
            _model.SetActivityResource(false, true, false, false, TextResources.ValidatingCredentials, string.Empty,
                string.Empty);
            BindingContext = _model;
            _backButtonPress = DependencyService.Get<IBackButtonPress>();
            await _model.OnLoad();
        }

        protected override void OnDisappearing()
        {
            if (_model.MediaFiles.Count > 0)
                _model.StopPlayer();
            base.OnDisappearing();
        }

        protected override bool OnBackButtonPressed()
        {
            return _backButtonPress.Exit();
        }
    }

    public abstract class WelcomePageXaml : ModelBoundContentPage<WelcomeViewModel>
    {
    }
}