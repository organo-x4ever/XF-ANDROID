using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using com.organo.x4ever.Droid.Services;
using com.organo.x4ever.Services;
using Xamarin.Forms;

[assembly:Dependency(typeof(InformationMessageServices))]

namespace com.organo.xchallenge.Droid.Services
{
    public class InformationMessageServices : IInformationMessageServices
    {
        private readonly Android.Content.Context context;

        public InformationMessageServices()
        {
            context = Forms.Context;
        }

        public void LongAlert(string message)
        {
            Toast.MakeText(context, message, ToastLength.Long).Show();
        }

        public void ShortAlert(string message)
        {
            Toast.MakeText(context, message, ToastLength.Short).Show();
        }
    }
}