using com.organo.xchallenge.Models.News;
using com.organo.xchallenge.Pages;
using com.organo.xchallenge.Services;
using com.organo.xchallenge.ViewModels.Base;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using com.organo.xchallenge.Globals;
using com.organo.xchallenge.Statics;
using Xamarin.Forms;

namespace com.organo.xchallenge.ViewModels.News
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
            NewsModels = (await _newsService.GetByLanguage(App.Configuration.AppConfig.DefaultLanguage, true))
                .OrderByDescending(n => n.PostDate).ToList();
            foreach (var news in NewsModels)
            {
                if (news.NewsImage != null && news.NewsImage.Trim().Length > 0)
                    news.NewsImageSource = DependencyService.Get<IHelper>().GetFileUri(news.NewsImage, FileType.None);
            }
        }

        private List<NewsModel> _newsModels;
        public const string NewsModelsPropertyName = "NewsModels";

        public List<NewsModel> NewsModels
        {
            get { return _newsModels; }
            set { SetProperty(ref _newsModels, value, NewsModelsPropertyName); }
        }
    }
}