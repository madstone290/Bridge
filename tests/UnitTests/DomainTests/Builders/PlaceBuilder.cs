using Bridge.Domain.Common.ValueObjects;
using Bridge.Domain.Places.Entities;
using Bridge.Domain.Places.Enums;

namespace Bridge.UnitTests.DomainTests.Builders
{
    public class PlaceBuilder
    {
        public Place Build(string? name = null, Location? location = null)
        {
            var place = new Place(Guid.NewGuid().ToString(),
                PlaceType.Restaurant,
                name ?? Guid.NewGuid().ToString(),
                new AddressBuilder().DaeguAddress1,
                location ?? Location.Create(0,0,0,0));

            return place;
        }

    }
}
