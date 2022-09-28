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
                Guid.NewGuid().ToString(),
                location ?? PlaceLocation.Create(0,0,0,0));

            return place;
        }

    }
}
