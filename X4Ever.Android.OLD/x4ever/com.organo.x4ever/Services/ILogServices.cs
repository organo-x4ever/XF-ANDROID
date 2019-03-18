using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.organo.x4ever.Services
{
  public  interface ILogServices : IBaseService
    {
      Task WriteLog(string title, string message, bool showMessage);
      Task WriteLog(Uri requestUri, Exception exception, bool showMessage = false);
      Task WriteDebug(string debugLog);
    }
}
