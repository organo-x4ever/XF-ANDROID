using Android.Content;
using Android.Media;
using com.organo.xchallenge.Droid.Services;
using com.organo.xchallenge.Services;
using System;
using Android.OS;
using Xamarin.Forms;

[assembly: Dependency(typeof(AudioPlayerService))]

namespace com.organo.xchallenge.Droid.Services
{
    public class AudioPlayerService : IAudioPlayerService
    {
        #region PRIVATE PROPERTIES

        private readonly MediaPlayer _mediaPlayer;
        private float MAX_VOLUME => 1.0f;

        #endregion PRIVATE PROPERTIES

        #region PUBLIC PROPERTIES

        public int Duration => _mediaPlayer?.Duration ?? 0;
        public int CurrentPosition => _mediaPlayer?.CurrentPosition ?? 0;
        public float Volume { get; set; }
        public bool CanSeek => true;
        public bool IsPlaying { get; set; }

        #endregion PUBLIC PROPERTIES

        #region PUBLIC ACTION

        public Action OnFinishedPlaying { get; set; }
        public string PlayNow { get; set; }
        public Context Context { get; set; }

        #endregion PUBLIC ACTION

        public AudioPlayerService()
        {
            Volume = 1.0f;
            //Get Activity
            MainActivity activity = Forms.Context as MainActivity;
            Context = activity.Window.Context;
            _mediaPlayer = new MediaPlayer();
            try
            {
                _mediaPlayer.SetWakeMode(Context, WakeLockFlags.ScreenDim);
            }
            catch (Exception ex)
            {
                _ = ex;
            }
        }

        public void Play(string pathToAudioFile)
        {
            if (_mediaPlayer != null)
            {
                _mediaPlayer.Completion -= MediaPlayer_Completion;
                _mediaPlayer.Stop();
            }

            PlayNow = pathToAudioFile;

            if (_mediaPlayer == null)
            {
                _mediaPlayer.Prepared += (sender, args) =>
                {
                    _mediaPlayer.Start();
                    _mediaPlayer.Completion += MediaPlayer_Completion;
                };
            }

            if (_mediaPlayer != null)
            {
                _mediaPlayer.Reset();
                SetVolume(Volume);
                _mediaPlayer.SetDataSource(PlayNow);
                _mediaPlayer.PrepareAsync();
                IsPlaying = _mediaPlayer.IsPlaying;
            }
        }

        public void SetVolume(float volume)
        {
            Volume = volume;
            float volumeValue = Volume / MAX_VOLUME;
            //(float) (1 - (Math.Log(MAX_VOLUME - soundVolume) / Math.Log(MAX_VOLUME)));
            if (_mediaPlayer != null)
                _mediaPlayer.SetVolume(volumeValue, volumeValue);
        }

        private void MediaPlayer_Completion(object sender, EventArgs e)
        {
            OnFinishedPlaying?.Invoke();
            if (_mediaPlayer != null)
                IsPlaying = _mediaPlayer.IsPlaying;
        }

        public void Pause()
        {
            _mediaPlayer?.Pause();
            if (_mediaPlayer != null)
                IsPlaying = _mediaPlayer.IsPlaying;
        }

        public void Play()
        {
            _mediaPlayer?.Start();
            if (_mediaPlayer != null)
                IsPlaying = _mediaPlayer.IsPlaying;
        }

        public void Stop()
        {
            _mediaPlayer?.Stop();
            if (_mediaPlayer != null)
                IsPlaying = _mediaPlayer.IsPlaying;
        }

        public void SeekTo(int seekValue)
        {
            _mediaPlayer?.SeekTo(seekValue);
            if (_mediaPlayer != null)
                IsPlaying = _mediaPlayer.IsPlaying;
        }

        public int GetTrackDuration(string pathToAudioFile)
        {
            _mediaPlayer.Reset();
            _mediaPlayer.SetDataSource(pathToAudioFile);
            return _mediaPlayer.Duration;
        }
        
        bool isDisposed = false;

        ///<Summary>
        /// Dispose SimpleAudioPlayer and release resources
        ///</Summary>
        protected virtual void Dispose(bool disposing)
        {
            if (isDisposed || _mediaPlayer == null)
                return;

            if (disposing)
            {
                _mediaPlayer.Dispose();
                }
            isDisposed = true;
        }

        ~AudioPlayerService()
        {
            Dispose(false);
        }

        ///<Summary>
        /// Dispose SimpleAudioPlayer and release resources
        ///</Summary>
        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }
    }
}