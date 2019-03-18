using com.organo.x4ever.Models;
using com.organo.x4ever.Services;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(ApplicationSettingService))]

namespace com.organo.x4ever.Services
{
    public class ApplicationSettingService : IApplicationSettingService
    {
        public string ControllerName => "applicationsettings";

        public ApplicationSetting Get()
        {
            var model = new ApplicationSetting();
            var response = ClientService.SendAsync(HttpMethod.Get, ControllerName, "get");
            var result = response.Result;
            if (result != null && result.StatusCode == HttpStatusCode.OK)
            {
                var jsonTask = result.Content.ReadAsStringAsync();
                jsonTask.Wait();
                model = JsonConvert.DeserializeObject<ApplicationSetting>(jsonTask.Result);
            }

            return model;
        }

        public async Task<ApplicationSetting> GetAsync()
        {
            var model = new ApplicationSetting();
            var response = await ClientService.SendAsync(HttpMethod.Get, ControllerName, "getasync");
            if (response != null && response.StatusCode == HttpStatusCode.OK)
            {
                var jsonTask = response.Content.ReadAsStringAsync();
                jsonTask.Wait();
                model = JsonConvert.DeserializeObject<ApplicationSetting>(jsonTask.Result);
            }

            return model;
        }
    }
}