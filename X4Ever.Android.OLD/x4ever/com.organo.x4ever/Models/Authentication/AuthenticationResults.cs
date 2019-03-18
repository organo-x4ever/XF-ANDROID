using System.Collections.Generic;
using System.Linq;

namespace com.organo.x4ever.Models.Authentication
{
    public class AuthenticationResults : IAuthenticationResults
    {
        public AuthenticationResults()
        {
            AuthenticationResultList = new List<AuthenticationResult>();
        }

        public List<AuthenticationResult> AuthenticationResultList { get; set; }

        public void Add(AuthenticationResult result)
        {
            AuthenticationResultList.Add(result);
        }

        public void Clear(AuthenticationResult result)
        {
            AuthenticationResultList.Remove(result);
        }

        public void ClearAll()
        {
            AuthenticationResultList.Clear();
            AuthenticationResultList = new List<AuthenticationResult>();
        }

        public int Count()
        {
            return AuthenticationResultList.Count;
        }

        public bool Exists()
        {
            return AuthenticationResultList.Count > 0;
        }

        public List<AuthenticationResult> Get()
        {
            return AuthenticationResultList;
        }

        public AuthenticationResult Get(int index)
        {
            return AuthenticationResultList[index];
        }

        public AuthenticationResult Get(long id)
        {
            return AuthenticationResultList.FirstOrDefault(r => r.UserInfo.ID == id);
        }

        public AuthenticationResult Get(string token)
        {
            return AuthenticationResultList.FirstOrDefault(r => r.AccessToken == token);
        }

        public AuthenticationResult GetFirst()
        {
            if (this.Count() > 0)
            {
                var authenticationResult = AuthenticationResultList.FirstOrDefault();
                if (authenticationResult != null)
                    return authenticationResult;
            }
            return null;
        }

        public AuthenticationResult GetByFirstName(string firstName)
        {
            return AuthenticationResultList.FirstOrDefault(r => r.UserInfo.UserFirstName == firstName);
        }

        public AuthenticationResult GetByFullName(string fullName)
        {
            return AuthenticationResultList.FirstOrDefault(r => r.UserInfo.FullName == fullName);
        }

        public AuthenticationResult GetByDisplayName(string displayName)
        {
            return AuthenticationResultList.FirstOrDefault(r => r.UserInfo.DisplayName == displayName);
        }

        public bool CheckExists(string username)
        {
            return AuthenticationResultList.Any(r => r.UserInfo.UserLogin.Trim().ToLower() == username.Trim().ToLower());
        }
    }
}