
using System.Collections.Generic;
using System.Threading.Tasks;

namespace com.organo.xchallenge.Statistic
{
    public interface IStatisticServices
    {
        List<string> Messages { get; set; }
        bool IsPermitted { get; set; }
        bool IsAllowed { get; set; }
        void Allow();
        Task Save(string page, string text);
        Task Send();
        Task Clear();
    }
}