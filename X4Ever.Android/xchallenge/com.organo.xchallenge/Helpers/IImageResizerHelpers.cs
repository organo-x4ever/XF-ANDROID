
using System.IO;
using System.Threading.Tasks;
using com.organo.xchallenge.Models;

namespace com.organo.xchallenge.Helpers
{
    public interface IImageResizerHelpers
    {
        Stream GetStream(string fileName);
        byte[] ResizeImage(string fileName, float width, float height);
        Task<byte[]> ResizeImageAsync(string fileName, float width, float height);
        byte[] ResizeImage(byte[] imageData, float width, float height);
        Task<byte[]> ResizeImageAsync(byte[] imageData, float width, float height);
        byte[] ResizeImage(string fileName, ImageSize imageSize);
        Task<byte[]> ResizeImageAsync(string fileName, ImageSize imageSize);
        byte[] ResizeImage(byte[] imageData, ImageSize imageSize);
        Task<byte[]> ResizeImageAsync(byte[] imageData, ImageSize imageSize);
        byte[] ResizeImage(ImageSize imageSize);
        Task<byte[]> ResizeImageAsync(ImageSize imageSize);
        byte[] ResizeImageFromRemote(ImageSize imageSize);
        Task<byte[]> ResizeImageFromRemoteAsync(ImageSize imageSize);
        byte[] ImageToBytes(ImageSize imageSize);
        Task<byte[]> ImageToBytesAsync(ImageSize imageSize);
    }
}
