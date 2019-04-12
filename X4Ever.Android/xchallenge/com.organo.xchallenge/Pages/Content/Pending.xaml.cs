using com.organo.xchallenge.Pages.Base;
using com.organo.xchallenge.ViewModels.Content;
using System;
using com.organo.xchallenge.Handler;
using com.organo.xchallenge.Services;
using Xamarin.Forms;

namespace com.organo.xchallenge.Pages.Content
{
    public partial class Pending : PendingXaml
    {
        private ContentViewModel _model;

        public Pending(RootPage root)
        {
            try
            {
                InitializeComponent();
                App.Configuration.InitialAsync(this);
                _model = new ContentViewModel()
                {
                    Root = root
                };
                BindingContext = _model;
            }
            catch (Exception ex)
            {
                var exceptionHandler = new ExceptionHandler(TAG, ex);
            }
        }

        protected override bool OnBackButtonPressed()
        {
            return DependencyService.Get<IBackButtonPress>().Redirect(_model.Root);
        }
    }

    public abstract class PendingXaml : ModelBoundContentPage<ContentViewModel>
    {
    }
}