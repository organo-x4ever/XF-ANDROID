using System;

namespace com.organo.xchallenge.Models.Notifications
{
    public class UserPushTokenModel
    {
        public Int64 ID { get; set; }
        public Int64 UserID { get; set; }
        public string DeviceToken { get; set; }
        public string DevicePlatform { get; set; }
        public string DeviceIdentity { get; set; }
        public string DeviceIdiom { get; set; }
        public string DeviceApplication { get; set; }
        public DateTime IssuedOn { get; set; }
    }

    public class UserPushTokenModelRegister
    {
        public Int64 ID { get; set; }
        public Int64 UserID { get; set; }
        public string OldDeviceToken { get; set; }
        public string DeviceToken { get; set; }
        public string DevicePlatform { get; set; }
        public string DeviceIdentity { get; set; }
        public string DeviceIdiom { get; set; }
        public string DeviceApplication { get; set; }
        public DateTime IssuedOn { get; set; }
    }
}