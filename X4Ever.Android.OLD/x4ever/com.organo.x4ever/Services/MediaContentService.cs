using com.organo.x4ever.Models.Media;
using com.organo.x4ever.Services;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(MediaContentService))]

namespace com.organo.x4ever.Services
{
    public sealed class MediaContentService : IMediaContentService, IBaseService
    {
        public string ControllerName => "mediacontents";

        public async Task<List<MediaContent>> GetAsync()
        {
            var model = new List<MediaContent>();
            var response = await ClientService.GetDataAsync(ControllerName, "get");
            if (response != null)
            {
                var jsonTask = response.Content.ReadAsStringAsync();
                jsonTask.Wait();
                model = JsonConvert.DeserializeObject<List<MediaContent>>(jsonTask.Result);
            }

            return model;
        }

        public async Task<List<MediaContentDetail>> GetDetailAsync()
        {
            var model = new List<MediaContentDetail>();
            var response = await ClientService.GetDataAsync(ControllerName, "getdetails");
            if (response != null)
            {
                var jsonTask = response.Content.ReadAsStringAsync();
                jsonTask.Wait();
                model = JsonConvert.DeserializeObject<List<MediaContentDetail>>(jsonTask.Result);
            }

            return model;
        }
    }
}