using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using com.organo.x4ever.Models.Youtube;
using com.organo.x4ever.Services;
using Newtonsoft.Json;
using Xamarin.Forms;

[assembly:Dependency(typeof(YoutubeDataServices))]

namespace com.organo.x4ever.Services
{
    public class YoutubeDataServices : IYoutubeDataServices
    {
        public string ControllerName => "youtubevideos";

        public async Task<YoutubeConfiguration> GetAsync()
        {
            var model = new YoutubeConfiguration();
            var response = await ClientService.SendAsync(HttpMethod.Get, ControllerName, "getasync");
            if (response != null && response.StatusCode == HttpStatusCode.OK)
            {
                var jsonTask = response.Content.ReadAsStringAsync();
                jsonTask.Wait();
                model = JsonConvert.DeserializeObject<YoutubeConfiguration>(jsonTask.Result);
            }

            return model;
        }

        public async Task<string> GetStringAsync(string requestUri)
        {
            return await ClientService.GetStringAsync(new Uri(requestUri));
        }
    }
}