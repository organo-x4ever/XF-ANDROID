using com.organo.xchallenge.Pages.Base;
using com.organo.xchallenge.ViewModels.News;
using System;
using System.Threading.Tasks;
using com.organo.xchallenge.Handler;
using com.organo.xchallenge.Pages.Notification;
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
                _model = new NewsViewModel()
                {
                    Root = root,
                    IsUserSettingVisible = false
                };
                Init();
            }
            catch (Exception ex)
            {
                var exceptionHandler = new ExceptionHandler(TAG, ex);
            }
        }

        private async void Init()
        {
            BindingContext = _model;
            SetGridNews();
            await App.Configuration.InitialAsync(this);
            NavigationPage.SetHasNavigationBar(this, false);

            //await Navigation.PushAsync(new NotificationPage());
        }

        private async void SetGridNews()
        {
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