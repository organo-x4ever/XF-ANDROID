using System;
using System.Threading.Tasks;

namespace com.organo.xchallenge.Services
{
    public interface IBackgroundService
    {
        Task RunTask();

        Task<TimeSpan> CurrentTimeAsync();
    }
}