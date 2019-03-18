using com.organo.xchallenge.Services;
using com.organo.xchallenge.ViewModels.Base;
using Xamarin.Forms;

namespace com.organo.xchallenge.ViewModels.Splash
{
    public class SplashViewModel : BaseViewModel
    {
        private readonly IConfigFetcher _ConfigFetcher;

        public SplashViewModel(INavigation navigation = null) : base(navigation)
        {
            _ConfigFetcher = DependencyService.Get<IConfigFetcher>();
        }

        private bool _IsPresentingLoginUI;

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