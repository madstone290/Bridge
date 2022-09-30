using Bridge.Application.Common;
using Bridge.Application.Common.Services;
using Bridge.Domain.Common.ValueObjects;
using Bridge.Infrastructure.NaverMaps;

namespace Bridge.Infrastructure.Services
{
    public class AddressLocationService : IAddressLocationService
    {
        private readonly GeoCodeApi _geoCodeApi;

        public AddressLocationService(GeoCodeApi geoCodeApi)
        {
            _geoCodeApi = geoCodeApi;
        }

        public async Task<Tuple<Address, PlaceLocation>> CreateAddressLocationAsync(string baseAddress, string details)
        {
            var responseBody = await _geoCodeApi.GetAddressInfo(baseAddress);
            if (responseBody.Status != NaverMaps.Data.GeoCodeResponseBody.StatusOk)
                throw new AppException("유효한 주소가 아닙니다");

            if (responseBody.Meta.TotalCount != 1)
                throw new AppException("유효한 주소가 아닙니다");

            var addressInfo = responseBody.Address;
            var address = Address.Create(addressInfo.RoadAddress,
                                  addressInfo.JibunAddress,
                                  details,
                                  addressInfo.SIDO,
                                  addressInfo.SIGUGUN,
                                  addressInfo.DONGMYUN,
                                  addressInfo.ROAD_NAME,
                                  addressInfo.POSTAL_CODE);

            var latitude = double.Parse(addressInfo.Y);
            var longitude = double.Parse(addressInfo.X);
            var easting = 100000;
            var northing = 200000;
            var location = PlaceLocation.Create(latitude, longitude, easting, northing);

            return new Tuple<Address, PlaceLocation>(address, location);
        }
    }
}
