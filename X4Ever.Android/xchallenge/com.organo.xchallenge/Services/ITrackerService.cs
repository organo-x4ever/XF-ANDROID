using com.organo.xchallenge.Models.User;
using System.Collections.Generic;
using System.Threading.Tasks;
using com.organo.xchallenge.Models;

namespace com.organo.xchallenge.Services
{
    public interface ITracker1Service : IBaseService
    {
        string Message { get; set; }
        Task<string> SaveTrackerAsync(List<Tracker> trackers);
        Task<bool> SaveTrackerStep3Async(List<Tracker> trackers, bool loadUserProfile = false);

        Task<List<UserTracker>> GetUserTrackerAsync();

        Task<List<Tracker>> GetTrackerAsync();

        Task<UserTracker> GetTrackerAsync(string key);

        Task<UserTracker> GetLatestTrackerAsync();

        Task<List<UserTracker>> GetFirstAndLastTrackerAsync();

        UserTracker Convert(List<Tracker> trackers, string revision = null);

        Task<List<UserTracker>> ConvertToList(List<Tracker> trackers);

        Task<Tracker> AddTracker(string attr_name, string attr_value);

        //Task<List<Tracker>> AddListTracker(string attr_name, string attr_value);
        Task<UserTracker> GetLatestTrackerAsync(string token);

        Task<bool> LastSubmitExpireAsync(string token);
    }
}