using System.Net.Http;

namespace com.organo.xchallenge.Services
{
    public interface IHttpClientHandlerFactory
    {
        HttpClientHandler GetHttpClientHandler();
    }
}