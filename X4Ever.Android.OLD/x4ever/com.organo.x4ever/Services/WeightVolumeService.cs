using com.organo.x4ever.Models;
using com.organo.x4ever.Services;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(WeightVolumeService))]

namespace com.organo.x4ever.Services
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