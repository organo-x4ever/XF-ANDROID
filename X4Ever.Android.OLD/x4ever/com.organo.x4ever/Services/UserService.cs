using com.organo.x4ever.Extensions;
using com.organo.x4ever.Localization;
using com.organo.x4ever.Models;
using com.organo.x4ever.Models.Authentication;
using com.organo.x4ever.Models.User;
using com.organo.x4ever.Services;
using com.organo.x4ever.Statics;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(UserService))]

namespace com.organo.x4ever.Services
{
    public class UserService : IUserService
    {
        private const string ChangePasswordApi = "changepassword";
        private readonly ITrackerService _trackerService;
        private readonly IAuthenticationService _authenticationService;
        public string ControllerName => "user";
        private string ControllerNameAction => "actions";

        public UserService()
        {
            _trackerService = DependencyService.Get<ITrackerService>();
            _authenticationService = DependencyService.Get<IAuthenticationService>();
        }

        public async Task<UserInfo> GetAsync()
        {
            var model = new UserInfo();
            var response = await ClientService.GetDataAsync(ControllerName, "getusernew");
            if (response != null && response.IsSuccessStatusCode && response.StatusCode == HttpStatusCode.OK)
            {
                var jsonTask = response.Content.ReadAsStringAsync();
                jsonTask.Wait();
                model = JsonConvert.DeserializeObject<UserInfo>(jsonTask.Result);
            }

            return model;
        }

        public async Task<bool> GetAuthenticationAsync()
        {
            var token = await App.Configuration.GetUserToken();
            if (!string.IsNullOrEmpty(token))
            {
                var request = new HttpRequestMessage()
                {
                    RequestUri = new Uri(ClientService.GetRequestUri(ControllerName, "authuser")),
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
                    if (App.CurrentUser != null && App.CurrentUser.UserInfo != null)
                    {
                        if (!string.IsNullOrEmpty(App.CurrentUser.UserInfo.UserFirstName))
                            App.GoToAccountPage(true);
                        else
                        {
                            var user = new UserFirstUpdate()
                            {
                                UserEmail = App.CurrentUser.UserInfo.UserEmail.Trim()
                            };
                            App.CurrentApp.MainPage = new Pages.Account.BasicInfoPage(user);
                        }

                        return true;
                    }
                }
                else
                    await _authenticationService.LogoutAsync();
            }

            return false;
        }

        public async Task<UserWithDetail> GetFullAsync()
        {
            var response = await ClientService.GetDataAsync(ControllerName, "getfulluser");
            if (response == null || !response.IsSuccessStatusCode || response.StatusCode != HttpStatusCode.OK)
                return null;
            var jsonTask = response.Content.ReadAsStringAsync();
            jsonTask.Wait();
            var model = JsonConvert.DeserializeObject<UserWithDetail>(jsonTask.Result);
            if (model.UserMetas != null)
            {
                var metaDetail = await model.UserMetas.ToList().Get();
                model.UserDetailMeta = metaDetail;
            }

            if (model.UserTrackers != null)
            {
                var trackerDetail = await _trackerService.ConvertToList(model.UserTrackers.ToList());
                model.UserDetailTrackers = trackerDetail;
            }

            return model;
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

            return TextResources.MessagePasswordUpdateFailed;
        }
    }
}