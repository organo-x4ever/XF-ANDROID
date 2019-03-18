using com.organo.xchallenge.Localization;
using com.organo.xchallenge.Models.User;
using com.organo.xchallenge.Services;
using com.organo.xchallenge.Statics;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using com.organo.xchallenge.Converters;
using com.organo.xchallenge.Globals;
using com.organo.xchallenge.Handler;
using Xamarin.Forms;
using com.organo.xchallenge.Models;
using com.organo.xchallenge.Pages.Account;

[assembly: Dependency(typeof(TrackerService))]

namespace com.organo.xchallenge.Services
{
    public class TrackerService : ITracker1Service
    {
        public string ControllerName => "trackers"; //"testtrackers"

        public string Message { get; set; }
        private bool retryLoading = false;

        private readonly PoundToKiligramConverter _converter;

        public TrackerService()
        {
            _converter = new PoundToKiligramConverter();
        }

        public async Task<UserTracker> GetLatestTrackerAsync()
        {
            var model = new List<Tracker>();
            if (App.CurrentUser != null)
                return null;
            try
            {
                var response = await ClientService.GetDataAsync(ControllerName, "getlatestbyuser");
                if (response != null)
                {
                    var jsonTask = response.Content.ReadAsStringAsync();
                    jsonTask.Wait();
                    model = JsonConvert.DeserializeObject<List<Tracker>>(jsonTask.Result);
                    return Convert(model);
                }
            }
            catch (Exception)
            {
                if (!retryLoading)
                {
                    retryLoading = true;
                    return await GetLatestTrackerAsync();
                }
            }

            return null;
        }

        public async Task<UserTracker> GetLatestTrackerAsync(string token)
        {
            var model = new List<Tracker>();
            try
            {
                var request = new HttpRequestMessage()
                {
                    RequestUri = new Uri(App.Configuration.AppConfig.BaseUrl + "api/" + ControllerName + "/getlatestbyuser"),
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
                    return Convert(model);
                }
            }
            catch (Exception)
            {
                if (!retryLoading)
                {
                    retryLoading = true;
                    return await GetLatestTrackerAsync(token);
                }
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
                var date = System.Convert.ToDateTime(String.Format("{0:MM-dd-yyyy HH:mm:ss}",
                    DateTime.Now.AddDays(UserSettings.WEIGHT_SUBMIT_DAYS_INTERVAL)));
                return modifyDate <= date;
            }

            return false;
        }

        public async Task<List<UserTracker>> GetFirstAndLastTrackerAsync()
        {
            var model = new List<Tracker>();
            try
            {
                var response = await ClientService.GetDataAsync(ControllerName, "getfirstandlast");
                if (response != null)
                {
                    var jsonTask = response.Content.ReadAsStringAsync();
                    jsonTask.Wait();
                    model = JsonConvert.DeserializeObject<List<Tracker>>(jsonTask.Result);
                    return await ConvertToList(model);
                }
            }
            catch (Exception)
            {
                if (!retryLoading)
                {
                    retryLoading = true;
                    return await GetFirstAndLastTrackerAsync();
                }
            }

            return new List<UserTracker>();
        }

        public async Task<List<Tracker>> GetTrackerAsync()
        {
            var model = new List<Tracker>();
            try
            {
                var response = await ClientService.GetDataAsync(ControllerName, "getbyuser");
                if (response != null)
                {
                    var jsonTask = response.Content.ReadAsStringAsync();
                    jsonTask.Wait();
                    model = JsonConvert.DeserializeObject<List<Tracker>>(jsonTask.Result);
                }
            }
            catch (Exception)
            {
                if (!retryLoading)
                {
                    retryLoading = true;
                    return await GetTrackerAsync();
                }
            }

            return model;
        }

        public async Task<List<UserTracker>> GetUserTrackerAsync()
        {
            return await ConvertToList(await GetTrackerAsync());
        }

        public async Task<UserTracker> GetTrackerAsync(string key)
        {
            var model = new List<Tracker>();
            try
            {
                var response = await ClientService.GetDataAsync(ControllerName, "getbykey?attributeKey=" + key);
                if (response != null)
                {
                    var jsonTask = response.Content.ReadAsStringAsync();
                    jsonTask.Wait();
                    model = JsonConvert.DeserializeObject<List<Tracker>>(jsonTask.Result);
                    return Convert(model);
                }
            }
            catch (Exception)
            {
                if (!retryLoading)
                {
                    retryLoading = true;
                    return await GetTrackerAsync(key);
                }
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
                    if (jsonTask.Result.Contains(HttpConstants.SUCCESS))
                        return HttpConstants.SUCCESS;
                    else if (response.ToString().Contains(HttpConstants.UNAUTHORIZED))
                        return response.ToString();
                    return jsonTask.Result;
                }
                else return TextResources.MessageSomethingWentWrong;
            }
            catch (Exception)
            {
                return TextResources.MessageSomethingWentWrong;
            }
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

                    //DependencyService.Get<IHelper>().GetFilePath(result.ProfileImage, FileType.User)

                    App.CurrentUser.UserInfo = result;
                }
            }
        }

        public UserTracker Convert(List<Tracker> trackers, string revision = null)
        {
            var userTracker = new UserTracker();
            if (trackers == null || trackers.Count == 0)
            {
                return null;
            }

            try
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
            }
            catch (Exception exception)
            {
                new ExceptionHandler("TrackerService.Convert(List<Tracker> trackers, string revision = null)",
                    exception);
            }

            return userTracker;
        }

        public async Task<List<UserTracker>> ConvertToList(List<Tracker> trackers)
        {
            return await Task.Factory.StartNew(() =>
            {
                var userTrackers = new List<UserTracker>();
                if (trackers?.Count > 0)
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
                        userTrackers.Add(Convert(tracker.List, tracker.RevisionNumber));
                    }
                }

                return userTrackers;
            });
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