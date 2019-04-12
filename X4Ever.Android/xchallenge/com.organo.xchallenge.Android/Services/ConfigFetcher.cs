using com.organo.xchallenge.Droid;
using com.organo.xchallenge.Services;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Linq;
using com.organo.xchallenge.Handler;
using com.organo.xchallenge.Models;
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

        public async Task<AppConfig> GetAsync()
        {
            try
            {
                var fileName = "settings.json";
                var type = this.GetType();
                var resource = type.Namespace + ".Config." + fileName;
                using (var stream = type.Assembly.GetManifestResourceStream(resource))
                    if (stream != null)
                    {
                        using (var reader = new StreamReader(stream))
                        {
                            var result = await reader.ReadToEndAsync();
                            return JsonConvert.DeserializeObject<AppConfig>(result);
                        }
                    }
            }
            catch (System.Exception ex)
            {
                var exceptionHandler = new ExceptionHandler("ConfigFetcher", ex);
            }

            return null;
        }

        #endregion IConfigFetcher implementation
    }
}