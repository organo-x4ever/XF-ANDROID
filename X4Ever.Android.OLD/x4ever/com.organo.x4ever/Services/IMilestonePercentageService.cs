using com.organo.x4ever.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace com.organo.x4ever.Services
{
    public interface IMilestonePercentageService : IBaseService
    {
        Task<List<MilestonePercentage>> GetByLanguageAsync(string languageCode);
    }
}