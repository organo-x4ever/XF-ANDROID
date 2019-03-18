using com.organo.x4ever.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace com.organo.x4ever.Services
{
    public sealed class MilestonePercentageService : IMilestonePercentageService
    {
        public string ControllerName => "milestonepercentages";

        public async Task<List<MilestonePercentage>> GetByLanguageAsync(string languageCode)
        {
            var model = new List<MilestonePercentage>();
            var response =
                await ClientService.GetDataAsync(ControllerName, "getbylangauge/?languageCode=" + languageCode);
            if (response != null)
            {
                var jsonTask = response.Content.ReadAsStringAsync();
                jsonTask.Wait();
                model = JsonConvert.DeserializeObject<List<MilestonePercentage>>(jsonTask.Result);
                return model;
            }

            return null;
        }
    }
}