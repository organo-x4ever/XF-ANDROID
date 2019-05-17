
using com.organo.xchallenge.Models.Authentication;
using com.organo.xchallenge.Services;
using com.organo.xchallenge.Statics;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using com.organo.xchallenge.Globals;
using Xamarin.Forms;

[assembly: Dependency(typeof(AuthenticationService))]

namespace com.organo.xchallenge.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private AuthenticationResult _authenticationResult;
        private readonly ISecureStorage _secureStorage;

        public AuthenticationService()
        {
            _secureStorage = DependencyService.Get<ISecureStorage>();
            Message = string.Empty;
        }

        public async Task<bool> AuthenticationAsync(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return false;

            // prompts the user for authentication
            _authenticationResult = await Authenticate(
                ClientService.GetRequestUri(App.Configuration.AppConfig.AuthenticationUrl),
                App.Configuration.GetApplication(), username, password);
            if (_authenticationResult != null)
            {
                App.CurrentUser = _authenticationResult;
                return true;
            }

            return false;
        }

        private async Task<AuthenticationResult> Authenticate(string authenticationUrl, string applicationId,
            string username, string password)
        {
            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri(authenticationUrl),
                Method = HttpMethod.Post
            };
            var userPasswordEncrypt = Convert.ToBase64String(Encoding.UTF8.GetBytes(username + ":" + password));
            var baseWithUserPassword = HttpConstants.BASIC + " " + userPasswordEncrypt;
            request.Headers.Add(HttpConstants.AUTHORIZATION, baseWithUserPassword);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(HttpConstants.MEDIA_TYPE_TEXT_PLAIN));
            var response = await ClientService.SendAsync(request);
            return await GetTokenAsync(response);
        }

        private async Task<AuthenticationResult> GetTokenAsync(HttpResponseMessage response)
        {
            if (response?.StatusCode == HttpStatusCode.OK)
            {
                if (!response.ToString().Contains(HttpConstants.UNAUTHORIZED))
                {
                    var date = DateTime.Now.AddHours(-1);
                    var tokenValue = GetValue(response, App.Configuration.AppConfig.TokenHeaderName);
                    if (!string.IsNullOrEmpty(tokenValue))
                    {
                        DateTime.TryParse(GetValue(response, App.Configuration.AppConfig.TokenExpiryHeaderName), out date);

                        if (date >= DateTime.Now.AddMinutes(-1))
                        {
                            await App.Configuration.SetUserTokenAsync(tokenValue);
                            var authenticationResult = new AuthenticationResult
                            {
                                AccessToken = tokenValue,
                                ExpiresOn = date,
                                ExtendedLifeTimeToken = true
                            };
                            await Task.Delay(TimeSpan.FromMilliseconds(1));
                            return authenticationResult;
                        }
                    }
                }
            }

            return null;
        }

        private string GetValue(HttpResponseMessage response, string headerName)
        {
            return response.Headers.GetValues(headerName)?.First();
        }

        public async Task<AuthenticationResult> GetDetailAsync(HttpResponseMessage response)
        {
            if (response?.StatusCode == HttpStatusCode.OK)
            {
                Task<string> jsonTask = response.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(jsonTask.Result) && !response.ToString().Contains(HttpConstants.UNAUTHORIZED))
                {
                    var tokenValue = GetValue(response, App.Configuration.AppConfig.TokenHeaderName);
                    if (!string.IsNullOrEmpty(tokenValue))
                    {
                        var model = JsonConvert.DeserializeObject<UserInfo>(jsonTask.Result);
                        if (model != null)
                        {
                            await App.Configuration.SetUserConfigurationAsync(tokenValue, model.LanguageCode, model.WeightVolumeType);
                            var user = new AuthenticationResult
                            {
                                AccessToken = tokenValue,
                                ExpiresOn = DateTime.Now.AddMonths(3),
                                ExtendedLifeTimeToken = true,
                                UserInfo = model
                            };
                            return user;
                        }
                    }
                }
            }

            return null;
        }

        public async Task LogoutAsync()
        {
            TokenKill();
            await App.Configuration.DeleteUserTokenAsync();
            _secureStorage.Delete(StorageConstants.KEY_USER_LANGUAGE);
            _secureStorage.Delete(StorageConstants.KEY_USER_WEIGHT_VOLUME);
            App.Configuration.DeleteVersionPrompt();
            App.Configuration = new AppConfiguration();
            await App.Configuration.InitAsync();
            App.CurrentUser = null;
        }

        private async void TokenKill()
        {
            if (App.CurrentUser == null)
                return;
            await ClientService.SendAsync(HttpMethod.Post, "api/user", "PostAuthTokenKill");
        }

        public bool IsAuthenticated => App.CurrentUser != null;

        public string Message { get; set; }
    }
}