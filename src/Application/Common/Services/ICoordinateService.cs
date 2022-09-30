namespace Bridge.Application.Common.Services
{
    /// <summary>
    /// 좌표 서비스
    /// </summary>
    public interface ICoordinateService
    {
        /// <summary>
        /// 위/경도를 UtmK 좌표로 변환한다.
        /// x,y 순서에 유의할 것. 경도, 위도 순서.
        /// </summary>
        /// <param name="longitude">x축 좌표(경도)</param>
        /// <param name="latitude">y축 좌표(위도)</param>
        /// <returns>동향/북향 쌍</returns>
        Tuple<double, double> ConvertToUtmK(double longitude, double latitude);
    }
}
