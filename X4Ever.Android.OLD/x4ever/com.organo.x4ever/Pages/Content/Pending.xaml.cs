using com.organo.x4ever.Pages.Base;
using com.organo.x4ever.ViewModels.Content;
using System;
using com.organo.x4ever.Handler;
using com.organo.x4ever.Services;
using Xamarin.Forms;

namespace com.organo.x4ever.Pages.Content
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
                new ExceptionHandler(TAG, ex);
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