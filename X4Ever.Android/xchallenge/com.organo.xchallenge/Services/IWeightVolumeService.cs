using com.organo.xchallenge.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace com.organo.xchallenge.Services
{
    internal interface IWeightVolumeService : IBaseService
    {
        Task<List<WeightVolume>> GetAsync();
    }
}