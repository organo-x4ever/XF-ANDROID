using System;

namespace com.organo.xchallenge.Models.Notifications
{
    public class UserPushTokenModel
    {
        public long ID { get; set; }
        public long UserID { get; set; }
        public string DeviceToken { get; set; }
        public string DevicePlatform { get; set; }
        public string DeviceIdentity { get; set; }
        public string DeviceIdiom { get; set; }
        public string DeviceApplication { get; set; }
        public DateTime IssuedOn { get; set; }
        public string UserKey { get; set; }
    }

    public class UserPushTokenModelRegister
    {
        public long ID { get; set; }
        public long UserID { get; set; }
        public string OldDeviceToken { get; set; }
        public string DeviceToken { get; set; }
        public string DevicePlatform { get; set; }
        public string DeviceIdentity { get; set; }
        public string DeviceIdiom { get; set; }
        public string DeviceApplication { get; set; }
        public DateTime IssuedOn { get; set; }
    }
}