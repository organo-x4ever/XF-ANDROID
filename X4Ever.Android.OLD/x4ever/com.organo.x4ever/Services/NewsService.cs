using com.organo.x4ever.Models.News;
using com.organo.x4ever.Services;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(NewsService))]

namespace com.organo.x4ever.Services
{
    public class NewsService : INewsService
    {
        public string ControllerName => "news";
        public long ExecutionTime { get; set; }

        public NewsService()
        {
            ExecutionTime = 0;
        }

        public async Task<List<NewsModel>> GetByActive(bool active)
        {
            var model = new List<NewsModel>();
            var response = await ClientService.GetDataAsync(ControllerName, "getbyactive?active=" + active);
            if (response != null && response.IsSuccessStatusCode == true && response.StatusCode == HttpStatusCode.OK)
            {
                var jsonTask = response.Content.ReadAsStringAsync();
                jsonTask.Wait();
                model = JsonConvert.DeserializeObject<List<NewsModel>>(jsonTask.Result);
            }

            return model;
        }

        public async Task<List<NewsModel>> GetByLanguage(string languageCode, bool active)
        {
            var model = new List<NewsModel>();
            var response = await ClientService.GetByApplicationHeaderDataAsync(ControllerName,
                "get?languageCode=" + languageCode + "&active=" + active);
            if (response != null && response.IsSuccessStatusCode == true && response.StatusCode == HttpStatusCode.OK)
            {
                var jsonTask = response.Content.ReadAsStringAsync();
                jsonTask.Wait();
                model = JsonConvert.DeserializeObject<List<NewsModel>>(jsonTask.Result);
                var executionTime = response.Headers.GetValues(App.Configuration.AppConfig.ExecutionTimeHeader).First();
                if (executionTime != null)
                {
                    if (long.TryParse(executionTime, out long execTime))
                        ExecutionTime = execTime;
                }
            }

            return model;
        }
    }
}