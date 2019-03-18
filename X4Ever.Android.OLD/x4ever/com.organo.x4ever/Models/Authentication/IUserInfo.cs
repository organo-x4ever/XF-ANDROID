using System;

namespace com.organo.x4ever.Models.Authentication
{
    public interface IUserInfo
    {
        Int64 ID { get; set; }
        string UserLogin { get; set; }
        string UserFirstName { get; set; }
        string UserLastName { get; set; }
        string UserEmail { get; set; }
        string UserType { get; set; }
        DateTime UserRegistered { get; set; }
        string UserStatus { get; set; }
        string ProfileImage { get; set; }
        string DisplayName { get; }
        string FullName { get; }
    }
}