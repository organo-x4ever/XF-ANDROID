using com.organo.xchallenge.Models.Media;
using com.organo.xchallenge.Pages.Base;
using com.organo.xchallenge.ViewModels.Media;
using System;
using System.Linq;
using com.organo.xchallenge.Handler;
using com.organo.xchallenge.Localization;
using com.organo.xchallenge.Notification;
using com.organo.xchallenge.Services;
using Plugin.MediaManager.Abstractions.Implementations;
using Xamarin.Forms;
using com.organo.xchallenge.Permissions;
using com.organo.xchallenge.Statics;
using com.organo.xchallenge.Utilities;
using System.Collections.Generic;

namespace com.organo.xchallenge.Pages.Media
{
    public partial class AudioPlayerPage : AudioPlayerPageXaml
    {
        private AudioPlayerViewModel _model;
        private readonly IDevicePermissionServices _devicePermissionServices;
        public AudioPlayerPage(RootPage rootPage)
        {
            try
            {
                InitializeComponent();
                _devicePermissionServices = DependencyService.Get<IDevicePermissionServices>();
                Init(rootPage);
            }
            catch (Exception ex)
            {
                var exceptionHandler = new ExceptionHandler(TAG, ex);
            }
        }

        private async void Init(object obj)
        {
            await App.Configuration.InitialAsync(this);
            _model = new AudioPlayerViewModel()
            {
                Root = (RootPage) obj,
                DisplaySortByListAction = DisplaySortByList
            };
            BindingContext = _model;
            if (await _devicePermissionServices.RequestDeviceKeepWakePermission())
            {
                _model.IsPermissionGranted = true;
            }
            await _model.GetFilesAsync();
        }

        private async void ListView_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                if (await _devicePermissionServices.RequestDeviceKeepWakePermission())
                {
                    _model.IsPermissionGranted = true;
                }

                if (_model.IsChecklistSelected)
                {
                    var musicFile = ((MusicFile) e.SelectedItem);
                    musicFile.IsPlaylistSelected = true;
                    musicFile.TextColor = musicFile.IsPlaylistSelected
                        ? Palette._LightGrayD
                        : Palette._ButtonBackgroundGray;

                    var playlistMusicFile = _model.PlaylistMusicFiles.Find(m =>
                        m.Title == musicFile.Title && m.Album == musicFile.Album && m.Artist == musicFile.Artist);
                    if (playlistMusicFile == null)
                        _model.PlaylistMusicFiles.Add(musicFile);
                    else
                        _model.PlaylistMusicFiles.Remove(playlistMusicFile);

                    _model.MusicFiles = _model.MusicFiles.Select(m =>
                        {
                            m.IsPlaylistSelected = _model.PlaylistMusicFiles.Any(t =>
                                t.AlbumId == m.AlbumId && t.Id == m.Id &&
                                t.Title == m.Title && t.Album == m.Album && t.Artist == m.Artist);
                            m.TextColor = m.IsPlaylistSelected ? Palette._LightGrayD : Palette._ButtonBackgroundGray;
                            return m;
                        }).ToList();
                }
                else if (!((MusicFile) e.SelectedItem).IsPlaylistSelected)
                {
                    _model.SetActivityResource(showMessage: true, message: "Song does not exist in playlist");
                    return;
                }
                else if (_model.CurrentMusicFile != (MusicFile) e.SelectedItem)
                {
                    var selectedContent = (MusicFile) e.SelectedItem;
                    _model.CurrentMusicFile = selectedContent;
                    int index = _model.MusicFiles.FindIndex(m => m == selectedContent && m == selectedContent);
                    await _model.PlayCurrent(index);
                }
            }

            ListViewPlayer.SelectedItem = null;
        }

        public async void DisplaySortByList()
        {
            var sortLists = EnumUtil.GetValues<PlaylistSortList>();
            var list = new List<string>();
            foreach (var sortList in sortLists)
            {
                list.Add(sortList.ToString());
            }

            var result =
                await DisplayActionSheet(TextResources.ChooseOption, TextResources.Cancel, null, list.ToArray());
            if (result != null && result != TextResources.Cancel)
            {
                _model.PlaylistSortBy = (PlaylistSortList) Enum.Parse(typeof(PlaylistSortList), result.ToString());
                _model.SortOrderBy(_model.PlaylistSortBy);
            }
        }

        protected override void OnDisappearing()
        {
            _model?.StopAsync();
            base.OnDisappearing();
        }

        protected override bool OnBackButtonPressed()
        {
            return DependencyService.Get<IBackButtonPress>().Redirect(_model.Root);
        }
    }

    public abstract class AudioPlayerPageXaml : ModelBoundContentPage<AudioPlayerViewModel>
    {
    }
}