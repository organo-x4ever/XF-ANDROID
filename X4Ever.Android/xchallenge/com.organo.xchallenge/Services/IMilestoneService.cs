using com.organo.xchallenge.Models.User;
using System.Collections.Generic;
using System.Threading.Tasks;
using com.organo.xchallenge.Models;

namespace com.organo.xchallenge.Services
{
    public interface IMilestoneService : IBaseService
    {
        Task<List<Milestone>> GetMilestoneAsync();
    }
}