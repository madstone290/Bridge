using Bridge.Application.Common;
using Bridge.Application.Places.Dtos;
using Bridge.Application.Products.ReadModels;
using Bridge.Application.Products.Repos;
using Bridge.Domain.Products.Entities;
using Bridge.Domain.Products.Enums;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Bridge.Infrastructure.Data.ReadRepos
{
    public class ProductReadRepository : ReadRepository<Product, ProductReadModel>, IProductReadRepository
    {
        public ProductReadRepository(BridgeContext context) : base(context)
        {
        }

        public override Expression<Func<Product, ProductReadModel>> SelectExpression { get; } = x => new ProductReadModel()
        {
            Id = x.Id,
            Type = x.Type,
            Status = x.Status,
            CreationDateTime = x.CreationDateTime,
            Name = x.Name,
            PlaceId = x.PlaceId,
            Price = x.Price,
            Categories = x.Categories.ToList(),
        };

        public async Task<PaginatedList<ProductReadModel>> GetPaginatedProductsAsync(Guid placeId, int pageNumber = 1, int pageSize = 50)
        {
            return await Set
                .Where(x=> x.Status == ProductStatus.Used)
                .Where(x => x.PlaceId == placeId)
                .Select(SelectExpression)
                .OrderByDescending(x => x.CreationDateTime)
                .ThenByDescending(x => x.Id)
                .PaginateAsync(pageNumber, pageSize);
        }

        public async Task<List<ProductReadModel>> SearchProductsAsync(string searchText, double easting, double northing, int maxCount = 1000, int? maxDistance = null)
        {
            if (string.IsNullOrWhiteSpace(searchText))
                return new List<ProductReadModel>();

            if (!double.IsNormal(easting) || !double.IsNormal(northing))
                return new List<ProductReadModel>();

            return await Set
                .Include(x => x.Place)
                .Where(x => x.Status == ProductStatus.Used)
                .Where(x => x.Name.Contains(searchText))
                .Select(x => new ProductReadModel()
                {
                    Id = x.Id,
                    Type = x.Type,
                    Status = x.Status,
                    CreationDateTime = x.CreationDateTime,
                    Name = x.Name,
                    PlaceId = x.PlaceId,
                    Price = x.Price,
                    Categories = x.Categories.ToList(),
                    Place = new Application.Places.ReadModels.PlaceReadModel()
                    {
                        Name = x.Place.Name,
                        Distance = Math.Sqrt(Math.Pow(Math.Abs(easting - x.Place.Location.Easting), 2) + Math.Pow(Math.Abs(northing - x.Place.Location.Northing), 2)),
                        Id = x.Place.Id,
                        Status = x.Place.Status,
                        CreationDateTimeUtc = x.Place.CreationDateTimeUtc,
                        LastUpdateDateTimeUtc = x.Place.LastUpdateDateTimeUtc,
                        Type = x.Place.Type,
                        Address = new AddressDto()
                        {
                            BaseAddress = x.Place.Address.RoadAddress,
                            DetailAddress = x.Place.Address.DetailAddress
                        },
                        Location = new PlaceLocationDto()
                        {

                            Latitude = x.Place.Location.Latitude,
                            Longitude = x.Place.Location.Longitude,
                            Easting = x.Place.Location.Easting,
                            Northing = x.Place.Location.Northing
                        },
                        Categories = x.Place.Categories.ToList(),
                        ContactNumber = x.Place.ContactNumber,
                        ImagePath = x.Place.ImagePath,
                        OpeningTimes = x.Place.OpeningTimes.Select(t => new OpeningTimeDto()
                        {
                            Day = t.Day,
                            IsDayoff = t.IsDayoff,
                            Is24Hours = t.Is24Hours,
                            OpenTime = t.OpenTime,
                            CloseTime = t.CloseTime,
                            BreakEndTime = t.BreakEndTime,
                            BreakStartTime = t.BreakStartTime
                        }).ToList()
                    }
                })
                .Where(x => !maxDistance.HasValue || x.Place!.Distance < maxDistance.Value)
                .OrderBy(x => x.Place!.Distance)
                .Take(maxCount)
                .ToListAsync();
                
        }
    }

}
