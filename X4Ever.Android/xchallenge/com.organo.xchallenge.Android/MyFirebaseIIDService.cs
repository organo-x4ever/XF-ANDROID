
using System;
using System.Text;
using Android.App;
using Android.Util;
using com.organo.xchallenge.Services;
using com.organo.xchallenge.Statics;
using Firebase.Iid;
using Xamarin.Forms;

namespace com.organo.xchallenge.Droid
{
    [Service]
    [IntentFilter(new[] {"com.google.firebase.INSTANCE_ID_EVENT"})]
    public class MyFirebaseIIDService : FirebaseInstanceIdService
    {
        private static readonly string TAG = typeof(MyFirebaseIIDService).FullName;

        public override void OnTokenRefresh()
        {
            var refreshedToken = FirebaseInstanceId.Instance.Token;
            Log.Debug(TAG, "Refreshed token: " + refreshedToken);
            SendRegistrationToServer(refreshedToken);
        }

        async void SendRegistrationToServer(string token)
        {
            // Add custom implementation, as needed.

            var oldDeviceToken = "";
            var data = await DependencyService.Get<ISecureStorage>().RetrieveAsync(Keys.DEVICE_TOKEN_IDENTITY);
            if (data != null)
                oldDeviceToken = Encoding.UTF8.GetString(data, 0, data.Length);
            // Has the token changed?
            if (string.IsNullOrEmpty(oldDeviceToken) || !oldDeviceToken.Equals(token))
            {
                //TODO: Put your own logic here to notify your server that the device token has changed/been created!
            }

            // Save new device token
            await DependencyService.Get<ISecureStorage>()
                .StoreAsync(Keys.DEVICE_TOKEN_IDENTITY, Encoding.UTF8.GetBytes(token));
        }
    }
}