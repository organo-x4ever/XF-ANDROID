using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.organo.x4ever.Services;
using Xamarin.Forms;

namespace com.organo.x4ever.Statics
{
    public static class TimerMessage
    {
        public static void ShortMessage(string message)
        {
            DependencyService.Get<IInformationMessageServices>().ShortAlert(message);
        }

        public static void LongMessage(string message)
        {
            DependencyService.Get<IInformationMessageServices>().LongAlert(message);
        }
    }
}