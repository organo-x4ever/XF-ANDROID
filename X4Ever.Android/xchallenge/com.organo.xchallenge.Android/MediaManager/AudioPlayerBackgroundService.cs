
//using System;
//using Android.App;
//using Android.Util;
//using Android.Content;
//using System.Threading;
//using System.IO;

//using Android.Content.Res;
//using Android.OS;
//using com.organo.xchallenge.Services;
//using Environment = System.Environment;
//using Uri = Android.Net.Uri;

//namespace com.organo.xchallenge
//{
//    /// <summary>
//    /// Implementation for Feature
//    /// </summary>
//    [Android.Runtime.Preserve(AllMembers = true)]
//    [Service]
//    public class AudioPlayerBackgroundService : Service, IAudioPlayer
//    {
//        private static readonly string TAG = typeof(AudioPlayerBackgroundService).FullName;
//        bool isStarted;

//        public AudioPlayerBackgroundService()
//        {
//            Log.Info(TAG, "OnCreate: the service is initializing.");
//            player = new Android.Media.MediaPlayer() {Looping = Loop};
//            player.Completion += OnPlaybackEnded;
//            player?.SetWakeMode(context, WakeLockFlags.ScreenDim);
//        }

//        /// <summary>
//        /// Instantiates a new AudioPlayer
//        /// </summary>
//        public override void OnCreate()
//        {
//            base.OnCreate();
//        }

//        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
//        {
//            if (isStarted)
//            {
//                Log.Info(TAG, "OnStartCommand: This service has already been started.");
//            }
//            else
//            {
//                Log.Info(TAG, "OnStartCommand: The service is starting.");
//                isStarted = true;
//            }

//            // This tells Android not to restart the service if it is killed to reclaim resources.
//            return StartCommandResult.Sticky;
//        }

//        public override Android.OS.IBinder OnBind(Intent intent)
//        {
//            // Return null because this is a pure started service. A hybrid service would return a binder that would
//            // allow access to the GetFormattedStamp() method.
//            return null;
//        }

//        public override void OnDestroy()
//        {
//            // We need to shut things down.
//            //Log.Debug(TAG, GetFormattedTimestamp());
//            Log.Info(TAG, "OnDestroy: The started service is shutting down.");

//            // Stop the handler.
//            //handler.RemoveCallbacks(runnable);

//            // Remove the notification from the status bar.
//            //var notificationManager = (NotificationManager)GetSystemService(NotificationService);
//            //notificationManager.Cancel(NOTIFICATION_ID);

//            //timestamper = null;
            
//            Dispose(false);

//            base.OnDestroy();
//        }


//        ///<Summary>
//        /// Raised when audio playback completes successfully 
//        ///</Summary>
//        public event EventHandler PlaybackEnded;

//        Android.Media.MediaPlayer player;

//        private Context context => Android.App.Application.Context;

//        static int index = 0;

//        ///<Summary>
//        /// Length of audio in seconds
//        ///</Summary>
//        public double Duration
//        {
//            get { return player == null ? 0 : ((double) player.Duration) / 1000.0; }
//        }

//        ///<Summary>
//        /// Current position of audio playback in seconds
//        ///</Summary>
//        public double CurrentPosition
//        {
//            get { return player == null ? 0 : ((double) player.CurrentPosition) / 1000.0; }
//        }

//        ///<Summary>
//        /// Playback volume (0 to 1)
//        ///</Summary>
//        public double Volume
//        {
//            get { return _volume; }
//            set { SetVolume(_volume = value, Balance); }
//        }

//        double _volume = 0.5;

//        ///<Summary>
//        /// Balance left/right: -1 is 100% left : 0% right, 1 is 100% right : 0% left, 0 is equal volume left/right
//        ///</Summary>
//        public double Balance
//        {
//            get { return _balance; }
//            set { SetVolume(Volume, _balance = value); }
//        }

//        double _balance = 0;

//        ///<Summary>
//        /// Indicates if the currently loaded audio file is playing
//        ///</Summary>
//        public bool IsPlaying
//        {
//            get { return player == null ? false : player.IsPlaying; }
//        }

//        ///<Summary>
//        /// Continously repeats the currently playing sound
//        ///</Summary>
//        public bool Loop
//        {
//            get { return _loop; }
//            set
//            {
//                _loop = value;
//                if (player != null) player.Looping = _loop;
//            }
//        }

//        bool _loop;

//        ///<Summary>
//        /// Indicates if the position of the loaded audio file can be updated
//        ///</Summary>
//        public bool CanSeek
//        {
//            get { return player == null ? false : true; }
//        }

//        string path;
        
//        ///<Summary>
//        /// Load wav or mp3 audio file as a stream
//        ///</Summary>
//        public bool Load(System.IO.Stream audioStream)
//        {
//            player.Reset();

//            DeleteFile(path);

//            //cache to the file system
//            path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), $"cache{index++}.wav");
//            var fileStream = File.Create(path);
//            audioStream.CopyTo(fileStream);
//            fileStream.Close();

//            try
//            {
//                player.SetDataSource(path);
//            }
//            catch
//            {
//                try
//                {
//                    player?.SetDataSource(context, Uri.Parse(Uri.Encode(path)));
//                }
//                catch
//                {
//                    return false;
//                }
//            }

//            return PreparePlayer();
//        }

//        ///<Summary>
//        /// Load wav or mp3 audio file from the iOS Resources folder
//        ///</Summary>
//        public bool Load(string fileName, bool fullPath = true)
//        {
//            player.Reset();

//            if (fullPath)
//            {
//                var file = new Java.IO.File(fileName);
//                player?.SetDataSource(context, Uri.FromFile(file));
//            }
//            else
//            {
//                AssetFileDescriptor afd = Android.App.Application.Context.Assets.OpenFd(fileName);
//                player?.SetDataSource(afd.FileDescriptor, afd.StartOffset, afd.Length);
//            }

//            return PreparePlayer();
//        }

//        bool PreparePlayer()
//        {
//            player?.Prepare();

//            return (player == null) ? false : true;
//        }

//        void DeletePlayer()
//        {
//            Stop();

//            if (player != null)
//            {
//                player.Completion -= OnPlaybackEnded;
//                player.Release();
//                player.Dispose();
//                player = null;
//            }

//            DeleteFile(path);
//            path = string.Empty;
//        }

//        void DeleteFile(string path)
//        {
//            if (string.IsNullOrWhiteSpace(path) == false)
//            {
//                try
//                {
//                    File.Delete(path);
//                }
//                catch
//                {
//                }
//            }
//        }

//        ///<Summary>
//        /// Begin playback or resume if paused
//        ///</Summary>
//        public void Play()
//        {
//            if (player == null)
//                return;

//            if (player.IsPlaying)
//            {
//                player.Pause();
//                player.SeekTo(0);
//            }

//            player.Start();
//        }

//        ///<Summary>
//        /// Stop playack and set the current position to the beginning
//        ///</Summary>
//        public void Stop()
//        {
//            player?.Pause();
//            player?.SeekTo(0);
//        }

//        ///<Summary>
//        /// Pause playback if playing (does not resume)
//        ///</Summary>
//        public void Pause()
//        {
//            player?.Pause();
//        }

//        ///<Summary>
//        /// Set the current playback position (in seconds)
//        ///</Summary>
//        public void Seek(double position)
//        {
//            player?.SeekTo((int) position * 1000);
//        }

//        ///<Summary>
//        /// Sets the playback volume as a double between 0 and 1
//        /// Sets both left and right channels
//        ///</Summary>
//        void SetVolume(double volume, double balance)
//        {
//            volume = Math.Max(0, volume);
//            volume = Math.Min(1, volume);

//            balance = Math.Max(-1, balance);
//            balance = Math.Min(1, balance);

//            // Using the "constant power pan rule." See: http://www.rs-met.com/documents/tutorials/PanRules.pdf
//            var left = Math.Cos((Math.PI * (balance + 1)) / 4) * volume;
//            var right = Math.Sin((Math.PI * (balance + 1)) / 4) * volume;

//            player?.SetVolume((float) left, (float) right);
//        }

//        void OnPlaybackEnded(object sender, EventArgs e)
//        {
//            PlaybackEnded?.Invoke(sender, e);

//            //this improves stability on older devices but has minor performance impact
//            if (Android.OS.Build.VERSION.SdkInt < Android.OS.BuildVersionCodes.M)
//            {
//                player.SeekTo(0);
//                player.Stop();
//                player.Prepare();
//            }
//        }

//        bool isDisposed = false;

//        ///<Summary>
//        /// Dispose SimpleAudioPlayer and release resources
//        ///</Summary>
//        protected virtual void Dispose(bool disposing)
//        {
//            isStarted = false;
//            if (isDisposed || player == null)
//                return;

//            if (disposing)
//                DeletePlayer();

//            isDisposed = true;
//        }

//        ///<Summary>
//        /// Dispose SimpleAudioPlayer and release resources
//        ///</Summary>
//        public void Dispose()
//        {
//            Dispose(true);

//            GC.SuppressFinalize(this);
//        }
//    }
//}