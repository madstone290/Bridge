using Bridge.Domain.Common.ValueObjects;
using Bridge.Domain.Places.Entities;

namespace Bridge.UnitTests.DomainTests.Builders
{
    public class PlaceBuilder
    {
        public Place Build(string? name = null, PlaceLocation? location = null)
        {
            var place = Place.Create(
                PlaceType.Restaurant,
                name ?? Guid.NewGuid().ToString(),
                new AddressBuilder().DaeguAddress1,
                location ?? PlaceLocation.Create(0,0,0,0));

            return place;
        }

    }
}
