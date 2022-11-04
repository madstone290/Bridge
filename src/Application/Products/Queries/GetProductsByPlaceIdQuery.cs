using Bridge.Application.Common;
using Bridge.Application.Products.ReadModels;
using Bridge.Application.Products.Repos;

namespace Bridge.Application.Products.Queries
{
    public class GetProductsByPlaceIdQuery : IQuery<List<ProductReadModel>>
    {
        public Guid PlaceId { get; set; }
    }

    public class GetProductsByPlaceIdQueryHandler : QueryHandler<GetProductsByPlaceIdQuery, List<ProductReadModel>>
    {
        private readonly IProductReadRepository _repository;

        public GetProductsByPlaceIdQueryHandler(IProductReadRepository repository)
        {
            _repository = repository;
        }

        public override async Task<List<ProductReadModel>> HandleQuery(GetProductsByPlaceIdQuery query, CancellationToken cancellationToken)
        {
            return await _repository.GetListAsync(x=> x.PlaceId == query.PlaceId);
        }
    }
}
