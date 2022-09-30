using Bridge.Application.Common;
using Bridge.Application.Common.Services;
using Bridge.Domain.Common.ValueObjects;
using Bridge.Infrastructure.NaverMaps;

namespace Bridge.Infrastructure.Services
{
    public class AddressLocationService : IAddressLocationService
    {
        private readonly GeoCodeApi _geoCodeApi;
        private readonly ICoordinateService _coordinateService;

        public AddressLocationService(GeoCodeApi geoCodeApi, ICoordinateService coordinateService)
        {
            _geoCodeApi = geoCodeApi;
            _coordinateService = coordinateService;
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

            var longitude = double.Parse(addressInfo.X);
            var latitude = double.Parse(addressInfo.Y);
            var utm_k = _coordinateService.ConvertToUtmK(longitude, latitude);
            var easting = utm_k.Item1;
            var northing = utm_k.Item2;

            var location = PlaceLocation.Create(latitude, longitude, easting, northing);

            return new Tuple<Address, PlaceLocation>(address, location);
        }
    }
}
