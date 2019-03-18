
using System;
using com.organo.x4ever.Services;

namespace com.organo.x4ever.Handler
{
    public class ExceptionHandler : Exception
    {
        public ExceptionHandler()
        {

        }

        public ExceptionHandler(string message) : base(message)
        {
            WriteLog(message);
        }

        public ExceptionHandler(string message, Exception exception) : base(message, exception)
        {
            WriteLog(message, exception);
        }

        private async void WriteLog(string message, Exception exception = null)
        {
            await ClientService.WriteLog(null, message, exception);
        }
    }
}