using Bridge.Application.Places.Dtos;
using Bridge.Application.Places.ReadModels;
using Bridge.Application.Places.Repos;
using Bridge.Domain.Places.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bridge.Infrastructure.Data.ReadRepos
{
    public class PlaceReadRepository : Repository<Place>, IPlaceReadRepository
    {
        public PlaceReadRepository(BridgeContext context) : base(context)
        {
        }

        public async Task<PlaceReadModel?> GetPlaceAsync(long id)
        {
            return await Set
                .Where(x => x.Id == id)
                .SelectPlaceReadModel()
                .FirstOrDefaultAsync();
        }

        public async Task<List<PlaceReadModel>> GetPlacesByEastingBetweenAndNorthingBetween(double leftEasting,
                                                                                            double rightEasting,
                                                                                            double bottomNorthing,
                                                                                            double topNorthing)
        {
            return await Set
               .Where(x => leftEasting <= x.Location.Easting  && x.Location.Easting <= rightEasting)
               .Where(x => bottomNorthing <= x.Location.Northing && x.Location.Northing <= topNorthing)
               .SelectPlaceReadModel()
               .ToListAsync();
        }
    }

    public static class PlaceReadRepositoryExtensions
    {
        public static IQueryable<PlaceReadModel> SelectPlaceReadModel(this IQueryable<Place> query)
        {
            return query
                .Select(x => new PlaceReadModel()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Address = x.Address,
                    Location = new PlaceLocationDto()
                    {
                       
                        Latitude = x.Location.Latitude,
                        Longitude = x.Location.Longitude,
                        Easting = x.Location.Easting,
                        Northing = x.Location.Northing
                    },
                    Categories = x.Categories.ToList(),
                    ContactNumber = x.ContactNumber,
                    OpeningTimes = x.OpeningTimes.Select(t=> new OpeningTimeDto()
                    {
                        Day = t.Day,
                        OpenTime = t.OpenTime,
                        CloseTime = t.CloseTime,
                        BreakEndTime = t.BreakEndTime,
                        BreakStartTime = t.BreakStartTime
                    }).ToList()
                });
        }
    }
}

