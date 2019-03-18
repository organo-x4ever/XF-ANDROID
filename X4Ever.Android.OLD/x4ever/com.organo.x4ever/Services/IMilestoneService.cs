using com.organo.x4ever.Models.User;
using System.Collections.Generic;
using System.Threading.Tasks;
using com.organo.x4ever.Models;

namespace com.organo.x4ever.Services
{
    public interface IMilestoneService : IBaseService
    {
        Task<List<Milestone>> GetMilestoneAsync();
    }
}