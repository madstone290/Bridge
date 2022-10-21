using Bridge.Application.Common;
using Bridge.Application.Places.ReadModels;
using Bridge.Domain.Places.Entities.Places;

namespace Bridge.Application.Places.Repos
{
    public interface IRestroomReadRepository : IReadRepository<Restroom, RestroomReadModel>
    {
    }
}
