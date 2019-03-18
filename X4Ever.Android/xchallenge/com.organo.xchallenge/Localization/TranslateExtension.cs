using System;
using System.Globalization;
using System.Reflection;
using System.Resources;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace com.organo.xchallenge.Localization
{
    [ContentProperty("Text")]
    public class TranslateExtension : IMarkupExtension
    {
        private readonly CultureInfo ci;
        private const string ResourceId = "com.organo.xchallenge.Localization.TextResources";
        private readonly ILocalize _localize;

        public TranslateExtension()
        {
            _localize = DependencyService.Get<ILocalize>();
            if (App.Configuration?.LanguageInfo == null)
                App.Configuration.LanguageInfo = _localize.GetCurrentCultureInfo("en-US");
            TextResources.Culture = App.Configuration.LanguageInfo;
            ci = App.Configuration.LanguageInfo;
        }

        public string Text { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Text == null)
                return "";

            ResourceManager resmgr = new ResourceManager(ResourceId, typeof(TranslateExtension).GetTypeInfo().Assembly);

            var translation = resmgr.GetString(Text, ci);

            if (translation == null)
            {
#if DEBUG
                throw new ArgumentException(
                    String.Format("Key '{0}' was not found in resources '{1}' for culture '{2}'.", Text, ResourceId,
                        ci.Name), "Text");
#else
                translation = Text; // HACK: returns the key, which GETS DISPLAYED TO THE USER
#endif
            }

            return translation;
        }
    }
}