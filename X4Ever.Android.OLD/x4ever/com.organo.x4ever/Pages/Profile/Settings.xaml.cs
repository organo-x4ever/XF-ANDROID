using com.organo.x4ever.ViewModels.Profile;
using System;
using com.organo.x4ever.Globals;
using com.organo.x4ever.Localization;
using com.organo.x4ever.Services;
using Xamarin.Forms;

namespace com.organo.x4ever.Pages.Profile
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
                this._model = new SettingsViewModel()
                {
                    Root = root
                };
                BindingContext = this._model;
            }
            catch (Exception ex)
            {
                DependencyService.Get<IMessage>().AlertAsync(TextResources.Alert,
                    ex.InnerException != null ? ex.InnerException.Message : ex.Message, TextResources.Ok);
            }
        }

        private void EditProfile_Tapped(object sender, EventArgs e)
        {
            if (this._model.Selected != TabTitle.EditProfile)
                this._model.TabSelected(TabTitle.EditProfile);
        }

        private void ChangePassword_Tapped(object sender, EventArgs e)
        {
            if (this._model.Selected != TabTitle.ChangePassword)
                this._model.TabSelected(TabTitle.ChangePassword);
        }

        private void UserSettings_Tapped(object sender, EventArgs e)
        {
            if (this._model.Selected != TabTitle.UserSettings)
                this._model.TabSelected(TabTitle.UserSettings);
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