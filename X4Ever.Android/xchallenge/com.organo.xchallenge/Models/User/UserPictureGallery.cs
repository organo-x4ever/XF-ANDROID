using com.organo.xchallenge.Localization;
using System;
using System.Collections.Generic;

namespace com.organo.xchallenge.Models.User
{
    public class UserPicture
    {
        public string FrontImage { get; set; }
        public string SideImage { get; set; }
        public string FrontImageWithUrl => this.FrontImage != null ? App.Configuration.AppConfig.BaseUrl + "" + this.FrontImage : "";
        public string SideImageWithUrl => this.SideImage != null ? App.Configuration.AppConfig.BaseUrl + "" + this.SideImage : "";
    }

    public class UserPictureGallery
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public DateTime ModifyDate => new DateTime(Year, Month, Day);
        public string ModifyDateDisplay => String.Format(TextResources.DateDisplayFormat, this.ModifyDate);  // "Sunday, March 9, 2008"
        public List<UserPicture> UserPictures { get; set; }
    }
}