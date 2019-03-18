using System;

namespace com.organo.x4ever.Models.User
{
    public class UserSetting
    {
        public int ID { get; set; }

        public long UserID { get; set; }

        public string LanguageCode { get; set; }

        public string LanguageDetail { get; set; }

        public string WeightVolumeType { get; set; }

        public DateTime ModifyDate { get; set; }
    }
}