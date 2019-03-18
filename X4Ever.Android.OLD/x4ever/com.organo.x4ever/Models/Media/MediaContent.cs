using com.organo.x4ever.Statics;
using System;
using Xamarin.Forms;

namespace com.organo.x4ever.Models.Media
{
    public class MediaContent
    {
        public MediaContent()
        {
            ID = 0;
            MediaCategoryID = 0;
            MediaTypeID = 0;
            MediaTitle = string.Empty;
            MediaUrl = string.Empty;
            SetsAndRepeats = string.Empty;
            TotalDuration = string.Empty;
            PreviewImageUrl = string.Empty;
            DisplaySequence = 0;
            CreateDate = new DateTime();
            MediaDescription = string.Empty;
            IsPlayingNow = false;
            MediaTitleColor = Palette._TitleTexts;
        }

        public int ID { get; set; }
        public Int16 MediaCategoryID { get; set; }
        public Int16 MediaTypeID { get; set; }
        public string MediaTitle { get; set; }
        public Color MediaTitleColor { get; set; }
        public string MediaUrl { get; set; }
        public string SetsAndRepeats { get; set; }
        public string TotalDuration { get; set; }
        public string PreviewImageUrl { get; set; }
        public Int16 DisplaySequence { get; set; }
        public DateTime CreateDate { get; set; }
        public string MediaDescription { get; set; }
        public bool IsPlayingNow { get; set; }
        public string WorkoutLevel { get; set; }
        public short WorkoutWeek { get; set; }
        public short WorkoutDay { get; set; }
        public bool Active { get; set; }
    }
}