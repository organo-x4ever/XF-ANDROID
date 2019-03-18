using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace com.organo.xchallenge.Controls
{
    public class CustomWebView : WebView
    {
        //public static readonly BindableProperty UriProperty =
        //    BindableProperty.Create<CustomWebView, string>(p => p.Uri, default(string));

        public static readonly BindableProperty UriProperty =
            BindableProperty.Create(UriPropertyName, typeof(string), typeof(string), string.Empty);

        public const string UriPropertyName = "Uri";
        public string Uri
        {
            get { return (string) GetValue(UriProperty); }
            set { SetValue(UriProperty, value); }
        }
    }
}