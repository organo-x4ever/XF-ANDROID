
using System;
using com.organo.xchallenge.Statics;
using Xamarin.Forms;

namespace com.organo.xchallenge
{
    public class MusicFile
    {
        public MusicFile()
        {
            Album = string.Empty;
            AlbumId = string.Empty;
            AlbumKey = string.Empty;
            Artist = string.Empty;
            ArtistId = string.Empty;
            ArtistKey = string.Empty;
            Bookmark = string.Empty;
            Composer = string.Empty;
            Data = string.Empty;
            DateAdded = string.Empty;
            DateModified = string.Empty;
            DisplayName = string.Empty;
            Duration = string.Empty;
            Id = string.Empty;
            IsAlarm = string.Empty;
            IsMusic = string.Empty;
            IsNotification = string.Empty;
            IsPodcast = string.Empty;
            IsRingtone = string.Empty;
            MimeType = string.Empty;
            Size = string.Empty;
            Title = string.Empty;
            TitleKey = string.Empty;
            Track = string.Empty;
            Year = string.Empty;

            _Duration = 0;
            _Date = new DateTime(1900, 1, 1);
            _Track = 0;
            _Year = 1900;
            _DurationTimeSpan = string.Empty;
            IsPlayNow = false;
            TextColor = Palette._LightGrayD;
            IsPlaylistSelected = true;
        }

        public string Album { get; set; }
        public string AlbumId { get; set; }
        public string AlbumKey { get; set; }
        public string Artist { get; set; }
        public string ArtistId { get; set; }
        public string ArtistKey { get; set; }
        public string Bookmark { get; set; }
        public string Composer { get; set; }
        public string Data { get; set; }
        public string DateAdded { get; set; }
        public string DateModified { get; set; }
        public string DisplayName { get; set; }
        public string Duration { get; set; }
        public string Id { get; set; }
        public string IsAlarm { get; set; }
        public string IsMusic { get; set; }
        public string IsNotification { get; set; }
        public string IsPodcast { get; set; }
        public string IsRingtone { get; set; }
        public string MimeType { get; set; }
        public string Size { get; set; }
        public string Title { get; set; }
        public string TitleKey { get; set; }
        public string Track { get; set; }
        public string Year { get; set; }

        public double _Duration { get; set; }
        public DateTime _Date { get; set; }
        public int _Track { get; set; }
        public long _Year { get; set; }
        public string _DurationTimeSpan { get; set; }
        public bool IsPlayNow { get; set; }
        public Color TextColor { get; set; }
        public bool IsPlaylistSelected { get; set; }
    }
}