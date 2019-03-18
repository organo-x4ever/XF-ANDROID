using System.Net.Http;
using Plugin.Media.Abstractions;
using System.Threading.Tasks;

namespace com.organo.x4ever.Globals
{
    public interface IMedia
    {
        string Message { get; }
        string FileName { get; }

        void Refresh();

        Task<MediaFile> PickPhotoAsync();

        Task<MediaFile> TakePhotoAsync();

        Task<MediaFile> UploadPhotoAsync(MediaFile mediaFile, bool takenPhoto = false);
        Task<HttpResponseMessage> UploadPhotoResponseAsync(MediaFile mediaFile);
        Task<bool> UploadPhotoAsync(MediaFile mediaFile);
    }
}