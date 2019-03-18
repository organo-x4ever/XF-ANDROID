using System;
using System.Collections.Generic;

namespace com.organo.x4ever.Models.Youtube
{
    public class YoutubeConfiguration
    {
        public short ID { get; set; }
        public int ApplicationID { get; set; }
        public string ApplicationKey { get; set; }
        public string UserApiKey { get; set; }
        public string VideoChannelApiUrl { get; set; }
        public string VideoPlaylistApiUrl { get; set; }
        public string VideoDetailApiUrl { get; set; }
        public string VideoWatchApiUrl { get; set; }
        public bool ShowDescription { get; set; }
        public bool ShowView { get; set; }
        public bool ShowLike { get; set; }
        public bool ShowComment { get; set; }
        public bool ShowFavourite { get; set; }
        public bool ShowDislike { get; set; }
        public DateTime ModifyDate { get; set; }
        public ICollection<YoutubeVideoCollection> YoutubeVideoCollection { get; set; }
    }
}