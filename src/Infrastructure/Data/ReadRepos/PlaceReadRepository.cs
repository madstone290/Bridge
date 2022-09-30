using Bridge.Application.Places.Dtos;
using Bridge.Application.Places.ReadModels;
using Bridge.Application.Places.Repos;
using Bridge.Domain.Places.Entities;
using System.Linq.Expressions;

namespace Bridge.Infrastructure.Data.ReadRepos
{
    public class PlaceReadRepository : ReadRepository<Place, PlaceReadModel>, IPlaceReadRepository
    {
        public PlaceReadRepository(BridgeContext context) : base(context)
        {
        }

        protected override Expression<Func<Place, PlaceReadModel>> SelectExpression { get; } = x => new PlaceReadModel()
        {
            Id = x.Id,
            Type = x.Type,
            Name = x.Name,
            Address =  new AddressDto()
            {
                BaseAddress = x.Address.RoadAddress,
                Details = x.Address.Details
            },
            Location = new PlaceLocationDto()
            {

                Latitude = x.Location.Latitude,
                Longitude = x.Location.Longitude,
                Easting = x.Location.Easting,
                Northing = x.Location.Northing
            },
            Categories = x.Categories.ToList(),
            ContactNumber = x.ContactNumber,
            OpeningTimes = x.OpeningTimes.Select(t => new OpeningTimeDto()
            {
                Day = t.Day,
                Dayoff = t.Dayoff,
                TwentyFourHours = t.TwentyFourHours,
                OpenTime = t.OpenTime,
                CloseTime = t.CloseTime,
                BreakEndTime = t.BreakEndTime,
                BreakStartTime = t.BreakStartTime
            }).ToList()
        };

        public async Task<PlaceReadModel?> GetPlaceAsync(long id)
        {
            return (await FilterAsync(x => x.Id == id))
                .FirstOrDefault();
        }
      
    }
}

