using com.organo.x4ever.Services;
using com.organo.x4ever.ViewModels.Base;
using Xamarin.Forms;

namespace com.organo.x4ever.ViewModels.Splash
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