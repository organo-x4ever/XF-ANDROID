using System.Collections.Generic;

namespace com.organo.x4ever.Models.Authentication
{
    public interface IAuthenticationResults
    {
        List<AuthenticationResult> AuthenticationResultList { get; set; }

        void Add(AuthenticationResult result);

        int Count();

        bool Exists();

        void Clear(AuthenticationResult result);

        void ClearAll();

        List<AuthenticationResult> Get();

        AuthenticationResult Get(int index);

        AuthenticationResult Get(string token);

        AuthenticationResult GetFirst();

        bool CheckExists(string username);

        AuthenticationResult GetByFirstName(string firstName);

        AuthenticationResult GetByFullName(string fullName);

        AuthenticationResult GetByDisplayName(string displayName);

        AuthenticationResult Get(long id);
    }
}