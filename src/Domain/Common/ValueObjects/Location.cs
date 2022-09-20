using Bridge.Domain.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bridge.Domain.Common.ValueObjects
{
    public class Location : ValueObject
    {
        public const double MinLatitude = -90;
        public const double MaxLatitude = 90;
        public const double MinLongitude = -180;
        public const double MaxLongitude = 180;

        private Location() { }
        private Location(double latitude, double longitude)
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
        }

        public static Location Default() => new(0, 0);

        public static Location From(double latitude, double longitude)
        {
            return new Location(latitude, longitude);
        }


        public double Latitude { get; private set; }
        public double Longitude { get; private set; }

        protected override IEnumerable<object?> GetEqualityPropertyValues()
        {
            yield return Latitude;
            yield return Longitude;

        }
    }
}
