using System;

namespace com.organo.x4ever.Models
{
    public sealed class ApplicationLanguage
    {
        public ApplicationLanguage()
        {
            this.ApplicationID = 0;
            this.CountryID = 0;
            this.CountryCode = string.Empty;
            this.CountryName = string.Empty;
            this.CountryFlag = string.Empty;
            this.LanguageID = 0;
            this.LanguageCode = string.Empty;
            this.LanguageName = string.Empty;
            this.LanguageNameEnglish = string.Empty;

            this.DisplayLanguageCode = string.Empty;
            this.DisplayCountryLanguage = string.Empty;
            this.IsSelected = false;
        }

        public Int32 ApplicationID { get; set; }
        public Int32 CountryID { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public string CountryFlag { get; set; }
        public Int32 LanguageID { get; set; }
        public string LanguageCode { get; set; }
        public string LanguageName { get; set; }
        public string LanguageNameEnglish { get; set; }
        public string DisplayLanguageCode { get; set; }
        public string DisplayCountryLanguage { get; set; }
        public bool IsSelected { get; set; }
    }

    public class ApplicationLanguageRequest
    {
        public ApplicationLanguageRequest()
        {
            this.LanguageCode = string.Empty;
            this.LanguageName = string.Empty;
        }

        public string LanguageCode { get; set; }
        public string LanguageName { get; set; }
    }

    public class UserWeightVolume
    {
        public UserWeightVolume()
        {
            this.WeightVolume = string.Empty;
        }

        public string WeightVolume { get; set; }
    }
}