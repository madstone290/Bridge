using Bridge.Application.Common;
using Bridge.Application.Products.ReadModels;
using Bridge.Application.Products.Repos;

namespace Bridge.Application.Products.Queries
{
    public class GetProductsPaginationQuery : IQuery<PaginatedList<ProductReadModel>>
    {
        /// <summary>
        /// 장소 아이디
        /// </summary>
        public long PlaceId { get; set; }

        /// <summary>
        /// 페이저 크기
        /// </summary>
        public int? PageSize { get; set; }

        /// <summary>
        /// 페이지 번호
        /// </summary>
        public int? PageNumber { get; set; }
    }

    public class GetProductsPaginationQueryHandler : QueryHandler<GetProductsPaginationQuery, PaginatedList<ProductReadModel>>
    {
        private readonly IProductReadRepository _repository;

        public GetProductsPaginationQueryHandler(IProductReadRepository repository)
        {
            _repository = repository;
        }

        public override async Task<PaginatedList<ProductReadModel>> HandleQuery(GetProductsPaginationQuery query, CancellationToken cancellationToken)
        {
            return await _repository.GetPaginatedProductsAsync(query.PlaceId, query.PageNumber ?? 1, query.PageSize ?? 100);

        }
    }

}
