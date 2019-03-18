using com.organo.x4ever.Models;
using com.organo.x4ever.Models.User;
using System.Threading.Tasks;

namespace com.organo.x4ever.Services
{
    public interface IUserSettingService : IBaseService
    {
        Task<UserSetting> GetAsync();

        Task<string> SaveAsync(UserSetting userSetting);

        Task<string> UpdateUserLanguageAsync(ApplicationLanguageRequest applicationLanguage);

        Task<string> UpdateUserWeightVolumeAsync(UserWeightVolume weightVolume);
    }
}