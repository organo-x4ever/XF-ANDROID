using com.organo.xchallenge.ViewModels.Profile;
using System;
using com.organo.xchallenge.Handler;
using com.organo.xchallenge.Services;
using Xamarin.Forms;

namespace com.organo.xchallenge.Pages.Profile
{
    public partial class Settings : SettingsXaml
    {
        private SettingsViewModel _model;

        public Settings(RootPage root)
        {
            try
            {
                InitializeComponent();
                App.Configuration.InitialAsync(this);
                NavigationPage.SetHasNavigationBar(this, false);
                _model = new SettingsViewModel()
                {
                    Root = root
                };
                BindingContext = _model;
            }
            catch (Exception ex)
            {
                var exceptionHandler = new ExceptionHandler("Settings.xaml.cs", ex);
            }
        }

        private async void EditProfile_Tapped(object sender, EventArgs e)
        {
            if (_model.Selected != TabTitle.EditProfile)
                await _model.TabSelected(TabTitle.EditProfile);
        }

        private async void ChangePassword_Tapped(object sender, EventArgs e)
        {
            if (_model.Selected != TabTitle.ChangePassword)
                await _model.TabSelected(TabTitle.ChangePassword);
        }

        private async void UserSettings_Tapped(object sender, EventArgs e)
        {
            if (_model.Selected != TabTitle.UserSettings)
                await _model.TabSelected(TabTitle.UserSettings);
        }

        protected override bool OnBackButtonPressed()
        {
            return DependencyService.Get<IBackButtonPress>().Redirect(_model.Root);
        }
    }

    public abstract class SettingsXaml : Base.ModelBoundContentPage<SettingsViewModel>
    {
    }
}