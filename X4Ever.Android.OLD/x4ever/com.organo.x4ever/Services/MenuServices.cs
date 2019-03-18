using com.organo.x4ever.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using com.organo.x4ever.Services;

[assembly: Xamarin.Forms.Dependency(typeof(MenuServices))]

namespace com.organo.x4ever.Services
{
    public class MenuServices : IMenuServices
    {
        public string ControllerName => "appmenus";

        public async Task<List<Menu>> GetByApplicationAsync()
        {
            var model = new List<Menu>();
            var response = await ClientService.GetByApplicationHeaderDataAsync(ControllerName, "get");
            if (response != null)
            {
                var jsonTask = response.Content.ReadAsStringAsync();
                jsonTask.Wait();
                model = JsonConvert.DeserializeObject<List<Menu>>(jsonTask.Result);
            }

            return model;
        }
    }
}