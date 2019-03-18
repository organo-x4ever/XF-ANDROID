using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.organo.xchallenge.Models;
using com.organo.xchallenge.Services;
using Newtonsoft.Json;
using Xamarin.Forms;

[assembly: Dependency(typeof(ProvinceServices))]

namespace com.organo.xchallenge.Services
{
    internal class ProvinceServices : IProvinceServices
    {
        public string ControllerName => "provinces";

        public async Task<List<CountryProvince>> GetAsync()
        {
            var model = new List<CountryProvince>();
            var response = await ClientService.GetByApplicationHeaderDataAsync(ControllerName, "get");
            if (response != null)
            {
                var jsonTask = response.Content.ReadAsStringAsync();
                jsonTask.Wait();
                model = JsonConvert.DeserializeObject<List<CountryProvince>>(jsonTask.Result);
                return model;
            }

            return null;
        }
    }
}