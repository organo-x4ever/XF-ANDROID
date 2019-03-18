using System;

namespace com.organo.xchallenge.Models
{
    public sealed class WeightVolume
    {
        public WeightVolume()
        {
            this.ID = 0;
            this.ApplicationID = 0;
            this.VolumeCode = string.Empty;
            this.VolumeName = string.Empty;
        }

        public Int32 ID { get; set; }
        public int ApplicationID { get; set; }
        public string VolumeCode { get; set; }
        public string VolumeName { get; set; }
        public string DisplayVolume => this.VolumeName + " (" + this.VolumeCode + ")";
    }
}