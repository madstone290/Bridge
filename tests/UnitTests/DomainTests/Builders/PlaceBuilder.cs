using Bridge.Domain.Common.ValueObjects;
using Bridge.Domain.Places.Entities;
using Bridge.Domain.Users.Entities;

namespace Bridge.UnitTests.DomainTests.Builders
{
    public class PlaceBuilder
    {
        public Place Build(User user, string? name = null, PlaceLocation? location = null)
        {
            var place = Place.Create(
                user,
                name ?? Guid.NewGuid().ToString(),
                Guid.NewGuid().ToString(),
                location ?? PlaceLocation.Create(0,0,0,0));

            return place;
        }

    }
}
