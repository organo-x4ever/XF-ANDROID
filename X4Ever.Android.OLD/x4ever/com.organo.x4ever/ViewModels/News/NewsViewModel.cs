using com.organo.x4ever.Models.News;
using com.organo.x4ever.Pages;
using com.organo.x4ever.Services;
using com.organo.x4ever.ViewModels.Base;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using com.organo.x4ever.Globals;
using com.organo.x4ever.Statics;
using Xamarin.Forms;

namespace com.organo.x4ever.ViewModels.News
{
    public class NewsViewModel : BaseViewModel
    {
        private INewsService _newsService;

        public NewsViewModel(INavigation navigation = null) : base(navigation)
        {
            NewsModels = new List<NewsModel>();
            _newsService = DependencyService.Get<INewsService>();
        }

        public async Task GetAsync()
        {
            var newsList = await _newsService.GetByLanguage(App.Configuration.AppConfig.DefaultLanguage, true);
            foreach (var news in newsList)
            {
                if (news.NewsImage != null && news.NewsImage.Trim().Length > 0)
                    news.NewsImageSource = DependencyService.Get<IHelper>().GetFileUri(news.NewsImage, FileType.None);
            }

            NewsModels = newsList.OrderByDescending(n => n.PostDate).ToList();
        }

        private List<NewsModel> _newsModels;
        public const string NewsModelsPropertyName = "NewsModels";

        public List<NewsModel> NewsModels
        {
            get { return _newsModels; }
            set { SetProperty(ref _newsModels, value, NewsModelsPropertyName); }
        }

        private RootPage root;
        public const string RootPropertyName = "Root";

        public RootPage Root
        {
            get { return root; }
            set { SetProperty(ref root, value, RootPropertyName); }
        }

        private ICommand _showSideMenuCommand;

        public ICommand ShowSideMenuCommand
        {
            get
            {
                return _showSideMenuCommand ?? (_showSideMenuCommand = new Command((obj) =>
                {
                    Root.IsPresented = Root.IsPresented == false;
                }));
            }
        }
    }
}