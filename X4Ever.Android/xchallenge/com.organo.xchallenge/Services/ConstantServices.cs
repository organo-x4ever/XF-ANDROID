
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using com.organo.xchallenge.Services;
using com.organo.xchallenge.Helpers;
using com.organo.xchallenge.Statics;
using System.Net.Http;

[assembly:Dependency(typeof(ConstantServices))]

namespace com.organo.xchallenge.Services
{
    public class ConstantServices : IConstantServices
    {
        private static readonly IDeviceInfo DeviceInfo = DependencyService.Get<IDeviceInfo>();
        public async Task<string> Blogs() =>
            await ClientService.GetStringAsync(new Uri(ClientService.GetRequestUri("constants",
                $"blogs?region={App.Configuration.GetApplication()}&lang={App.Configuration?.AppConfig.DefaultLanguage}")));

        public async Task<string> MoreWebLinks() => await ClientService.GetStringAsync(new Uri(ClientService.GetRequestUri("constants",
                $"more_links_path" +
            $"?{App.Configuration?.AppConfig.ApplicationRequestHeader}={App.Configuration?.GetApplication()}" +
            $"&{HttpConstants.REQUEST_HEADER_LANGUAGE}={App.Configuration?.AppConfig.DefaultLanguage}" +
            $"&{HttpConstants.VERSION}={App.Configuration?.AppConfig.ApplicationVersion}" +
            $"&{HttpConstants.PLATFORM}={DeviceInfo.GetPlatform}")));
        
        public async Task<bool> TrackerSkipPhotos()
        {
            var response = await ClientService.SendAsync(HttpMethod.Get, "constants", "trackerskipphotoonsteps");
            if (response != null)
                {
                var json = response.Content.ReadAsStringAsync();
                return json.Result.ToLower().Contains("yes");
            }
            return false;

            //var response = await ClientService.GetStringAsync(new Uri(ClientService.GetRequestUri("constants", "trackerskipphotoonsteps")));
            //if (response != null)
            //    return response.ToLower().Contains("yes");
            //return false;
        }

        public async Task<string> WeightLoseWarningPercentile() => await ClientService.GetStringAsync(new Uri(ClientService.GetRequestUri("constants", $"weightlosewarningpercentile")));
    }
}