using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using PushNotification.Plugin;
using PushNotification.Plugin.Abstractions;

namespace com.organo.x4ever.Notification
{
    public class XPushNotificationListener : IPushNotificationListener
    {
        //Fires when error
        public void OnError(string message, DeviceType deviceType)
        {
            Debug.WriteLine(string.Format("Push notification error - {0}", message));
        }

        //Here you will receive all push notification messages
        //Messages arrives as a dictionary, the device type is also sent in order to check specific keys correctly depending on the platform.
        public void OnMessage(JObject parameters, DeviceType deviceType)
        {
            Debug.WriteLine("Message Arrived");
        }

        //Gets the registration token after push registration
        public void OnRegistered(string Token, DeviceType deviceType)
        {
            Debug.WriteLine(string.Format("Push Notification - Device Registered - Token : {0}", Token));
        }

        //Fires when device is unregistered
        public void OnUnregistered(DeviceType deviceType)
        {
            Debug.WriteLine("Push Notification - Device Unnregistered");

        }

        //Enable/Disable Showing the notification
        public bool ShouldShowNotification()
        {
            return true;
        }
    }
}