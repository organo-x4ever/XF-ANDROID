
using com.organo.xchallenge.Controls;
using com.organo.xchallenge.Droid.Renderers;
using System.Net;
using Xamarin.Forms.Platform.Android;
using WebView = Xamarin.Forms.WebView;
using Android.Content;
using System.IO;
using com.organo.xchallenge.Services;
using Xamarin.Forms;

[assembly: Xamarin.Forms.ExportRenderer(typeof(CustomWebView), typeof(CustomWebViewRenderer))]

namespace com.organo.xchallenge.Droid.Renderers
{
    public class CustomWebViewRenderer : WebViewRenderer
    {
        public CustomWebViewRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<WebView> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                var customWebView = Element as CustomWebView;
                Control.Settings.AllowUniversalAccessFromFileURLs = true;
                if (customWebView != null)
                {
                    var filePath = DependencyService.Get<IFileDownloadService>()
                        .GetFile(WebUtility.UrlEncode(customWebView.Uri));
                    var finalPath = string.Format($"file:///android_asset/pdfjs/web/viewer.html?file={filePath}");
                    Control.LoadUrl(finalPath);
                }
            }
        }
    }
}