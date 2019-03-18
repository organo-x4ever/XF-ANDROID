using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using com.organo.x4ever.Localization;
using com.organo.x4ever.ViewModels.Base;
using Xamarin.Forms;

namespace com.organo.x4ever.ViewModels.ErrorPages
{
    public class InternetConnectionViewModel : BaseViewModel
    {
        public InternetConnectionViewModel(INavigation navigation = null) : base(navigation)
        {
            var seconds = TimeSpan.FromSeconds(2);
            Device.StartTimer(seconds, () =>
            {
                if (IsConnected())
                    App.GoToAccountPage();
                return !App.Configuration.IsConnected;
            });
        }

        private bool IsConnected()
        {
            App.Configuration.GetConnectionInfo();
            IsVisible = App.Configuration.IsConnected;
            return IsVisible;
        }

        private bool _isVisible;
        public const string IsVisiblePropertyName = "IsVisible";

        public bool IsVisible
        {
            get { return _isVisible; }
            set { SetProperty(ref _isVisible, value, IsVisiblePropertyName); }
        }

        private ICommand _loginCommand;

        public ICommand LoginCommand
        {
            get
            {
                return _loginCommand ?? (_loginCommand = new Command((obj) =>
                {
                    SetActivityResource(false, true);
                    IsConnected();
                    if (App.Configuration.IsConnected)
                        App.GoToAccountPage();
                    else
                        SetActivityResource();
                }));
            }
        }

        private ICommand _connectionCommand;

        public ICommand ConnectionCommand
        {
            get
            {
                return _connectionCommand ?? (_connectionCommand = new Command((obj) =>
                {
                    IsConnected();
                }));
            }
        }
    }
}
