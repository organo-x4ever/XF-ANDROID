
using System;
using System.Collections.Generic;
using Android.Content;
using Android.Database;
using Android.Provider;
using com.organo.xchallenge;

[assembly: Xamarin.Forms.Dependency(typeof(MusicDictionary))]

namespace com.organo.xchallenge
{
    public class MusicDictionary : IMusicDictionary
    {
        private Context _Context => Android.App.Application.Context;
        private readonly ContentResolver _contentResolver;
        private readonly Android.Net.Uri _songUri;
        private List<MusicFile> _musicFiles { get; set; }

        public MusicDictionary()
        {
            _contentResolver = _Context.ContentResolver;
            _songUri = MediaStore.Audio.Media.ExternalContentUri;
        }

        public List<MusicFile> GetMusic()
        {
            String[] projection =
            {
                MediaStore.Audio.Media.InterfaceConsts.Album,
                MediaStore.Audio.Media.InterfaceConsts.AlbumId,
                MediaStore.Audio.Media.InterfaceConsts.Artist,
                MediaStore.Audio.Media.InterfaceConsts.ArtistId,
                MediaStore.Audio.Media.InterfaceConsts.Bookmark,
                MediaStore.Audio.Media.InterfaceConsts.Data,
                MediaStore.Audio.Media.InterfaceConsts.DateModified,
                MediaStore.Audio.Media.InterfaceConsts.DisplayName,
                MediaStore.Audio.Media.InterfaceConsts.Duration,
                MediaStore.Audio.Media.InterfaceConsts.Id,
                MediaStore.Audio.Media.InterfaceConsts.IsMusic,
                MediaStore.Audio.Media.InterfaceConsts.MimeType,
                MediaStore.Audio.Media.InterfaceConsts.Size,
                MediaStore.Audio.Media.InterfaceConsts.Title,
                MediaStore.Audio.Media.InterfaceConsts.Track,
                MediaStore.Audio.Media.InterfaceConsts.Year,
                MediaStore.Audio.Media.InterfaceConsts.AlbumKey,
                MediaStore.Audio.Media.InterfaceConsts.ArtistKey,
                MediaStore.Audio.Media.InterfaceConsts.Composer,
                MediaStore.Audio.Media.InterfaceConsts.DateAdded,
                MediaStore.Audio.Media.InterfaceConsts.IsAlarm,
                MediaStore.Audio.Media.InterfaceConsts.IsNotification,
                MediaStore.Audio.Media.InterfaceConsts.IsPodcast,
                MediaStore.Audio.Media.InterfaceConsts.IsRingtone,
                MediaStore.Audio.Media.InterfaceConsts.TitleKey
            };

            ICursor cursor = _contentResolver.Query(_songUri, projection, null, null, null);

            _musicFiles = new List<MusicFile>();
            while (cursor.MoveToNext())
            {
                _musicFiles.Add(new MusicFile
                {
                    Album = cursor.GetString(0),
                    AlbumId = cursor.GetString(1),
                    Artist = cursor.GetString(2),
                    ArtistId = cursor.GetString(3),
                    Bookmark = cursor.GetString(4),
                    Data = cursor.GetString(5),
                    DateModified = cursor.GetString(6),
                    DisplayName = cursor.GetString(7),
                    Duration = cursor.GetString(8),
                    Id = cursor.GetString(9),
                    IsMusic = cursor.GetString(10),
                    MimeType = cursor.GetString(11),
                    Size = cursor.GetString(12),
                    Title = cursor.GetString(13),
                    Track = cursor.GetString(14),
                    Year = cursor.GetString(15),
                    AlbumKey = cursor.GetString(16),
                    ArtistKey = cursor.GetString(17),
                    Composer = cursor.GetString(18),
                    DateAdded = cursor.GetString(19),
                    IsAlarm = cursor.GetString(20),
                    IsNotification = cursor.GetString(21),
                    IsPodcast = cursor.GetString(22),
                    IsRingtone = cursor.GetString(23),
                    TitleKey = cursor.GetString(24)
                });
            }

            return _musicFiles;
        }
    }
}