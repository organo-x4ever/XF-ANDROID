using com.organo.x4ever.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace com.organo.x4ever.Services
{
    internal interface IWeightVolumeService : IBaseService
    {
        Task<List<WeightVolume>> GetAsync();
    }
}