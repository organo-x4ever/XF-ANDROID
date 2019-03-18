using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.organo.xchallenge.Models;
using com.organo.xchallenge.Models.Authentication;
using com.organo.xchallenge.Models.User;

namespace com.organo.xchallenge.Services
{
    public interface IUserPivotService : IBaseService
    {
        string ControllerNameUnauthorized { get; }
        string Message { get; set; }

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
        Task<UserPivot> GetFullAsync();

        /// <summary>
        /// Get Asynchronous User Information
        /// </summary>
        /// <returns>
        /// </returns>
        Task GetAuthenticationAsync(Action callbackSuccess, Action callbackFailed);

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

        /// <summary>
        /// To update user first time send user object
        /// </summary>
        /// <param name="user">
        /// UserFirstUpdate
        /// </param>
        /// <param name="callbackSuccess">If successfully saved</param>
        /// <param name="callbackFailed">Is saving data failed</param>
        /// <returns>
        /// string (success/error)
        /// </returns>
        Task<bool> UpdateStep1Async(UserStep1 user);
    }
}