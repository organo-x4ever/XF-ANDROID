using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Gms.Common;
using Android.OS;
using Android.Support.V4.App;
using Android.Util;
using Android.Views;
using Android.Widget;
using com.organo.x4ever;
using com.organo.x4ever.Droid;
using com.organo.x4ever.Droid.Renderers;
using com.organo.x4ever.Services;
using com.organo.x4ever.Statics;
using Firebase.Iid;
using Plugin.CurrentActivity;
using Plugin.MediaManager.Forms.Android;
using Plugin.Toasts;
using Xamarin.Forms;
using Resource = com.organo.x4ever.Droid.Resource;

namespace com.organo.xchallenge.Droid
{
    [Activity(Label = "X4Ever", Icon = "@drawable/logo", Theme = "@style/Theme.Splash", MainLauncher = true,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : Xamarin.Forms.Platform.Android.FormsAppCompatActivity
        , ActivityCompat.IOnRequestPermissionsResultCallback
    {
        private Context _ctx;

        private static readonly string TAG = typeof(MainActivity).FullName;
        private ISecureStorage _secureStorage;
        internal static readonly string CHANNEL_ID = "com.organo.x4ever.Droid.MyFirebaseMessagingService";
        internal static readonly int NOTIFICATION_ID = 100;

        TextView msgText;

        /**
     	* Id to identify a camera permission request.
     	*/
        public static readonly int REQUEST_CAMERA = 0;
        public static readonly int REQUEST_READ_EXTERNAL_STORAGE = 1;
        public static readonly int REQUEST_WRITE_EXTERNAL_STORAGE = 2;

        public Context GetContext()
        {
            return _ctx;
        }

        protected override void OnCreate(Bundle bundle)
        {
            //Build.VERSION
            try
            {
                base.Window.RequestFeature(WindowFeatures.ActionBar);
                // Name of the MainActivity theme you had there before.
                // Or you can use global::Android.Resource.Style.ThemeHoloLight
                base.SetTheme(Resource.Style.MainTheme);

                TabLayoutResource = Resource.Layout.Tabbar;
                ToolbarResource = Resource.Layout.Toolbar;
                base.OnCreate(bundle);
                _ctx = this;
                global::Xamarin.Forms.Forms.Init(this, bundle);
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
            catch (System.Exception ex)
            {
                try
                {
                    WriteLog.Remote(ex.ToString());
                }
                catch (System.Exception)
                {
                    //
                }
            }
        }

        private async void SaveFirebaseToken(string deviceToken)
        {
            var oldDeviceToken = "";
            var data = await _secureStorage.RetrieveAsync(Keys.DEVICE_TOKEN_IDENTITY);
            if (data != null)
                oldDeviceToken = Encoding.UTF8.GetString(data, 0, data.Length);
            // Has the token changed?
            if (string.IsNullOrEmpty(oldDeviceToken) || oldDeviceToken.Equals(deviceToken))
            {
                //TODO: Put your own logic here to notify your server that the device token has changed/been created!
                await DependencyService.Get<IUserPushTokenServices>()
                    .SaveDeviceTokenByOldToken(deviceToken, oldDeviceToken);
            }

            // Save new device token
            await _secureStorage.StoreAsync(Keys.DEVICE_TOKEN_IDENTITY, Encoding.UTF8.GetBytes(deviceToken));

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
                Log.Debug(TAG, "Received response for contact permissions request.");
                // We have requested multiple permissions for contacts, so all of them need to be checked.
                if (PermissionUtil.VerifyPermissions(grantResults))
                {
                    // All required permissions have been granted, display contacts fragment.
                    GrantPermissionTaskCompletionSource.SetResult(true);
                }
                else
                {
                    Log.Debug(TAG, "Contacts permissions were NOT granted.");
                    GrantPermissionTaskCompletionSource.SetResult(false);
                }
            }
            else if (requestCode == REQUEST_WRITE_EXTERNAL_STORAGE)
            {
                Log.Debug(TAG, "Received response for contact permissions request.");
                // We have requested multiple permissions for contacts, so all of them need to be checked.
                if (PermissionUtil.VerifyPermissions(grantResults))
                {
                    // All required permissions have been granted, display contacts fragment.
                    GrantPermissionTaskCompletionSource.SetResult(true);
                }
                else
                {
                    Log.Debug(TAG, "Contacts permissions were NOT granted.");
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
            var notificationManager = (NotificationManager) GetSystemService(NotificationService);
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