namespace Bridge.WebApp.Services.GeoLocation
{
    /// <summary>
    /// Geolocation api 위치
    /// </summary>
    public class GeoPoint
    {
        public GeoPoint(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        public double Latitude { get; }
        public double Longitude { get; }
    }
}
