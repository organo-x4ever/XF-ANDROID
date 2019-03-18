using com.organo.xchallenge.Converters;
using com.organo.xchallenge.Handler;
using com.organo.xchallenge.Localization;
using com.organo.xchallenge.Models.User;
using com.organo.xchallenge.Services;
using com.organo.xchallenge.Statics;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(MetaService))]

namespace com.organo.xchallenge.Services
{
    public class MetaService : IMeta1Service
    {
        private string TAG => typeof(MetaService).FullName;
        public string ControllerName => "usermeta";
        private bool retryLoading = false;
        public string Message { get; set; }

        private readonly PoundToKiligramConverter _converter;

        public MetaService()
        {
            _converter = new PoundToKiligramConverter();
        }

        public async Task<UserMeta> GetLastestMetaAsync()
        {
            return await Convert(await GetMetaAsync());
        }

        public async Task<List<Meta>> GetMetaAsync()
        {
            retryLoading = false;
            var model = new List<Meta>();
            try
            {
                var response = await ClientService.GetDataAsync(ControllerName, "getbyuser");
                if (response != null)
                {
                    var jsonTask = response.Content.ReadAsStringAsync();
                    jsonTask.Wait();
                    model = JsonConvert.DeserializeObject<List<Meta>>(jsonTask.Result);
                }
            }
            catch (Exception)
            {
                if (!retryLoading)
                {
                    retryLoading = true;
                    return await GetMetaAsync();
                }
            }

            return model;
        }

        public async Task<UserMeta> GetLastestMetaAsync(long userId)
        {
            return await Convert(await GetMetaAsync(userId));
        }

        public async Task<List<Meta>> GetMetaAsync(long userId)
        {
            var model = new List<Meta>();
            try
            {
                var response = await ClientService.GetDataAsync(ControllerName, "getbyid?id=" + userId);
                if (response != null)
                {
                    var jsonTask = response.Content.ReadAsStringAsync();
                    jsonTask.Wait();
                    model = JsonConvert.DeserializeObject<List<Meta>>(jsonTask.Result);
                }
            }
            catch (Exception)
            {
                if (!retryLoading)
                {
                    retryLoading = true;
                    return await GetMetaAsync(userId);
                }
            }

            return model;
        }

        public async Task<UserMeta> GetMetaAsync(string keys)
        {
            var model = new List<Meta>();
            try
            {
                var response = await ClientService.GetDataAsync(ControllerName, "getbykeys?key=" + keys);
                if (response != null)
                {
                    var jsonTask = response.Content.ReadAsStringAsync();
                    jsonTask.Wait();
                    model = JsonConvert.DeserializeObject<List<Meta>>(jsonTask.Result);
                    return await Convert(model);
                }
            }
            catch (Exception)
            {
                if (!retryLoading)
                {
                    retryLoading = true;
                    return await GetMetaAsync(keys);
                }
            }

            return null;
        }

        public async Task<UserMeta> GetMetaAsync(string[] keys)
        {
            var model = new List<Meta>();
            try
            {
                var param = "";
                foreach (var key in keys)
                    param += (param.Trim().Length > 0 ? "," : "") + key;
                var response = await ClientService.GetDataAsync(ControllerName, "getbykeys?key=" + param);
                if (response != null)
                {
                    var jsonTask = response.Content.ReadAsStringAsync();
                    jsonTask.Wait();
                    model = JsonConvert.DeserializeObject<List<Meta>>(jsonTask.Result);
                    return await Convert(model);
                }
            }
            catch (Exception)
            {
                if (!retryLoading)
                {
                    retryLoading = true;
                    return await GetMetaAsync(keys);
                }
            }

            return null;
        }

        public async Task<string> SaveMetaAsync(List<Meta> metas)
        {
            try
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
            catch (Exception ex)
            {
                new ExceptionHandler(TAG, ex);
                return TextResources.MessageSomethingWentWrong;
            }
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

        public async Task<string> SaveMetaAsync(Meta metas)
        {
            try
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
            catch (Exception ex)
            {
                new ExceptionHandler(TAG, ex);
                return TextResources.MessageSomethingWentWrong;
            }
        }

        public async Task<UserMeta> Convert(List<Meta> metas)
        {
            if (metas == null || metas.Count == 0)
            {
                return null;
            }

            var userMeta = new UserMeta();
            await Task.Run(() =>
            {
                var profilePhoto = metas.FirstOrDefault(u => u.MetaKey.ToLower().Contains(MetaConstants.PROFILE_PHOTO));
                if (profilePhoto != null)
                    userMeta.ProfilePhoto = profilePhoto.MetaValue;

                var address = metas.FirstOrDefault(u => u.MetaKey.ToLower().Contains(MetaConstants.ADDRESS));
                if (address != null)
                    userMeta.Address = address.MetaValue;

                var age = metas.FirstOrDefault(u => u.MetaKey.ToLower().Contains(MetaConstants.AGE));
                if (age != null)
                {
                    short a = 0;
                    if (short.TryParse(age.MetaValue, out a))
                        userMeta.Age = a;
                }

                var city = metas.FirstOrDefault(u => u.MetaKey.ToLower().Contains(MetaConstants.CITY));
                if (city != null)
                    userMeta.City = city.MetaValue;

                var country = metas.FirstOrDefault(u => u.MetaKey.ToLower().Contains(MetaConstants.COUNTRY));
                if (country != null)
                    userMeta.Country = country.MetaValue;

                var gender = metas.FirstOrDefault(u => u.MetaKey.ToLower().Contains(MetaConstants.GENDER));
                if (gender != null)
                    userMeta.Gender = gender.MetaValue;

                var oGXProductPurchased =
                    metas.FirstOrDefault(u => u.MetaKey.ToLower().Contains(MetaConstants.PRODUCT_PURCHASED));
                if (oGXProductPurchased != null)
                {
                    bool isPurchased = false;
                    if (bool.TryParse(oGXProductPurchased.MetaValue, out isPurchased))
                        userMeta.IsOGXProductPurchased = isPurchased;
                }

                var postalCode = metas.FirstOrDefault(u => u.MetaKey.ToLower().Contains(MetaConstants.POSTAL_CODE));
                if (postalCode != null)
                    userMeta.PostalCode = postalCode.MetaValue;

                var state = metas.FirstOrDefault(u => u.MetaKey.ToLower().Contains(MetaConstants.STATE));
                if (state != null)
                    userMeta.State = state.MetaValue;

                var weightToLose =
                    metas.FirstOrDefault(u => u.MetaKey.ToLower().Contains(MetaConstants.WEIGHT_TO_LOSE));
                if (weightToLose != null)
                {
                    short l = 0;
                    if (short.TryParse(weightToLose.MetaValue, out l))
                        userMeta.WeightToLose = _converter.DisplayWeightVolume(l);
                }

                var weightLossGoal =
                    metas.FirstOrDefault(u => u.MetaKey.ToLower().Contains(MetaConstants.WEIGHT_LOSS_GOAL));
                if (weightLossGoal != null)
                {
                    short g = 0;
                    if (short.TryParse(weightLossGoal.MetaValue, out g))
                        userMeta.WeightGoal = _converter.DisplayWeightVolume(g);
                }

                var whyJoiningChallenge =
                    metas.FirstOrDefault(u => u.MetaKey.ToLower().Contains(MetaConstants.WHY_JOINING_CHALLENGE));
                if (whyJoiningChallenge != null)
                    userMeta.WhyJoiningChallenge = whyJoiningChallenge.MetaValue;

                var modifyDate = metas.FirstOrDefault();
                if (modifyDate != null)
                    userMeta.ModifyDate = modifyDate.ModifyDate;
            });
            return userMeta;
        }

        public async Task<List<UserMeta>> ConvertToList(List<Meta> metas)
        {
            var userMetas = new List<UserMeta>();
            if (metas != null && metas.Count > 0)
            {
                userMetas.Add(await Convert(metas));
            }

            return userMetas;
        }

        public async Task<Meta> AddMeta(string metaValue, string description, string key, string type)
        {
            var userId = App.CurrentUser.UserInfo.ID;
            var modifyDate = DateTime.Now;
            var meta = new Meta();
            await Task.Run(() =>
            {
                if (key == MetaConstants.WEIGHT_LOSS_GOAL || key == MetaConstants.WEIGHT_TO_LOSE)
                    metaValue = _converter.StorageWeightVolume(metaValue).ToString();
                meta = new Meta()
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
            return meta;
        }

        //public async Task<List<Meta>> AddListMeta(string metaValue, string description, string key, string type)
        //{
        //    var userId = App.CurrentUser.UserInfo.ID;
        //    var modifyDate = DateTime.Now;
        //    var metas = new List<Meta>();
        //    await Task.Run(() =>
        //    {
        //        metas.Add(new Meta()
        //        {
        //            MetaValue = metaValue,
        //            MetaDescription = description,
        //            MetaKey = key,
        //            MetaLabel = MetaConstants.LABEL,
        //            MetaType = type,
        //            ModifyDate = modifyDate,
        //            UserID = userId
        //        });
        //        if (key == MetaConstants.WEIGHT_LOSS_GOAL)
        //        {
        //            PoundToKiligramConverter converter = new PoundToKiligramConverter();
        //            bool isPound = App.Configuration.AppConfig.DefaultWeightVolume == "lb";
        //            metas.Add(new Meta()
        //            {
        //                MetaValue = isPound
        //                    ? converter.Convert(metaValue, typeof(System.Double), null,
        //                        CultureInfo.CurrentCulture).ToString()
        //                    : metaValue,
        //                MetaDescription = description + "-kg",
        //                MetaKey = key + "-kg",
        //                MetaLabel = MetaConstants.LABEL,
        //                MetaType = type,
        //                ModifyDate = modifyDate,
        //                UserID = userId
        //            });
        //            metas.Add(new Meta()
        //            {
        //                MetaValue = !isPound
        //                    ? converter.ConvertBack(metaValue, typeof(System.Double), null,
        //                        CultureInfo.CurrentCulture).ToString()
        //                    : metaValue,
        //                MetaDescription = description + "-lb",
        //                MetaKey = key + "-lb",
        //                MetaLabel = MetaConstants.LABEL,
        //                MetaType = type,
        //                ModifyDate = modifyDate,
        //                UserID = userId
        //            });
        //        }
        //    });
        //    return metas;
        //}
    }
}