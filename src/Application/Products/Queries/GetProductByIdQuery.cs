using Bridge.Application.Common;
using Bridge.Application.Products.ReadModels;
using Bridge.Application.Products.Repos;

namespace Bridge.Application.Products.Queries
{
    public class GetProductByIdQuery : IQuery<ProductReadModel?>
    {
        public long Id { get; set; }
    }
    
    public class GetProductByIdQueryHandler : QueryHandler<GetProductByIdQuery, ProductReadModel?>
    {
        private readonly IProductReadRepository _repository;

        public GetProductByIdQueryHandler(IProductReadRepository repository)
        {
            _repository = repository;
        }

        public override async Task<ProductReadModel?> HandleQuery(GetProductByIdQuery query, CancellationToken cancellationToken)
        {
            return await _repository.GetAsync(x=> x.Id == query.Id);
        }
    }
}
