using System;
using System.Collections.Generic;

namespace com.organo.xchallenge.Models
{
    public class MealPlanDetail
    {
        public Int16 ID { get; set; }
        public string LanguageCode { get; set; }
        public string MealTitle { get; set; }
        public string MealTitleCompare => this.MealTitle.Replace(" ", "").Replace("-", "").Replace("_", "");
        public string MealPlanPhoto { get; set; }
        public Int16 DisplaySequence { get; set; }
        public bool Active { get; set; }
        public DateTime ModifyDate { get; set; }
        public string ModifiedBy { get; set; }
        public string ViewType { get; set; }
        public List<MealPlanOptionDetail> MealPlanOptionDetails { get; set; }
    }
}