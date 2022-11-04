using Bridge.Application.Places.Dtos;
using Bridge.Application.Places.ReadModels;
using Bridge.Application.Places.Repos;
using Bridge.Domain.Places.Entities.Places;
using System.Linq.Expressions;

namespace Bridge.Infrastructure.Data.ReadRepos
{
    public class RestroomReadRepository : ReadRepository<Restroom, RestroomReadModel>, IRestroomReadRepository
    {
        public RestroomReadRepository(BridgeContext context) : base(context)
        {
        }

        public override Expression<Func<Restroom, RestroomReadModel>> SelectExpression { get; } = x => new RestroomReadModel()
        {
            Id = x.Id,
            Status = x.Status,
            CreationDateTimeUtc = x.CreationDateTimeUtc,
            LastUpdateDateTimeUtc = x.LastUpdateDateTimeUtc,
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
            OpeningTimes = x.OpeningTimes.Select(t => new OpeningTimeDto()
            {
                Day = t.Day,
                IsDayoff = t.IsDayoff,
                Is24Hours = t.Is24Hours,
                OpenTime = t.OpenTime,
                CloseTime = t.CloseTime,
                BreakEndTime = t.BreakEndTime,
                BreakStartTime = t.BreakStartTime
            }).ToList(),
            IsUnisex = x.IsUnisex,
            DiaperTableLocation = x.DiaperTableLocation,
            MaleToilet = x.MaleToilet,
            MaleUrinal = x.MaleUrinal,
            MaleDisabledToilet = x.MaleDisabledToilet,
            MaleDisabledUrinal = x.MaleDisabledUrinal,
            MaleKidToilet = x.MaleKidToilet,
            MaleKidUrinal = x.MaleKidUrinal,
            FemaleToilet = x.FemaleToilet,
            FemaleDisabledToilet = x.FemaleDisabledToilet,
            FemaleKidToilet = x.FemaleKidToilet,
        };
    }
}
