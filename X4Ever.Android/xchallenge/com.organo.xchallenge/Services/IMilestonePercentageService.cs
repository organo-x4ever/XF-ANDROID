using com.organo.xchallenge.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace com.organo.xchallenge.Services
{
    public interface IMilestonePercentageService : IBaseService
    {
        Task<List<MilestonePercentage>> GetByLanguageAsync(string languageCode);
    }
}