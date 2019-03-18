using com.organo.x4ever.Models;
using com.organo.x4ever.Services;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(ApplicationLanguageService))]

namespace com.organo.x4ever.Services
{
    public class ApplicationLanguageService : IApplicationLanguageService
    {
        public string ControllerName => "application_languages";

        public async Task<List<ApplicationLanguage>> GetAsync()
        {
            var model = new List<ApplicationLanguage>();
            var response = await ClientService.GetDataAsync(ControllerName, "get");
            if (response != null)
            {
                var jsonTask = response.Content.ReadAsStringAsync();
                jsonTask.Wait();
                model = JsonConvert.DeserializeObject<List<ApplicationLanguage>>(jsonTask.Result);
            }

            return model;
        }

        public async Task<List<ApplicationLanguage>> GetByCountryAsync(int countryID)
        {
            var model = new List<ApplicationLanguage>();
            var method = "getbycountry?countryID=" + countryID;
            var response = await ClientService.GetDataAsync(ControllerName, method);
            if (response != null)
            {
                var jsonTask = response.Content.ReadAsStringAsync();
                jsonTask.Wait();
                model = JsonConvert.DeserializeObject<List<ApplicationLanguage>>(jsonTask.Result);
            }

            return model;
        }

        public async Task<List<ApplicationLanguage>> GetByLanguageAsync(int languageID)
        {
            var model = new List<ApplicationLanguage>();
            var method = "getbylanguage?languageID=" + languageID;
            var response = await ClientService.GetDataAsync(ControllerName, method);
            if (response != null)
            {
                var jsonTask = response.Content.ReadAsStringAsync();
                jsonTask.Wait();
                model = JsonConvert.DeserializeObject<List<ApplicationLanguage>>(jsonTask.Result);
            }

            return model;
        }

        public async Task<List<ApplicationLanguage>> GetWithCountryAsync()
        {
            var model = new List<ApplicationLanguage>();
            var response = await ClientService.GetDataAsync(ControllerName, "getwithcountry");
            if (response != null)
            {
                var jsonTask = response.Content.ReadAsStringAsync();
                jsonTask.Wait();
                model = JsonConvert.DeserializeObject<List<ApplicationLanguage>>(jsonTask.Result);
            }

            return model;
        }
    }
}