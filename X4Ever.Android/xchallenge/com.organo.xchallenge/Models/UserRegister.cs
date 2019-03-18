using com.organo.xchallenge.Models.User;
using System;
using System.Collections.Generic;

namespace com.organo.xchallenge.Models
{
    public class UserRegister
    {
        public UserRegister()
        {
            UserKey = string.Empty;
            UserLogin = string.Empty;
            UserPassword = string.Empty;
            UserFirstName = string.Empty;
            UserLastName = string.Empty;
            UserEmail = string.Empty;
            UserType = string.Empty;
            LanguageCode = string.Empty;
            LanguageDetail = string.Empty;
            WeightVolumeType = string.Empty;
            UserRegistered = new DateTime();
            UserActivationKey = string.Empty;
            UserStatus = string.Empty;
            UserMetas = new List<Meta>();
            UserApplication = string.Empty;
        }

        public String UserKey { get; set; }
        public String UserLogin { get; set; }
        public String UserPassword { get; set; }
        public String UserFirstName { get; set; }
        public String UserLastName { get; set; }
        public string UserEmail { get; set; }
        public String UserType { get; set; }
        public string LanguageCode { get; set; }
        public string LanguageDetail { get; set; }
        public string WeightVolumeType { get; set; }
        public DateTime UserRegistered { get; set; }
        public string UserActivationKey { get; set; }
        public string UserStatus { get; set; }
        public List<Meta> UserMetas { get; set; }
        public string UserApplication { get; set; }
    }

    public class UserFirstUpdate
    {
        public UserFirstUpdate()
        {
            UserFirstName = string.Empty;
            UserLastName = string.Empty;
            UserEmail = string.Empty;
            UserType = string.Empty;
            UserMetas = new List<Meta>();
            UserTrackers = new List<Tracker>();
            UserSettings = new UserSetting();
        }

        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }
        public string UserEmail { get; set; }
        public string UserType { get; set; }
        public List<Meta> UserMetas { get; set; }
        public List<Tracker> UserTrackers { get; set; }
        public UserSetting UserSettings { get; set; }
    }

    public class UserStep1
    {
        public UserStep1()
        {
            UserFirstName = string.Empty;
            UserLastName = string.Empty;
            UserEmail = string.Empty;
            UserType = string.Empty;
        }

        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }
        public string UserEmail { get; set; }
        public string UserType { get; set; }
    }
}