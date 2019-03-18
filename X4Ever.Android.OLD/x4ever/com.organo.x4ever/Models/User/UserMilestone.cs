using System;

namespace com.organo.x4ever.Models.User
{
    public class UserMilestone
    {
        public int ID { get; set; }
        public Int64 UserID { get; set; }
        public int MilestoneID { get; set; }
        public DateTime AchieveDate { get; set; }

        public Int16 MilestonePercentageId { get; set; }
        public bool IsPercentage { get; set; }
    }
}