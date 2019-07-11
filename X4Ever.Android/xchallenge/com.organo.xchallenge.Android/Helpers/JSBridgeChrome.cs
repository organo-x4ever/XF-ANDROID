
using Android.Webkit;
using com.organo.xchallenge.Droid.Renderers;
using Java.Interop;
using System;

namespace com.organo.xchallenge.Droid.Helpers
{
    public class JSBridgeChrome : Java.Lang.Object
    {
        readonly WeakReference<HybridChromeWebViewRenderer> hybridChromeWebViewRenderer;

        public JSBridgeChrome(HybridChromeWebViewRenderer hybridRenderer)
        {
            hybridChromeWebViewRenderer = new WeakReference<HybridChromeWebViewRenderer>(hybridRenderer);
        }

        [JavascriptInterface]
        [Export("invokeAction")]
        public void InvokeAction(string data)
        {
            HybridChromeWebViewRenderer hybridRenderer;

            if (hybridChromeWebViewRenderer != null && hybridChromeWebViewRenderer.TryGetTarget(out hybridRenderer))
            {
                hybridRenderer.Element.InvokeAction(data);
            }
        }
    }
}