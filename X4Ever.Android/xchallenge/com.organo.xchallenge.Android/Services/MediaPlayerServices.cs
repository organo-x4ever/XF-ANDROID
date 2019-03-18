using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using com.organo.xchallenge.Droid.Services;
using com.organo.xchallenge.Services;
using Xamarin.Forms;

[assembly:Dependency(typeof(MediaPlayerServices))]

namespace com.organo.xchallenge.Droid.Services
{
    public class MediaPlayerServices : IMediaPlayerServices
    {
        private MediaPlayer player;

        public void StartPlayer(String filePath)
        {
            if (player == null)
            {
                player = new MediaPlayer();
            }

            player.Reset();
            player.SetDataSource(filePath);
            player.Prepare();
            player.Start();
            player.TimedText += Player_TimedText;
        }

        private void Player_TimedText(object sender, MediaPlayer.TimedTextEventArgs e)
        {
            TimedTextAction?.Invoke();
        }

        public void Pause()
        {
            player.Pause();
        }

        public void Play()
        {
            player.Start();
        }

        public void Stop()
        {
            try
            {
                player.Stop();
            }
            catch
            {
                //
            }
        }

        public void Release()
        {
            player.Release();
        }

        private Action _timedTextAction;
        private const string TimedTextActionPropertyName = "TimedTextAction";

        public Action TimedTextAction
        {
            get { return _timedTextAction; }
            set { SetProperty(ref _timedTextAction, value, TimedTextActionPropertyName); }
        }

        private double _currentPosition;
        private const string CurrentPositionPropertyName = "CurrentPosition";

        public double CurrentPosition
        {
            get { return player.CurrentPosition; }
            set { _currentPosition = value; }
        }

        private double _duration;
        private const string DurationPropertyName = "Duration";

        public double Duration
        {
            get { return player.Duration; }
            set { SetProperty(ref _duration, value, DurationPropertyName); }
        }

        private bool _isPlaying;
        private const string IsPlayingPropertyName = "IsPlaying";

        public bool IsPlaying
        {
            get { return player.IsPlaying; }
            set { SetProperty(ref _isPlaying, value, IsPlayingPropertyName); }
        }

        public void SetVolume(float left, float right)
        {
            player.SetVolume(left, right);
        }

        protected void SetProperty<U>(
            ref U backingStore, U value,
            string propertyName,
            Action onChanged = null,
            Action<U> onChanging = null)
        {
            if (EqualityComparer<U>.Default.Equals(backingStore, value))
                return;

            onChanging?.Invoke(value);

            OnPropertyChanging(propertyName);

            backingStore = value;

            onChanged?.Invoke();

            OnPropertyChanged(propertyName);
        }

        #region INotifyPropertyChanging implementation

        public event Xamarin.Forms.PropertyChangingEventHandler PropertyChanging;

        #endregion INotifyPropertyChanging implementation

        public void OnPropertyChanging(string propertyName)
        {
            if (PropertyChanging == null)
                return;

            PropertyChanging(this, new Xamarin.Forms.PropertyChangingEventArgs(propertyName));
        }

        #region INotifyPropertyChanged implementation

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion INotifyPropertyChanged implementation

        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged == null)
                return;

            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}