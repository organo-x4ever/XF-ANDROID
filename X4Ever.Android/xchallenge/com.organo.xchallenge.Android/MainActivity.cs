
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Support.V4.App;
using com.organo.xchallenge.Droid.Services;
using Plugin.MediaManager.Forms.Android;
using System.Threading.Tasks;
using Android.Content.Res;
using Android.Util;
using Android.Views;
using com.organo.xchallenge.Notification;
using com.organo.xchallenge.Statics;
using Java.Lang;
using System;
using System.Text;
using Android.Gms.Common;
using Android.Support.V7.App;
using Android.Widget;
using com.organo.xchallenge.Droid.Renderers;
using com.organo.xchallenge.Handler;
using com.organo.xchallenge.Services;
using Firebase.Iid;
using Firebase.Messaging;
using Plugin.CurrentActivity;
using Plugin.Toasts;
using Xamarin.Forms;
using System.IO;
using System.Diagnostics;
using com.organo.xchallenge.Globals;
using Exception = Java.Lang.Exception;
using String = Java.Lang.String;

namespace com.organo.xchallenge.Droid
{
    [Activity(Label = "X4Ever", Icon = "@drawable/logo", Theme = "@style/Theme.Splash", MainLauncher = true,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : Xamarin.Forms.Platform.Android.FormsAppCompatActivity
        , ActivityCompat.IOnRequestPermissionsResultCallback
    {
        private Context _ctx;

        private static readonly string TAG = typeof(MainActivity).FullName;
        private ISecureStorage _secureStorage;
        internal static readonly string CHANNEL_ID = "com.organo.xchallenge.Droid.MyFirebaseMessagingService";
        internal static readonly int NOTIFICATION_ID = 100;
        //private static readonly string ErrorFileName = "Exception-" + DateTime.Now.ToString("s") + ".log";

        TextView msgText;

        /**
     	* Id to identify a permission request.
     	*/
        public static readonly int REQUEST_CAMERA = 0;
        public static readonly int REQUEST_READ_EXTERNAL_STORAGE = 1;
        public static readonly int REQUEST_WRITE_EXTERNAL_STORAGE = 2;
        public static readonly int REQUEST_DEVICE_KEEP_AWAKE_STORAGE = 3;

        public Context GetContext()
        {
            return _ctx;
        }

        protected override void OnCreate(Bundle bundle)
        {
            //Build.VERSION

            base.Window.RequestFeature(WindowFeatures.ActionBar);
            // Name of the MainActivity theme you had there before.
            // Or you can use global::Android.Resource.Style.ThemeHoloLight
            base.SetTheme(Resource.Style.MainTheme);

            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
            base.OnCreate(bundle);

            // Exception Handling 
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
            TaskScheduler.UnobservedTaskException += TaskSchedulerOnUnobservedTaskException;

            global::Xamarin.Forms.Forms.Init(this, bundle);
            // APP Crash Report
            //DisplayCrashReport();

            _secureStorage = DependencyService.Get<ISecureStorage>();
            //Firebase Cloud Messaging :: START
            msgText = FindViewById<TextView>(Resource.Id.msgText);

            if (Intent.Extras != null)
            {
                foreach (var key in Intent.Extras.KeySet())
                {
                    var value = Intent.Extras.GetString(key);
                    Log.Debug(TAG, "Key: {0} Value: {1}", key, value);
                }
            }

            CreateNotificationChannel();
            IsPlayServicesAvailable();
            _ctx = this;
            Firebase.FirebaseApp.InitializeApp(_ctx);

            //var logTokenButton = FindViewById<Android.Widget.Button>(Resource.Id.logTokenButton);
            //logTokenButton.Click += delegate { Log.Debug(TAG, "InstanceID token: " + FirebaseInstanceId.Instance.Token); };
            var firebaseToken = FirebaseInstanceId.Instance.Token;
            if (!string.IsNullOrEmpty(firebaseToken))
                SaveFirebaseToken(firebaseToken);
            Log.Debug(TAG, "InstanceID token: " + firebaseToken);
            // Get previous device token

            //var subscribeButton = FindViewById<Android.Widget.Button>(Resource.Id.subscribeButton);
            //subscribeButton.Click += delegate
            //{
            //    FirebaseMessaging.Instance.SubscribeToTopic("news");
            //    Log.Debug(TAG, "Subscribed to remote notifications");
            //};

            //Firebase Cloud Messaging :: END

            VideoViewRenderer.Init();
            CrossCurrentActivity.Current.Init(this, bundle);
            ImageCircleRenderer.Init();

            // Push Notification Initialization
            //Should specify android Sender Id as parameter 
            //PushNotification.Initialize<PushNotificationListener>(Keys.GOOGLE_CLOUD_MESSAGING_APIs_ID);

            //https://github.com/EgorBo/Toasts.Forms.Plugin
            DependencyService.Register<ToastNotification>(); // Register your dependency
            // If you are using Android you must pass through the activity
            ToastNotification.Init(this);

            // FOR iOS
            //ToastNotification.Init();

            // Get the message from the intent:
            string message = "";
            if (Intent.Extras != null)
                message = Intent.Extras.GetString("action", "");

            LoadApplication(new App(this.GetActivity(message)));
        }

        private Action<int, Result, Intent> _resultCallback;
        public void StartActivity(Intent intent, int requestCode, Action<int, Result, Intent> resultCallback)
        {
            _resultCallback = resultCallback;
            StartActivityForResult(intent, requestCode);
        }
        protected override void OnActivityResult (int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult (requestCode, resultCode, data);
            if (_resultCallback != null)
            {
                _resultCallback(requestCode, resultCode, data);
                _resultCallback = null;
            }
        }

        // Error/Exception Handling
        private static void TaskSchedulerOnUnobservedTaskException(object sender,
            UnobservedTaskExceptionEventArgs unobservedTaskExceptionEventArgs)
        {
            try
            {
                var newExc = new System.Exception("TaskSchedulerOnUnobservedTaskException",
                    unobservedTaskExceptionEventArgs.Exception);
                LogUnhandledException(newExc);
            }
            catch
            {
                //
            }
        }

        private static void CurrentDomainOnUnhandledException(object sender,
            UnhandledExceptionEventArgs unhandledExceptionEventArgs)
        {
            try
            {
                var newExc = new System.Exception("CurrentDomainOnUnhandledException",
                    unhandledExceptionEventArgs.ExceptionObject as System.Exception);
                LogUnhandledException(newExc);
            }
            catch
            {
                //
            }
        }

        private static void LogUnhandledException(System.Exception exception)
        {
            //var libraryPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            //// iOS: Environment.SpecialFolder.Resources
            //var errorFilePath = Path.Combine(libraryPath, ErrorFileName);
            //File.WriteAllText(errorFilePath, errorMessage);

            // Log to Android Device Logging.
            //Android.Util.Log.Error("Crash Report", errorMessage);
            // just suppress any error logging exceptions
            CollectionCrashReport(
                $"Time: {DateTime.Now}\r\nError: Unhandled Exception\r\n{exception?.ToString() ?? ""}Inner Exception: \r\n{exception?.InnerException?.ToString() ?? ""}");
        }

        //// If there is an unhandled exception, the exception information is diplayed 
        //// on screen the next time the app is started (only in debug configuration)
        ////[Conditional("DEBUG")]
        //private void DisplayCrashReport()
        //{
        //    var libraryPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
        //    var errorFilePath = Path.Combine(libraryPath, ErrorFileName);

        //    if (!File.Exists(errorFilePath))
        //    {
        //        return;
        //    }

        //    try
        //    {
        //        var errorText = File.ReadAllText(errorFilePath);
        //        new Android.App.AlertDialog.Builder(this)
        //            //.SetPositiveButton("Clear", (sender, args) => { File.Delete(errorFilePath); })
        //            .SetNegativeButton("Send & Close", (sender, args) =>
        //            {
        //                // User pressed Close.
        //                File.Delete(errorFilePath);
        //                CollectionCrashReport(errorText);
        //            })
        //            .SetMessage(errorText)
        //            .SetTitle("Crash Report")
        //            .Show();
        //    }
        //    catch (Exception ex)
        //    {
        //        CollectionCrashReport(ex);
        //    }
        //}

        private static void CollectionCrashReport(string message) => new ExceptionHandler(TAG, message);

        // Error/Exception Handling

        protected async void SaveFirebaseToken(string deviceToken)
        {
            var oldDeviceToken = "";
            var data = await _secureStorage.RetrieveAsync(Keys.DEVICE_TOKEN_IDENTITY);
            if (data != null)
                oldDeviceToken = Encoding.UTF8.GetString(data, 0, data.Length) ?? "";
            // Has the token changed?
            if (!oldDeviceToken.Equals(deviceToken))
            {
                //TODO: Put your own logic here to notify your server that the device token has changed/been created!
                // Save new device token
                await _secureStorage.StoreAsync(Keys.DEVICE_TOKEN_IDENTITY, Encoding.UTF8.GetBytes(deviceToken));
            }
        }

        /**
     	* Callback received when a permissions request has been completed.
     	*/
        public TaskCompletionSource<bool> GrantPermissionTaskCompletionSource { set; get; }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions,
            Permission[] grantResults)
        {
            if (requestCode == REQUEST_CAMERA)
            {
                // Received permission result for camera permission.
                Log.Debug(TAG, "Received response for Camera permission request.");
                // Check if the only required permission has been granted
                if (grantResults.Length == 1 && grantResults[0] == Permission.Granted)
                {
                    // Camera permission has been granted, preview can be displayed
                    Log.Debug(TAG, "CAMERA permission has now been granted. Showing preview.");
                    GrantPermissionTaskCompletionSource.SetResult(true);
                }
                else
                {
                    Log.Debug(TAG, "CAMERA permission was NOT granted.");
                    GrantPermissionTaskCompletionSource.SetResult(false);
                }
            }
            else if (requestCode == REQUEST_READ_EXTERNAL_STORAGE)
            {
                Log.Debug(TAG, "Received response for Read from External Storage permissions request.");
                // We have requested multiple permissions for Read from External Storage, so all of them need to be checked.
                if (PermissionUtil.VerifyPermissions(grantResults))
                {
                    // All required permissions have been granted, display Read from External Storage fragment.
                    GrantPermissionTaskCompletionSource.SetResult(true);
                }
                else
                {
                    Log.Debug(TAG, "Read from External Storage permissions were NOT granted.");
                    GrantPermissionTaskCompletionSource.SetResult(false);
                }
            }
            else if (requestCode == REQUEST_WRITE_EXTERNAL_STORAGE)
            {
                Log.Debug(TAG, "Received response for Write to External Storage permissions request.");
                // We have requested multiple permissions for Write to External Storage, so all of them need to be checked.
                if (PermissionUtil.VerifyPermissions(grantResults))
                {
                    // All required permissions have been granted, display Write to External Storage fragment.
                    GrantPermissionTaskCompletionSource.SetResult(true);
                }
                else
                {
                    Log.Debug(TAG, "Read to External Storage permissions were NOT granted.");
                    GrantPermissionTaskCompletionSource.SetResult(false);
                }
            }
            else if (requestCode == REQUEST_DEVICE_KEEP_AWAKE_STORAGE)
            {
                Log.Debug(TAG, "Received response for Device Keep Awake permissions request.");
                // We have requested multiple permissions for Device Keep Awake, so all of them need to be checked.
                if (PermissionUtil.VerifyPermissions(grantResults))
                {
                    // All required permissions have been granted, display Device Keep Awake fragment.
                    GrantPermissionTaskCompletionSource.SetResult(true);
                }
                else
                {
                    Log.Debug(TAG, "Device Keep Awake permissions were NOT granted.");
                    GrantPermissionTaskCompletionSource.SetResult(false);
                }
            }

            Plugin.Permissions.PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions,
                grantResults);
        }

        private string GetActivity(string action = "")
        {
            return action != null && action.Trim().Length > 0 ? action : "";
        }

        //Firebase Cloud Messaging :: START
        public bool IsPlayServicesAvailable()
        {
            var resultCode = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(this);
            if (resultCode != ConnectionResult.Success)
            {
                if (GoogleApiAvailability.Instance.IsUserResolvableError(resultCode))
                {
                    if (msgText != null)
                        msgText.Text = GoogleApiAvailability.Instance.GetErrorString(resultCode);
                }
                else
                {
                    if (msgText != null)
                        msgText.Text = "This device is not supported";
                    Finish();
                }

                return false;
            }

            if (msgText != null)
                msgText.Text = "Google Play Services is available.";
            return true;
        }

        void CreateNotificationChannel()
        {
            if (Build.VERSION.SdkInt < BuildVersionCodes.O)
            {
                // Notification channels are new in API 26 (and not a part of the
                // support library). There is no need to create a notification 
                // channel on older versions of Android.
                return;
            }

            var channel = new NotificationChannel(CHANNEL_ID, "X4Ever Notification", NotificationImportance.High)
            {
                Description = "Firebase Cloud Messages appear in this channel"
            };
            channel.EnableVibration(true);
            channel.LockscreenVisibility = NotificationVisibility.Private;
            var notificationManager = (NotificationManager)GetSystemService(NotificationService);
            notificationManager.CreateNotificationChannel(channel);
        }
        //Firebase Cloud Messaging :: END

        protected override void OnResume()
        {
            base.OnResume();
            IsPlayServicesAvailable();
        }
    }
}