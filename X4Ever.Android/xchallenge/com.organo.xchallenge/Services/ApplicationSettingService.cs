using com.organo.xchallenge.Models;
using com.organo.xchallenge.Services;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using com.organo.xchallenge.Extensions;
using Xamarin.Forms;

[assembly: Dependency(typeof(ApplicationSettingService))]

namespace com.organo.xchallenge.Services
{
    public class ApplicationSettingService : IApplicationSettingService
    {
        public string ControllerName => "applicationsettings";

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