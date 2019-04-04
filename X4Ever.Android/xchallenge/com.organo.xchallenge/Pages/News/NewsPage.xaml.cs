using com.organo.xchallenge.Pages.Base;
using com.organo.xchallenge.ViewModels.News;
using System;
using System.Threading.Tasks;
using com.organo.xchallenge.Handler;
using com.organo.xchallenge.Services;
using Xamarin.Forms;

namespace com.organo.xchallenge.Pages.News
{
    public partial class NewsPage : NewsPageXaml
    {
        private NewsViewModel _model;

        public NewsPage(RootPage root)
        {
            try
            {
                InitializeComponent();
                this._model = new NewsViewModel()
                {
                    Root = root
                };
                this.Init();
            }
            catch (Exception ex)
            {
                new ExceptionHandler(TAG, ex);
            }
        }

        private async void Init()
        {
            await App.Configuration.InitialAsync(this);
            NavigationPage.SetHasNavigationBar(this, false);
            BindingContext = this._model;
            GridNews.Source = await _model.GetAsync();
        }

        protected override bool OnBackButtonPressed()
        {
            return DependencyService.Get<IBackButtonPress>().Redirect(_model.Root);
        }
    }

    public abstract class NewsPageXaml : ModelBoundContentPage<NewsViewModel>
    {
    }
}