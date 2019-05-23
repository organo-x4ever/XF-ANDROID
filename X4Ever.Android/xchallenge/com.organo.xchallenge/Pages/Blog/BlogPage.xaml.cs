
using com.organo.xchallenge.ViewModels.Blog;
using com.organo.xchallenge.Pages.Base;
using System;
using com.organo.xchallenge.Handler;
using com.organo.xchallenge.Services;
using Xamarin.Forms;
using com.organo.xchallenge.Controls;
using com.organo.xchallenge.Statics;

namespace com.organo.xchallenge.Pages.Blog
{
    public partial class BlogPage : BlogPageXaml
    {
        private readonly BlogViewModel _model;

        public BlogPage(RootPage root)
        {
            try
            {
                InitializeComponent();
                _model = new BlogViewModel() { Root = root };
                Init();
            }
            catch (Exception ex)
            {
                new ExceptionHandler(nameof(BlogPage), ClientService.GetExceptionDetail(ex));
            }
        }

        private async void Init()
        {
            await App.Configuration.InitialAsync(this);
            NavigationPage.SetHasNavigationBar(this, false);
            BindingContext = _model;
            _model.WebUri = await _model.GetLink();
            var webUri =
                $"?token={App.Configuration.UserToken}&theme=dark&languagecode={App.Configuration?.AppConfig.DefaultLanguage}&active=true&application={App.Configuration.GetApplication()}";

            contentView.Content = new HybridWebView()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Margin = new Thickness(0, -6, 0, 0),
                BackgroundColor = Palette._MainBackground,
                Uri = _model.WebUri + webUri
            };
        }

        protected override bool OnBackButtonPressed()
        {
            return DependencyService.Get<IBackButtonPress>().Redirect(_model.Root);
        }
    }

    public abstract class BlogPageXaml : ModelBoundContentPage<BlogViewModel>
    {
    }
}