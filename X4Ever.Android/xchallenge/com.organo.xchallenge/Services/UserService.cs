using com.organo.xchallenge.Extensions;
using com.organo.xchallenge.Localization;
using com.organo.xchallenge.Models;
using com.organo.xchallenge.Models.Authentication;
using com.organo.xchallenge.Models.User;
using com.organo.xchallenge.Services;
using com.organo.xchallenge.Statics;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using com.organo.xchallenge.Pages.Account;
using Xamarin.Forms;

[assembly: Dependency(typeof(UserService))]

namespace com.organo.xchallenge.Services
{
    public class UserService : IUser1Service
    {
        private const string ChangePasswordApi = "changepassword";
        private readonly ITracker1Service _trackerService;
        private readonly IAuthenticationService _authenticationService;
        public string ControllerName => "user";
        private string ControllerNameAction => "actions";

        public string Message { get; set; }
        private bool retryLoading = false;

        public UserService()
        {
            _trackerService = DependencyService.Get<ITracker1Service>();
            _authenticationService = DependencyService.Get<IAuthenticationService>();
        }

        public async Task<UserInfo> GetAsync()
        {
            var model = new UserInfo();
            try
            {
                var response = await ClientService.GetDataAsync(ControllerName, "getusernew");
                if (response != null && response.IsSuccessStatusCode && response.StatusCode == HttpStatusCode.OK)
                {
                    var jsonTask = response.Content.ReadAsStringAsync();
                    jsonTask.Wait();
                    model = JsonConvert.DeserializeObject<UserInfo>(jsonTask.Result);
                }
            }
            catch (Exception)
            {
                if (!retryLoading)
                {
                    retryLoading = true;
                    return await GetAsync();
                }
            }

            return model;
        }

        public async Task GetAuthenticationAsync(Action callbackSuccess, Action callbackFailed)
        {
            try
            {
                var token = await App.Configuration.GetUserToken();
                if (!string.IsNullOrEmpty(token))
                {
                    var request = new HttpRequestMessage()
                    {
                        RequestUri = new Uri(ClientService.GetRequestUri(ControllerName, "authuser_v2")),
                        Method = HttpMethod.Post,
                    };
                    request.Headers.Add(App.Configuration.AppConfig.TokenHeaderName, token);
                    request.Headers.Accept.Add(
                        new MediaTypeWithQualityHeaderValue(HttpConstants.MEDIA_TYPE_TEXT_PLAIN));
                    var response = await ClientService.SendAsync(request);
                    var authenticationResult = await _authenticationService.GetDetailAsync(response);
                    if (authenticationResult != null)
                    {
                        App.CurrentUser = authenticationResult;
                        callbackSuccess();
                        return;
                    }
                    else
                        await _authenticationService?.LogoutAsync();
                }
            }
            catch (Exception)
            {
                if (!retryLoading)
                {
                    retryLoading = true;
                    await GetAuthenticationAsync(callbackSuccess, callbackFailed);
                    return;
                }
            }

            callbackFailed();

        }

        public async Task<UserWithDetail> GetFullAsync()
        {
            try
            {
                var response = await ClientService.GetDataAsync(ControllerName, "getfulluser");
                if (response == null || !response.IsSuccessStatusCode || response.StatusCode != HttpStatusCode.OK)
                    return null;
                var jsonTask = response.Content.ReadAsStringAsync();
                jsonTask.Wait();
                var model = JsonConvert.DeserializeObject<UserWithDetail>(jsonTask.Result);
                if (model?.UserMetas != null)
                    model.UserDetailMeta = await model.UserMetas.ToList().Get();

                if (model?.UserTrackers != null)
                    model.UserDetailTrackers = await _trackerService.ConvertToList(model.UserTrackers.ToList());

                return model;
            }
            catch (Exception)
            {
                if (!retryLoading)
                {
                    retryLoading = true;
                    return await GetFullAsync();
                }
            }

            return null;
        }

        public async Task<string> RegisterAsync(UserRegister user)
        {
            var response = await ClientService.PostDataNoHeaderAsync(user, ControllerNameAction, "register");
            if (response != null && response.IsSuccessStatusCode && response.StatusCode == HttpStatusCode.OK)
            {
                Task<string> jsonTask = response.Content.ReadAsStringAsync();
                if (jsonTask.Result.Contains(HttpConstants.SUCCESS))
                {
                    return HttpConstants.SUCCESS;
                }

                return jsonTask.Result;
            }
            else return TextResources.MessageSomethingWentWrong;
        }

        public async Task<string> ChangePasswordAsync(string currentPassword, string newPassword)
        {
            var user = new PasswordChange()
            {
                CurrentPassword = currentPassword,
                Password = newPassword,
                UserID = App.CurrentUser.UserInfo.ID
            };
            var response = await ClientService.PostDataAsync(user, ControllerName, ChangePasswordApi);
            if (response != null && response.IsSuccessStatusCode && response.StatusCode == HttpStatusCode.OK)
            {
                Task<string> jsonTask = response.Content.ReadAsStringAsync();
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

        public async Task<string> RequestForgotPasswordAsync(string username, string email)
        {
            var recover = new ForgotPassword()
            {
                UserLogin = username,
                UserEmail = email
            };

            var response = await ClientService.PostDataNoHeaderAsync(recover, ControllerNameAction, "requestpassword");
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

        public async Task<string> ChangeForgotPasswordAsync(string requestCode, string password)
        {
            var user = new PasswordDetail()
            {
                RequestCode = requestCode,
                Password = password
            };
            var response = await ClientService.PostDataNoHeaderAsync(user, ControllerNameAction, "updatepassword");
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

        public async Task<string> UpdateAsync(UserFirstUpdate user)
        {
            var response = await ClientService.PostDataAsync(user, ControllerName, "updatefirst");
            if (response != null && response.IsSuccessStatusCode && response.StatusCode == HttpStatusCode.OK)
            {
                Task<string> jsonTask = response.Content.ReadAsStringAsync();
                if (jsonTask.Result.Contains(HttpConstants.SUCCESS))
                {
                    return HttpConstants.SUCCESS;
                }

                return jsonTask.Result;
            }

            return TextResources.MessageUpdateFailed;
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
    }
}