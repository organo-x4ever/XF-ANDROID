using System;
using System.Net.Http;
using com.organo.x4ever.Services;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly:Dependency(typeof(LogServices))]

namespace com.organo.x4ever.Services
{
    public class LogServices : ILogServices
    {
        public string ControllerName => "logs";

        public async Task WriteLog(string title, string message, bool showMessage)
        {
            await ClientService.WriteLog(null, title, message, showMessage);
        }

        public async Task WriteDebug(string debugLog)
        {
            var methodWithParam = "postdebuglog?debugLogstring=" + debugLog;
            await ClientService.SendAsync(HttpMethod.Post, ControllerName, methodWithParam);
        }

        public async Task WriteLog(Uri requestUri, Exception exception, bool showMessage = false)
        {
            await ClientService.WriteLog(requestUri, exception, showMessage);
        }
    }
}