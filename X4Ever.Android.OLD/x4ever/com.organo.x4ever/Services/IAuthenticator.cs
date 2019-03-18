using com.organo.x4ever.Models.Authentication;
using System.Threading.Tasks;

namespace com.organo.x4ever.Services
{
    public interface IAuthenticator
    {
        Task<AuthenticationResult> Authenticate(string authority, string resource, string clientid, string returnUri);

        Task<AuthenticationResult> Authenticate(string application_id, string client_id, string user_id, string password, string returnUri);

        Task<bool> DeAuthenticate(string authority);

        Task<string> FetchToken(string authority);
    }
}