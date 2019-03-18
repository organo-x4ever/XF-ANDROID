using System;

namespace com.organo.x4ever.Models
{
    public sealed class MilestonePercentage
    {
        public Int16 ID { get; set; }

        public string LanguageCode { get; set; }

        public String MilestoneTitle { get; set; }

        public String MilestoneSubTitle { get; set; }

        public Int16 TargetValue { get; set; }

        public Int16 TargetPercentValue { get; set; }

        public bool IsPercent { get; set; }

        public String AchievedMessage { get; set; }

        public String AchievementIcon { get; set; }

        public String AchievementGiftImage { get; set; }

        public bool Active { get; set; }
    }
}