using Bridge.Application.Common;
using Bridge.Application.Places.ReadModels;
using Bridge.Application.Places.Repos;

namespace Bridge.Application.Places.Queries
{
    /// <summary>
    /// 영역에 포함된 장소를 조회한다
    /// </summary>
    public class GetPlacesByRegionQuery : IQuery<List<PlaceReadModel>>
    {
        /// <summary>
        /// 영역 좌측의 동향
        /// </summary>
        public double LeftEasting { get; set; }

        /// <summary>
        /// 영역 우측의 동향
        /// </summary>
        public double RightEasting { get; set; }

        /// <summary>
        /// 영역 위쪽의 북향
        /// </summary>
        public double TopNorthing { get; set; }

        /// <summary>
        /// 영역 아래쪽의 북향
        /// </summary>
        public double BottomNorthing { get; set; }
    }

    public class GetPlacesByRegionQueryHandler : QueryHandler<GetPlacesByRegionQuery, List<PlaceReadModel>>
    {
        private readonly IPlaceReadRepository _repository;

        public GetPlacesByRegionQueryHandler(IPlaceReadRepository repository)
        {
            _repository = repository;
        }

        public override async Task<List<PlaceReadModel>> HandleQuery(GetPlacesByRegionQuery query, CancellationToken cancellationToken)
        {
            return await _repository.GetPlacesByEastingBetweenAndNorthingBetween(query.LeftEasting, query.RightEasting, query.BottomNorthing, query.TopNorthing);
        }
    }
}
