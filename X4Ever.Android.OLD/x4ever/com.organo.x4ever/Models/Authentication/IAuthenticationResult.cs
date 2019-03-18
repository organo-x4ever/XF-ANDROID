using System;
using System.Net.Http;
using System.Threading.Tasks;
using com.organo.x4ever.Services;

namespace com.organo.x4ever.Models.Authentication
{
    public interface IAuthenticationResult
    {
        /// <summary>
        /// Gets the Access Token requested.
        /// </summary>
        string AccessToken { get; set; }

        /// <summary>
        /// Gets the point in time in which the Access Token returned in the AccessToken property
        /// ceases to be valid. This value is calculated based on current UTC time measured locally
        /// and the value expiresIn received from the service.
        /// </summary>
        DateTimeOffset ExpiresOn { get; set; }

        /// <summary>
        /// Gives information to the developer whether token returned is during normal or extended lifetime.
        /// </summary>
        bool ExtendedLifeTimeToken { get; set; }

        /// <summary>
        /// Gets user information including user Id. Some elements in UserInfo might be null if not
        /// returned by the service.
        /// </summary>
        UserInfo UserInfo { get; set; }

        ///// <summary>
        ///// Creates authorization header from authentication result.
        ///// </summary>
        ///// <param name="httpMethod">
        ///// HttpMethod.Get to get data or HttpMethod.Post to post data
        ///// </param>
        ///// <param name="controller">
        ///// RESTful API controller name
        ///// </param>
        ///// <param name="method">
        ///// RESTful API method name
        ///// </param>
        ///// <returns>
        ///// returns Creates Authorization header with token
        ///// </returns>
        //HttpRequestMessage CreateAuthorizationHeader(HttpMethod httpMethod, string controller, string method);

        ///// <summary>
        ///// Sends Post request asynchronously.
        ///// </summary>
        ///// <param name="data">
        ///// Object to post data
        ///// </param>
        ///// <param name="controller">
        ///// RESTful API controller name
        ///// </param>
        ///// <param name="method">
        ///// RESTful API method name
        ///// </param>
        ///// <returns>
        ///// returns Response from Post Method
        ///// </returns>
        //Task<HttpResponseMessage> PostDataAsync(object data, string controller, string method);

        ///// <summary>
        ///// Sends Post request asynchronously.
        ///// </summary>
        ///// <param name="data">
        ///// Object to post data
        ///// </param>
        ///// <param name="controller">
        ///// RESTful API controller name
        ///// </param>
        ///// <param name="method">
        ///// RESTful API method name
        ///// </param>
        ///// <returns>
        ///// returns Response from Post Method
        ///// </returns>
        //Task<HttpResponseMessage> GetDataAsync(string controller, string methodWithParams);

        ///// <summary>
        ///// Gets Post header.
        ///// </summary>
        ///// <param name="data">
        ///// Object to post data
        ///// </param>
        ///// <returns>
        ///// returns Headers for Post request
        ///// </returns>
        //ByteArrayContent DataHeaderAsync(object data);
    }
}