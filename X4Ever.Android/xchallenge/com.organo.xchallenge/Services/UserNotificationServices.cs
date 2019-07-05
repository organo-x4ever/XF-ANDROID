
using System;
using System.Threading.Tasks;
using com.organo.xchallenge.Localization;
using com.organo.xchallenge.Models.Notifications;
using com.organo.xchallenge.Services;
using com.organo.xchallenge.Statics;
using Newtonsoft.Json;
using Xamarin.Forms;

[assembly:Dependency(typeof(UserNotificationServices))]
namespace com.organo.xchallenge.Services
{
    public class UserNotificationServices : IUserNotificationServices
    {
        public string ControllerName => "notificationsettings";

        public async Task<UserNotificationSetting> GetAsync()
        {
            var response = await ClientService.GetDataAsync(ControllerName, "getbytokenasync");
            if (response != null)
            {
                var jsonTask = response.Content.ReadAsStringAsync();
                jsonTask.Wait();
                return JsonConvert.DeserializeObject<UserNotificationSetting>(jsonTask.Result);
            }

            return null;
        }

        public async Task<UserNotificationSettingViewModel> GetAdvancedAsync()
        {
            var response = await ClientService.GetDataAsync(ControllerName, "getbytokenasync");
            if (response != null)
            {
                var jsonTask = response.Content.ReadAsStringAsync();
                jsonTask.Wait();
                return JsonConvert.DeserializeObject<UserNotificationSettingViewModel>(jsonTask.Result);
            }

            return null;
        }

        public async Task<string> Update(UserNotificationSetting notificationSetting)
        {
            try
            {
                var response = await ClientService.PostDataAsync(notificationSetting, ControllerName, "postsettings");
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
            catch (Exception)
            {
                return TextResources.MessageSomethingWentWrong;
            }
        }
    }
}