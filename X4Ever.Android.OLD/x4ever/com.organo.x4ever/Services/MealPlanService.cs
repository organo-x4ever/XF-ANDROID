using com.organo.x4ever.Models;
using com.organo.x4ever.Services;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(MealPlanService))]

namespace com.organo.x4ever.Services
{
    public sealed class MealPlanService : IMealPlanService
    {
        public string ControllerName => "mealplans";

        public async Task<List<MealPlanDetail>> GetDetailAsync()
        {
            var model = new List<MealPlanDetail>();
            var response = await ClientService.GetDataAsync(ControllerName, "getdetails");
            if (response != null)
            {
                var jsonTask = response.Content.ReadAsStringAsync();
                jsonTask.Wait();
                model = JsonConvert.DeserializeObject<List<MealPlanDetail>>(jsonTask.Result);
            }

            return model;
        }

        public async Task<List<MealPlanDetail>> GetDetailAsync(bool active)
        {
            var model = new List<MealPlanDetail>();
            var response = await ClientService.GetDataAsync(ControllerName, "getdetails?active=" + active);
            if (response != null)
            {
                var jsonTask = response.Content.ReadAsStringAsync();
                jsonTask.Wait();
                model = JsonConvert.DeserializeObject<List<MealPlanDetail>>(jsonTask.Result);
            }

            return model;
        }
    }
}