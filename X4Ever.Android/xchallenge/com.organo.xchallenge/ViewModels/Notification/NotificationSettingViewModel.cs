using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.organo.xchallenge.Localization;
using com.organo.xchallenge.ViewModels.Base;
using Xamarin.Forms;

namespace com.organo.xchallenge.ViewModels.Notification
{
    public class NotificationSettingViewModel : BaseViewModel
    {
        public NotificationSettingViewModel(INavigation navigation = null) : base(navigation)
        {
            IsGeneralMessage = false;
            IsPromotional = false;
            IsSpecialOffer = false;
            IsVersionUpdate = false;
            IsWeightSubmitReminder = false;

            WeightSubmitReminderText = TextResources.Notification_WeightSubmitReminder;
            PromotionalText = TextResources.Notification_Promotional;
            SpecialOfferText = TextResources.Notification_SpecialOffer;
            VersionUpdateText = TextResources.Notification_VersionUpdate;
            GeneralMessageText = TextResources.Notification_GeneralMessage;
            NotificationsText = TextResources.Notifications;
        }

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
            set => SetProperty(ref _isWeightSubmitReminder, value, IsWeightSubmitReminderPropertyName);
        }

        private bool _isPromotional;
        public const string IsPromotionalPropertyName = "IsPromotional";

        public bool IsPromotional
        {
            get => _isPromotional;
            set => SetProperty(ref _isPromotional, value, IsPromotionalPropertyName);
        }


        private bool _isSpecialOffer;
        public const string IsSpecialOfferPropertyName = "IsSpecialOffer";

        public bool IsSpecialOffer
        {
            get => IsSpecialOffer;
            set => SetProperty(ref _isSpecialOffer, value, IsSpecialOfferPropertyName);
        }


        private bool _isVersionUpdate;
        public const string IsVersionUpdatePropertyName = "IsVersionUpdate";

        public bool IsVersionUpdate
        {
            get => _isVersionUpdate;
            set => SetProperty(ref _isVersionUpdate, value, IsVersionUpdatePropertyName);
        }


        public bool _isGeneralMessage;
        public const string IsGeneralMessagePropertyName = "IsGeneralMessage";

        public bool IsGeneralMessage
        {
            get => _isGeneralMessage;
            set => SetProperty(ref _isGeneralMessage, value, IsGeneralMessagePropertyName);
        }
    }
}