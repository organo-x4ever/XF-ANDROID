using System.Collections.Generic;

namespace com.organo.x4ever.Models.Youtube
{
    public class YoutubeVideo
    {
        public string VideoCollectionType { get; set; }
        public List<YoutubeItem> YoutubeItems { get; set; }
    }
}