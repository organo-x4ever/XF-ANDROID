using System.Threading.Tasks;
using Xamarin.Forms.Maps;

namespace com.organo.x4ever.Services
{
    public interface IGeoCodingService
    {
        Position NullPosition { get; }

        Task<Position> GeoCodeAddress(string addressString);
    }
}