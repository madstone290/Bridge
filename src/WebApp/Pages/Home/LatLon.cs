namespace Bridge.WebApp.Pages.Home
{
    /// <summary>
    /// 위도/경도
    /// </summary>
    public class LatLon
    {
        public LatLon(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        /// <summary>
        /// 위도
        /// </summary>
        public double Latitude { get; init; }

        /// <summary>
        /// 경도
        /// </summary>
        public double Longitude { get; init; }
    }
}
