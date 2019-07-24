using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using com.organo.xchallenge.Converters;
using com.organo.xchallenge.Localization;
using com.organo.xchallenge.Models.User;
using com.organo.xchallenge.Services;
using com.organo.xchallenge.Statics;
using Newtonsoft.Json;
using Xamarin.Forms;

[assembly:Dependency(typeof(TrackerPivotService))]

namespace com.organo.xchallenge.Services
{
    public class TrackerPivotService : ITrackerPivotService
    {
        public string Message { get; set; }

        public string ControllerName => "trackerpivot";
        private readonly PoundToKiligramConverter _converter = new PoundToKiligramConverter();

        public Tracker AddTracker(string attr_name, string attr_value)
        {
            var userId = App.CurrentUser.UserInfo.ID;
            var modifyDate = DateTime.Now;
            if (attr_name == TrackerConstants.CURRENT_WEIGHT)
                attr_value = _converter.StorageWeightVolume(attr_value).ToString();
            var tracker = new Tracker()
            {
                AttributeLabel = TrackerConstants.LABEL,
                AttributeName = attr_name,
                AttributeValue = attr_value,
                ModifyDate = modifyDate,
                UserID = userId
            };
            return tracker;
        }

        public async Task<Tracker> AddTrackerAsync(string attr_name, string attr_value)
        {
            var userId = App.CurrentUser.UserInfo.ID;
            var modifyDate = DateTime.Now;
            return await Task.Factory.StartNew(() =>
            {
                if (attr_name == TrackerConstants.CURRENT_WEIGHT)
                    attr_value = _converter.StorageWeightVolume(attr_value).ToString();
                return new Tracker()
                {
                    AttributeLabel = TrackerConstants.LABEL,
                    AttributeName = attr_name,
                    AttributeValue = attr_value,
                    ModifyDate = modifyDate,
                    UserID = userId
                };
            });
        }

        public async Task<TrackerPivot> GetLatestTrackerAsync()
        {
            var response = await ClientService.GetDataAsync(ControllerName, "latesttrackerpivotasync");
            if (response != null)
            {
                var jsonTask = response.Content.ReadAsStringAsync();
                jsonTask.Wait();
                return JsonConvert.DeserializeObject<TrackerPivot>(jsonTask.Result);
            }

            return null;
        }

        public async Task<List<TrackerPivot>> GetUserTrackerAsync()
        {
            var model = new List<TrackerPivot>();
            var response = await ClientService.GetDataAsync(ControllerName, "trackerspivotasync");
            if (response != null)
            {
                var jsonTask = response.Content.ReadAsStringAsync();
                jsonTask.Wait();
                model = JsonConvert.DeserializeObject<List<TrackerPivot>>(jsonTask.Result);
            }

            return model;
        }

        public async Task<string> SaveTrackerAsync(List<Tracker> trackers)
        {
            var response = await ClientService.PostDataAsync(trackers, ControllerName, "posttrackers");
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

        public async Task<bool> SaveTrackerStep3Async(List<Tracker> trackers, bool loadUserProfile = false)
        {
            Message = string.Empty;
            var response = await SaveTrackerAsync(trackers);
            if (response != null)
            {
                if (response.Contains(HttpConstants.UNAUTHORIZED))
                {
                    App.GoToAccountPage();
                }
                else if (!response.Contains(HttpConstants.SUCCESS))
                {
                    Message = response;
                }
                else if (loadUserProfile)
                {
                    await GetUserData();
                }
            }

            return string.IsNullOrEmpty(Message);

            async Task GetUserData()
            {
                var result = await DependencyService.Get<IUserPivotService>().GetAsync();
                if (result != null)
                {
                    result.ProfileImage = result.ProfileImage ?? TextResources.ImageNotAvailable;
                    App.CurrentUser.UserInfo = result;
                }
            }
        }

        public async Task<bool> UpdateLatestTrackerAsync(double newValue, double oldValue, DateTime lastModifyDate)
        {
            Message = string.Empty;
            if (string.IsNullOrEmpty(newValue.ToString()) && !double.TryParse(newValue.ToString(), out double nVal))
            {
                Message += "Invalid New Value";
            }

            if (string.IsNullOrEmpty(oldValue.ToString()) && !double.TryParse(oldValue.ToString(), out double oVal))
            {
                Message += "Invalid Old Value";
            }

            if (string.IsNullOrEmpty(lastModifyDate.ToString()) &&
                !DateTime.TryParse(lastModifyDate.ToString(), out DateTime dVal))
            {
                Message += "Invalid Last Modify date";
            }

            var trackerEditViewModel = new TrackerEditViewModel()
            {
                NewValue = newValue,
                OldValue = oldValue,
                LastModifyDate = lastModifyDate
            };
            var response = await ClientService.PostDataAsync(trackerEditViewModel, ControllerName, "posttrackeredit");
            if (response != null)
            {
                Task<string> jsonTask = response.Content.ReadAsStringAsync();
                if (jsonTask.Result.Contains(HttpConstants.SUCCESS))
                    return true;
            }

            return false;
        }
        
        public async Task<string> DeleteTrackerAsync(string revisionNumber)
        {
            var method = "posttrackerdelete?revisionNumber=" + revisionNumber;
            var response = await ClientService.PostDataAsync(null, ControllerName, method);
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
        
        public async Task PostSkipOptionAsync(string email, bool skip)
        {
            await ClientService.PostDataAsync(null, ControllerName, "posttrackerskipphotos?options=" + Convert.ToBase64String(Encoding.UTF8.GetBytes(email + ":" + (skip ? "1" : "0"))));
        }
    }
}