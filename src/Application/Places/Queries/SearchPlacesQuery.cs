using Bridge.Application.Common;
using Bridge.Application.Common.Services;
using Bridge.Application.Places.ReadModels;
using Bridge.Application.Places.Repos;

namespace Bridge.Application.Places.Queries
{
    public class SearchPlacesQuery : IQuery<List<PlaceReadModel>>
    {
        /// <summary>
        /// 검색어
        /// </summary>
        public string SearchText { get; set; } = string.Empty;

        /// <summary>
        /// 검색위치 위도
        /// </summary>
        public double Latitude { get; set; } = 32.111d;

        /// <summary>
        /// 검색위치 경도
        /// </summary>
        public double Longitude { get; set; } = 127.333d;
    }

    public class SearchPlacesQueryHandler : QueryHandler<SearchPlacesQuery, List<PlaceReadModel>>
    {
        private readonly IPlaceReadRepository _repository;
        private readonly ICoordinateService _coordinateService;

        public SearchPlacesQueryHandler(IPlaceReadRepository repository, ICoordinateService coordinateService)
        {
            _repository = repository;
            _coordinateService = coordinateService;
        }

        public override async Task<List<PlaceReadModel>> HandleQuery(SearchPlacesQuery query, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(query.SearchText))
                return new List<PlaceReadModel>();

            var eatingNorthing = _coordinateService.ConvertToUtmK(query.Longitude, query.Latitude);
            var easting = eatingNorthing.Item1;
            var northing = eatingNorthing.Item2;
            
            return await _repository.SearchPlacesAsync(query.SearchText, easting, northing);

        }
    }
}
