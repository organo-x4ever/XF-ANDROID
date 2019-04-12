using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.organo.xchallenge.Converters;
using com.organo.xchallenge.Localization;
using com.organo.xchallenge.Models.User;
using com.organo.xchallenge.Services;
using com.organo.xchallenge.Statics;
using Newtonsoft.Json;
using Xamarin.Forms;

[assembly:Dependency(typeof(MetaPivotService))]
namespace com.organo.xchallenge.Services
{
    public class MetaPivotService : IMetaPivotService
    {
        public string Message { get; set; }
        public string ControllerName => "metapivot";
        private readonly PoundToKiligramConverter _converter = new PoundToKiligramConverter();

        public Meta AddMeta(string metaValue, string description, string key, string type)
        {
            var userId = App.CurrentUser.UserInfo.ID;
            var modifyDate = DateTime.Now;
            if (key == MetaConstants.WEIGHT_LOSS_GOAL || key == MetaConstants.WEIGHT_TO_LOSE)
                metaValue = _converter.StorageWeightVolume(metaValue).ToString();
            return new Meta()
            {
                MetaValue = metaValue,
                MetaDescription = description,
                MetaKey = key,
                MetaLabel = MetaConstants.LABEL,
                MetaType = type,
                ModifyDate = modifyDate,
                UserID = userId
            };
        }

        public async Task<Meta> AddMetaAsync(string metaValue, string description, string key, string type)
        {
            var userId = App.CurrentUser.UserInfo.ID;
            var modifyDate = DateTime.Now;
            return await Task.Factory.StartNew(() =>
            {
                if (key == MetaConstants.WEIGHT_LOSS_GOAL || key == MetaConstants.WEIGHT_TO_LOSE)
                    metaValue = _converter.StorageWeightVolume(metaValue).ToString();
                return new Meta()
                {
                    MetaValue = metaValue,
                    MetaDescription = description,
                    MetaKey = key,
                    MetaLabel = MetaConstants.LABEL,
                    MetaType = type,
                    ModifyDate = modifyDate,
                    UserID = userId
                };
            });
        }

        public async Task<MetaPivot> GetMetaAsync()
        {
            var response = await ClientService.GetDataAsync(ControllerName, "getbyuserasync");
            if (response != null)
            {
                var jsonTask = response.Content.ReadAsStringAsync();
                jsonTask.Wait();
                return JsonConvert.DeserializeObject<MetaPivot>(jsonTask.Result);
            }

            return null;
        }

        public async Task<string> SaveMetaAsync(List<Meta> metas)
        {
            var response = await ClientService.PostDataAsync(metas, ControllerName, "postmeta");
            if (response != null)
            {
                Task<string> jsonTask = response.Content.ReadAsStringAsync();
                if (jsonTask.Result.Contains(HttpConstants.SUCCESS))
                    return HttpConstants.SUCCESS;
                else if (response.ToString().Contains(HttpConstants.UNAUTHORIZED))
                    return response.ToString();
                return jsonTask.Result;
            }
            else return TextResources.MessageSomethingWentWrong;
        }

        public async Task<string> SaveMetaAsync(Meta metas)
        {
            var response = await ClientService.PostDataAsync(metas, ControllerName, "postmetadata");
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

        public async Task<bool> SaveMetaStep2Async(List<Meta> metas)
        {
            Message = string.Empty;
            var response = await SaveMetaAsync(metas);
            if (response != null)
            {
                if (response.Contains(HttpConstants.UNAUTHORIZED))
                    App.GoToAccountPage();
                else if (!response.Contains(HttpConstants.SUCCESS))
                    Message = response;
            }

            return string.IsNullOrEmpty(Message);
        }
    }
}