using System;
using com.organo.x4ever.Handler;
using com.organo.x4ever.Pages.Base;
using com.organo.x4ever.Services;
using com.organo.x4ever.ViewModels.ErrorPages;
using Xamarin.Forms;

namespace com.organo.x4ever.Pages.ErrorPages
{
    public partial class InternetConnectionPage : InternetConnectionXamlPage
    {
        private InternetConnectionViewModel _model;

        public InternetConnectionPage()
        {
            try
            {
                App.Configuration.InitialAsync(this, false);
                NavigationPage.SetHasNavigationBar(this, false);
                this._model = new InternetConnectionViewModel();
                InitializeComponent();
                BindingContext = this._model;
            }
            catch (Exception ex)
            {
                new ExceptionHandler(TAG, ex);
            }
        }

        protected override bool OnBackButtonPressed()
        {
            return DependencyService.Get<IBackButtonPress>().Exit();
        }
    }

    public abstract class InternetConnectionXamlPage : ModelBoundContentPage<InternetConnectionViewModel>
    {
    }
}