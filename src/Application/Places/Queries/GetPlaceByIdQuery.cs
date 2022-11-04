using Bridge.Application.Common;
using Bridge.Application.Places.ReadModels;
using Bridge.Application.Places.Repos;

namespace Bridge.Application.Places.Queries
{
    /// <summary>
    /// 아이디로 장소를 조회한다
    /// </summary>
    public class GetPlaceByIdQuery : IQuery<PlaceReadModel?>
    {
        /// <summary>
        /// 장소 아이디
        /// </summary>
        public Guid Id { get; set; }
    }

    public class GetPlaceByIdQueryHandler : QueryHandler<GetPlaceByIdQuery, PlaceReadModel?>
    {
        private readonly IPlaceReadRepository _repository;

        public GetPlaceByIdQueryHandler(IPlaceReadRepository repository)
        {
            _repository = repository;
        }

        public override async Task<PlaceReadModel?> HandleQuery(GetPlaceByIdQuery query, CancellationToken cancellationToken)
        {
            return await _repository.GetAsync(x => x.Id == query.Id);
        }
    }
}
