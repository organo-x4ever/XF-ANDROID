using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace com.organo.x4ever.Services
{
    public interface IResourceService
    {
        void Add(string key, object value);
        void Add(Style value);
        bool Exists(string key);
        void Delete(string key);
        void ClearAll();
    }
}