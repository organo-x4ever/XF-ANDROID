using System.Threading.Tasks;

namespace com.organo.xchallenge.Services
{
    public interface IMessageService : IBaseService
    {
        Task<bool> SendEmailAsync(string token, string subject, string body);
    }
}