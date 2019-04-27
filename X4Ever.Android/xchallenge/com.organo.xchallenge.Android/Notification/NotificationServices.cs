using Android.App;
using Android.Content;
using com.organo.xchallenge.Droid.Notification;
using com.organo.xchallenge.Notification;
using Xamarin.Forms;

[assembly: Dependency(typeof(NotificationServices))]

namespace com.organo.xchallenge.Droid.Notification
{
    public class NotificationServices : INotificationServices
    {
        private int NOTIFICATION_ID = 1;
        public void Send(string title, string message)
        {
            //Get Activity
            MainActivity activity = Forms.Context as MainActivity;
            var context = activity.Window.Context;

            // Instantiate the builder and set notification elements:
            
            Android.App.Notification.Builder notificationBuilder = new Android.App.Notification.Builder(context)
                .SetSmallIcon(Resource.Drawable.icon)
                .SetContentTitle(title)
                .SetContentText(message)
                .SetDefaults(NotificationDefaults.Sound);

            // Build the notification:
            Android.App.Notification notification = notificationBuilder.Build();

            // Turn on vibrate:
            notification.Defaults |= NotificationDefaults.Vibrate;

            //Auto cancel will remove the notification once the user touches it
            notification.Flags = NotificationFlags.AutoCancel;

            // Get the notification manager:
            NotificationManager notificationManager =
                context.GetSystemService(Context.NotificationService) as NotificationManager;

            // Publish the notification:
            notificationManager.Notify(NOTIFICATION_ID, notification);
        }

        public void Send(string title, string message, ActivityType activityType)
        {
            this.Send(title, message, activityType, NOTIFICATION_ID);
        }

        public void Send(string title, string message, ActivityType activityType, int notificationID)
        {
            MainActivity activity = Forms.Context as MainActivity;
            var context = activity.Window.Context;

            // Setup an intent for SecondActivity:
            Intent secondIntent = new Intent(context, typeof(MainActivity));

            // Pass some information to SecondActivity:
            secondIntent.PutExtra("action", activityType.ToString());

            // Create a task stack builder to manage the back stack:
            TaskStackBuilder stackBuilder = TaskStackBuilder.Create(context);

            // Add all parents of SecondActivity to the stack:
            stackBuilder.AddParentStack(Java.Lang.Class.FromType(typeof(MainActivity)));

            // Push the intent that starts SecondActivity onto the stack:
            stackBuilder.AddNextIntent(secondIntent);

            // Obtain the PendingIntent for launching the task constructed by stackbuilder. The
            // pending intent can be used only once (one shot):
            const int pendingIntentId = 0;
            PendingIntent pendingIntent =
                stackBuilder.GetPendingIntent(pendingIntentId, PendingIntentFlags.OneShot);

            // Instantiate the builder and set notification elements, including the pending intent:
            Android.App.Notification.Builder builder = new Android.App.Notification.Builder(context)
                .SetContentIntent(pendingIntent)
                .SetContentTitle(title)
                .SetContentText(message)
                .SetSmallIcon(Resource.Drawable.icon)
                .SetDefaults(NotificationDefaults.Sound);

            // Build the notification:
            Android.App.Notification notification = builder.Build();

            // Turn on vibrate:
            notification.Defaults |= NotificationDefaults.Vibrate;

            // Get the notification manager:
            NotificationManager notificationManager =
                context.GetSystemService(Context.NotificationService) as NotificationManager;

            // Publish the notification:
            const int notificationId = 0;
            notificationManager.Notify(notificationId, notification);
        }
    }
}