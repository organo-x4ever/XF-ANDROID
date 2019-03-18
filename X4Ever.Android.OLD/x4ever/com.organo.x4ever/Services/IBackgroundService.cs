using System;
using System.Threading.Tasks;

namespace com.organo.x4ever.Services
{
    public interface IBackgroundService
    {
        Task RunTask();

        Task<TimeSpan> CurrentTimeAsync();
    }
}