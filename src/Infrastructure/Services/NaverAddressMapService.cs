using Bridge.Application.Common.Services;

namespace Bridge.Infrastructure.Services
{
    /// <summary>
    /// 네이버 API를 이용하여 주소의 지리정보를 불러온다.
    /// </summary>
    public class NaverAddressMapService : IAddressMapService
    {
        public Task<Tuple<double, double>> GetLatitudeAndLongitudeAsync(string address)
        {
            throw new NotImplementedException();
        }

        public Task<Tuple<double, double>> GetUTM_K_EastingAndNorthingAsync(string address)
        {
            throw new NotImplementedException();
        }
    }
}
