using com.organo.x4ever.Localization;
using System;
using com.organo.x4ever.Globals;
using com.organo.x4ever.Statics;
using Xamarin.Forms;

namespace com.organo.x4ever.Models.User
{
    public class UserTracker
    {
        public string FrontImageWithUrl => this.FrontImage != null
            ? DependencyService.Get<IHelper>().GetFilePath(this.FrontImage, FileType.User)
            : "";

        public string SideImageWithUrl => this.SideImage != null
            ? DependencyService.Get<IHelper>().GetFilePath(this.SideImage, FileType.User)
            : "";

        public double CurrentWeight { get; set; }
        public double WeightLost { get; set; }
        public string ShirtSize { get; set; }
        public string FrontImage { get; set; }
        public string SideImage { get; set; }
        public string AboutYourJourney { get; set; }
        public string RevisionNumber { get; set; }
        public DateTime ModifyDate { get; set; }
        public Color BackgroundColor { get; set; }
        public string CurrentWeightDisplay => CurrentWeight + App.Configuration.AppConfig.DefaultWeightVolume;
        public string WeightLostDisplay => WeightLost + App.Configuration.AppConfig.DefaultWeightVolume;

        public string RevisionNumberDisplayShort =>
            RevisionNumber != null ? TextResources.RevisionShort + RevisionNumber : "";

        public string RevisionNumberDisplay => RevisionNumber != null ? TextResources.Revision + RevisionNumber : "";

        public string ModifyDateDisplay =>
            string.Format(TextResources.DateDisplayFormat, ModifyDate); // "Sunday, March 9, 2008"

        public string ModifyDateDisplayMonthDay =>
            string.Format(TextResources.DatetimeDisplayFormatMonthDay, ModifyDate); // "Sunday, March 9, 2008"

        public ImageSource FrontImageSource { get; set; }
        public ImageSource SideImageSource { get; set; }
        public float PictureHeight { get; set; }
        public float PictureWidth { get; set; }
    }
}