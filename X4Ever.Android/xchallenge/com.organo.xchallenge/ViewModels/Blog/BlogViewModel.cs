
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using com.organo.xchallenge.Services;
using com.organo.xchallenge.ViewModels.Base;
using Xamarin.Forms;

namespace com.organo.xchallenge.ViewModels.Blog
{
    public class BlogViewModel : BaseViewModel
    {
        public BlogViewModel(INavigation navigation = null) : base(navigation)
        {

        }

        public async Task<string> GetLink() => await DependencyService.Get<IConstantServices>().Blogs();

        private string _webUri;
        public const string WebUriPropertyName = "WebUri";

        public string WebUri
        {
            get => _webUri;
            set
            {
                var url = Regex.Replace(value, "\"", "");
                SetProperty(ref _webUri, url, WebUriPropertyName);
            }
        }
    }
}