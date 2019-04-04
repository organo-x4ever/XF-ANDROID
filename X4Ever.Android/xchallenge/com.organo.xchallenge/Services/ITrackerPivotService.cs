using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.organo.xchallenge.Models.User;

namespace com.organo.xchallenge.Services
{
    public interface ITrackerPivotService : IBaseService
    {
        string Message { get; set; }

        Task<List<TrackerPivot>> GetUserTrackerAsync();

        //Task<List<Tracker>> GetTrackerAsync();
        //Task<TrackerPivot> GetTrackerAsync(string key);
        Task<TrackerPivot> GetLatestTrackerAsync();

        //Task<List<TrackerPivot>> GetFirstAndLastTrackerAsync();
        Task<Tracker> AddTracker(string attr_name, string attr_value);
        Task<string> SaveTrackerAsync(List<Tracker> trackers);
        Task<bool> SaveTrackerStep3Async(List<Tracker> trackers, bool loadUserProfile = false);
        Task<bool> UpdateLatestTrackerAsync(double newValue, double oldValue, DateTime lastModifyDate);
        Task<string> DeleteTrackerAsync(string revisionNumber);
    }
}