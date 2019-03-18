using com.organo.xchallenge.Models;
using com.organo.xchallenge.Services;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(WeightVolumeService))]

namespace com.organo.xchallenge.Services
{
    public sealed class WeightVolumeService : IWeightVolumeService
    {
        public string ControllerName => "weight_volumes";

        public async Task<List<WeightVolume>> GetAsync()
        {
            var model = new List<WeightVolume>();
            var response = await ClientService.GetByApplicationHeaderDataAsync(ControllerName, "get");
            if (response != null)
            {
                var jsonTask = response.Content.ReadAsStringAsync();
                jsonTask.Wait();
                model = JsonConvert.DeserializeObject<List<WeightVolume>>(jsonTask.Result);
            }

            return model;
        }
    }
}