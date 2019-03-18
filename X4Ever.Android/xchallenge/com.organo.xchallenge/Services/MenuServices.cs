using System;
using com.organo.xchallenge.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using com.organo.xchallenge.Services;

[assembly: Xamarin.Forms.Dependency(typeof(MenuServices))]

namespace com.organo.xchallenge.Services
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
                var jsonTask = await response.Content.ReadAsStringAsync();
                if (jsonTask != null)
                    model = JsonConvert.DeserializeObject<List<Menu>>(jsonTask);
            }

            return model;
        }
    }
}