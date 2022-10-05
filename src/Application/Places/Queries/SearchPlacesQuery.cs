using Bridge.Application.Common;
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
    }

    public class SearchPlacesQueryHandler : QueryHandler<SearchPlacesQuery, List<PlaceReadModel>>
    {
        private readonly IPlaceReadRepository _repository;

        public SearchPlacesQueryHandler(IPlaceReadRepository repository)
        {
            _repository = repository;
        }

        public override async Task<List<PlaceReadModel>> HandleQuery(SearchPlacesQuery query, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(query.SearchText))
                return new List<PlaceReadModel>();

            return await _repository.FilterAsync(place => place.Name.Contains(query.SearchText));

        }
    }
}
