using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.organo.xchallenge.Helpers
{
    public interface IDeviceInfo
    {
        string GetModel { get; }
        string GetManufacturer { get; }
        string GetVersionString { get; }
        string GetPlatform { get; }
        string GetAppName { get; }
    }
}