using System.Collections.Generic;
using com.organo.x4ever.Models.Authentication;
using System.Net.Http;
using System.Threading.Tasks;

namespace com.organo.x4ever.Services
{
    public interface IAuthenticationService
    {
        /// <summary>
        /// User Authentication using username and password
        /// </summary>
        /// <param name="username">
        /// User login username
        /// </param>
        /// <param name="password">
        /// User login password
        /// </param>
        /// <returns>
        /// </returns>
        Task<bool> AuthenticationAsync(string username, string password);

        Task<AuthenticationResult> GetDetailAsync(HttpResponseMessage response);

        /// <summary>
        /// User logout from current application
        /// </summary>
        /// <returns>
        /// </returns>
        Task LogoutAsync();

        /// <summary>
        /// Check user authentication on server
        /// </summary>
        bool IsAuthenticated { get; }

        string Message { get; set; }
    }
}