using System.Threading.Tasks;

namespace com.organo.x4ever.Permissions
{
    public interface IDevicePermissionServices
    {
        Task<bool> RequestCameraPermission();

        Task<bool> RequestReadStoragePermission();

        Task<bool> RequestWriteStoragePermission();
    }
}