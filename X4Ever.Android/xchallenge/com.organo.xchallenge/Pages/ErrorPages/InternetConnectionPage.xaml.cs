using System;
using com.organo.xchallenge.Handler;
using com.organo.xchallenge.Pages.Base;
using com.organo.xchallenge.Services;
using com.organo.xchallenge.ViewModels.ErrorPages;
using Xamarin.Forms;

namespace com.organo.xchallenge.Pages.ErrorPages
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
                var exceptionHandler = new ExceptionHandler(TAG, ex);
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