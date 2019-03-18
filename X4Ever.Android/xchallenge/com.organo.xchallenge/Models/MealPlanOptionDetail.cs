using System;
using System.Collections.Generic;

namespace com.organo.xchallenge.Models
{
    public class MealPlanOptionDetail
    {
        public Int16 ID { get; set; }
        public Int16 MealPlanID { get; set; }
        public string LanguageCode { get; set; }
        public string MealOptionTitle { get; set; }
        public string MealOptionPhoto { get; set; }
        public string MealOptionSubtitle { get; set; }
        public string MealOptionDesc { get; set; }
        public Int16 DisplaySequence { get; set; }
        public bool Active { get; set; }
        public DateTime ModifyDate { get; set; }
        public string ModifiedBy { get; set; }
        public List<MealPlanOptionGridDetail> MealPlanOptionGridDetails { get; set; }
        public List<MealPlanOptionListDetail> MealPlanOptionListDetails { get; set; }
    }
}