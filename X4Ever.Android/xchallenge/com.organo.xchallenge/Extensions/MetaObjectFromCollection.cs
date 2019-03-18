using System;
using com.organo.xchallenge.Models.User;
using com.organo.xchallenge.Statics;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using com.organo.xchallenge.Converters;
using com.organo.xchallenge.Handler;

namespace com.organo.xchallenge.Extensions
{
    public static class MetaObjectFromCollection
    {
        private static readonly PoundToKiligramConverter _converter = new PoundToKiligramConverter();

        public async static Task<string> Get(this List<Meta> meta, MetaEnum key)
        {
            return await Convert(meta, key);
        }

        public static async Task<UserMeta> Get(this List<Meta> meta)
        {
            return await meta.Convert();
        }

        private static async Task<string> Convert(List<Meta> metas, MetaEnum key)
        {
            var val = "";
            if (metas == null || metas.Count == 0)
                return val;
            var userMeta = new UserMeta();
            await Task.Run(() =>
            {
                var meta = metas.FirstOrDefault(u => u.MetaKey.ToLower().Contains(key.ToString()));
                if (meta != null)
                    val = meta.MetaValue;
            });
            return val;
        }

        public static async Task<UserMeta> Convert(this List<Meta> metas)
        {
            return await Task.Factory.StartNew(() =>
            {
                if (metas?.Count == 0) return null;
                var userMeta = new UserMeta();
                try
                {
                    var profilePhoto =
                        metas.FirstOrDefault(u => u.MetaKey.ToLower().Contains(MetaConstants.PROFILE_PHOTO));
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
                }
                catch (Exception exception)
                {
                    new ExceptionHandler("MetaObjectFromCollection.Convert(this List<Meta> metas)", exception);
                }

                return userMeta;
            });
        }

        public static async Task<List<UserMeta>> ConvertToList(this List<Meta> metas)
        {
            var userMetas = new List<UserMeta>();
            if (metas != null && metas.Count > 0)
            {
                userMetas.Add(await Convert(metas));
            }

            return userMetas;
        }
    }
}