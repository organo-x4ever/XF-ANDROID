using com.organo.xchallenge.Models.Media;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace com.organo.xchallenge.Services
{
    public interface IMediaContentService
    {
        Task<List<MediaContent>> GetAsync();

        Task<List<MediaContentDetail>> GetDetailAsync();
    }
}