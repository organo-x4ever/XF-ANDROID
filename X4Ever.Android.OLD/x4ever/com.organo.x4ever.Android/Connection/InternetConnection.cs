using Android.Content;
using Android.Net;
using com.organo.x4ever.Connection;
using com.organo.x4ever.Droid.Connection;
using System.Threading.Tasks;
using com.organo.xchallenge.Droid;
using Xamarin.Forms;

[assembly: Dependency(typeof(InternetConnection))]

namespace com.organo.x4ever.Droid.Connection
{
    public class InternetConnection : IInternetConnection
    {
        public Context Context { get; set; }
        private NetworkInfo networkInfo;

        public InternetConnection()
        {
            //Get Activity
            MainActivity activity = Forms.Context as MainActivity;
            this.Context = activity.Window.Context;
        }

        public async Task<bool> CheckAsync()
        {
            await Task.Run(() =>
            {
                ConnectivityManager manager =
                    (ConnectivityManager)this.Context.GetSystemService(Context.ConnectivityService);
                networkInfo = manager.ActiveNetworkInfo;
            });
            return networkInfo != null ? networkInfo.IsConnectedOrConnecting : false;
        }

        public bool Check()
        {
            ConnectivityManager manager =
                (ConnectivityManager)this.Context.GetSystemService(Context.ConnectivityService);
            NetworkInfo ni = manager.ActiveNetworkInfo;
            return ni != null ? ni.IsConnectedOrConnecting : false;
        }
    }
}