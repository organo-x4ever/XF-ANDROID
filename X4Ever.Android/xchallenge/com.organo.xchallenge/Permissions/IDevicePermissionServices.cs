using System.Threading.Tasks;

namespace com.organo.xchallenge.Permissions
{
    public interface IDevicePermissionServices
    {
        Task<bool> RequestCameraPermission();

        Task<bool> RequestReadStoragePermission();

        Task<bool> RequestWriteStoragePermission();
    }
}