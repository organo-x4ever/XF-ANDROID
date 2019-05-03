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
            NavigationPage.SetHasNavigationBar(this, true);
            _model = new NotificationSettingViewModel();
            BindingContext = _model;
            _model.EventSetupAction = async () =>
            {
                await Task.Delay(TimeSpan.FromMilliseconds(500));
                switchPromotional.Toggled += async (sender, e) => await _model.Update(NotifyType.PROMOTIONAL, e.Value);
                switchWeightSubmitReminder.Toggled += async (sender, e) =>
                    await _model.Update(NotifyType.WEIGHT_SUBMIT_REMINDER, e.Value);
                switchGeneralMessage.Toggled +=
                    async (sender, e) => await _model.Update(NotifyType.GENERAL_MESSAGE, e.Value);
                switchSpecialOffer.Toggled +=
                    async (sender, e) => await _model.Update(NotifyType.SPECIAL_OFFER, e.Value);
                switchVersionUpdate.Toggled +=
                    async (sender, e) => await _model.Update(NotifyType.VERSION_UPDATE, e.Value);
            };
        }
    }

    public abstract class NotificationSettingPageXaml : ModelBoundContentPage<NotificationSettingViewModel>
    {
    }
}