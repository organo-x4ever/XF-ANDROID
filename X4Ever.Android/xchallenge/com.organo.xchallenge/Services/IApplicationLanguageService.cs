using com.organo.xchallenge.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace com.organo.xchallenge.Services
{
    public interface IApplicationLanguageService : IBaseService
    {
        Task<List<ApplicationLanguage>> GetAsync();

        Task<List<ApplicationLanguage>> GetWithCountryAsync();

        Task<List<ApplicationLanguage>> GetByCountryAsync(int countryID);

        Task<List<ApplicationLanguage>> GetByLanguageAsync(int languageID);
    }
}