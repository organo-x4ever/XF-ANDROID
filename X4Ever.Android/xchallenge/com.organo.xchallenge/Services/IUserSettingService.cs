using com.organo.xchallenge.Models;
using com.organo.xchallenge.Models.User;
using System.Threading.Tasks;

namespace com.organo.xchallenge.Services
{
    public interface IUserSettingService : IBaseService
    {
        Task<UserSetting> GetAsync();

        Task<string> SaveAsync(UserSetting userSetting);

        Task<string> UpdateUserLanguageAsync(ApplicationLanguageRequest applicationLanguage);

        Task<string> UpdateUserWeightVolumeAsync(UserWeightVolume weightVolume);
    }
}