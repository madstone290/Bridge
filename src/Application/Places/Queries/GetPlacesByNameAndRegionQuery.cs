using Bridge.Application.Common;
using Bridge.Application.Places.ReadModels;
using Bridge.Application.Places.Repos;

namespace Bridge.Application.Places.Queries
{
    /// <summary>
    /// 영역에 포함된 장소를 이름으로 검색한다.
    /// </summary>
    public class GetPlacesByNameAndRegionQuery : IQuery<List<PlaceReadModel>> 
    {
        /// <summary>
        /// 검색할 장소의 이름
        /// </summary>
        public string Name { get; set; } = string.Empty;

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

    public class GetPlacesByNameAndRegionQueryHandler : QueryHandler<GetPlacesByNameAndRegionQuery, List<PlaceReadModel>>
    {
        private readonly IPlaceReadRepository _repository;

        public GetPlacesByNameAndRegionQueryHandler(IPlaceReadRepository repository)
        {
            _repository = repository;
        }

        public override async Task<List<PlaceReadModel>> HandleQuery(GetPlacesByNameAndRegionQuery query, CancellationToken cancellationToken)
        {
            var places = await _repository.GetPlacesByEastingBetweenAndNorthingBetween(query.LeftEasting, query.RightEasting, query.BottomNorthing, query.TopNorthing);
            return places.Where(x => x.Name.Contains(query.Name)).ToList();

        }
    }

}
