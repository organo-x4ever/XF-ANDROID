using System;

namespace com.organo.x4ever.Models.Authentication
{
    public sealed class UserInfo : IUserInfo
    {
        // Summary: Create user information for token cache lookup
        public UserInfo()
        {
        }

        // Summary: Create user information copied from another UserInfo object
        public UserInfo(UserInfo other)
        {
            ID = other.ID;
            UserLogin = other.UserLogin;
            UserFirstName = other.UserFirstName;
            UserLastName = other.UserLastName;
            UserEmail = other.UserEmail;
            UserType = other.UserType;
            UserRegistered = other.UserRegistered;
            UserStatus = other.UserStatus;
            ProfileImage = other.ProfileImage;
            LanguageCode = string.Empty;
            WeightVolumeType = string.Empty;
        }

        /// <summary>
        ///     User registration date
        /// </summary>
        public string UserRegisteredDisplay =>
            string.Format("{0:dddd, MMMM d, yyyy}", UserRegistered); // "Sunday, March 9, 2008"

        /// <summary>
        ///     Language Code
        /// </summary>
        public string LanguageCode { get; set; }

        /// <summary>
        ///     Weight Volume Type
        /// </summary>
        public string WeightVolumeType { get; set; }

        /// <summary>
        ///     User Unique ID
        /// </summary>
        public long ID { get; set; }

        /// <summary>
        ///     login name or username
        /// </summary>
        public string UserLogin { get; set; }

        /// <summary>
        ///     First Name
        /// </summary>
        public string UserFirstName { get; set; }

        /// <summary>
        ///     Last Name or Family Name
        /// </summary>
        public string UserLastName { get; set; }

        /// <summary>
        ///     Email Address (Unique)
        /// </summary>
        public string UserEmail { get; set; }

        /// <summary>
        ///     User Type
        /// </summary>
        public string UserType { get; set; }

        /// <summary>
        ///     User registration date
        /// </summary>
        public DateTime UserRegistered { get; set; }

        /// <summary>
        ///     Status (Active/Inactive)
        /// </summary>
        public string UserStatus { get; set; }

        /// <summary>
        ///     Profile Photo
        /// </summary>
        public string ProfileImage { get; set; }

        /// <summary>
        ///     Display Name
        /// </summary>
        public string DisplayName => string.Format("{0}", UserFirstName);

        /// <summary>
        ///     Full name (First and Last name)
        /// </summary>
        public string FullName => string.Format("{0} {1}", UserFirstName, UserLastName);

        /// <summary>
        /// User Application Key
        /// </summary>
        public string UserApplication { get; set; }
    }
}