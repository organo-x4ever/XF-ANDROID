using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.organo.xchallenge.Services;
using Xamarin.Forms;

namespace com.organo.xchallenge.Statics
{
    public static class WriteLog
    {
        private static readonly ILogServices LogServices = DependencyService.Get<ILogServices>();

        public static void Write(string message, bool remote = false)
        {
            Debug.WriteLine(DateWithMiliseconds + " " + Tag + " " + message);
            if (remote)
                Remote(message);
        }

        public static async void Remote(string message)
        {
            await LogServices.WriteDebug(message);
        }

        //2018-08-27 08:43:01.297
        private static string DateWithMiliseconds =>
            DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss." + DateTime.Now.Millisecond);

        private static string Tag => "Organo.Debug";
    }
}
