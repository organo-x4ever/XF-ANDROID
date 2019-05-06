//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//using Android.App;
//using Android.Content;
//using Android.OS;
//using Android.Runtime;
//using Android.Views;
//using Android.Widget;

//using com.organo.xchallenge;
//using com.organo.xchallenge.Droid;
//using Xamarin.Forms;

//[assembly: Xamarin.Forms.Dependency(typeof(AudioPlayerManagerOnBackgound))]

//namespace com.organo.xchallenge
//{
//    /// <summary>
//    /// Cross platform AudioPlayer implemenations
//    /// </summary>
//#if __ANDROID__
//    [Android.Runtime.Preserve(AllMembers = true)]
//#endif
//    public class AudioPlayerManagerOnBackgound : IAudioPlayerManager
//    {
//        static Lazy<IAudioPlayer> Implementation = new Lazy<IAudioPlayer>(() => CreateAudioPlayer(),
//            System.Threading.LazyThreadSafetyMode.PublicationOnly);

//        private static Intent serviceToStart;

//        public AudioPlayerManagerOnBackgound()
//        {
//            StartService();
//        }

//        private static IAudioPlayer Current
//        {
//            get
//            {
//                var ret = Implementation.Value;
//                if (ret == null)
//                {
//                    throw NotImplementedInReferenceAssembly();
//                }

//                return ret;
//            }
//        }

//        public static void StartService()
//        {
//            // Get the MainActivity instance
//            MainActivity activity = Forms.Context as MainActivity;
//            serviceToStart = new Intent(activity, typeof(AudioPlayerBackgroundService));
//            activity.StartService(serviceToStart);
//        }

//        public void StopService()
//        {
//            // Get the MainActivity instance
//            MainActivity activity = Forms.Context as MainActivity;
//            serviceToStart = new Intent(activity, typeof(AudioPlayerBackgroundService));
//            activity.StopService(serviceToStart);
//        }

//        public IAudioPlayer CurrentPlayer => Current;

//        ///<Summary>
//        /// Create a new AudioPlayer object
//        ///</Summary>
//        internal static IAudioPlayer CreateAudioPlayer()
//        {
//#if NETSTANDARD1_0
//            return null;
//#else
//            return new AudioPlayerBackgroundService();
//#endif
//        }

//        internal static Exception NotImplementedInReferenceAssembly()
//        {
//            return new NotImplementedException(
//                "This functionality is not implemented in the .NET standard version of this assembly. Reference the NuGet package from your platform-specific (head) application project in order to reference the platform-specific implementation.");
//        }


//        bool isDisposed = false;

//        ///<Summary>
//        /// Dispose SimpleAudioPlayer and release resources
//        ///</Summary>
//        protected virtual void Dispose(bool disposing)
//        {
//            if (isDisposed)
//                return;

//            if (disposing)
//                StopService();

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