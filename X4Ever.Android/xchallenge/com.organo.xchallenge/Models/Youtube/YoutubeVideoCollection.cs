using System;

namespace com.organo.xchallenge.Models.Youtube
{
    public class YoutubeVideoCollection
    {
        public short ID { get; set; }
        public short YoutubeConfigurationID { get; set; }
        public string VideoCollectionApiKey { get; set; }
        public string VideoCollectionType { get; set; }
        public bool Active { get; set; }
        public DateTime ModifyDate { get; set; }
    }
}