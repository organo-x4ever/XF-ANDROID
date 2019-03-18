using System;
using Xamarin.Forms;

namespace com.organo.xchallenge.Models.News
{
    public class NewsModel
    {
        public NewsModel()
        {
            ID = 0;
            Header = string.Empty;
            Body = string.Empty;
            PostDate = new DateTime();

            PostedBy = string.Empty;
            NewsImage = string.Empty;
            NewsImagePosition = string.Empty;

            Active = false;
            ModifyDate = new DateTime();
            ModifiedBy = string.Empty;

            LanguageCode = string.Empty;
            ApplicationId = 0;
        }

        public Int16 ID { get; set; }
        public string Header { get; set; }
        public string Body { get; set; }
        public DateTime PostDate { get; set; }
        public string PostedBy { get; set; }
        public string NewsImage { get; set; }
        public ImageSource NewsImageSource { get; set; }
        public string NewsImagePosition { get; set; }
        public bool Active { get; set; }
        public DateTime ModifyDate { get; set; }
        public string ModifiedBy { get; set; }

        public bool IsTop => (NewsImage != null && NewsImage.Trim().Length > 0
            ? (NewsImagePosition != null && NewsImagePosition.Trim().Length > 0 && NewsImagePosition.Contains("top")
                ? true
                : false)
            : false);

        public bool IsBottom => (NewsImage != null && NewsImage.Trim().Length > 0
            ? (NewsImagePosition != null && NewsImagePosition.Trim().Length > 0 && NewsImagePosition.Contains("bottom")
                ? true
                : false)
            : false);

        public string LanguageCode { get; set; }
        public int ApplicationId { get; set; }
    }
}