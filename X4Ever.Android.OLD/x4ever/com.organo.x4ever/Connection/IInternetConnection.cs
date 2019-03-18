using System.Threading.Tasks;

namespace com.organo.x4ever.Connection
{
    public interface IInternetConnection
    {
        bool Check();

        Task<bool> CheckAsync();
    }
}