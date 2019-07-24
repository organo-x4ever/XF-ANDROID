
using com.organo.xchallenge.Helpers;
using com.organo.xchallenge.Localization;
using com.organo.xchallenge.Models;
using com.organo.xchallenge.Notification;
using com.organo.xchallenge.Statics;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace com.organo.xchallenge.Services
{
    public static class ClientService
    {
        private static readonly HttpClient HttpClient = new HttpClient();
        private static readonly IDeviceInfo DeviceInfo = DependencyService.Get<IDeviceInfo>();
        private static readonly string ApiUrl = "https://mapp.oghq.ca/";
        private static HttpResponseMessage _response;

        public static string GetRequestUri(string controller, string method)
        {
            return GetRequestUri(controller + "/" + method);
        }

        public static string GetRequestUri(string controllerWithMethod)
        {
            if (App.Configuration != null)
                return App.Configuration?.AppConfig.BaseUrl + "api/" + controllerWithMethod;
            else
                return ApiUrl + "api/" + controllerWithMethod;
        }

        public static async Task<string> GetStringAsync(Uri requestUri)
        {
            try
            {
                return await HttpClient.GetStringAsync(requestUri);
            }
            catch (Exception exception)
            {
                await WriteLog(requestUri, exception);
            }

            return "";
        }

        public static async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
        {
            _response = null;
            try
            {
                _response = await HttpClient.SendAsync(CheckHeaders(request));
                return await CheckHttpStatusCode(request.RequestUri, _response);
            }
            catch (Exception exception)
            {
                await WriteLog(request.RequestUri, request, _response, exception);
            }

            return null;
        }

        public static async Task<HttpResponseMessage> SendAsync(HttpMethod httpMethod, string controller, string method)
        {
            var request = new HttpRequestMessage();
            try
            {
                request = CreateAuthorizationHeader(httpMethod, controller, method);
                return await SendAsync(request);
            }
            catch (Exception exception)
            {
                await WriteLog(request.RequestUri, exception);
            }

            return null;
        }

        public static async Task<HttpResponseMessage> PostAsync(string requestUri, HttpContent httpContent)
        {
            _response = null;
            try
            {
                _response = await HttpClient.PostAsync(requestUri, CheckHeaders(httpContent));
                return await CheckHttpStatusCode(new Uri(requestUri), _response);
            }
            catch (Exception exception)
            {
                await WriteLog(new Uri(requestUri), httpContent, _response, exception);
            }

            return null;
        }

        /// <summary>
        /// Creates authorization header from authentication result.
        /// </summary>
        /// <param name="httpMethod">
        /// HttpMethod.Get to get data or HttpMethod.Post to post data
        /// </param>
        /// <param name="controller">
        /// RESTful API controller name
        /// </param>
        /// <param name="method">
        /// RESTful API method name
        /// </param>
        /// <returns>
        /// returns Creates Authorization header with token
        /// </returns>
        private static HttpRequestMessage CreateAuthorizationHeader(HttpMethod httpMethod, string controller,
            string method)
        {
            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri(GetRequestUri(controller, method)),
                Method = httpMethod,
            };
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(HttpConstants.MEDIA_TYPE_TEXT_PLAIN));
            return request;
        }

        /// <summary>
        /// Sends Post request asynchronously.
        /// </summary>
        /// <param name="data">
        /// Object to post data
        /// </param>
        /// <param name="controller">
        /// RESTful API controller name
        /// </param>
        /// <param name="method">
        /// RESTful API method name
        /// </param>
        /// <returns>
        /// returns Response from Post Method
        /// </returns>
        public static async Task<HttpResponseMessage> PostDataNoHeaderAsync(object data, string controller,
            string method)
        {
            try
            {
                var myContent = JsonConvert.SerializeObject(data);
                var buffer = Encoding.UTF8.GetBytes(myContent);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue(HttpConstants.MEDIA_TYPE_APPLICATION_JSON);
                return await PostAsync(GetRequestUri(controller, method), byteContent);
            }
            catch (Exception exception)
            {
                await WriteLog(new Uri(GetRequestUri(controller, method)), exception);
            }

            return null;
        }

        /// <summary>
        /// Sends Post request asynchronously.
        /// </summary>
        /// <param name="data">
        /// Object to post data
        /// </param>
        /// <param name="controller">
        /// RESTful API controller name
        /// </param>
        /// <param name="method">
        /// RESTful API method name
        /// </param>
        /// <returns>
        /// returns Response from Post Method
        /// </returns>
        public static async Task<HttpResponseMessage> PostDataAsync(object data, string controller, string method)
        {
            try
            {
                var myContent = JsonConvert.SerializeObject(data);
                var buffer = Encoding.UTF8.GetBytes(myContent);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue(HttpConstants.MEDIA_TYPE_APPLICATION_JSON);
                byteContent.Headers.Add(App.Configuration?.AppConfig.AcceptedTokenName,
                    App.Configuration.UserToken);
                return await PostAsync(GetRequestUri(controller, method), byteContent);
            }
            catch (Exception exception)
            {
                await WriteLog(new Uri(GetRequestUri(controller, method)), exception);
            }

            return null;
        }


        /// <summary>
        /// Sends Post request asynchronously.
        /// </summary>
        /// <param name="controller">
        /// RESTful API controller name
        /// </param>
        /// <param name="methodWithParams"></param>
        /// <returns>
        /// returns Response from Post Method
        /// </returns>
        public static async Task<HttpResponseMessage> GetDataAsync(string controller, string methodWithParams)
        {
            var header = CreateAuthorizationHeader(HttpMethod.Get, controller, methodWithParams);
            try
            {
                return await SendAsync(header);
            }
            catch (Exception exception)
            {
                await WriteLog(new Uri(GetRequestUri(controller, methodWithParams)), header, exception);
            }

            return null;
        }

        /// <summary>
        /// Sends Post request asynchronously.
        /// </summary>
        /// <param name="controller">
        /// RESTful API controller name
        /// </param>
        /// <param name="methodWithParams"></param>
        /// <returns>
        /// returns Response from Post Method
        /// </returns>
        public static async Task<HttpResponseMessage> GetByApplicationHeaderDataAsync(string controller,
            string methodWithParams)
        {
            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri(GetRequestUri(controller, methodWithParams)),
                Method = HttpMethod.Get,
            };
            try
            {
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(HttpConstants.MEDIA_TYPE_TEXT_PLAIN));
                return await SendAsync(request);
            }
            catch (Exception exception)
            {
                await WriteLog(new Uri(GetRequestUri(controller, methodWithParams)), request, exception);
            }

            return null;
        }

        private static HttpRequestMessage CheckHeaders(HttpRequestMessage request)
        {
            if (App.Configuration != null)
            {
                if (!request.Headers.Contains(App.Configuration?.AppConfig.AcceptedTokenName))
                {
                    if (!string.IsNullOrEmpty(App.Configuration.UserToken))
                        request.Headers.Add(App.Configuration?.AppConfig.AcceptedTokenName,
                            App.Configuration.UserToken);
                }

                if (!request.Headers.Contains(App.Configuration?.AppConfig.ApplicationRequestHeader))
                {
                    var application = App.Configuration?.GetApplication();
                    if (!string.IsNullOrEmpty(application))
                        request.Headers.Add(App.Configuration?.AppConfig.ApplicationRequestHeader, application);
                }

                if (!request.Headers.Contains(HttpConstants.REQUEST_HEADER_LANGUAGE))
                {
                    var language = App.Configuration?.AppConfig.DefaultLanguage;
                    if (!string.IsNullOrEmpty(language))
                        request.Headers.Add(HttpConstants.REQUEST_HEADER_LANGUAGE, language);
                }
            }

            if (!request.Headers.Contains(HttpConstants.PLATFORM))
            {
                var platform = DeviceInfo.GetPlatform;
                if (!string.IsNullOrEmpty(platform))
                    request.Headers.Add(HttpConstants.PLATFORM, platform);
            }

            if (!request.Headers.Contains(HttpConstants.VERSION))
            {
                var version = App.Configuration?.AppConfig.ApplicationVersion;
                if (!string.IsNullOrEmpty(version))
                    request.Headers.Add(HttpConstants.VERSION, version);
            }

            return request;
        }

        private static HttpContent CheckHeaders(HttpContent request)
        {
            if (App.Configuration != null)
            {
                if (!request.Headers.Contains(App.Configuration?.AppConfig.AcceptedTokenName))
                {
                    if (!string.IsNullOrEmpty(App.Configuration.UserToken))
                        request.Headers.Add(App.Configuration?.AppConfig.AcceptedTokenName,
                            App.Configuration.UserToken);
                }

                if (!request.Headers.Contains(App.Configuration?.AppConfig.ApplicationRequestHeader))
                {
                    var application = App.Configuration?.GetApplication();
                    if (!string.IsNullOrEmpty(application))
                        request.Headers.Add(App.Configuration?.AppConfig.ApplicationRequestHeader, application);
                }

                if (!request.Headers.Contains(HttpConstants.REQUEST_HEADER_LANGUAGE))
                {
                    var language = App.Configuration?.AppConfig.DefaultLanguage;
                    if (!string.IsNullOrEmpty(language))
                        request.Headers.Add(HttpConstants.REQUEST_HEADER_LANGUAGE, language);
                }
            }

            if (!request.Headers.Contains(HttpConstants.PLATFORM))
            {
                var platform = DeviceInfo.GetPlatform;
                if (!string.IsNullOrEmpty(platform))
                    request.Headers.Add(HttpConstants.PLATFORM, platform);
            }
            
            if (!request.Headers.Contains(HttpConstants.VERSION))
            {
                var version = App.Configuration?.AppConfig.ApplicationVersion;
                if (!string.IsNullOrEmpty(version))
                    request.Headers.Add(HttpConstants.VERSION, version);
            }

            return request;
        }

        private static async Task<HttpResponseMessage> CheckHttpStatusCode(Uri requestUri, HttpResponseMessage response)
        {
            if (response != null && response.StatusCode != HttpStatusCode.OK &&
                !requestUri.AbsolutePath.Contains("userauth") &&
                !requestUri.AbsolutePath.Contains("authuser_v2") &&
                !requestUri.AbsolutePath.Contains("PostAuthTokenKill"))
                await WriteLog(requestUri, response.StatusCode.ToString(), response.ToString());

            return response;
        }

        private static async Task WriteLog(List<Log> logs)
        {
            try
            {
                var myContent = JsonConvert.SerializeObject(logs);
                var buffer = Encoding.UTF8.GetBytes(myContent);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue(HttpConstants.MEDIA_TYPE_APPLICATION_JSON);
                await HttpClient.PostAsync(GetRequestUri("logs", "post"), byteContent);
            }
            catch (Exception)
            {
                DependencyService.Get<IInformationMessageServices>().LongAlert("Network Failure Error");
            }
        }

        public static async Task WriteLog(Uri requestUri, string message, string messageDetail,
            bool showMessage = false)
        {
            try
            {
                WriteDebugger(message, messageDetail);
                var token = App.Configuration.UserToken;
                var deviceDetail = "";
                try
                {
                    deviceDetail = DeviceInfo.GetManufacturer + " " + DeviceInfo.GetModel;
                }
                catch
                {
                    //
                }

                var logs = new List<Log>();
                var log = new Log
                {
                    Application = App.Configuration?.GetApplication(),
                    Device = deviceDetail,
                    Platform = DeviceInfo.GetPlatform,
                    IPAddress = "Device",
                    Identity = string.Format(TextResources.AppVersion, App.Configuration?.AppConfig.ApplicationVersion),
                    Idiom = Device.Idiom.ToString(),
                    Message = messageDetail.Replace(':', '#'),
                    Title = message.Replace(':', '#'),
                    Token = token,
                    Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture)
                };
                if (requestUri != null)
                    log.RequestUri = requestUri;
                logs.Add(log);
                await WriteLog(logs);
                if (showMessage) DependencyService.Get<IInformationMessageServices>().LongAlert(TextResources.GotError);
            }
            catch
            {
                //
            }
        }

        public static async Task WriteLog(Uri requestUri, Exception exception = null, bool showMessage = false)
        {
            await WriteLog(requestUri, exception?.Message, GetExceptionDetail(exception), showMessage);
        }

        private static async Task WriteLog(Uri requestUri, HttpRequestMessage requestMessage, Exception exception,
            bool showMessage = false)
        {
            await WriteLog(requestUri, exception.Message,
                GetExceptionDetail(exception, requestMessage, null), showMessage);
        }

        private static async Task WriteLog(Uri requestUri, HttpRequestMessage requestMessage,
            HttpResponseMessage responseMessage, Exception exception,
            bool showMessage = false)
        {
            await WriteLog(requestUri, exception.Message,
                GetExceptionDetail(exception, requestMessage, responseMessage), showMessage);
        }

        private static async Task WriteLog(Uri requestUri, HttpContent httpContent, HttpResponseMessage responseMessage,
            Exception exception, bool showMessage = false)
        {
            await WriteLog(requestUri, exception.Message,
                GetExceptionDetailContent(exception, httpContent, responseMessage), showMessage);
        }

        public static async Task WriteLog(Uri requestUri, string message, Exception exception, bool showMessage = false)
        {
            await WriteLog(requestUri, message, GetExceptionDetail(exception), showMessage);
        }

        public static string GetExceptionDetail(Exception exception)
        {
            var stringBuilder = new StringBuilder();
            while (exception != null)
            {
                stringBuilder.AppendLine(exception.Message);
                stringBuilder.AppendLine(exception.StackTrace);
                exception = exception.InnerException;
            }

            return stringBuilder.ToString();
        }

        private static string GetExceptionDetail(Exception exception, HttpRequestMessage requestMessage,
            HttpResponseMessage responseMessage)
        {
            var stringBuilder = new StringBuilder();
            while (exception != null)
            {
                stringBuilder.AppendLine(exception.Message);
                stringBuilder.AppendLine(exception.StackTrace);
                exception = exception.InnerException;
            }

            if (requestMessage != null)
            {
                stringBuilder.AppendLine("HttpRequestMessage Detail: ");
                stringBuilder.AppendLine(requestMessage.ToString());
            }

            if (responseMessage != null)
            {
                stringBuilder.AppendLine("HttpResponseMessage Detail: ");
                stringBuilder.AppendLine(responseMessage.ToString());
            }

            return stringBuilder.ToString();
        }

        private static string GetExceptionDetailContent(Exception exception, HttpContent httpContent,
            HttpResponseMessage responseMessage)
        {
            var stringBuilder = new StringBuilder();
            while (exception != null)
            {
                stringBuilder.AppendLine(exception.Message);
                stringBuilder.AppendLine(exception.StackTrace);
                exception = exception.InnerException;
            }

            if (httpContent != null)
            {
                stringBuilder.AppendLine("HttpContent Detail: ");
                stringBuilder.AppendLine(httpContent.ToString());
            }

            if (responseMessage != null)
            {
                stringBuilder.AppendLine("HttpResponseMessage Detail: ");
                stringBuilder.AppendLine(responseMessage.ToString());
            }

            return stringBuilder.ToString();
        }

        private static void WriteDebugger(string message, string detail)
        {
            Debug.WriteLine(message);
            Debug.WriteLine(detail);
        }
    }
}