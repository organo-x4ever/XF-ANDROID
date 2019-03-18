using com.organo.xchallenge.Localization;
using com.organo.xchallenge.Models.User;
using com.organo.xchallenge.Services;
using com.organo.xchallenge.Statics;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(UserMilestoneService))]

namespace com.organo.xchallenge.Services
{
    public class UserMilestoneService : IUserMilestoneService
    {
        private const string controller = "usermilestones"; //"testusermilestones"
        private bool retryLoading = false;
        public async Task<Dictionary<string, object>> GetDetailAsync()
        {
            var model = new Dictionary<string, object>();
            try
            {
                var response = await ClientService.GetDataAsync(controller, "getdetail");
                if (response != null)
                {
                    var jsonTask = response.Content.ReadAsStringAsync();
                    jsonTask.Wait();
                    model = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonTask.Result);
                    return model;
                }
            }
            catch (Exception)
            {
                if (!retryLoading)
                {
                    retryLoading = true;
                    return await GetDetailAsync();
                }
            }

            return null;
        }

        public async Task<UserMilestoneExtended> GetExtendedAsync()
        {
            var model = new UserMilestoneExtended();
            try
            {
                var response = await ClientService.GetDataAsync(controller, "getextended");
                if (response != null)
                {
                    var jsonTask = response.Content.ReadAsStringAsync();
                    jsonTask.Wait();
                    model = JsonConvert.DeserializeObject<UserMilestoneExtended>(jsonTask.Result);
                    return model;
                }
            }
            catch (Exception)
            {
                if (!retryLoading)
                {
                    retryLoading = true;
                    return await GetExtendedAsync();
                }
            }

            return null;
        }

        public async Task<UserMilestoneExtended> GetExtendedAsync(string languageCode)
        {
            var model = new UserMilestoneExtended();
            try
            {
                var response = await ClientService.GetDataAsync(controller, "getextended?languageCode=" + languageCode);
                if (response != null)
                {
                    var jsonTask = response.Content.ReadAsStringAsync();
                    jsonTask.Wait();
                    model = JsonConvert.DeserializeObject<UserMilestoneExtended>(jsonTask.Result);
                    return model;
                }
            }
            catch (Exception)
            {
                if (!retryLoading)
                {
                    retryLoading = true;
                    return await GetExtendedAsync(languageCode);
                }
            }

            return null;
        }

        public async Task<List<UserMilestone>> GetUserMilestoneAsync()
        {
            var model = new List<UserMilestone>();
            try
            {
                var response = await ClientService.GetDataAsync(controller, "getbyuser");
                if (response != null)
                {
                    var jsonTask = response.Content.ReadAsStringAsync();
                    jsonTask.Wait();
                    model = JsonConvert.DeserializeObject<List<UserMilestone>>(jsonTask.Result);
                    return model;
                }
            }
            catch (Exception)
            {
                if (!retryLoading)
                {
                    retryLoading = true;
                    return await GetUserMilestoneAsync();
                }
            }

            return null;
        }

        public async Task<string> SaveUserMilestoneAsync(UserMilestone userMilestone)
        {
            try
            {
                var response = await ClientService.PostDataAsync(userMilestone, controller, "postusermilestone");
                if (response != null && response.StatusCode == HttpStatusCode.OK)
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