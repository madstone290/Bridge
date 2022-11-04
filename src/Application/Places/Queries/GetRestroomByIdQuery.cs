using Bridge.Application.Common;
using Bridge.Application.Places.ReadModels;
using Bridge.Application.Places.Repos;

namespace Bridge.Application.Places.Queries
{
    /// <summary>
    /// 아이디로 화장실을 조회한다
    /// </summary>
    public class GetRestroomByIdQuery : IQuery<RestroomReadModel?>
    {
        /// <summary>
        /// 장소 아이디
        /// </summary>
        public Guid Id { get; set; }
    }

    public class GetRestroomByIdQueryHandler : QueryHandler<GetRestroomByIdQuery, RestroomReadModel?>
    {
        private readonly IRestroomReadRepository _repository;

        public GetRestroomByIdQueryHandler(IRestroomReadRepository repository)
        {
            _repository = repository;
        }

        public override async Task<RestroomReadModel?> HandleQuery(GetRestroomByIdQuery query, CancellationToken cancellationToken)
        {
            return await _repository.GetAsync(x => x.Id == query.Id);
        }
    }
}
