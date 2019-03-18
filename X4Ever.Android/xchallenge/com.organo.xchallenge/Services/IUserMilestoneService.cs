using com.organo.xchallenge.Models.User;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace com.organo.xchallenge.Services
{
    public interface IUserMilestoneService
    {
        Task<List<UserMilestone>> GetUserMilestoneAsync();

        Task<UserMilestoneExtended> GetExtendedAsync();

        Task<UserMilestoneExtended> GetExtendedAsync(string languageCode);

        Task<Dictionary<string, object>> GetDetailAsync();

        Task<string> SaveUserMilestoneAsync(UserMilestone userMilestone);
    }
}