using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.organo.xchallenge.Pages.Base;
using com.organo.xchallenge.ViewModels.Notification;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace com.organo.xchallenge.Pages.Notification
{
    public partial class NotificationSettingPage : NotificationSettingPageXaml
    {
        private NotificationSettingViewModel _model;

        public NotificationSettingPage()
        {
            try
            {
                InitializeComponent();
                Init();
            }
            catch (Exception ex)
            {
                _ = ex;
            }
        }

        private async void Init()
        {
            await App.Configuration.InitialAsync(this);
            //NavigationPage.SetHasNavigationBar(this, false);
            _model = new NotificationSettingViewModel();
            BindingContext = _model;
        }
    }

    public abstract class NotificationSettingPageXaml : ModelBoundContentPage<NotificationSettingViewModel>
    {
    }
}