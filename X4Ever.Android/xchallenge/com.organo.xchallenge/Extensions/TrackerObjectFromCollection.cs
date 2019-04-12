using com.organo.xchallenge.Models.User;
using com.organo.xchallenge.Statics;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace com.organo.xchallenge.Extensions
{
    public static class TrackerObjectFromCollection
    {
        public static string Get(this List<Tracker> tracker, TrackerEnum key)
        {
            return tracker.FirstOrDefault(u => u.AttributeName.ToLower().Contains(key.ToString()))?.AttributeValue ?? "";
        }

        public static async Task<string> GetAsync(this List<Tracker> tracker, TrackerEnum key)
        {
            return await Task.Factory.StartNew(() =>
                tracker.FirstOrDefault(u => u.AttributeName.ToLower().Contains(key.ToString()))?.AttributeValue ?? "");
        }

        private static async Task<string> Convert(List<Tracker> trackers, TrackerEnum key)
        {
            var val = "";
            if (trackers == null || trackers.Count == 0)
                return val;
            var userTracker = new UserTracker();
            await Task.Run(() =>
            {
                var tracker = trackers.FirstOrDefault(u => u.AttributeName.ToLower().Contains(key.ToString()));
                if (tracker != null)
                    val = tracker.AttributeValue;
            });
            return val;
        }
    }
}