using System;

namespace com.organo.xchallenge.Models
{
    public class MealPlanOptionGridDetail
    {
        public Int16 ID { get; set; }
        public Int16 MealPlanOptionID { get; set; }
        public string LanguageCode { get; set; }
        public string MealOptionVolume { get; set; }
        public string MealOptionVolumeType { get; set; }
        public string MealOptionShakeTitle { get; set; }
        public string MealOptionDetailPhoto { get; set; }
        public bool IsPhotoAvailable => this.MealOptionDetailPhoto != null;
        public Int16 DisplaySequence { get; set; }
        public bool Active { get; set; }
        public DateTime ModifyDate { get; set; }
        public string ModifiedBy { get; set; }
    }
}