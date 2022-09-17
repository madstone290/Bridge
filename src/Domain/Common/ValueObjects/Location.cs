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
        public const decimal MinLatitude = -90;
        public const decimal MaxLatitude = 90;
        public const decimal MinLongitude = -180;
        public const decimal MaxLongitude = 180;

        private Location() { }
        private Location(decimal latitude, decimal longitude)
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

        public static Location From(decimal latitude, decimal longitude)
        {
            return new Location(latitude, longitude);
        }


        public decimal Latitude { get; private set; }
        public decimal Longitude { get; private set; }

        protected override IEnumerable<object?> GetEqualityPropertyValues()
        {
            yield return Latitude;
            yield return Longitude;

        }
    }
}
