
using System;
using System.Text;
using System.Threading.Tasks;
using com.organo.xchallenge.Localization;
using com.organo.xchallenge.Models.Notifications;
using com.organo.xchallenge.Services;
using com.organo.xchallenge.Statics;
using Newtonsoft.Json;
using Xamarin.Forms;

[assembly: Dependency(typeof(UserPushTokenServices))]

namespace com.organo.xchallenge.Services
{
    public class UserPushTokenServices : IUserPushTokenServices
    {
        public string ControllerName => "pushnotifications";

        public async Task<UserPushTokenModel> Get()
        {
            var model = new UserPushTokenModel();
            var response = await ClientService.GetDataAsync(ControllerName, "get");
            if (response != null)
            {
                var jsonTask = response.Content.ReadAsStringAsync();
                jsonTask.Wait();
                model = JsonConvert.DeserializeObject<UserPushTokenModel>(jsonTask.Result);
            }

            return model;
        }

        public async Task<string> Insert(UserPushTokenModel model)
        {
            try
            {
                await Task.Delay(TimeSpan.FromSeconds(5));
                var response = await ClientService.PostDataAsync(model, ControllerName, "post");
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

        public async Task<string> InsertByOldToken(UserPushTokenModelRegister model)
        {
            try
            {
                var response = await ClientService.PostDataAsync(model, ControllerName, "posttokenasync");
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

        public async Task<string> SaveDeviceToken()
        {
            var tokenChangedData =
                await DependencyService.Get<ISecureStorage>().RetrieveAsync(Keys.DEVICE_TOKEN_CHANGED);
            if (tokenChangedData != null)
            {
                var tokenChanged = Encoding.UTF8.GetString(tokenChangedData, 0, tokenChangedData.Length);
                if (string.IsNullOrEmpty(tokenChanged) || !tokenChanged.Equals(CommonConstants.YES))
                    return "";
            }
            else
                return "";

            var deviceToken = "";
            var data = await DependencyService.Get<ISecureStorage>().RetrieveAsync(Keys.DEVICE_TOKEN_IDENTITY);
            if (data != null)
                deviceToken = Encoding.UTF8.GetString(data, 0, data.Length);

            if (string.IsNullOrEmpty(deviceToken))
                return "";
            var identity = string.Format(TextResources.AppVersion, App.Configuration.AppConfig.ApplicationVersion);
            return await Insert(new UserPushTokenModel()
            {
                DeviceToken = deviceToken,
                IssuedOn = DateTime.Now,
                DeviceIdentity = identity,
                DeviceIdiom = Device.Idiom.ToString(),
            });
        }

        public async Task<string> SaveDeviceTokenByOldToken(string deviceToken, string oldDeviceToken)
        {
            if (!string.IsNullOrEmpty(deviceToken))
                //    WriteLog.Remote("Device Token does not exist. DeviceToken: " + deviceToken + ".OldDeviceToken: " + oldDeviceToken);
                //else
            {
                var identity = string.Format(TextResources.AppVersion, App.Configuration.AppConfig.ApplicationVersion);
                return await InsertByOldToken(new UserPushTokenModelRegister()
                {
                    DeviceToken = deviceToken,
                    OldDeviceToken = oldDeviceToken,
                    IssuedOn = DateTime.Now,
                    DeviceIdentity = identity,
                    DeviceIdiom = Device.Idiom.ToString(),
                });
            }

            return "";
        }
    }
}