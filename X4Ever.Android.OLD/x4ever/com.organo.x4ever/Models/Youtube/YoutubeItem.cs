using System;
using System.Collections.Generic;

namespace com.organo.x4ever.Models.Youtube
{
    public class YoutubeItem
    {
        public string VideoId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ChannelTitle { get; set; }
        public string DefaultThumbnailUrl { get; set; }
        public string MediumThumbnailUrl { get; set; }
        public string HighThumbnailUrl { get; set; }
        public string StandardThumbnailUrl { get; set; }
        public string MaxResThumbnailUrl { get; set; }
        public DateTime PublishedAt { get; set; }
        public int? ViewCount { get; set; }
        public int? LikeCount { get; set; }
        public int? DislikeCount { get; set; }
        public int? FavoriteCount { get; set; }
        public int? CommentCount { get; set; }
        public bool ShowDescription { get; set; }
        public bool ShowView { get; set; }
        public bool ShowLike { get; set; }
        public bool ShowComment { get; set; }
        public bool ShowFavourite { get; set; }
        public bool ShowDislike { get; set; }
        public List<string> Tags { get; set; }
    }
}