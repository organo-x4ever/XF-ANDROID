using System;
using com.organo.x4ever.Pages.Base;
using com.organo.x4ever.Services;
using com.organo.x4ever.ViewModels.ErrorPages;
using Xamarin.Forms;

namespace com.organo.x4ever.Pages.ErrorPages
{
    public partial class ErrorPage : ErrorPageXaml
    {
        private ErrorViewModel _model;

        public ErrorPage(Exception ex)
        {
            InitializeComponent();
            _model = new ErrorViewModel();
            App.Configuration.InitialAsync(this, false);
            NavigationPage.SetHasNavigationBar(this, false);
            BindingContext = _model;
            LoadPage(ex);
        }

        private async void LoadPage(Exception ex)
        {
            await ClientService.WriteLog(null, ex.Message, ex.InnerException != null ? ex.InnerException.Message : "",
                true);
        }

        protected override bool OnBackButtonPressed()
        {
            return DependencyService.Get<IBackButtonPress>().Exit();
        }
    }

    public abstract class ErrorPageXaml : ModelBoundContentPage<ErrorViewModel>
    {
    }
}