using com.organo.x4ever.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace com.organo.x4ever.Services
{
    public interface IApplicationLanguageService : IBaseService
    {
        Task<List<ApplicationLanguage>> GetAsync();

        Task<List<ApplicationLanguage>> GetWithCountryAsync();

        Task<List<ApplicationLanguage>> GetByCountryAsync(int countryID);

        Task<List<ApplicationLanguage>> GetByLanguageAsync(int languageID);
    }
}