using com.organo.x4ever.Localization;
using com.organo.x4ever.Models;
using com.organo.x4ever.Models.User;
using com.organo.x4ever.Services;
using com.organo.x4ever.Statics;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(UserSettingService))]

namespace com.organo.x4ever.Services
{
    public class UserSettingService : IUserSettingService
    {
        public string ControllerName => "usersettings";

        public async Task<UserSetting> GetAsync()
        {
            var model = new UserSetting();
            var response = await ClientService.GetDataAsync(ControllerName, "getsetting");
            if (response != null)
            {
                var jsonTask = response.Content.ReadAsStringAsync();
                jsonTask.Wait();
                model = JsonConvert.DeserializeObject<UserSetting>(jsonTask.Result);
                return model;
            }

            return null;
        }

        public async Task<string> SaveAsync(UserSetting userSetting)
        {
            var response = await ClientService.PostDataAsync(userSetting, ControllerName, "postsetting");
            if (response != null)
            {
                Task<string> jsonTask = response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject(jsonTask.Result);
                if (jsonTask.Result.Contains(HttpConstants.SUCCESS))
                    return HttpConstants.SUCCESS;
                else if (response.ToString().Contains(HttpConstants.UNAUTHORIZED))
                    return response.ToString();
                return jsonTask.Result;
            }
            else return TextResources.MessageSomethingWentWrong;
        }

        public async Task<string> UpdateUserLanguageAsync(ApplicationLanguageRequest applicationLanguage)
        {
            var response = await ClientService.PostDataAsync(applicationLanguage, ControllerName, "postuserlanguage");
            if (response != null)
            {
                Task<string> jsonTask = response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject(jsonTask.Result);
                if (jsonTask.Result.Contains(HttpConstants.SUCCESS))
                    return HttpConstants.SUCCESS;
                else if (response.ToString().Contains(HttpConstants.UNAUTHORIZED))
                    return response.ToString();
                return jsonTask.Result;
            }
            else return TextResources.MessageSomethingWentWrong;
        }

        public async Task<string> UpdateUserWeightVolumeAsync(UserWeightVolume weightVolume)
        {
            var response = await ClientService.PostDataAsync(weightVolume, ControllerName, "postuserweightvolume");
            if (response != null)
            {
                Task<string> jsonTask = response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject(jsonTask.Result);
                if (jsonTask.Result.Contains(HttpConstants.SUCCESS))
                    return HttpConstants.SUCCESS;
                else if (response.ToString().Contains(HttpConstants.UNAUTHORIZED))
                    return response.ToString();
                return jsonTask.Result;
            }
            else return TextResources.MessageSomethingWentWrong;
        }
    }
}