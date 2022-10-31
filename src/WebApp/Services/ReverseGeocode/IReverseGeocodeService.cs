using Bridge.Shared;

namespace Bridge.WebApp.Services.ReverseGeocode
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
        Task<Result<string>> GetAddressAsync(double latitude, double longitude);
    }

}
