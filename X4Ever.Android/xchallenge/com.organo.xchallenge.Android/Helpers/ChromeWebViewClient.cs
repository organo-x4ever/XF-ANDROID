
using Android.Webkit;
using Android.Content;
using Android.App;

namespace com.organo.xchallenge.Droid.Helpers
{
    public class ChromeWebViewClient : WebChromeClient
    {
        Context _context;
        public ChromeWebViewClient(Context context)
        {
            _context = context;
        }

        int FILECHOOSER_RESULTCODE = 1001;
        private IValueCallback mUploadMessage;
        private void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            if (data != null)
            {
                if (requestCode == FILECHOOSER_RESULTCODE)
                {
                    if (null == mUploadMessage || data == null)
                        return;
                    mUploadMessage.OnReceiveValue(WebChromeClient.FileChooserParams.ParseResult((int)resultCode, data));
                    mUploadMessage = null;
                }
            }
        }

        [Android.Runtime.Register("onShowFileChooser", "(Landroid/webkit/WebView;Landroid/webkit/ValueCallback;Landroid/webkit/WebChromeClient$FileChooserParams;)Z", "GetOnShowFileChooser_Landroid_webkit_WebView_Landroid_webkit_ValueCallback_Landroid_webkit_WebChromeClient_FileChooserParams_Handler")]
        public override bool OnShowFileChooser(Android.Webkit.WebView webView, IValueCallback filePathCallback, FileChooserParams fileChooserParams)
        {
            var appActivity = _context as MainActivity;
            mUploadMessage = filePathCallback;
            Intent chooserIntent = fileChooserParams.CreateIntent();
            appActivity.StartActivity(chooserIntent, FILECHOOSER_RESULTCODE, OnActivityResult);
            //return base.OnShowFileChooser (webView, filePathCallback, fileChooserParams);
            return true;
        }
    }
}