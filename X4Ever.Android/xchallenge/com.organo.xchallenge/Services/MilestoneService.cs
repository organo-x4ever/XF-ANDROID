
using com.organo.xchallenge.Services;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using com.organo.xchallenge.Models;
using Xamarin.Forms;

[assembly: Dependency(typeof(MilestoneService))]

namespace com.organo.xchallenge.Services
{
    public class MilestoneService : IMilestoneService
    {
        public string ControllerName => "milestones";

        public async Task<List<Models.Milestone>> GetMilestoneAsync()
        {
            var model = new List<Models.Milestone>();
            var response = await ClientService.GetDataAsync(ControllerName, "get");
            if (response != null)
            {
                var jsonTask = response.Content.ReadAsStringAsync();
                jsonTask.Wait();
                model = JsonConvert.DeserializeObject<List<Milestone>>(jsonTask.Result);
                return model;
            }

            return null;
        }
    }
}