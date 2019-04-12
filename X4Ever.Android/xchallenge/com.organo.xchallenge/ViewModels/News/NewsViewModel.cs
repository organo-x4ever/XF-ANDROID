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
        private readonly INewsService _newsService;

        public NewsViewModel(INavigation navigation = null) : base(navigation)
        {
            _newsService = DependencyService.Get<INewsService>();
        }

        public async Task<List<NewsModel>> GetAsync()
        {
            var newsList = await _newsService.GetByLanguage(App.Configuration.AppConfig.DefaultLanguage, true);
            return (from n in newsList
                select new NewsModel()
                {
                    Active = n.Active,
                    ApplicationId = n.ApplicationId,
                    Body = n.Body,
                    Header = n.Header,
                    ID = n.ID,
                    LanguageCode = n.LanguageCode,
                    ModifiedBy = n.ModifiedBy,
                    ModifyDate = n.ModifyDate,
                    NewsImage = n.NewsImage,
                    NewsImagePosition = n.NewsImagePosition,
                    NewsImageSource = DependencyService.Get<IHelper>().GetFileUri(n.NewsImage, FileType.None),
                    PostDate = n.PostDate,
                    PostedBy = n.PostedBy
                }).OrderByDescending(n => n.PostDate).ToList();
        }
    }
}