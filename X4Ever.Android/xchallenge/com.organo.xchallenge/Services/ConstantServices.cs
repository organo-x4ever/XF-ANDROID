using com.organo.xchallenge.Services;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly:Dependency(typeof(ConstantServices))]

namespace com.organo.xchallenge.Services
{
    public class ConstantServices : IConstantServices
    {
        public async Task<string> Blogs() => await ClientService.GetStringAsync(new Uri(
            ClientService.GetRequestUri("constants",
                $"blogs?region={App.Configuration.GetApplication()}&lang={App.Configuration?.AppConfig.DefaultLanguage}")));
    }
}