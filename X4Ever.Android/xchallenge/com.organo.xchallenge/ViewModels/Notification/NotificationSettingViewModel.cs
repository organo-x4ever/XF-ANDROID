using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using com.organo.xchallenge.Localization;
using com.organo.xchallenge.Models.Notifications;
using com.organo.xchallenge.Services;
using com.organo.xchallenge.Statics;
using com.organo.xchallenge.ViewModels.Base;
using Xamarin.Forms;

namespace com.organo.xchallenge.ViewModels.Notification
{
    public class NotificationSettingViewModel : BaseViewModel
    {
        private readonly IUserNotificationServices _notificationServices;
        private short NotificationFailureCount = 0;

        public NotificationSettingViewModel(INavigation navigation = null) : base(navigation)
        {
            _notificationServices = DependencyService.Get<IUserNotificationServices>();

            WeightSubmitReminderText = TextResources.Notification_WeightSubmitReminder;
            PromotionalText = TextResources.Notification_Promotional;
            SpecialOfferText = TextResources.Notification_SpecialOffer;
            VersionUpdateText = TextResources.Notification_VersionUpdate;
            GeneralMessageText = TextResources.Notification_GeneralMessage;
            NotificationsText = TextResources.Notifications;
            GetNotificationStatus();
        }

        private async void GetNotificationStatus()
        {
            var notification = await _notificationServices.GetAsync();
            if (notification != null)
            {
                IsWeightSubmitReminder = notification.IsWeightSubmitReminder;
                IsGeneralMessage = notification.IsGeneralMessage;
                IsSpecialOffer = notification.IsSpecialOffer;
                IsPromotional = notification.IsPromotional;
                IsVersionUpdate = notification.IsVersionUpdate;
                CreateAllEventAction();
            }
            else
            {
                IsGeneralMessage = false;
                IsPromotional = false;
                IsSpecialOffer = false;
                IsVersionUpdate = false;
                IsWeightSubmitReminder = false;
            }
        }

        private void Set(NotifyType NotifyType, bool status)
        {
            switch (NotifyType)
            {
                case NotifyType.GENERAL_MESSAGE:
                    IsGeneralMessage = status;
                    break;
                case NotifyType.PROMOTIONAL:
                    IsPromotional = status;
                    break;
                case NotifyType.SPECIAL_OFFER:
                    IsSpecialOffer = status;
                    break;
                case NotifyType.VERSION_UPDATE:
                    IsVersionUpdate = status;
                    break;
                case NotifyType.WEIGHT_SUBMIT_REMINDER:
                    IsWeightSubmitReminder = status;
                    break;
            }
        }

        public async Task<bool> Update(NotifyType NotifyType, bool status)
        {
            if (NotificationFailureCount >= 3)
            {
                SetActivityResource(showError: true,
                    errorMessage:
                    "Sorry, we're have a problem with notification status update, please have patience concerned person has been informed");
                await DependencyService.Get<ILogServices>().WriteLog("Notification Status update problem",
                    "Notification Status update error in SettingViewModel.cs", false);
                GetNotificationStatus();
                return false;
            }

            Set(NotifyType, status);
            var response = await _notificationServices.Update(new UserNotificationSetting()
            {
                IsSpecialOffer = IsSpecialOffer,
                IsPromotional = IsPromotional,
                IsGeneralMessage = IsGeneralMessage,
                IsWeightSubmitReminder = IsWeightSubmitReminder,
                IsVersionUpdate = IsVersionUpdate
            });
            if (response != HttpConstants.SUCCESS)
            {
                Set(NotifyType, !status);
                NotificationFailureCount++;
                SetActivityResource(showError: true, errorMessage: response);
                return false;
            }

            NotificationFailureCount = 0;
            return true;
        }

        public Action CreateAllEventAction { get; set; }

        private string _weightSubmitReminderText;
        public const string WeightSubmitReminderTextPropertyName = "WeightSubmitReminderText";

        public string WeightSubmitReminderText
        {
            get => _weightSubmitReminderText;
            set => SetProperty(ref _weightSubmitReminderText, value, WeightSubmitReminderTextPropertyName);
        }

        private string _promotionalText;
        public const string PromotionalTextPropertyName = "PromotionalText";

        public string PromotionalText
        {
            get => _promotionalText;
            set => SetProperty(ref _promotionalText, value, PromotionalTextPropertyName);
        }

        private string _specialOfferText;
        public const string SpecialOfferTextPropertyName = "SpecialOfferText";

        public string SpecialOfferText
        {
            get => _specialOfferText;
            set => SetProperty(ref _specialOfferText, value, SpecialOfferTextPropertyName);
        }

        private string _versionUpdateText;
        public const string VersionUpdateTextPropertyName = "VersionUpdateText";

        public string VersionUpdateText
        {
            get => _versionUpdateText;
            set => SetProperty(ref _versionUpdateText, value, VersionUpdateTextPropertyName);
        }

        private string _generalMessageText;
        public const string GeneralMessageTextPropertyName = "GeneralMessageText ";

        public string GeneralMessageText
        {
            get => _generalMessageText;
            set => SetProperty(ref _generalMessageText, value, GeneralMessageTextPropertyName);
        }

        private string _notificationsText;
        public const string NotificationsTextPropertyName = "NotificationsText ";

        public string NotificationsText
        {
            get => _notificationsText;
            set => SetProperty(ref _notificationsText, value, NotificationsTextPropertyName);
        }

        private bool _isWeightSubmitReminder;
        public const string IsWeightSubmitReminderPropertyName = "IsWeightSubmitReminder";

        public bool IsWeightSubmitReminder
        {
            get => _isWeightSubmitReminder;
            set => SetProperty(ref _isWeightSubmitReminder, value, IsWeightSubmitReminderPropertyName,
                SwitchWeightSubmitReminderLabelStyleChange);
        }

        private bool _isPromotional;
        public const string IsPromotionalPropertyName = "IsPromotional";

        public bool IsPromotional
        {
            get => _isPromotional;
            set => SetProperty(ref _isPromotional, value, IsPromotionalPropertyName, SwitchPromotionalLabelStyleChange);
        }

        private bool _isSpecialOffer;
        public const string IsSpecialOfferPropertyName = "IsSpecialOffer";

        public bool IsSpecialOffer
        {
            get => _isSpecialOffer;
            set => SetProperty(ref _isSpecialOffer, value, IsSpecialOfferPropertyName,
                SwitchSpecialOfferLabelStyleChange);
        }

        private bool _isVersionUpdate;
        public const string IsVersionUpdatePropertyName = "IsVersionUpdate";

        public bool IsVersionUpdate
        {
            get => _isVersionUpdate;
            set => SetProperty(ref _isVersionUpdate, value, IsVersionUpdatePropertyName,
                SwitchVersionUpdateLabelStyleChange);
        }

        public bool _isGeneralMessage;
        public const string IsGeneralMessagePropertyName = "IsGeneralMessage";

        public bool IsGeneralMessage
        {
            get => _isGeneralMessage;
            set => SetProperty(ref _isGeneralMessage, value, IsGeneralMessagePropertyName,
                SwitchGeneralMessageLabelStyleChange);
        }

        private void SwitchSpecialOfferLabelStyleChange()
        {
            SwitchSpecialOfferLabelStyle = IsSpecialOffer
                ? (Style) App.CurrentApp.Resources["labelStyleTableViewItem"]
                : (Style) App.CurrentApp.Resources["labelStyleLink"];
        }

        private void SwitchVersionUpdateLabelStyleChange()
        {
            SwitchVersionUpdateLabelStyle = IsVersionUpdate
                ? (Style) App.CurrentApp.Resources["labelStyleTableViewItem"]
                : (Style) App.CurrentApp.Resources["labelStyleLink"];
        }

        private void SwitchWeightSubmitReminderLabelStyleChange()
        {
            SwitchWeightSubmitReminderLabelStyle = IsWeightSubmitReminder
                ? (Style) App.CurrentApp.Resources["labelStyleTableViewItem"]
                : (Style) App.CurrentApp.Resources["labelStyleLink"];
        }

        private void SwitchPromotionalLabelStyleChange()
        {
            SwitchPromotionalLabelStyle = IsPromotional
                ? (Style) App.CurrentApp.Resources["labelStyleTableViewItem"]
                : (Style) App.CurrentApp.Resources["labelStyleLink"];
        }

        private void SwitchGeneralMessageLabelStyleChange()
        {
            SwitchGeneralMessageLabelStyle = IsGeneralMessage
                ? (Style) App.CurrentApp.Resources["labelStyleTableViewItem"]
                : (Style) App.CurrentApp.Resources["labelStyleLink"];
        }

        private Style _switchSpecialOfferLabelStyle;
        public const string SwitchSpecialOfferLabelStylePropertyName = "SwitchSpecialOfferLabelStyle";

        public Style SwitchSpecialOfferLabelStyle
        {
            get => _switchSpecialOfferLabelStyle;
            set => SetProperty(ref _switchSpecialOfferLabelStyle, value, SwitchSpecialOfferLabelStylePropertyName);
        }

        private Style _switchGeneralMessageLabelStyle;
        public const string SwitchGeneralMessageLabelStylePropertyName = "SwitchGeneralMessageLabelStyle";

        public Style SwitchGeneralMessageLabelStyle
        {
            get => _switchGeneralMessageLabelStyle;
            set => SetProperty(ref _switchGeneralMessageLabelStyle, value, SwitchGeneralMessageLabelStylePropertyName);
        }

        private Style _switchVersionUpdateLabelStyle;
        public const string SwitchVersionUpdateLabelStylePropertyName = "SwitchVersionUpdateLabelStyle";

        public Style SwitchVersionUpdateLabelStyle
        {
            get => _switchVersionUpdateLabelStyle;
            set => SetProperty(ref _switchVersionUpdateLabelStyle, value, SwitchVersionUpdateLabelStylePropertyName);
        }

        private Style _switchPromotionalLabelStyle;
        public const string SwitchPromotionalLabelStylePropertyName = "SwitchPromotionalLabelStyle";

        public Style SwitchPromotionalLabelStyle
        {
            get => _switchPromotionalLabelStyle;
            set => SetProperty(ref _switchPromotionalLabelStyle, value, SwitchPromotionalLabelStylePropertyName);
        }

        private Style _switchWeightSubmitReminderLabelStyle;
        public const string SwitchWeightSubmitReminderLabelStylePropertyName = "SwitchWeightSubmitReminderLabelStyle";

        public Style SwitchWeightSubmitReminderLabelStyle
        {
            get => _switchWeightSubmitReminderLabelStyle;
            set => SetProperty(ref _switchWeightSubmitReminderLabelStyle, value,
                SwitchWeightSubmitReminderLabelStylePropertyName);
        }
    }

    public enum NotifyType
    {
        WEIGHT_SUBMIT_REMINDER = 0,
        PROMOTIONAL = 1,
        SPECIAL_OFFER = 2,
        VERSION_UPDATE = 3,
        GENERAL_MESSAGE = 4
    }
}