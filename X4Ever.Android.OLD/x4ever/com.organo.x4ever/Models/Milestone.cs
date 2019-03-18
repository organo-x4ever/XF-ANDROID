using System;

namespace com.organo.x4ever.Models
{
    public class Milestone
    {
        public int ID { get; set; }
        public string MilestoneTitle { get; set; }
        public string MilestoneSubTitle { get; set; }
        public int TargetValue { get; set; }
        public string AchievedMessage { get; set; }
        public string LanguageCode { get; set; }
        public bool Active { get; set; }
    }
}