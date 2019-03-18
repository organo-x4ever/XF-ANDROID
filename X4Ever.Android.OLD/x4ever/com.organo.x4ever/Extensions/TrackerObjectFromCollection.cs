using com.organo.x4ever.Models.User;
using com.organo.x4ever.Statics;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace com.organo.x4ever.Extensions
{
    public static class TrackerObjectFromCollection
    {
        public async static Task<string> Get(this List<Tracker> tracker, TrackerEnum key)
        {
            return await Convert(tracker, key);
        }

        public async static Task<UserTracker> Get(this List<Tracker> tracker)
        {
            return await tracker.Convert();
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

        public async static Task<UserTracker> Convert(this List<Tracker> trackers, string revision = null)
        {
            if (trackers == null || trackers.Count == 0)
            {
                return null;
            }
            var userTracker = new UserTracker();
            await Task.Run(() =>
            {
                var aboutYourJourney = trackers.FirstOrDefault(u => u.AttributeName.ToLower().Contains(TrackerConstants.ABOUT_JOURNEY));
                if (aboutYourJourney != null)
                    userTracker.AboutYourJourney = aboutYourJourney.AttributeValue;

                var currentWeight = trackers.FirstOrDefault(u => u.AttributeName.ToLower().Contains(TrackerConstants.CURRENT_WEIGHT));
                if (currentWeight != null)
                {
                    short w = 0;
                    if (short.TryParse(currentWeight.AttributeValue, out w))
                        userTracker.CurrentWeight = w;
                }

                var frontImage = trackers.FirstOrDefault(u => u.AttributeName.ToLower().Contains(TrackerConstants.FRONT_IMAGE));
                if (frontImage != null)
                    userTracker.FrontImage = frontImage.AttributeValue;

                var sideImage = trackers.FirstOrDefault(u => u.AttributeName.ToLower().Contains(TrackerConstants.SIDE_IMAGE));
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

                var shirtSize = trackers.FirstOrDefault(u => u.AttributeName.ToLower().Contains(TrackerConstants.TSHIRT_SIZE));
                if (shirtSize != null)
                    userTracker.ShirtSize = shirtSize.AttributeValue;
            });
            return userTracker;
        }

        public async static Task<List<UserTracker>> ConvertToList(List<Tracker> trackers)
        {
            var userTrackers = new List<UserTracker>();
            if (trackers != null && trackers.Count > 0)
            {
                var trackerList = (from t in trackers
                                   group t by new { t.RevisionNumber } into rn
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
                    userTrackers.Add(await Convert(tracker.List, tracker.RevisionNumber));
                }
            }
            return userTrackers;
        }
    }
}