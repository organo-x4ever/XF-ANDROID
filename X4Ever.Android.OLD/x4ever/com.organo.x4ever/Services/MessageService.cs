using com.organo.x4ever.Models;
using com.organo.x4ever.Services;
using com.organo.x4ever.Statics;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(MessageService))]

namespace com.organo.x4ever.Services
{
    public class MessageService : IMessageService
    {
        public string ControllerName => "messageservice";

        public async Task<bool> SendEmailAsync(string token, string subject, string body)
        {
            var model = new MessageDetail()
            {
                Body = body,
                Subject = subject
            };
            var myContent = JsonConvert.SerializeObject(model);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue(HttpConstants.MEDIA_TYPE_APPLICATION_FORM);
            byteContent.Headers.Add(App.Configuration.AppConfig.TokenHeaderName, token);
            var requestUri = ClientService.GetRequestUri(ControllerName, "send");
            var response = await ClientService.PostAsync(requestUri, byteContent);
            if (response != null)
            {
                Task<string> jsonTask = response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject(jsonTask.Result);
                if (jsonTask.Result.Contains(HttpConstants.SUCCESS))
                    return true;
            }

            return false;
        }
    }
}