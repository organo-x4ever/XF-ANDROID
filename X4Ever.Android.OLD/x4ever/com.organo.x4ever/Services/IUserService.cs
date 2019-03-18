using com.organo.x4ever.Models;
using com.organo.x4ever.Models.Authentication;
using com.organo.x4ever.Models.User;
using System.Threading.Tasks;

namespace com.organo.x4ever.Services
{
    public interface IUserService:IBaseService
    {
        /// <summary>
        /// Get Asynchronous User Information
        /// </summary>
        /// <returns>
        /// </returns>
        Task<UserInfo> GetAsync();

        /// <summary>
        /// Get Asynchronous User Information
        /// </summary>
        /// <returns>
        /// </returns>
        Task<UserWithDetail> GetFullAsync();

        /// <summary>
        /// Get Asynchronous User Information
        /// </summary>
        /// <returns>
        /// </returns>
        Task<bool> GetAuthenticationAsync();

        /// <summary>
        /// Register User
        /// </summary>
        /// <returns>
        /// </returns>
        Task<string> RegisterAsync(UserRegister user);

        /// <summary>
        /// Change User Password
        /// </summary>
        /// <param name="currentPassword">
        /// Password already used to login for current session
        /// </param>
        /// <param name="newPassword">
        /// New Desired Password
        /// </param>
        /// <returns>
        /// </returns>
        Task<string> ChangePasswordAsync(string currentPassword, string newPassword);

        /// <summary>
        /// Request User Password
        /// </summary>
        /// <param name="username">
        /// Username (Login name)
        /// </param>
        /// <param name="newPassword">
        /// New Desired Password
        /// </param>
        /// <returns>
        /// </returns>
        Task<string> RequestForgotPasswordAsync(string username, string email);

        /// <summary>
        /// Provide Request Password Code
        /// </summary>
        /// <param name="requestCode">
        /// Unique Code sent to email.
        /// </param>
        /// <param name="newPassword">
        /// New Desired Password
        /// </param>
        /// <returns>
        /// </returns>
        Task<string> ChangeForgotPasswordAsync(string requestCode, string password);

        ///// <summary>
        ///// To update user send user object
        ///// </summary>
        ///// <param name="user">
        ///// Object
        ///// </param>
        ///// <returns>
        ///// string (success/error)
        ///// </returns>
        //Task<string> UpdateAsync(UserRegister user);

        /// <summary>
        /// To update user first time send user object
        /// </summary>
        /// <param name="user">
        /// UserFirstUpdate
        /// </param>
        /// <returns>
        /// string (success/error)
        /// </returns>
        Task<string> UpdateAsync(UserFirstUpdate user);
    }
}