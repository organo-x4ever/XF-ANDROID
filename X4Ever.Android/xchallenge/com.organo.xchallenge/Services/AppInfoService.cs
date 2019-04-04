using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using com.organo.xchallenge.Helpers;
using com.organo.xchallenge.Models;
using com.organo.xchallenge.Services;
using Newtonsoft.Json;
using Xamarin.Forms;

[assembly:Dependency(typeof(AppInfoService))]

namespace com.organo.xchallenge.Services
{
    public class AppInfoService : IAppInfoService
    {
        public string ControllerName => "appinfo";
        readonly IDeviceInfo _deviceInfo = DependencyService.Get<IDeviceInfo>();

        public async Task<AppInfoModel> GetAsync()
        {
            var platform = _deviceInfo.GetPlatform;
            var response =
                await ClientService.SendAsync(HttpMethod.Get, ControllerName, $"get?platform={platform}");
            if (response != null && response.IsSuccessStatusCode && response.StatusCode == HttpStatusCode.OK)
            {
                Task<string> jsonTask = response.Content.ReadAsStringAsync();
                jsonTask.Wait();
                var model = JsonConvert.DeserializeObject<AppInfoModel>(jsonTask.Result);
                return model;
            }

            return null;
        }
    }
}