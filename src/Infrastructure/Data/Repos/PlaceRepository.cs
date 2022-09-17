using Bridge.Domain.Places.Entities;
using Bridge.Domain.Places.Repos;

namespace Bridge.Infrastructure.Data.Repos
{
    public class PlaceRepository : Repository<Place>, IPlaceRepository
    {
        public PlaceRepository(BridgeContext context) : base(context)
        {
        }
    }
}
