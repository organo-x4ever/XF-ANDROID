using com.organo.x4ever.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

[assembly: Dependency(typeof(GeoCodingService))]

namespace com.organo.x4ever.Services
{
    public class GeoCodingService : IGeoCodingService
    {
        private readonly Geocoder _GeoCoder;

        public GeoCodingService()
        {
            _GeoCoder = new Geocoder();
        }

        #region IGeoCodingService implementation

        public Position NullPosition
        {
            get { return new Position(0, 0); }
        }

        public async Task<Position> GeoCodeAddress(string addressString)
        {
            Position p = NullPosition;

            try
            {
                p = (await _GeoCoder.GetPositionsForAddressAsync(addressString)).FirstOrDefault();
            }
            catch (Exception ex)
            {
                var msg1 = ex;
                // TODO: log error
            }

            return p;
        }

        #endregion IGeoCodingService implementation
    }
}