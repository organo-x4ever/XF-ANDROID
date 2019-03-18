using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.organo.x4ever.Helpers
{
    public interface IDeviceInfo
    {
        string GetModel();
        string GetManufacturer();
        string GetVersionString();
        string GetPlatform();
    }
}