using com.organo.x4ever.Models.User;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace com.organo.x4ever.Services
{
    public interface ITrackerService : IBaseService
    {
        Task<string> SaveTrackerAsync(List<Tracker> trackers);

        Task<List<UserTracker>> GetUserTrackerAsync();

        Task<List<Tracker>> GetTrackerAsync();

        Task<UserTracker> GetTrackerAsync(string key);

        Task<UserTracker> GetLatestTrackerAsync();

        Task<List<UserTracker>> GetFirstAndLastTrackerAsync();

        Task<UserTracker> Convert(List<Tracker> trackers, string revision = null);

        Task<List<UserTracker>> ConvertToList(List<Tracker> trackers);

        Task<Tracker> AddTracker(string attr_name, string attr_value);

        //Task<List<Tracker>> AddListTracker(string attr_name, string attr_value);
        Task<UserTracker> GetLatestTrackerAsync(string token);

        Task<bool> LastSubmitExpireAsync(string token);
    }
}