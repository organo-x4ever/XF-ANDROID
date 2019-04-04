
using System;

namespace com.organo.xchallenge.Models.Authentication
{
    /// <summary>
    /// Contains the results of one token acquisition operation.
    /// </summary>
    public sealed class AuthenticationResult : IAuthenticationResult
    {
        public AuthenticationResult()
        {
            UserInfo = new UserInfo();
        }

        /// <summary>
        /// Gets the Access Token requested.
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// Gets the point in time in which the Access Token returned in the AccessToken property
        /// ceases to be valid. This value is calculated based on current UTC time measured locally
        /// and the value expiresIn received from the service.
        /// </summary>
        public DateTimeOffset ExpiresOn { get; set; }

        /// <summary>
        /// Gives information to the developer whether token returned is during normal or extended lifetime.
        /// </summary>
        public bool ExtendedLifeTimeToken { get; set; }

        /// <summary>
        /// Gets user information including user Id. Some elements in UserInfo might be null if not
        /// returned by the service.
        /// </summary>
        public UserInfo UserInfo { get; set; }
    }
}