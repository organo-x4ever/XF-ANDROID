
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
using com.organo.xchallenge;
using Plugin.MediaManager.Abstractions;

namespace com.organo.xchallenge.ViewModels.Media
{
    public class AudioPlayerViewModel : BaseViewModel
    {
        private IAudioPlayerManager _audioPlayerManager;
        private readonly IMusicDictionary _musicDictionary;
        private IDevicePermissionServices _devicePermissionServices;
        private static short timerSeconds = 1;

        public AudioPlayerViewModel(INavigation navigation = null) : base(navigation)
        {
            _audioPlayerManager = DependencyService.Get<IAudioPlayerManager>();
            _musicDictionary = DependencyService.Get<IMusicDictionary>();
            _devicePermissionServices = DependencyService.Get<IDevicePermissionServices>();
            SetPageImageSize();
            MusicFiles = new List<MusicFile>();
            CurrentMusicFile = new MusicFile();
            PlayButton = ImageResizer.ResizeImage(TextResources.icon_media_play, ButtonImageSize);
            PauseButton = ImageResizer.ResizeImage(TextResources.icon_media_pause, ButtonImageSize);
            StopButton = ImageResizer.ResizeImage(TextResources.icon_media_stop, ButtonImageSize);
            NextButton = ImageResizer.ResizeImage(TextResources.icon_media_next, ButtonImageSize);
            PreviousButton = ImageResizer.ResizeImage(TextResources.icon_media_previous, ButtonImageSize);
            ForwardButton = ImageResizer.ResizeImage(TextResources.icon_media_forward, ButtonImageSize);
            BackwardButton = ImageResizer.ResizeImage(TextResources.icon_media_backward, ButtonImageSize);
            PlayPauseButton = PlayButton;
            NowPlayingButton = PlayButton;
            IsPlaying = false;
            IsPause = false;
            IsMediaExists = false;
            CurrentSongIndex = 0;
            IsLoopStarted = false;
            IsPermissionGranted = false;
        }

        private void CurrentPlay()
        {
            if (IsLoopStarted) return;
            var seconds = TimeSpan.FromSeconds(timerSeconds);
            Device.StartTimer(seconds, () =>
            {
                IsLoopStarted = true;
                PlayerLoop().GetAwaiter();
                return IsLoopStarted;
            });
        }

        private async Task PlayerLoop()
        {
            if (IsPlaying)
            {
                CurrentPosition = (_audioPlayerManager.CurrentPlayer.CurrentPosition * 100) /
                                  _audioPlayerManager.CurrentPlayer.Duration;
                CurrentTimer = TimeSpan.FromSeconds(_audioPlayerManager.CurrentPlayer.CurrentPosition)
                    .ToString(TimeStyle);
                double TOLERANCE = _audioPlayerManager.CurrentPlayer.Duration;
                if (_audioPlayerManager.CurrentPlayer.IsPlaying && _audioPlayerManager.CurrentPlayer.CurrentPosition >=
                    _audioPlayerManager.CurrentPlayer.Duration)
                    await PlayCurrent(Next());
            }
        }

        public async void OnLoad() => await GetFilesAsync();

        public async Task GetFilesAsync()
        {
            MusicFiles = new List<MusicFile>();
            if (await _devicePermissionServices.RequestReadStoragePermission())
            {
                var musicFiles = _musicDictionary.GetMusic();

                MusicFiles = musicFiles.Select(music =>
                {
                    music._Duration = int.TryParse(music.Duration, out int duration) ? duration : 0;
                    music._Date = DateTime.TryParse(music.DateModified, out DateTime date)
                        ? date
                        : new DateTime(1900, 1, 1);
                    music._Track = int.TryParse(music.Track, out int trackNumber) ? trackNumber : 0;
                    music._Year = long.TryParse(music.Year, out long year) ? year : 0;
                    music._DurationTimeSpan = (TimeSpan.FromMilliseconds(duration).ToString().Split('.'))[0];
                    return music;
                }).OrderBy(m => m.Title).ToList();

                IsMediaExists = MusicFiles.Count > 0;
                if (MusicFiles.Count == 0)
                    SetActivityResource(showError: true, errorMessage: TextResources.NoFileExists);
            }
            else
            {
                ErrorMessage = TextResources.MessagePermissionReadStorageRequired;
                IsError = true;
            }
        }

        private bool IsLoopStarted { get; set; }
        private bool IsDurationLong { get; set; }
        private string TimeStyle { get; set; }

        private bool _isPermissionGranted;
        public const string IsPermissionGrantedPropertyName = "IsPermissionGranted";

        public bool IsPermissionGranted
        {
            get { return _isPermissionGranted; }
            set { SetProperty(ref _isPermissionGranted, value, IsPermissionGrantedPropertyName); }
        }

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

        private ImageSource _forwardButton;
        public string ForwardButtonPropertyName = "ForwardButton";

        public ImageSource ForwardButton
        {
            get { return _forwardButton; }
            set { SetProperty(ref _forwardButton, value, ForwardButtonPropertyName); }
        }
        
        private ImageSource _backwardButton;
        public string BackwardButtonPropertyName = "BackwardButton";

        public ImageSource BackwardButton
        {
            get { return _backwardButton; }
            set { SetProperty(ref _backwardButton, value, BackwardButtonPropertyName); }
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
        
        private ImageSource _nowPlayingButton;
        public string NowPlayingButtonPropertyName = "NowPlayingButton";

        public ImageSource NowPlayingButton
        {
            get { return _nowPlayingButton; }
            set { SetProperty(ref _nowPlayingButton, value, NowPlayingButtonPropertyName); }
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
                    MusicFiles = MusicFiles.Select(m =>
                    {
                        m.IsPlayNow = false;
                        m.TextColor = Palette._LightGrayD;
                        return m;
                    }).ToList();
                    var currentMusicFile = MusicFiles[songIndex];
                    currentMusicFile.IsPlayNow = true;
                    currentMusicFile.TextColor = Palette._MainAccent;
                    CurrentMusicFile = currentMusicFile;
                    MediaTitle = MusicFiles[songIndex].Title;
                    _audioPlayerManager.CurrentPlayer.Load(CurrentMusicFile.Data, true);
                    _audioPlayerManager.CurrentPlayer.Play();
                    IsDurationLong = _audioPlayerManager.CurrentPlayer.Duration > (60 * 60);
                    TimeStyle = IsDurationLong ? @"hh\:mm\:ss" : @"mm\:ss";
                    TotalTimer = TimeSpan.FromSeconds(_audioPlayerManager.CurrentPlayer.Duration).ToString(TimeStyle);
                    CurrentPlay();
                });
            }
            catch (Exception ex)
            {
                //Error
                _ = ex;
            }
        }

        private int Next()
        {
            if (MusicFiles.Count() > (CurrentSongIndex + 1))
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
                CurrentSongIndex = MusicFiles.Count - 1;
            return CurrentSongIndex;
        }

        private List<MusicFile> _musicFiles;
        public const string MusicFilesPropertyName = "MusicFiles";

        public List<MusicFile> MusicFiles
        {
            get { return _musicFiles; }
            set { SetProperty(ref _musicFiles, value, MusicFilesPropertyName); }
        }

        private MusicFile _currentMusicFile;
        public const string CurrentMusicFilePropertyName = "CurrentMusicFile";

        public MusicFile CurrentMusicFile
        {
            get { return _currentMusicFile; }
            set { SetProperty(ref _currentMusicFile, value, CurrentMusicFilePropertyName); }
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
                            _audioPlayerManager.CurrentPlayer.Play();
                        else
                            await PlayCurrent(CurrentSongIndex);
                        IsPause = false;
                    }
                    else
                    {
                        IsPause = true;
                        _audioPlayerManager.CurrentPlayer.Pause();
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
                    CurrentTimer = TimeSpan.FromSeconds(0).ToString(TimeStyle);
                    TimeSplitor = "";
                }));
            }
        }

        public void StopAsync()
        {
            try
            {
                IsLoopStarted = false;
                CurrentPosition = 0;
                _audioPlayerManager.CurrentPlayer.Stop();
            }
            catch
            {
                // Commented
            }
        }

        private ICommand _nextCommand;

        public ICommand NextCommand
        {
            get { return _nextCommand ?? (_nextCommand = new Command(async () => { await PlayCurrent(Next()); })); }
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

        private ICommand _forwardCommand;

        public ICommand ForwardCommand
        {
            get
            {
                return _forwardCommand ?? (_forwardCommand = new Command(async () =>
                {
                    if (_audioPlayerManager.CurrentPlayer.Duration >
                        _audioPlayerManager.CurrentPlayer.CurrentPosition + 10)
                        _audioPlayerManager.CurrentPlayer.Seek(_audioPlayerManager.CurrentPlayer.CurrentPosition + 10);
                    else
                        await PlayCurrent(Next());
                }));
            }
        }

        private ICommand _backwardCommand;

        public ICommand BackwardCommand
        {
            get
            {
                return _backwardCommand ?? (_backwardCommand = new Command(() =>
                {
                    if (_audioPlayerManager.CurrentPlayer.CurrentPosition - 10 > 0)
                        _audioPlayerManager.CurrentPlayer.Seek(_audioPlayerManager.CurrentPlayer.CurrentPosition - 10);
                    else
                        _audioPlayerManager.CurrentPlayer.Seek(0);
                }));
            }
        }
    }
}