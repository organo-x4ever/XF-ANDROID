
using System.Threading.Tasks;

namespace com.organo.xchallenge.Services
{
    public interface IConstantServices
    {
        Task<string> Blogs();
        Task<string> MoreWebLinks();
    }
}