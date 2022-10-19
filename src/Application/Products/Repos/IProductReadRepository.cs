using Bridge.Application.Common;
using Bridge.Application.Products.ReadModels;
using Bridge.Domain.Products.Entities;

namespace Bridge.Application.Products.Repos
{
    public interface IProductReadRepository : IReadRepository<Product, ProductReadModel>
    {
        /// <summary>
        /// 제품 페이지 조회
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<PaginatedList<ProductReadModel>> GetPaginatedProductsAsync(long placeId, int pageNumber = 1, int pageSize = 50);
    }
}
