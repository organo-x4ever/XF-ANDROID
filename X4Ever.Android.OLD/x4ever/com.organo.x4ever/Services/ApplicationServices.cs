using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using com.organo.x4ever.Models;
using com.organo.x4ever.Services;
using Newtonsoft.Json;
using Xamarin.Forms;

[assembly: Dependency(typeof(ApplicationServices))]

namespace com.organo.x4ever.Services
{
    public class ApplicationServices : IApplicationServices
    {
        public string ControllerName => "applications";

        public async Task<List<ApplicationUserSelection>> GetAsync()
        {
            var models = new List<ApplicationUserSelection>();
            var response = await ClientService.SendAsync(HttpMethod.Get, ControllerName, "getasync");
            if (response != null)
            {
                var jsonResult = response.Content.ReadAsStringAsync();
                models = JsonConvert.DeserializeObject<List<ApplicationUserSelection>>(jsonResult.Result);
            }

            return models;
        }
    }
}