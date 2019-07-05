using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.organo.xchallenge.Models.Notifications
{
    public class UserNotificationSettingViewModel
    {
        public UserNotificationSettingViewModel()
        {
            IsGeneralMessage = false;
            Intimation = false;
            IsPromotional = false;
            IsSpecialOffer = false;
            IsVersionUpdate = false;
            IsWeightSubmitReminder = false;

            IsVisibleGeneralMessage = false;
            IsVisibleIntimation = false;
            IsVisiblePromotional = false;
            IsVisibleSpecialOffer = false;
            IsVisibleVersionUpdate = false;
            IsVisibleWeightSubmitReminder = false;
        }

        public bool IsVisibleGeneralMessage { get; set; }
        public bool IsGeneralMessage { get; set; }
        public bool IsVisibleIntimation { get; set; }
        public bool Intimation { get; set; }
        public bool IsPromotional { get; set; }
        public bool IsVisiblePromotional { get; set; }
        public bool IsSpecialOffer { get; set; }
        public bool IsVisibleSpecialOffer { get; set; }
        public bool IsVersionUpdate { get; set; }
        public bool IsVisibleVersionUpdate { get; set; }
        public bool IsWeightSubmitReminder { get; set; }
        public bool IsVisibleWeightSubmitReminder { get; set; }

    }
}