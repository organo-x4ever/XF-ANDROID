using com.organo.xchallenge.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace com.organo.xchallenge.Services
{
    public interface IMealPlanService : IBaseService
    {
        Task<List<MealPlanDetail>> GetDetailAsync();

        Task<List<MealPlanDetail>> GetDetailAsync(bool active);
    }
}