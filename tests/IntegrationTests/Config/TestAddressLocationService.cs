using Bridge.Application.Common.Services;
using Bridge.Domain.Common.ValueObjects;

namespace Bridge.IntegrationTests.Config
{
    public class TestAddressLocationService : IAddressLocationService
    {
        public Task<Tuple<Address, Location>> CreateAddressLocationAsync(string baseAddress, string detailAddress)
        {
            var utmIndex = baseAddress.IndexOf("utm:");
            var easting = 100000d;
            var northing = 200000d;
            if (0 <= utmIndex)
            {
                var eastingNorthingStr = baseAddress.Substring(utmIndex + 4, baseAddress.Length - (utmIndex + 4));
                var eastingNorthing = eastingNorthingStr.Split(",");
                easting = double.Parse(eastingNorthing[0]);
                northing = double.Parse(eastingNorthing[1]);
            }
           

            var address = Address.Create(baseAddress, baseAddress, detailAddress, "", "", "", "", "");
            var location = Location.Create(32.0000, 127.0000, easting, northing);

            return Task.FromResult(new Tuple<Address, Location>(address, location));
        }
    }
}
