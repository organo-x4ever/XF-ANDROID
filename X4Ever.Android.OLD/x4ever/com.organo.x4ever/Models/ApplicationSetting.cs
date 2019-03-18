using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.organo.x4ever.Models
{
    public class ApplicationSetting
    {
        public short ID { get; set; }
        public int ApplicationID { get; set; }
        public string ApplicationKey { get; set; }
        public string LanguageCode { get; set; }
        public string CommunityFacebookUrl { get; set; }
        public string CommunityInstagramUrl { get; set; }
        public string OGXFileUrl { get; set; }
        public DateTime ModifyDate { get; set; }
    }
}