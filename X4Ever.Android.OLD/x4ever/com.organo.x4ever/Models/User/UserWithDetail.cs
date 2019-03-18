using System;
using System.Collections.Generic;

namespace com.organo.x4ever.Models.User
{
    public class UserWithDetail
    {
        public UserWithDetail()
        {
            ID = 0;

            UserKey = string.Empty;

            UserLogin = string.Empty;

            UserPassword = string.Empty;

            UserFirstName = string.Empty;

            UserLastName = string.Empty;

            UserEmail = string.Empty;

            UserType = string.Empty;
            IsWeightSubmissionRequired = false;

            UserRegistered = DateTime.Now;

            UserActivationKey = string.Empty;

            UserStatus = string.Empty;

            ProfileImage = string.Empty;

            UserMetas = new List<Meta>();

            UserTrackers = new List<Tracker>();

            UserDetailMeta = new UserMeta();

            UserDetailTrackers = new List<UserTracker>();

            Achievement = new MilestonePercentage();
        }

        public Int64 ID { get; set; }

        public string UserKey { get; set; }

        public string UserLogin { get; set; }

        public string UserPassword { get; set; }

        public string UserFirstName { get; set; }

        public string UserLastName { get; set; }

        public string UserEmail { get; set; }

        public string UserType { get; set; }
        public bool IsWeightSubmissionRequired { get; set; }

        public DateTime UserRegistered { get; set; }

        /// <summary>
        /// User registration date
        /// </summary>
        public string UserRegisteredDisplay =>
            String.Format("{0:dddd, MMMM d, yyyy}", UserRegistered); // "Sunday, March 9, 2008"

        public string UserActivationKey { get; set; }

        public string UserStatus { get; set; }

        public string ProfileImage { get; set; }

        public string DisplayName => UserFirstName;

        public MilestonePercentage Achievement { get; set; }

        public ICollection<Meta> UserMetas { get; set; }

        public ICollection<Tracker> UserTrackers { get; set; }

        public UserMeta UserDetailMeta { get; set; }

        public List<UserTracker> UserDetailTrackers { get; set; }
    }
}