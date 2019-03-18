using com.organo.x4ever.Models.Media;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace com.organo.x4ever.Services
{
    public interface IMediaContentService
    {
        Task<List<MediaContent>> GetAsync();

        Task<List<MediaContentDetail>> GetDetailAsync();
    }
}