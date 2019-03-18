using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.organo.xchallenge.Globals;
using com.organo.xchallenge.Localization;
using com.organo.xchallenge.Statics;
using Xamarin.Forms;

namespace com.organo.xchallenge.Models.User
{
    public class TrackerPivot
    {
        public TrackerPivot()
        {
            UserId = 0;
            ShirtSize = string.Empty;
            CurrentWeight = string.Empty;
            CurrentWeightUI = string.Empty;
            WeightVolumeType = string.Empty;
            FrontImage = string.Empty;
            SideImage = string.Empty;
            AboutJourney = string.Empty;
            ModifyDate = DateTime.Now;
            RevisionNumber = string.Empty;
            IsImageAvailable = false;
        }

        public Int64 UserId { get; set; }
        public string ShirtSize { get; set; }
        public string CurrentWeight { get; set; }
        public string CurrentWeightUI { get; set; }
        public string WeightVolumeType { get; set; }
        public string FrontImage { get; set; }
        public string SideImage { get; set; }
        public string AboutJourney { get; set; }
        public DateTime ModifyDate { get; set; }
        public string RevisionNumber { get; set; }

        public string FrontImageWithUrl => this.FrontImage != null
            ? DependencyService.Get<IHelper>().GetFilePath(this.FrontImage, FileType.User)
            : "";

        public string SideImageWithUrl => this.SideImage != null
            ? DependencyService.Get<IHelper>().GetFilePath(this.SideImage, FileType.User)
            : "";

        public double WeightLost { get; set; }
        public string AboutYourJourney { get; set; }
        public Color BackgroundColor { get; set; }
        public string CurrentWeightDisplayText => string.Format(TextResources.YourWeight, CurrentWeightDisplay);
        public string CurrentWeightDisplay => CurrentWeight + App.Configuration.AppConfig.DefaultWeightVolume;
        public string WeightLostDisplayText => string.Format(TextResources.YouLost, WeightLostDisplay);
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
        public bool IsImageAvailable { get; set; }
    }
}