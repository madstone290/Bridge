using Bridge.Domain.Common.Exceptions;

namespace Bridge.Domain.Common.ValueObjects
{
    /// <summary>
    /// 위치.
    /// 위도, 경도 및 UTM-K도법의 좌표를 가진다.
    /// </summary>
    public class Location : ValueObject
    {
        public const double MinLatitude = -90;
        public const double MaxLatitude = 90;
        public const double MinLongitude = -180;
        public const double MaxLongitude = 180;

        private Location() { }
        private Location(double latitude, double longitude, double easting, double northing)
        {
            if (latitude < MinLatitude || MaxLatitude < latitude)
            {
                throw new InvalidLocationException();
            }

            if (longitude < MinLongitude || MaxLongitude < longitude)
            {
                throw new InvalidLocationException();
            }

            Latitude = latitude;
            Longitude = longitude;
            Easting = easting;
            Northing = northing;
        }

        public static Location Default() => new(0, 0, 0, 0);

        /// <summary>
        /// 위치를 생성한다
        /// </summary>
        /// <param name="latitude">위도</param>
        /// <param name="longitude">경도</param>
        /// <param name="easting">UTM-K도법의 동쪽방향 좌표</param>
        /// <param name="northing">UTM-K도법의 북쪽방향 좌표</param>
        /// <returns></returns>
        public static Location Create(double latitude, double longitude, double easting, double northing)
        {
            return new Location(latitude, longitude, easting, northing);
        }

        /// <summary>
        /// 위도
        /// </summary>
        public double Latitude { get; private set; }

        /// <summary>
        /// 경도
        /// </summary>
        public double Longitude { get; private set; }

        /// <summary>
        /// UTM-K도법의 동쪽방향 좌표
        /// </summary>
        public double Easting { get; private set; }

        /// <summary>
        /// UTM-K도법의 북쪽방향 좌표
        /// </summary>
        public double Northing { get; private set; }

        protected override IEnumerable<object?> GetEqualityPropertyValues()
        {
            yield return Latitude;
            yield return Longitude;

        }
    }
}
