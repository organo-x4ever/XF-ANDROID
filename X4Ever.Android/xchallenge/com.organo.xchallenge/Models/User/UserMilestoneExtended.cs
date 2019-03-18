using System.Collections.Generic;

namespace com.organo.xchallenge.Models.User
{
    public class UserMilestoneExtended
    {
        public IEnumerable<UserMilestone> UserMilestones { get; set; }

        public IEnumerable<Milestone> Milestones { get; set; }

        public IEnumerable<MilestonePercentage> MilestonePercentages { get; set; }
    }
}