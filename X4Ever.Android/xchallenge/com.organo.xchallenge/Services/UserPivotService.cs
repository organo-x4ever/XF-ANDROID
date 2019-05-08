
using System;
using System.Net;
using System.Threading.Tasks;
using com.organo.xchallenge.Localization;
using com.organo.xchallenge.Models;
using com.organo.xchallenge.Models.Authentication;
using com.organo.xchallenge.Models.User;
using com.organo.xchallenge.Services;
using com.organo.xchallenge.Statics;
using Newtonsoft.Json;
using Xamarin.Forms;

[assembly:Dependency(typeof(UserPivotService))]

namespace com.organo.xchallenge.Services
{
    public class UserPivotService : IUserPivotService
    {
        private readonly ITrackerPivotService _trackerPivotService;
        private readonly IAuthenticationService _authenticationService;
        public string Message { get; set; }
        public string ControllerName => "userpivot";
        public string ControllerNameUnauthorized => "actions";

        public UserPivotService()
        {
            _trackerPivotService = DependencyService.Get<ITrackerPivotService>();
            _authenticationService = DependencyService.Get<IAuthenticationService>();
        }

        // Authorized::REQUESTS
        public async Task<string> ChangePasswordAsync(string currentPassword, string newPassword)
        {
            var response = await ClientService.PostDataAsync(new PasswordChange()
            {
                CurrentPassword = currentPassword,
                Password = newPassword,
                UserID = App.CurrentUser.UserInfo.ID
            }, ControllerName, "changepassword");
            if (response != null && response.IsSuccessStatusCode && response.StatusCode == HttpStatusCode.OK)
            {
                var jsonTask = response.Content.ReadAsStringAsync();
                if (jsonTask.Result.Contains(HttpConstants.SUCCESS) &&
                    !jsonTask.Result.Contains(HttpConstants.INVALID) &&
                    !jsonTask.Result.Contains(HttpConstants.ERROR) &&
                    !jsonTask.Result.Contains(HttpConstants.UNSUPPORTED) &&
                    !jsonTask.Result.Contains(CommonConstants.Message))
                {
                    return HttpConstants.SUCCESS;
                }

                return jsonTask.Result;
            }
            else return TextResources.MessageSomethingWentWrong;
        }

        public async Task<UserInfo> GetAsync()
        {
            var model = new UserInfo();
            var response = await ClientService.GetDataAsync(ControllerName, "getuser");
            if (response != null && response.IsSuccessStatusCode && response.StatusCode == HttpStatusCode.OK)
            {
                var jsonTask = response.Content.ReadAsStringAsync();
                jsonTask.Wait();
                model = JsonConvert.DeserializeObject<UserInfo>(jsonTask.Result);
            }

            return model;
        }

        public async Task GetAuthenticationAsync(Action callbackSuccess, Action callbackFailed)
        {
            if (await App.Configuration.IsUserTokenExistsAsync())
            {
                var authenticationResult = await _authenticationService.GetDetailAsync(
                    await ClientService.GetDataAsync(ControllerName, "authuser_v3"));
                if (authenticationResult != null)
                {
                    App.CurrentUser = authenticationResult;
                    callbackSuccess();
                    return;
                }
                else
                    await App.LogoutAsync();
            }

            callbackFailed();
        }

        public async Task<UserPivot> GetFullAsync()
        {
            var response = await ClientService.GetDataAsync(ControllerName, "getfulluser");
            if (response != null && response.IsSuccessStatusCode && response.StatusCode == HttpStatusCode.OK)
            {
                var jsonTask = await response.Content.ReadAsStringAsync();
                if (jsonTask != null)
                    return JsonConvert.DeserializeObject<UserPivot>(jsonTask);
            }

            return null;
        }

        public async Task<bool> UpdateStep1Async(UserStep1 user)
        {
            Message = string.Empty;
            var response = await ClientService.PostDataAsync(user, ControllerName, "userstep1");
            if (response != null && response.IsSuccessStatusCode && response.StatusCode == HttpStatusCode.OK)
            {
                Task<string> jsonTask = response.Content.ReadAsStringAsync();
                if (jsonTask.Result.Contains(HttpConstants.UNAUTHORIZED))
                    App.GoToAccountPage();
                else if (!jsonTask.Result.Contains(HttpConstants.SUCCESS))
                    Message = jsonTask.Result;
            }

            return string.IsNullOrEmpty(Message);
        }


        // Unauthorized::REQUESTS
        public async Task<string> ChangeForgotPasswordAsync(string requestCode, string password)
        {
            var response =
                await ClientService.PostDataNoHeaderAsync(new PasswordDetail()
                {
                    RequestCode = requestCode,
                    Password = password
                }, ControllerNameUnauthorized, "updatepassword");
            if (response != null && response.IsSuccessStatusCode && response.StatusCode == HttpStatusCode.OK)
            {
                Task<string> jsonTask = response.Content.ReadAsStringAsync();
                if (jsonTask.Result.Contains(HttpConstants.SUCCESS))
                {
                    return HttpConstants.SUCCESS;
                }

                return jsonTask.Result;
            }

            return TextResources.MessagePasswordUpdateFailed;
        }

        public async Task<string> RegisterAsync(UserRegister user)
        {
            var response = await ClientService.PostDataNoHeaderAsync(user, ControllerNameUnauthorized, "register");
            if (response != null && response.IsSuccessStatusCode && response.StatusCode == HttpStatusCode.OK)
            {
                var jsonTask = response.Content.ReadAsStringAsync();
                if (jsonTask.Result.Contains(HttpConstants.SUCCESS))
                {
                    return HttpConstants.SUCCESS;
                }

                return jsonTask.Result;
            }
            else return TextResources.MessageSomethingWentWrong;
        }

        public async Task<string> RequestForgotPasswordAsync(string username, string email)
        {
            var response =
                await ClientService.PostDataNoHeaderAsync(new ForgotPassword()
                {
                    UserLogin = username,
                    UserEmail = email
                }, ControllerNameUnauthorized, "requestpassword");
            if (response != null && response.IsSuccessStatusCode && response.StatusCode == HttpStatusCode.OK)
            {
                Task<string> jsonTask = response.Content.ReadAsStringAsync();
                if (jsonTask.Result.Contains(HttpConstants.SUCCESS))
                {
                    return HttpConstants.SUCCESS;
                }

                return jsonTask.Result;
            }

            return TextResources.MessagePasswordRequestFailed;
        }
    }
}