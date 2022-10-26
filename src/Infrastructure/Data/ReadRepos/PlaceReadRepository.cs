using Bridge.Application.Common;
using Bridge.Application.Places.Dtos;
using Bridge.Application.Places.ReadModels;
using Bridge.Application.Places.Repos;
using Bridge.Domain.Places.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Bridge.Infrastructure.Data.ReadRepos
{
    public class PlaceReadRepository : ReadRepository<Place, PlaceReadModel>, IPlaceReadRepository
    {
        public PlaceReadRepository(BridgeContext context) : base(context)
        {
        }

        public override Expression<Func<Place, PlaceReadModel>> SelectExpression { get; } = x => new PlaceReadModel()
        {
            Id = x.Id,
            Status = x.Status,
            CreationDateTimeUtc = x.CreationDateTimeUtc,
            LastUpdateDateTimeUtc = x.LastUpdateDateTimeUtc,
            Type = x.Type,
            Name = x.Name,
            Address = new AddressDto()
            {
                BaseAddress = x.Address.RoadAddress,
                DetailAddress = x.Address.DetailAddress
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
            ImagePath = x.ImagePath,
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

        public async Task<PaginatedList<PlaceReadModel>> GetPaginatedPlacesAsync(string? placeName, PlaceType? placeType = null, int pageNumber = 1, int pageSize = 100)
        {
            return await Set
                .Where(x=> x.Status == PlaceStatus.Open)
                .Where(x => placeType == null || x.Type == placeType.Value)
                .Where(x => placeName == null || x.Name.Contains(placeName))
                .Select(SelectExpression)
                .OrderByDescending(x=> x.CreationDateTimeUtc)
                .ThenByDescending(x=> x.Id)
                .PaginateAsync(pageNumber, pageSize);
        }

        public async Task<List<PlaceReadModel>> SearchPlacesAsync(string searchText, double easting, double northing, int maxCount = 200, int? maxDistance = null)
        {
            if (string.IsNullOrWhiteSpace(searchText))
                return new List<PlaceReadModel>();

            if (!double.IsNormal(easting) || !double.IsNormal(northing))
                return new List<PlaceReadModel>();

            return await Set
                .Where(x => x.Status == PlaceStatus.Open)
                .Where(x => x.Name.Contains(searchText))
                .Select(x => new PlaceReadModel()
                {
                    Distance = Math.Sqrt(Math.Pow(Math.Abs(easting - x.Location.Easting), 2) + Math.Pow(Math.Abs(northing - x.Location.Northing), 2)),
                    Id = x.Id,
                    Status = x.Status,
                    CreationDateTimeUtc = x.CreationDateTimeUtc,
                    LastUpdateDateTimeUtc = x.LastUpdateDateTimeUtc,
                    Type = x.Type,
                    Name = x.Name,
                    Address = new AddressDto()
                    {
                        BaseAddress = x.Address.RoadAddress,
                        DetailAddress = x.Address.DetailAddress
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
                    ImagePath = x.ImagePath,
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
               })
               .Where(x => !maxDistance.HasValue || x.Distance < maxDistance.Value)
               .OrderBy(x => x.Distance)
               .Take(maxCount)
               .ToListAsync();
        }
    }
}

