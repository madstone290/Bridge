using Bridge.Application.Common;
using Bridge.Application.Places.ReadModels;
using Bridge.Application.Places.Repos;
using Bridge.Domain.Places.Entities;

namespace Bridge.Application.Places.Queries
{
    public class GetPlacesPaginationQuery : IQuery<PaginatedList<PlaceReadModel>>
    {
        /// <summary>
        /// 페이저 크기
        /// </summary>
        public int? PageSize { get; set; }

        /// <summary>
        /// 페이지 번호
        /// </summary>
        public int? PageNumber { get; set; }

        /// <summary>
        /// 장소 타입. null인 경우 모든 장소 유형 조회
        /// </summary>
        public PlaceType? PlaceType { get; set; }
    }

    public class GetPlacesPaginationQueryHandler : QueryHandler<GetPlacesPaginationQuery, PaginatedList<PlaceReadModel>>
    {
        private readonly IPlaceReadRepository _repository;

        public GetPlacesPaginationQueryHandler(IPlaceReadRepository repository)
        {
            _repository = repository;
        }

        public override async Task<PaginatedList<PlaceReadModel>> HandleQuery(GetPlacesPaginationQuery query, CancellationToken cancellationToken)
        {
            return await _repository.GetPaginatedPlacesAsync(query.PlaceType, query.PageNumber ?? 1, query.PageSize ?? 100);

        }
    }

}
