using System.Net.Http;
using Plugin.Media.Abstractions;
using System.Threading.Tasks;

namespace com.organo.x4ever.Services
{
    public interface IFileService : IBaseService
    {
        Task<string> UploadFileAsync(MediaFile _mediaFile);

        Task<string> GetFileAsync(string fileIdentity);
        Task<HttpResponseMessage> UploadFileResponseAsync(MediaFile _mediaFile);
    }
}