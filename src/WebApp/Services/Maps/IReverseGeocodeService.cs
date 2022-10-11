namespace Bridge.WebApp.Services.Maps
{
    /// <summary>
    /// 역좌표변환 서비스.
    /// 좌표를 주소로 변환한다.
    /// </summary>
    public interface IReverseGeocodeService
    {
        /// <summary>
        /// 주어진 위/경도의 주소를 반환한다
        /// </summary>
        /// <param name="latitude">위도</param>
        /// <param name="longitude">경도</param>
        /// <returns>주소</returns>
        Task<string> GetAddressAsync(double latitude, double longitude);
    }

    public class NaverReverseGeocodeService : IReverseGeocodeService
    {
        private readonly NaverReverseGeocodeApi _api;

        public NaverReverseGeocodeService(NaverReverseGeocodeApi api)
        {
            _api = api;
        }

        public async Task<string> GetAddressAsync(double latitude, double longitude)
        {
            var body = await _api.GetAddressResult(latitude, longitude);

            var region = body.Results[0].Region;
            return $"{region.Area1.Name} {region.Area2.Name} {region.Area3.Name} {region.Area4.Name}".Trim();
        }
    }
}
