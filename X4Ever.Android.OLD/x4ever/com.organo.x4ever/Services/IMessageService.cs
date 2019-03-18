using System.Threading.Tasks;

namespace com.organo.x4ever.Services
{
    public interface IMessageService : IBaseService
    {
        Task<bool> SendEmailAsync(string token, string subject, string body);
    }
}