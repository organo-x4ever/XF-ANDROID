using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.organo.x4ever.Services;
using Xamarin.Forms;

[assembly:Dependency(typeof(ResourceService))]

namespace com.organo.x4ever.Services
{
    public class ResourceService : IResourceService
    {
        public void Add(string key, object value)
        {
            try
            {
                if (this.Exists(key))
                    this.Delete(key);
                App.CurrentApp.Resources.Add(key, value);
            }
            catch (Exception ex)
            {
                var msg = ex;
            }
        }

        public void Add(Style value)
        {
            try
            {
                App.CurrentApp.Resources.Add(value);
            }
            catch (Exception ex)
            {
                var msg = ex;
            }
        }

        public void ClearAll()
        {
            App.CurrentApp.Resources.Clear();
        }

        public void Delete(string key)
        {
            if (this.Exists(key))
                App.CurrentApp.Resources.Remove(key);
        }

        public bool Exists(string key)
        {
            return App.CurrentApp.Resources.ContainsKey(key);
        }
    }
}