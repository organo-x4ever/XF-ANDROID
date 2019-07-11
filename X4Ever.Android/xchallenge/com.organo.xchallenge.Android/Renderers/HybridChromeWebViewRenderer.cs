
using Android.Content;
using Android.Webkit;
using com.organo.xchallenge.Controls;
using com.organo.xchallenge.Droid.Helpers;
using com.organo.xchallenge.Droid.Renderers;
using Xamarin.Forms.Platform.Android;

[assembly: Xamarin.Forms.ExportRenderer(typeof(HybridChromeWebView), typeof(HybridChromeWebViewRenderer))]
namespace com.organo.xchallenge.Droid.Renderers
{
    public class HybridChromeWebViewRenderer : ViewRenderer<HybridChromeWebView, WebView>
    {
        Context _context;

        public HybridChromeWebViewRenderer(Context context) : base(context)
        {
            _context = context;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<HybridChromeWebView> e)
        {
            base.OnElementChanged(e);

            if (Control == null)
            {
                var webView = new WebView(_context);
                webView.Settings.JavaScriptEnabled = true;
                webView.Settings.JavaScriptCanOpenWindowsAutomatically = true;
                webView.Settings.AllowFileAccess = true;
                webView.Settings.AllowContentAccess = true;
                webView.SetWebChromeClient(new ChromeWebViewClient(_context));
                SetNativeControl(webView);
            }
            if (e.OldElement != null)
            {
                Control.RemoveJavascriptInterface("jsBridgeChrome");
                Control.Settings.JavaScriptEnabled = true;
                Control.Settings.JavaScriptCanOpenWindowsAutomatically = true;
                Control.Settings.AllowFileAccess = true;
                Control.Settings.AllowContentAccess = true;
                var hybridWebView = e.OldElement as HybridChromeWebView;
                hybridWebView.Cleanup();
            }
            if (e.NewElement != null)
            {
                Control.AddJavascriptInterface(new JSBridgeChrome(this), "jsBridgeChrome");
                Control.Settings.JavaScriptEnabled = true;
                Control.Settings.JavaScriptCanOpenWindowsAutomatically = true;
                Control.Settings.AllowFileAccess = true;
                Control.Settings.AllowContentAccess = true;
                Control.LoadUrl(Element.Uri);
            }
        }
    }
}