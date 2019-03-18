using com.organo.x4ever.Droid;
using com.organo.x4ever.Services;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Linq;
using Android.OS;
using Java.Lang;
using Newtonsoft.Json;
using Xamarin.Forms;

[assembly: Dependency(typeof(ConfigFetcher))]

namespace com.organo.xchallenge.Droid
{
    /// <summary>
    /// Fetches settings from embedded resources in the Android project.
    /// </summary>
    public class ConfigFetcher : IConfigFetcher
    {
        #region IConfigFetcher implementation

        public async Task<string> GetAsync(string configElementName, bool readFromSensitiveConfig = false)
        {
            //try
            //{
            //    var fileName = "settings.json";
            //    var type = this.GetType();
            //    var resource = type.Namespace + ".Config." + fileName;
            //    using (var stream = type.Assembly.GetManifestResourceStream(resource))
            //        if (stream != null)
            //        {
            //            using (var reader = new StreamReader(stream))
            //            {
            //                var jsonTask = reader.ReadToEndAsync();
            //                jsonTask.Wait();
            //                var model = JsonConvert.DeserializeObject<JsonData>(jsonTask.Result);
            //            }
            //        }
            //}
            //catch (Exception ex)
            //{
            //}

            try
            {
                var fileName = (readFromSensitiveConfig) ? "config-sensitive.xml" : "config.xml";
                var type = this.GetType();
                var resource = type.Namespace + ".Config." + fileName;
                using (var stream = type.Assembly.GetManifestResourceStream(resource))
                    if (stream != null)
                    {
                        using (var reader = new StreamReader(stream))
                        {
                            var doc = XDocument.Parse(await reader.ReadToEndAsync());
                            return doc.Element("config").Element(configElementName)?.Value;
                        }
                    }
            }
            catch
            {
            }

            return "";
        }

        public string GetPictureDirectory()
        {
            return Environment.DirectoryPictures;
        }

        #endregion IConfigFetcher implementation
    }

    public class JsonData
    {
        public string tokenHeaderName { get; set; }
        public string tokenExpiryHeaderName { get; set; }
        public string videoUrl { get; set; }
        public string audioUrl { get; set; }
        public string fileUploadUrl { get; set; }
        public string userImageUrl { get; set; }
        public string documentUrl { get; set; }
        public string defaultLanguage { get; set; }
        public string defaultWeightVolume { get; set; }
        public double weightSubmitInterval { get; set; }
        public string weightSubmitIntervalType { get; set; }
        public double TARGET_DATE_CALCULATION { get; set; }
        public string TESTIMONIAL_PHOTO_URL { get; set; }
        public string TESTIMONIAL_VIDEO_URL { get; set; }
    }
}