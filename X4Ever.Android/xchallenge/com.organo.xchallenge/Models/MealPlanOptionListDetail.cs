using System;

namespace com.organo.xchallenge.Models
{
    public class MealPlanOptionListDetail
    {
        public Int16 ID { get; set; }
        public Int16 MealPlanOptionID { get; set; }
        public string LanguageCode { get; set; }
        public string MealOptionDetail { get; set; }
        public string MealOptionDetailPhoto { get; set; }
        public bool IsPhotoAvailable => MealOptionDetailPhoto != null;
        public string MealOptionDesc { get; set; }
        public Int16 DisplaySequence { get; set; }
        public bool Active { get; set; }
        public DateTime ModifyDate { get; set; }
        public string ModifiedBy { get; set; }
    }
}