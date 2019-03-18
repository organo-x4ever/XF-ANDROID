using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.organo.xchallenge.Models.User
{
    public class UserPivot
    {
        public UserPivot()
        {
            ID = 0;
            UserFirstName = string.Empty;
            UserLastName = string.Empty;
            UserEmail = string.Empty;
            IsWeightSubmissionRequired = false;
            UserRegistered = new DateTime(1900, 1, 1);
            TargetDate = string.Empty;
            IsTrackerEditAllowed = false;
            IsTrackerViewAllowed = false;
            IsDownloadAllowed = false;
            Achievement = new MilestonePercentage();
            MetaPivot = new MetaPivot();
            TrackerPivot = new List<TrackerPivot>();
        }

        public Int64 ID { get; set; }

        public string UserFirstName { get; set; }

        public string UserLastName { get; set; }

        public string UserEmail { get; set; }

        public bool IsWeightSubmissionRequired { get; set; }

        public DateTime UserRegistered { get; set; }
        public string TargetDate { get; set; }
        public bool IsTrackerEditAllowed { get; set; }
        public bool IsTrackerViewAllowed { get; set; }
        public bool IsDownloadAllowed { get; set; }

        /// <summary>
        /// User registration date
        /// </summary>
        public string UserRegisteredDisplay =>
            String.Format("{0:dddd, MMMM d, yyyy}", UserRegistered); // "Sunday, March 9, 2008"

        public string DisplayName => UserFirstName;

        public MilestonePercentage Achievement { get; set; }

        public MetaPivot MetaPivot { get; set; }

        public IEnumerable<TrackerPivot> TrackerPivot { get; set; }
    }
}