using Bridge.Application.Common;
using Bridge.Application.Products.ReadModels;
using Bridge.Application.Products.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bridge.Application.Products.Queries
{
    public class GetProductsByPlaceIdQuery : IQuery<List<ProductReadModel>>
    {
        public long PlaceId { get; set; }
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
            return await _repository.GetProductsByPlaceIdAsync(query.PlaceId);
        }
    }
}
