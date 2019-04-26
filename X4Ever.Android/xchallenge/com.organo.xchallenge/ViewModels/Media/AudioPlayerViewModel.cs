
using com.organo.xchallenge.Localization;
using com.organo.xchallenge.Permissions;
using com.organo.xchallenge.Services;
using com.organo.xchallenge.Statics;
using com.organo.xchallenge.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using com.organo.xchallenge.Helpers;
using com.organo.xchallenge.Models;
using Plugin.MediaManager.Abstractions.Enums;
using Plugin.MediaManager.Abstractions.Implementations;
using Xamarin.Forms;
using com.organo.xchallenge.MediaManager;
using Plugin.MediaManager.Abstractions;

namespace com.organo.xchallenge.ViewModels.Media
{
    public class AudioPlayerViewModel : BaseViewModel
    {
        private IMediaPlayerServices _mediaPlayerServices;
        private readonly IMusicDictionary _musicDictionary;
        private IAudioPlayerService _audioPlayer;
        private IDevicePermissionServices _devicePermissionServices;
        private static short timerSeconds = 1;
        public AudioPlayerViewModel(INavigation navigation = null) : base(navigation)
        {
            _mediaPlayerServices = DependencyService.Get<IMediaPlayerServices>();
            _musicDictionary = DependencyService.Get<IMusicDictionary>();
            _audioPlayer = DependencyService.Get<IAudioPlayerService>();
            _devicePermissionServices = DependencyService.Get<IDevicePermissionServices>();
            SetPageImageSize();
            MediaFiles = new List<MediaFile>();
            CurrentMediaFile = new MediaFile();
            PlayButton = ImageResizer.ResizeImage(TextResources.icon_media_play, ButtonImageSize);
            PauseButton = ImageResizer.ResizeImage(TextResources.icon_media_pause, ButtonImageSize);
            StopButton = ImageResizer.ResizeImage(TextResources.icon_media_stop, ButtonImageSize);
            NextButton = ImageResizer.ResizeImage(TextResources.icon_media_next, ButtonImageSize);
            PreviousButton = ImageResizer.ResizeImage(TextResources.icon_media_previous, ButtonImageSize);
            PlayPauseButton = PlayButton;
            IsPlaying = false;
            IsPause = false;
            IsMediaExists = false;
            CurrentSongIndex = 0;
            _mediaPlayerServices.TimedTextAction = TimedText;
            //CrossMediaManager.Current.PlayingChanged += async (sender, e) =>
            //{
            //    CurrentPosition = e.Progress;
            //    CurrentTimer = e.Position.ToString(@"hh\:mm\:ss");
            //    TotalTimer = e.Duration.ToString(@"hh\:mm\:ss");
            //    double TOLERANCE = 0;
            //    if (Math.Abs(e.Progress - 1) < TOLERANCE)
            //        await PlayCurrent(Next());
            //};
        }

        private void TimedText()
        {

        }

        private void CurrentPlay()
        {
            var seconds = TimeSpan.FromSeconds(timerSeconds);
            Device.StartTimer(seconds, () =>
            {
                PlayerLoop().GetAwaiter();
                return true;
            });
        }

        private async Task PlayerLoop()
        {
            if (IsPlaying)
            {
                CurrentPosition = (_mediaPlayerServices.CurrentPosition * 100) / _mediaPlayerServices.Duration;
                CurrentTimer = TimeSpan.FromMilliseconds(_mediaPlayerServices.CurrentPosition).ToString(@"hh\:mm\:ss");
                TotalTimer = TimeSpan.FromMilliseconds(_mediaPlayerServices.Duration).ToString(@"hh\:mm\:ss");
                double TOLERANCE = _mediaPlayerServices.Duration;
                if (_mediaPlayerServices.CurrentPosition >= _mediaPlayerServices.Duration)
                    await PlayCurrent(Next());
            }
        }

        public async void OnLoad() => await GetFilesAsync();

        public async Task GetFilesAsync()
        {
            MediaFiles = new List<MediaFile>();
            var mediaFiles = new List<MediaFile>();
            if (await _devicePermissionServices.RequestReadStoragePermission())
            {
                var fileDetails = _musicDictionary.GetMusic();
                //if (fileDetails == null || fileDetails.Count == 0 && !fileDetails.Any(f => f.Type.EndsWith(FileType)))
                //    SetActivityResource(showError: true, errorMessage: TextResources.NoFileExists);
                //else
                //{
                    //foreach (var content in fileDetails.Where(f => f.Type.EndsWith(FileType)).OrderBy(f => f.Name))
                    foreach (var content in fileDetails.OrderBy(f => f.Title))
                    {
                        int.TryParse(content.Duration, out int duration);
                        int.TryParse(content.Track, out int trackNumber);
                        long.TryParse(content.Year, out long year);
                        DateTime.TryParse(content.DateModified, out DateTime date);

                        IMediaFileMetadata mediaFileMetadata = new MediaFileMetadata()
                        {
                            Title = content.Title,
                            Album = content.Album,
                            Artist = content.Artist,
                            DisplayTitle = content.DisplayName,
                            Duration = duration,
                            Date = date,
                            MediaUri = content.Data,
                            NumTracks = trackNumber,
                            Year = year,
                            AlbumArt = (TimeSpan.FromMilliseconds(duration).ToString().Split('.'))[0]
                        };

                        mediaFiles.Add(new MediaFile()
                        {
                            Url = content.Data,
                            Type = MediaFileType.Audio,
                            MetadataExtracted = false,
                            Availability = ResourceAvailability.Local,
                            Metadata = mediaFileMetadata
                        });
                    }
                    //}

                MediaFiles = mediaFiles;
                IsMediaExists = MediaFiles.Count > 0;
                if (MediaFiles.Count == 0)
                    SetActivityResource(showError: true, errorMessage: TextResources.NoFileExists);
            }
            else
            {
                ErrorMessage = TextResources.MessagePermissionReadStorageRequired;
                IsError = true;
            }
        }

        private bool _showPlayer;
        public const string ShowPlayerPropertyName = "ShowPlayer";

        public bool ShowPlayer
        {
            get { return _showPlayer; }
            set { SetProperty(ref _showPlayer, value, ShowPlayerPropertyName); }
        }

        private string FileType => "MP3";

        private double _currentPosition;
        public const string CurrentPositionPropertyName = "CurrentPosition";

        public double CurrentPosition
        {
            get { return _currentPosition; }
            set { SetProperty(ref _currentPosition, value, CurrentPositionPropertyName); }
        }

        private string _currentTimer;
        public const string CurrentTimerPropertyName = "CurrentTimer";

        public string CurrentTimer
        {
            get { return _currentTimer; }
            set { SetProperty(ref _currentTimer, value, CurrentTimerPropertyName); }
        }

        
        private string _timeSplitor;
        public const string TimeSplitorPropertyName = "TimeSplitor";

        public string TimeSplitor
        {
            get { return _timeSplitor; }
            set { SetProperty(ref _timeSplitor, value, TimeSplitorPropertyName); }
        }

        private string _totalTimer;
        public const string TotalTimerPropertyName = "TotalTimer";

        public string TotalTimer
        {
            get { return _totalTimer; }
            set { SetProperty(ref _totalTimer, value, TotalTimerPropertyName); }
        }

        private string _mediaTitle;
        public const string MediaTitlePropertyName = "MediaTitle";

        public string MediaTitle
        {
            get { return _mediaTitle; }
            set { SetProperty(ref _mediaTitle, value, MediaTitlePropertyName); }
        }

        private ImageSource _playButton;
        public string PlayButtonPropertyName = "PlayButton";

        public ImageSource PlayButton
        {
            get { return _playButton; }
            set { SetProperty(ref _playButton, value, PlayButtonPropertyName); }
        }

        private ImageSource _pauseButton;
        public string PauseButtonPropertyName = "PauseButton";

        public ImageSource PauseButton
        {
            get { return _pauseButton; }
            set { SetProperty(ref _pauseButton, value, PauseButtonPropertyName); }
        }

        private ImageSource _playPauseButton;
        public string PlayPauseButtonPropertyName = "PlayPauseButton";

        public ImageSource PlayPauseButton
        {
            get { return _playPauseButton; }
            set { SetProperty(ref _playPauseButton, value, PlayPauseButtonPropertyName); }
        }

        private ImageSource _stopButton;
        public string StopButtonPropertyName = "StopButton";

        public ImageSource StopButton
        {
            get { return _stopButton; }
            set { SetProperty(ref _stopButton, value, StopButtonPropertyName); }
        }

        private ImageSource _nextButton;
        public string NextButtonPropertyName = "NextButton";

        public ImageSource NextButton
        {
            get { return _nextButton; }
            set { SetProperty(ref _nextButton, value, NextButtonPropertyName); }
        }

        private ImageSource _previousButton;
        public string PreviousButtonPropertyName = "PreviousButton";

        public ImageSource PreviousButton
        {
            get { return _previousButton; }
            set { SetProperty(ref _previousButton, value, PreviousButtonPropertyName); }
        }

        private ImageSize ButtonImageSize { get; set; }

        private void SetPageImageSize()
        {
            ButtonImageSize = App.Configuration.GetImageSizeByID(ImageIdentity.AUDIO_PLAYER_PAGE_COMMAND_IMAGE);
            if (ButtonImageSize != null)
            {
                AudioCommandImageHeight = ButtonImageSize.Height;
                AudioCommandImageWidth = ButtonImageSize.Width;
            }
        }

        private float _audioCommandImageHeight;
        public const string AudioCommandImageHeightPropertyName = "AudioCommandImageHeight";

        public float AudioCommandImageHeight
        {
            get { return _audioCommandImageHeight; }
            set { SetProperty(ref _audioCommandImageHeight, value, AudioCommandImageHeightPropertyName); }
        }

        private float _audioCommandImageWidth;
        public const string AudioCommandImageWidthPropertyName = "AudioCommandImageWidth";

        public float AudioCommandImageWidth
        {
            get { return _audioCommandImageWidth; }
            set { SetProperty(ref _audioCommandImageWidth, value, AudioCommandImageWidthPropertyName); }
        }

        private int _currentSongIndex;
        public const string CurrentSongIndexPropertyName = "CurrentSongIndex";

        public int CurrentSongIndex
        {
            get { return _currentSongIndex; }
            set { SetProperty(ref _currentSongIndex, value, CurrentSongIndexPropertyName); }
        }

        private bool _isMediaExists;
        public const string IsMediaExistsPropertyName = "IsMediaExists";

        public bool IsMediaExists
        {
            get { return _isMediaExists; }
            set { SetProperty(ref _isMediaExists, value, IsMediaExistsPropertyName); }
        }

        private bool _isPlaying;
        public const string IsPlayingPropertyName = "IsPlaying";

        public bool IsPlaying
        {
            get { return _isPlaying; }
            set { SetProperty(ref _isPlaying, value, IsPlayingPropertyName, IsPlayingChange); }
        }

        private void IsPlayingChange()
        {
            if (IsPlaying)
                PlayPauseButton = PauseButton;
            else
                PlayPauseButton = PlayButton;
        }

        private bool _isPause;
        public const string IsPausePropertyName = "IsPause";

        public bool IsPause
        {
            get { return _isPause; }
            set { SetProperty(ref _isPause, value, IsPausePropertyName); }
        }

        public async Task PlayCurrent(int songIndex)
        {
            try
            {
                await Task.Run(() =>
                {
                    CurrentSongIndex = songIndex;
                    IsPlaying = true;
                    CurrentMediaFile = MediaFiles[songIndex];
                    MediaTitle = MediaFiles[songIndex].Metadata.Title;
                    _mediaPlayerServices.StartPlayer(CurrentMediaFile.Url);
                    CurrentPlay();
                    //_mediaPlayerServices.Play(CurrentMediaFile);
                });
            }
            catch (Exception)
            {
                //Error
            }
        }

        private int Next()
        {
            if (MediaFiles.Count() > (CurrentSongIndex + 1))
                CurrentSongIndex++;
            else
                CurrentSongIndex = 0;
            return CurrentSongIndex;
        }

        private int Previous()
        {
            if (CurrentSongIndex > 0)
                CurrentSongIndex--;
            else
                CurrentSongIndex = MediaFiles.Count - 1;
            return CurrentSongIndex;
        }

        private List<MediaFile> _mediaFiles;
        public const string MediaFilesPropertyName = "MediaFiles";

        public List<MediaFile> MediaFiles
        {
            get { return _mediaFiles; }
            set { SetProperty(ref _mediaFiles, value, MediaFilesPropertyName); }
        }
        
        private MediaFile _currentMediaFile;
        public const string CurrentMediaFilePropertyName = "CurrentMediaFile";

        public MediaFile CurrentMediaFile
        {
            get { return _currentMediaFile; }
            set { SetProperty(ref _currentMediaFile, value, CurrentMediaFilePropertyName); }
        }

        private ICommand _playCommand;

        public ICommand PlayCommand
        {
            get
            {
                return _playCommand ?? (_playCommand = new Command(async (obj) =>
                {
                    IsPlaying = IsPlaying == false;
                    if (IsPlaying)
                    {
                        if (IsPause)
                            _mediaPlayerServices.Play();
                        else
                            await PlayCurrent(CurrentSongIndex);
                        IsPause = false;
                    }
                    else
                    {
                        IsPause = true;
                        _mediaPlayerServices.Pause();
                    }
                }));
            }
        }

        private ICommand _stopCommand;

        public ICommand StopCommand
        {
            get
            {
                return _stopCommand ?? (_stopCommand = new Command(() =>
                {
                    IsPlaying = false;
                    StopAsync();
                    CurrentTimer = TimeSpan.FromSeconds(0).ToString(@"hh\:mm\:ss");
                    TimeSplitor = "";
                }));
            }
        }

        public void StopAsync()
        {
            try
            {
                CurrentPosition = 0;
                _mediaPlayerServices.Stop();
            }
            catch
            {
                // Commented
            }
        }

        private ICommand _nextCommand;

        public ICommand NextCommand
        {
            get
            {
                return _nextCommand ?? (_nextCommand = new Command(async () => { await PlayCurrent(Next()); }));
            }
        }

        private ICommand _previousCommand;

        public ICommand PreviousCommand
        {
            get
            {
                return _previousCommand ?? (_previousCommand = new Command(async () =>
                {
                    await PlayCurrent(Previous());
                }));
            }
        }
    }
}