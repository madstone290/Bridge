using Bridge.Application.Common;
using Bridge.Application.Places.ReadModels;
using Bridge.Application.Places.Repos;
using Bridge.Domain.Places.Enums;

namespace Bridge.Application.Places.Queries
{
    public class GetPlacesByPlaceTypeQuery : IQuery<List<PlaceReadModel>>
    {
        public PlaceType PlaceType { get; set; }
    }

    public class GetPlacesByAllCategoriesQueryHandler : QueryHandler<GetPlacesByPlaceTypeQuery, List<PlaceReadModel>>
    {
        private readonly IPlaceReadRepository _repository;

        public GetPlacesByAllCategoriesQueryHandler(IPlaceReadRepository repository)
        {
            _repository = repository;
        }

        public override async Task<List<PlaceReadModel>> HandleQuery(GetPlacesByPlaceTypeQuery query, CancellationToken cancellationToken)
        {
            return await _repository.GetListAsync(place =>  place.Type == query.PlaceType);

        }
    }
}
