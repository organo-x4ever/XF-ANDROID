using System.Net.Http;

namespace com.organo.x4ever.Services
{
    public interface IHttpClientHandlerFactory
    {
        HttpClientHandler GetHttpClientHandler();
    }
}