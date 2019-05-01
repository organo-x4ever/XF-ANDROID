using com.organo.xchallenge.Services;
//using com.organo.xchallenge.ViewModels.About;
using com.organo.xchallenge.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace com.organo.xchallenge.ViewModels.Splash
{
    public class SplashListViewModel : BaseViewModel
    {
        readonly IConfigFetcher _ConfigFetcher;
        public List<SplashItemViewModel> Items { get; private set; }

        public string Overview { get; private set; }

        public string ListHeading { get; private set; }
        public SplashListViewModel(INavigation navigation = null) : base(navigation)
        {
            _ConfigFetcher = DependencyService.Get<IConfigFetcher>();

            Items = new List<SplashItemViewModel>()
            {
                new SplashItemViewModel()
                {
                    Title = "HOW IT WORKS",
                    Uri = "https://www.x4ever.club/"
                },

                new SplashItemViewModel()
                {
                    Title = "SUCCESS STORIES",
                    Uri = "http://blog.organogold.com/category/x4ever/"
                },

                new SplashItemViewModel()
                {
                    Title = "REWARDS",
                    Uri = "https://www.x4ever.club/rewards/"
                },

                new SplashItemViewModel()
                {
                    Title = "BROCHURE",
                    Uri = "https://www.x4ever.club/wp-content/uploads/2017/10/X_challenge_brochure_v3.pdf"
                },

                new SplashItemViewModel()
                {
                    Title = "BUY PRODUCT",
                    Uri = "http://shopog.com/en-us/productcategory.aspx?lnmt=1&pt=0&catid=23"
                },

                new SplashItemViewModel()
                {
                    Title = "COMMUNITY",
                    Uri = "https://www.facebook.com/X4ever.club/"
                },
            };

            Overview =
                "Xamarin CRM is a demo app whose imagined purpose is to serve the mobile workforce of a " +
            "fictitious company that sells 3D printer hardware and supplies. The app empowers salespeople " +
            "to track their sales performance, manage leads, view their contacts, manage orders, and " +
            "browse a product catalog.";

            ListHeading =
                "The app is built with Xamarin Platform and Xamarin.Forms, and takes advantage of " +
                "several other supporting technologies:";

        }

        bool _IsPresentingLoginUI;

        public bool IsPresentingLoginUI
        {
            get { return _IsPresentingLoginUI; }
            set
            {
                _IsPresentingLoginUI = value;
                OnPropertyChanged("IsPresentingLoginUI");
            }
        }

        string _Username;

        public string Username
        {
            get { return _Username; }
            set
            {
                _Username = value;
                OnPropertyChanged("Username");
            }
        }

        string _Password;

        public string Password
        {
            get { return _Password; }
            set
            {
                _Password = value;
                OnPropertyChanged("Password");
            }
        }

        public async Task LoadDemoCredentials()
        {
            //Username = await _ConfigFetcher.GetAsync("azureActiveDirectoryUsername", true);
            //Password = await _ConfigFetcher.GetAsync("azureActiveDirectoryPassword", true);
        }
    }
}