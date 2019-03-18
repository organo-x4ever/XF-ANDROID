using com.organo.x4ever.Localization;
using com.organo.x4ever.Models.User;
using com.organo.x4ever.Services;
using com.organo.x4ever.Statics;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using com.organo.x4ever.Converters;
using Xamarin.Forms;

[assembly: Dependency(typeof(TrackerService))]

namespace com.organo.x4ever.Services
{
    public class TrackerService : ITrackerService
    {
        public string ControllerName => "trackers"; //"testtrackers"
        private readonly PoundToKiligramConverter _converter = new PoundToKiligramConverter();
        public async Task<UserTracker> GetLatestTrackerAsync()
        {
            var model = new List<Tracker>();
            if (App.CurrentUser != null)
                return null;
            var response = await ClientService.GetDataAsync(ControllerName, "getlatestbyuser");
            if (response != null)
            {
                var jsonTask = response.Content.ReadAsStringAsync();
                jsonTask.Wait();
                model = JsonConvert.DeserializeObject<List<Tracker>>(jsonTask.Result);
                return await Convert(model);
            }

            return null;
        }

        public async Task<UserTracker> GetLatestTrackerAsync(string token)
        {
            var model = new List<Tracker>();
            var request = new HttpRequestMessage()
            {
                RequestUri =
                    new Uri(App.Configuration.AppConfig.BaseUrl + "api/" + ControllerName + "/getlatestbyuser"),
                Method = HttpMethod.Get,
            };
            request.Headers.Add(App.Configuration.AppConfig.TokenHeaderName, token);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(HttpConstants.MEDIA_TYPE_TEXT_PLAIN));
            var response = await ClientService.SendAsync(request);
            if (response != null)
            {
                var jsonTask = response.Content.ReadAsStringAsync();
                jsonTask.Wait();
                model = JsonConvert.DeserializeObject<List<Tracker>>(jsonTask.Result);
                return await Convert(model);
            }

            return null;
        }

        public async Task<bool> LastSubmitExpireAsync(string token)
        {
            var trackerLast = await this.GetLatestTrackerAsync(token);
            if (trackerLast != null &&
                DateTime.TryParse(String.Format("{0:MM-dd-yyyy HH:mm:ss}", trackerLast.ModifyDate),
                    out DateTime modifyDate))
            {
                //var date = System.Convert.ToDateTime(String.Format("{0:MM-dd-yyyy HH:mm:ss}",
                //    DateTime.Now.AddMinutes(-1)));
                var date = System.Convert.ToDateTime(String.Format("{0:MM-dd-yyyy HH:mm:ss}",
                    DateTime.Now.AddDays(UserSettings.WEIGHT_SUBMIT_DAYS_INTERVAL)));
                return modifyDate <= date;
            }

            return false;
        }

        public async Task<List<UserTracker>> GetFirstAndLastTrackerAsync()
        {
            var model = new List<Tracker>();
            var response = await ClientService.GetDataAsync(ControllerName, "getfirstandlast");
            if (response != null)
            {
                var jsonTask = response.Content.ReadAsStringAsync();
                jsonTask.Wait();
                model = JsonConvert.DeserializeObject<List<Tracker>>(jsonTask.Result);
                return await ConvertToList(model);
            }

            return new List<UserTracker>();
        }

        public async Task<List<Tracker>> GetTrackerAsync()
        {
            var model = new List<Tracker>();
            var response = await ClientService.GetDataAsync(ControllerName, "getbyuser");
            if (response != null)
            {
                var jsonTask = response.Content.ReadAsStringAsync();
                jsonTask.Wait();
                try
                {
                    model = JsonConvert.DeserializeObject<List<Tracker>>(jsonTask.Result);
                }
                catch (Exception ex)
                {
                    WriteLog.Remote(ex.ToString());
                    //
                }
            }

            return model;
        }

        public async Task<List<UserTracker>> GetUserTrackerAsync()
        {
            return await ConvertToList(await this.GetTrackerAsync());
        }

        public async Task<UserTracker> GetTrackerAsync(string key)
        {
            var model = new List<Tracker>();
            var response = await ClientService.GetDataAsync(ControllerName, "getbykey?attributeKey=" + key);
            if (response != null)
            {
                var jsonTask = response.Content.ReadAsStringAsync();
                jsonTask.Wait();
                model = JsonConvert.DeserializeObject<List<Tracker>>(jsonTask.Result);
                return await Convert(model);
            }

            return null;
        }

        public async Task<string> SaveTrackerAsync(List<Tracker> trackers)
        {
            try
            {
                var response = await ClientService.PostDataAsync(trackers, ControllerName, "posttrackers");
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
            catch (Exception ex)
            {
                var msg = ex;
                return TextResources.MessageSomethingWentWrong;
            }
        }

        public async Task<UserTracker> Convert(List<Tracker> trackers, string revision = null)
        {
            if (trackers == null || trackers.Count == 0)
            {
                return null;
            }

            var userTracker = new UserTracker();
            await Task.Run(() =>
            {
                var aboutYourJourney = trackers.FirstOrDefault(u =>
                    u.AttributeName.ToLower().Contains(TrackerConstants.ABOUT_JOURNEY));
                if (aboutYourJourney != null)
                    userTracker.AboutYourJourney = aboutYourJourney.AttributeValue;

                var currentWeight = trackers.FirstOrDefault(u =>
                    u.AttributeName.ToLower().Contains(TrackerConstants.CURRENT_WEIGHT));
                if (currentWeight != null)
                {
                    short w = 0;
                    if (short.TryParse(currentWeight.AttributeValue, out w))
                        userTracker.CurrentWeight = _converter.DisplayWeightVolume(w);
                }

                var frontImage =
                    trackers.FirstOrDefault(u => u.AttributeName.ToLower().Contains(TrackerConstants.FRONT_IMAGE));
                if (frontImage != null)
                    userTracker.FrontImage = frontImage.AttributeValue;

                var sideImage =
                    trackers.FirstOrDefault(u => u.AttributeName.ToLower().Contains(TrackerConstants.SIDE_IMAGE));
                if (sideImage != null)
                    userTracker.SideImage = sideImage.AttributeValue;

                if (revision == null || revision.Trim().Length == 0)
                {
                    var revisionNumber = trackers.FirstOrDefault();
                    if (revisionNumber != null)
                        userTracker.RevisionNumber = revisionNumber.RevisionNumber;
                }
                else
                    userTracker.RevisionNumber = revision;

                var modifyDate = trackers.FirstOrDefault();
                if (modifyDate != null)
                    userTracker.ModifyDate = modifyDate.ModifyDate;

                var shirtSize =
                    trackers.FirstOrDefault(u => u.AttributeName.ToLower().Contains(TrackerConstants.TSHIRT_SIZE));
                if (shirtSize != null)
                    userTracker.ShirtSize = shirtSize.AttributeValue;
            });
            return userTracker;
        }

        public async Task<List<UserTracker>> ConvertToList(List<Tracker> trackers)
        {
            var userTrackers = new List<UserTracker>();
            if (trackers != null && trackers.Count > 0)
            {
                var trackerList = (from t in trackers
                    group t by new {t.RevisionNumber}
                    into rn
                    orderby rn.Key.RevisionNumber ascending
                    select new
                    {
                        RevisionNumber = rn.Key.RevisionNumber,
                        List = (from r in rn.ToList()
                            select new Tracker()
                            {
                                AttributeLabel = r.AttributeLabel,
                                AttributeName = r.AttributeName,
                                AttributeValue = r.AttributeValue,
                                MediaLink = r.MediaLink,
                                ModifyDate = r.ModifyDate,
                            }).Distinct().ToList()
                    }).ToList();
                foreach (var tracker in trackerList)
                {
                    userTrackers.Add(await this.Convert(tracker.List, tracker.RevisionNumber));
                }
            }

            return userTrackers;
        }

        public async Task<Tracker> AddTracker(string attr_name, string attr_value)
        {
            var userId = App.CurrentUser.UserInfo.ID;
            var modifyDate = DateTime.Now;
            var tracker = new Tracker();
            await Task.Run(() =>
            {
                if (attr_name == TrackerConstants.CURRENT_WEIGHT)
                    attr_value = _converter.StorageWeightVolume(attr_value).ToString();
                tracker = new Tracker()
                {
                    AttributeLabel = TrackerConstants.LABEL,
                    AttributeName = attr_name,
                    AttributeValue = attr_value,
                    ModifyDate = modifyDate,
                    UserID = userId
                };
            });
            return tracker;
        }

        //public async Task<List<Tracker>> AddListTracker(string attr_name, string attr_value)
        //{
        //    var userId = App.CurrentUser.UserInfo.ID;
        //    var modifyDate = DateTime.Now;
        //    var trackers = new List<Tracker>();
        //    await Task.Run(() =>
        //    {
        //        trackers.Add(new Tracker()
        //        {
        //            AttributeLabel = TrackerConstants.LABEL,
        //            AttributeName = attr_name,
        //            AttributeValue = attr_value,
        //            ModifyDate = modifyDate,
        //            UserID = userId
        //        });
        //        if (attr_name == TrackerConstants.CURRENT_WEIGHT)
        //        {
        //            PoundToKiligramConverter converter = new PoundToKiligramConverter();
        //            bool isPound = App.Configuration.AppConfig.DefaultWeightVolume == "lb";
        //            trackers.Add(new Tracker()
        //            {
        //                AttributeLabel = TrackerConstants.LABEL,
        //                AttributeName = attr_name + "-kg",
        //                AttributeValue = isPound
        //                    ? converter.Convert(attr_value, typeof(double), null, App.Configuration.LanguageInfo)
        //                        .ToString()
        //                    : attr_value,
        //                ModifyDate = modifyDate,
        //                UserID = userId
        //            });
        //            trackers.Add(new Tracker()
        //            {
        //                AttributeLabel = TrackerConstants.LABEL,
        //                AttributeName = attr_name + "-lb",
        //                AttributeValue = !isPound
        //                    ? converter.ConvertBack(attr_value, typeof(double), null, App.Configuration.LanguageInfo)
        //                        .ToString()
        //                    : attr_value,
        //                ModifyDate = modifyDate,
        //                UserID = userId
        //            });
        //        }
        //    });
        //    return trackers;
        //}
    }
}