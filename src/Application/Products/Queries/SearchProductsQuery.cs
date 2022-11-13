using Bridge.Application.Common;
using Bridge.Application.Common.Services;
using Bridge.Application.Products.ReadModels;
using Bridge.Application.Products.Repos;

namespace Bridge.Application.Products.Queries
{
    public class SearchProductsQuery : IQuery<List<ProductReadModel>>
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

    public class SearchProductsQueryHandler : QueryHandler<SearchProductsQuery, List<ProductReadModel>>
    {
        private readonly IProductReadRepository _repository;
        private readonly ICoordinateService _coordinateService;

        public SearchProductsQueryHandler(IProductReadRepository repository, ICoordinateService coordinateService)
        {
            _repository = repository;
            _coordinateService = coordinateService;
        }

        public override async Task<List<ProductReadModel>> HandleQuery(SearchProductsQuery query, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(query.SearchText))
                return new List<ProductReadModel>();

            var eatingNorthing = _coordinateService.ConvertToUtmK(query.Longitude, query.Latitude);
            var easting = eatingNorthing.Item1;
            var northing = eatingNorthing.Item2;

            return await _repository.SearchProductsAsync(query.SearchText, easting, northing);

        }
    }
}
