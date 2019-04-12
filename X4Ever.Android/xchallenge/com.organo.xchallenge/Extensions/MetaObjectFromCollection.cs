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
        public static string Get(this List<Meta> meta, MetaEnum key)
        {
            return meta.FirstOrDefault(u => u.MetaKey.ToLower().Contains(key.ToString()))?.MetaValue ?? "";
        }

        public static async Task<string> GetAsync(this List<Meta> meta, MetaEnum key)
        {
            return await Task.Factory.StartNew(() =>
                meta.FirstOrDefault(u => u.MetaKey.ToLower().Contains(key.ToString()))?.MetaValue ?? "");
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
    }
}