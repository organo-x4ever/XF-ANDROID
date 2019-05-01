using com.organo.xchallenge.Services;
using com.organo.xchallenge.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace com.organo.xchallenge.ViewModels.Splash
{
    public class SplashLiveViewModel : BaseViewModel
    {
        readonly IConfigFetcher _ConfigFetcher;
        public SplashLiveViewModel(INavigation navigation = null) : base(navigation)
        {
            _ConfigFetcher = DependencyService.Get<IConfigFetcher>();
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
    }
}