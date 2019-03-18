using Android.Content;
using Android.Media;
using com.organo.xchallenge.Droid.Services;
using com.organo.xchallenge.Services;
using System;
using Xamarin.Forms;

[assembly: Dependency(typeof(AudioPlayerService))]

namespace com.organo.xchallenge.Droid.Services
{
    public class AudioPlayerService : IAudioPlayerService
    {
        #region PRIVATE PROPERTIES

        private MediaPlayer _mediaPlayer;
        private float MAX_VOLUME => 1.0f;

        #endregion PRIVATE PROPERTIES

        #region PUBLIC PROPERTIES

        public int Duration => _mediaPlayer != null ? _mediaPlayer.Duration : 0;
        public int CurrentPosition => _mediaPlayer != null ? _mediaPlayer.CurrentPosition : 0;
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
            this.Volume = 1.0f;
            //Get Activity
            MainActivity activity = Forms.Context as MainActivity;
            this.Context = activity.Window.Context;
        }

        public void Play(string pathToAudioFile)
        {
            if (_mediaPlayer != null)
            {
                _mediaPlayer.Completion -= MediaPlayer_Completion;
                _mediaPlayer.Stop();
            }

            this.PlayNow = pathToAudioFile;

            if (_mediaPlayer == null)
            {
                _mediaPlayer = new MediaPlayer();
                _mediaPlayer.Prepared += (sender, args) =>
                {
                    _mediaPlayer.Start();
                    _mediaPlayer.Completion += MediaPlayer_Completion;
                };
            }

            if (_mediaPlayer != null)
            {
                _mediaPlayer.Reset();
                SetVolume(this.Volume);
                _mediaPlayer.SetDataSource(this.PlayNow);
                _mediaPlayer.PrepareAsync();
                this.IsPlaying = _mediaPlayer.IsPlaying;
            }
        }

        public void SetVolume(float volume)
        {
            this.Volume = volume;
            float volumeValue = this.Volume / MAX_VOLUME;
            //(float) (1 - (Math.Log(MAX_VOLUME - soundVolume) / Math.Log(MAX_VOLUME)));
            if (_mediaPlayer != null)
                _mediaPlayer.SetVolume(volumeValue, volumeValue);
        }

        private void MediaPlayer_Completion(object sender, EventArgs e)
        {
            OnFinishedPlaying?.Invoke();
            if (_mediaPlayer != null)
                this.IsPlaying = _mediaPlayer.IsPlaying;
        }

        public void Pause()
        {
            _mediaPlayer?.Pause();
            if (_mediaPlayer != null)
                this.IsPlaying = _mediaPlayer.IsPlaying;
        }

        public void Play()
        {
            _mediaPlayer?.Start();
            if (_mediaPlayer != null)
                this.IsPlaying = _mediaPlayer.IsPlaying;
        }

        public void Stop()
        {
            _mediaPlayer?.Stop();
            if (_mediaPlayer != null)
                this.IsPlaying = _mediaPlayer.IsPlaying;
        }

        public void SeekTo(int seekValue)
        {
            _mediaPlayer?.SeekTo(seekValue);
            if (_mediaPlayer != null)
                this.IsPlaying = _mediaPlayer.IsPlaying;
        }

        public int GetTrackDuration(string pathToAudioFile)
        {
            if (_mediaPlayer == null)
            {
                _mediaPlayer = new MediaPlayer();
            }

            this._mediaPlayer.Reset();
            this._mediaPlayer.SetDataSource(pathToAudioFile);
            return this._mediaPlayer.Duration;
        }
    }
}