using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using com.organo.x4ever.Extensions;
using com.organo.x4ever.Models.Youtube;
using com.organo.x4ever.Pages;
using com.organo.x4ever.Services;
using com.organo.x4ever.Statics;
using com.organo.x4ever.ViewModels.Base;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xamarin.Forms;

namespace com.organo.x4ever.ViewModels.YouTube
{
    public class YoutubeViewModel : BaseViewModel
    {
        private static readonly IYoutubeDataServices _youtubeDataServices = DependencyService.Get<IYoutubeDataServices>();

        public YoutubeViewModel(INavigation navigation = null) : base(navigation)
        {
            PlaylistCollectionType = string.Empty;
            ChannelCollectionType = string.Empty;
            CurrentCollectionType = string.Empty;
            PlaylistExists = ChannelExists = false;
            YoutubeConfiguration = new YoutubeConfiguration();
            YoutubeVideos = new List<YoutubeVideo>();
            YoutubeItems = new List<YoutubeItem>();
            PageLoad();
        }

        public async Task SwitchCollectionType()
        {
            await Task.Run(() => { SetActivityResource(false, true); });
            await Task.Run(() =>
            {
                CurrentCollectionType = CurrentCollectionType == YoutubeVideoCollectionType.Playlist
                    ? YoutubeVideoCollectionType.Channel
                    : YoutubeVideoCollectionType.Playlist;

                if (CurrentCollectionType == YoutubeVideoCollectionType.Playlist)
                {
                    PlaylistStyle = SelectedStyle;
                    ChannelStyle = DefaultStyle;
                }
                else
                {
                    PlaylistStyle = DefaultStyle;
                    ChannelStyle = SelectedStyle;
                }

                YoutubeItems = YoutubeVideos?.Where(v => v.VideoCollectionType == CurrentCollectionType)
                    .FirstOrDefault()?.YoutubeItems;
            });
            SetActivityResource();
        }

        private async void PageLoad()
        {
            await Task.Run(() => { SetActivityResource(false, true); });
            YoutubeConfiguration = new YoutubeConfiguration();
            YoutubeConfiguration = await _youtubeDataServices.GetAsync();

            if (YoutubeConfiguration == null)
            {
                SetActivityResource();
                return;
            }

            CurrentCollectionType = YoutubeVideoCollectionType.Channel;
            PlaylistCollectionType = YoutubeVideoCollectionType.Playlist.ToCapital();
            ChannelCollectionType = YoutubeVideoCollectionType.Channel.ToCapital();

            foreach (var videoCollection in YoutubeConfiguration.YoutubeVideoCollection)
            {
                var videoCollectionIdApiUrl = "";
                if (YoutubeVideoCollectionType.Channel == videoCollection.VideoCollectionType)
                {
                    ChannelExists = !string.IsNullOrEmpty(videoCollection.VideoCollectionApiKey);
                    videoCollectionIdApiUrl = string.Format(YoutubeConfiguration.VideoChannelApiUrl,
                        videoCollection.VideoCollectionApiKey, YoutubeConfiguration.UserApiKey);
                }
                else if (YoutubeVideoCollectionType.Playlist == videoCollection.VideoCollectionType)
                {
                    PlaylistExists = !string.IsNullOrEmpty(videoCollection.VideoCollectionApiKey);
                    videoCollectionIdApiUrl = string.Format(YoutubeConfiguration.VideoPlaylistApiUrl,
                        videoCollection.VideoCollectionApiKey, YoutubeConfiguration.UserApiKey);
                }

                var stringList =
                    await GetVideoIdStringAsync(videoCollectionIdApiUrl, videoCollection.VideoCollectionType);
                var result = await GetVideosDetailsAsync(stringList, YoutubeConfiguration.VideoDetailApiUrl, YoutubeConfiguration.UserApiKey);
                YoutubeVideos.Add(new YoutubeVideo()
                {
                    VideoCollectionType = videoCollection.VideoCollectionType,
                    YoutubeItems = result
                });
            }

            await SwitchCollectionType();
        }

        private async Task<List<string>> GetVideoIdStringAsync(string apiUrl, string videoCollectionType)
        {
            var json = await _youtubeDataServices.GetStringAsync(apiUrl);
            JObject response = JsonConvert.DeserializeObject<dynamic>(json);
            var items = response.Value<JArray>("items");
            return await GetTagDataAsync(items,
                YoutubeVideoCollectionType.Playlist == videoCollectionType ? "contentDetails" : "id");
        }

        private async Task<List<string>> GetTagDataAsync(JArray items, string tagName = "id")
        {
            var videoIds = new List<string>();
            return await Task.Run(() =>
            {
                foreach (var item in items)
                    videoIds.Add(item.Value<JObject>(tagName)?.Value<string>("videoId"));
                return videoIds;
            });
        }

        private async Task<List<YoutubeItem>> GetVideosDetailsAsync(List<string> videoIds, string videoDetailApiUrl,
            string userKey)
        {
            var videoIdsString = "";
            foreach (var s in videoIds)
                videoIdsString += s + ",";

            var index = videoIdsString.LastIndexOf(',');
            videoIdsString = videoIdsString.Remove(index);
            var json = await _youtubeDataServices.GetStringAsync(string.Format(videoDetailApiUrl,
                new string[] {videoIdsString, userKey}));
            var youtubeItems = new List<YoutubeItem>();
            try
            {
                JObject response = JsonConvert.DeserializeObject<dynamic>(json);
                var items = response.Value<JArray>("items");
                foreach (var item in items)
                {
                    var snippet = item.Value<JObject>("snippet");
                    var statistics = item.Value<JObject>("statistics");
                    var youtubeItem = new YoutubeItem
                    {
                        Title = snippet.Value<string>("title"),
                        Description = snippet.Value<string>("description"),
                        ChannelTitle = snippet.Value<string>("channelTitle"),
                        PublishedAt = snippet.Value<DateTime>("publishedAt"),
                        VideoId = item?.Value<string>("id"),
                        DefaultThumbnailUrl = snippet?.Value<JObject>("thumbnails")?.Value<JObject>("default")
                            ?.Value<string>("url"),
                        MediumThumbnailUrl = snippet?.Value<JObject>("thumbnails")?.Value<JObject>("medium")
                            ?.Value<string>("url"),
                        HighThumbnailUrl = snippet?.Value<JObject>("thumbnails")?.Value<JObject>("high")
                            ?.Value<string>("url"),
                        StandardThumbnailUrl = snippet?.Value<JObject>("thumbnails")?.Value<JObject>("standard")
                            ?.Value<string>("url"),
                        MaxResThumbnailUrl = snippet?.Value<JObject>("thumbnails")?.Value<JObject>("maxres")
                            ?.Value<string>("url"),

                        ViewCount = statistics?.Value<int>("viewCount"),
                        LikeCount = statistics?.Value<int>("likeCount"),
                        DislikeCount = statistics?.Value<int>("dislikeCount"),
                        FavoriteCount = statistics?.Value<int>("favoriteCount"),
                        CommentCount = statistics?.Value<int>("commentCount"),

                        ShowComment = YoutubeConfiguration.ShowComment,
                        ShowView = YoutubeConfiguration.ShowView,
                        ShowLike = YoutubeConfiguration.ShowLike,
                        ShowDescription = YoutubeConfiguration.ShowDescription,
                        ShowFavourite = YoutubeConfiguration.ShowFavourite,
                        ShowDislike = YoutubeConfiguration.ShowDislike
                    };
                    if (snippet?.Value<JArray>("tags") != null)
                        youtubeItem.Tags = (from tag in snippet?.Value<JArray>("tags") select tag.ToString())
                            ?.ToList();

                    if (string.IsNullOrEmpty(youtubeItem.Description))
                        youtubeItem.Description = "";

                    youtubeItems.Add(youtubeItem);
                }

                return youtubeItems;
            }
            catch (Exception)
            {
                return youtubeItems;
            }
        }

        private RootPage root;
        public const string RootPropertyName = "Root";

        public RootPage Root
        {
            get { return root; }
            set { SetProperty(ref root, value, RootPropertyName); }
        }

        private ICommand _showSideMenuCommand;

        public ICommand ShowSideMenuCommand
        {
            get
            {
                return _showSideMenuCommand ?? (_showSideMenuCommand = new Command((obj) =>
                {
                    Root.IsPresented = Root.IsPresented == false;
                }));
            }
        }

        private YoutubeConfiguration _youtubeConfiguration;
        public const string YoutubeConfigurationPropertyName = "YoutubeConfiguration";

        public YoutubeConfiguration YoutubeConfiguration
        {
            get { return _youtubeConfiguration; }
            set { SetProperty(ref _youtubeConfiguration, value, YoutubeConfigurationPropertyName); }
        }

        private List<YoutubeVideo> _youtubeVideos;
        public const string YoutubeVideosPropertyName = "YoutubeVideos";

        public List<YoutubeVideo> YoutubeVideos
        {
            get { return _youtubeVideos; }
            set { SetProperty(ref _youtubeVideos, value, YoutubeVideosPropertyName); }
        }

        private List<YoutubeItem> _youtubeItems;
        public const string YoutubeItemsPropertyName = "YoutubeItems";

        public List<YoutubeItem> YoutubeItems
        {
            get { return _youtubeItems; }
            set { SetProperty(ref _youtubeItems, value, YoutubeItemsPropertyName); }
        }


        private string _currentCollectionType;
        public const string CurrentCollectionTypePropertyName = "CurrentCollectionType";

        public string CurrentCollectionType
        {
            get { return _currentCollectionType; }
            set { SetProperty(ref _currentCollectionType, value, CurrentCollectionTypePropertyName); }
        }

        private string _playlistCollectionType;
        public const string PlaylistCollectionTypePropertyName = "PlaylistCollectionType";

        public string PlaylistCollectionType
        {
            get { return _playlistCollectionType; }
            set { SetProperty(ref _playlistCollectionType, value, PlaylistCollectionTypePropertyName); }
        }

        private string _channelCollectionType;
        public const string ChannelCollectionTypePropertyName = "ChannelCollectionType";

        public string ChannelCollectionType
        {
            get { return _channelCollectionType; }
            set { SetProperty(ref _channelCollectionType, value, ChannelCollectionTypePropertyName); }
        }

        private Style DefaultStyle => (Style) App.CurrentApp.Resources["labelStyleSwitch"];
        private Style SelectedStyle => (Style)App.CurrentApp.Resources["labelStyleSwitchHighlight"];

        private Style _playlistStyle;
        public const string PlaylistStylePropertyName = "PlaylistStyle";

        public Style PlaylistStyle
        {
            get { return _playlistStyle; }
            set { SetProperty(ref _playlistStyle, value, PlaylistStylePropertyName); }
        }

        private Style _channelStyle;
        public const string ChannelStylePropertyName = "ChannelStyle";

        public Style ChannelStyle
        {
            get { return _channelStyle; }
            set { SetProperty(ref _channelStyle, value, ChannelStylePropertyName); }
        }

        private ICommand _playlistCommand;

        public ICommand PlaylistCommand
        {
            get
            {
                return _playlistCommand ?? (_playlistCommand = new Command(async () =>
                {
                    if (CurrentCollectionType != YoutubeVideoCollectionType.Playlist)
                        await SwitchCollectionType();
                }));
            }
        }

        private ICommand _channelCommand;

        public ICommand ChannelCommand
        {
            get
            {
                return _channelCommand ?? (_channelCommand = new Command(async () =>
                {
                    if (CurrentCollectionType != YoutubeVideoCollectionType.Channel)
                        await SwitchCollectionType();
                }));
            }
        }

        private bool _playlistExists;
        public const string PlaylistExistsPropertyName = "PlaylistExists";

        public bool PlaylistExists
        {
            get { return _playlistExists; }
            set { SetProperty(ref _playlistExists, value, PlaylistExistsPropertyName); }
        }

        private bool _channelExists;
        public const string ChannelExistsPropertyName = "ChannelExists";
        public bool ChannelExists
        {
            get { return _channelExists; }
            set { SetProperty(ref _channelExists, value, ChannelExistsPropertyName); }
        }
    }
}