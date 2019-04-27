using Android;
using Android.Content.PM;
using Android.Support.V4.App;
using com.organo.xchallenge.Droid.Permissions;
using com.organo.xchallenge.Permissions;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(DevicePermissionServices))]

namespace com.organo.xchallenge.Droid.Permissions
{
    public class DevicePermissionServices : IDevicePermissionServices
    {
        private static readonly string TAG = typeof(DevicePermissionServices).FullName;

        private MainActivity activity => Forms.Context as MainActivity;

        /*** Requests the Camera permission.
      * If the permission has been denied previously, a SnackBar will prompt the user to grant the
      * permission, otherwise it is requested directly.*/

        public async Task<bool> RequestCameraPermission()
        {
            // Check if the Camera permission is already available.
            if (ActivityCompat.CheckSelfPermission(activity, Manifest.Permission.Camera) != (int)Permission.Granted)
            {
                // Camera permission has not been granted
                //if (ActivityCompat.ShouldShowRequestPermissionRationale(activity, Manifest.Permission.Camera))
                //{
                // Provide an additional rationale to the user if the permission was not granted and
                //the user would benefit from additional context for the use of the permission.For
                //example if the user has previously denied the permission.
                //                    Log.Info(TAG, "Displaying camera permission rationale to provide additional context.");

                // Camera permission has not been granted yet. Request it directly.
                ActivityCompat.RequestPermissions(activity, new String[]
                {
                    Manifest.Permission.Camera
                }, MainActivity.REQUEST_CAMERA);

                // Save the TaskCompletionSource object as a MainActivity property
                activity.GrantPermissionTaskCompletionSource = new TaskCompletionSource<bool>();

                // Return Task object
                return await activity.GrantPermissionTaskCompletionSource.Task;
                // }
            }

            return true;
        }

        public async Task<bool> RequestDeviceKeepWakePermission()
        {
            // Check if the Camera permission is already available.
            if (ActivityCompat.CheckSelfPermission(activity, Manifest.Permission.WakeLock) !=
                (int)Permission.Granted)
            {
                // Camera permission has not been granted
                //if (ActivityCompat.ShouldShowRequestPermissionRationale(activity, Manifest.Permission.Camera))
                //{
                // Provide an additional rationale to the user if the permission was not granted and
                //the user would benefit from additional context for the use of the permission.For
                //example if the user has previously denied the permission.
                //                    Log.Info(TAG, "Displaying camera permission rationale to provide additional context.");

                // Camera permission has not been granted yet. Request it directly.
                ActivityCompat.RequestPermissions(activity, new String[]
                {
                    Manifest.Permission.WakeLock
                }, MainActivity.REQUEST_DEVICE_KEEP_AWAKE_STORAGE);

                // Save the TaskCompletionSource object as a MainActivity property
                activity.GrantPermissionTaskCompletionSource = new TaskCompletionSource<bool>();

                // Return Task object
                return await activity.GrantPermissionTaskCompletionSource.Task;
                // }
            }

            return true;
        }

        public async Task<bool> RequestReadStoragePermission()
        {
            // Check if the Camera permission is already available.
            if (ActivityCompat.CheckSelfPermission(activity, Manifest.Permission.ReadExternalStorage) !=
                (int)Permission.Granted)
            {
                // Camera permission has not been granted
                //if (ActivityCompat.ShouldShowRequestPermissionRationale(activity, Manifest.Permission.Camera))
                //{
                // Provide an additional rationale to the user if the permission was not granted and
                //the user would benefit from additional context for the use of the permission.For
                //example if the user has previously denied the permission.
                //                    Log.Info(TAG, "Displaying camera permission rationale to provide additional context.");

                // Camera permission has not been granted yet. Request it directly.
                ActivityCompat.RequestPermissions(activity, new String[]
                {
                    Manifest.Permission.ReadExternalStorage
                }, MainActivity.REQUEST_READ_EXTERNAL_STORAGE);

                // Save the TaskCompletionSource object as a MainActivity property
                activity.GrantPermissionTaskCompletionSource = new TaskCompletionSource<bool>();

                // Return Task object
                return await activity.GrantPermissionTaskCompletionSource.Task;
                // }
            }

            return true;
        }

        public async Task<bool> RequestWriteStoragePermission()
        {
            // Check if the Camera permission is already available.
            if (ActivityCompat.CheckSelfPermission(activity, Manifest.Permission.WriteExternalStorage) !=
                (int)Permission.Granted)
            {
                // Camera permission has not been granted
                //if (ActivityCompat.ShouldShowRequestPermissionRationale(activity, Manifest.Permission.Camera))
                //{
                // Provide an additional rationale to the user if the permission was not granted and
                //the user would benefit from additional context for the use of the permission.For
                //example if the user has previously denied the permission.
                //                    Log.Info(TAG, "Displaying camera permission rationale to provide additional context.");

                // Camera permission has not been granted yet. Request it directly.
                ActivityCompat.RequestPermissions(activity, new String[]
                {
                    Manifest.Permission.WriteExternalStorage
                }, MainActivity.REQUEST_WRITE_EXTERNAL_STORAGE);

                // Save the TaskCompletionSource object as a MainActivity property
                activity.GrantPermissionTaskCompletionSource = new TaskCompletionSource<bool>();

                // Return Task object
                return await activity.GrantPermissionTaskCompletionSource.Task;
                // }
            }

            return true;
        }
    }
}