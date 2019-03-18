using System;
using Xamarin.Forms;

namespace com.organo.x4ever.Models
{
    public class Testimonial
    {
        public int ID { get; set; }
        public string PersonName { get; set; }
        public string PersonPhoto { get; set; }
        public ImageSource PersonPhotoSource { get; set; }
        public float PersonImageHeight { get; set; }
        public float PersonImageWidth { get; set; }
        public string VideoUrl { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public Int64 CreatedBy { get; set; }
        public decimal StarRating { get; set; }
        public Int16 DisplaySequence { get; set; }
        public bool Active { get; set; }
        public bool IsVideoExists => this.VideoUrl != null && this.VideoUrl.Trim().Length > 0 ? true : false;
        public bool IsPhoto => !this.IsVideoExists;
    }
}