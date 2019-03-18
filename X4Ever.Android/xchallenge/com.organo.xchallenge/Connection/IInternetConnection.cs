using System.Threading.Tasks;

namespace com.organo.xchallenge.Connection
{
    public interface IInternetConnection
    {
        bool Check();

        Task<bool> CheckAsync();
    }
}