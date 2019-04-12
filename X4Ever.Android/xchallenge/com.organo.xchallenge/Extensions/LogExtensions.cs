using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.organo.xchallenge.Handler;
using com.organo.xchallenge.Statics;

namespace com.organo.xchallenge.Extensions
{
    public static class LogExtensions
    {
        public static async Task LogExceptionsAsync(this Task task)
        {
            await task.ContinueWith(t =>
                {
                    var aggException = t.Exception.Flatten();
                    if (aggException != null)
                    {
                        var exceptionHandler = new ExceptionHandler(typeof(LogExtensions).FullName, aggException);
                    }
                },
                TaskContinuationOptions.OnlyOnFaulted);
        }


        public static void LogExceptions(this Task task)
        {
            task.ContinueWith(t =>
                {
                    var aggException = t.Exception.Flatten();
                    if (aggException != null)
                    {
                        var exceptionHandler = new ExceptionHandler(typeof(LogExtensions).FullName, aggException);
                    }
                },
                TaskContinuationOptions.OnlyOnFaulted);
        }
    }

    public interface ILogExtensions<T>
    {
        Task<T> LogExceptionsAsync(Task task, Type type);
    }
}