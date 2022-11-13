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
        Task<PaginatedList<ProductReadModel>> GetPaginatedProductsAsync(Guid placeId, int pageNumber = 1, int pageSize = 50);

        /// <summary>
        /// 주어진 위치에서 검색어로 제품을 검색한다.
        /// </summary>
        /// <param name="searchText">검색어</param>
        /// <param name="easting">검색위치 동향</param>
        /// <param name="northing">검색위치 북향</param>
        /// <param name="maxCount">장소 검색 수</param>
        /// <param name="maxDistance">최대 검색거리</param>
        /// <returns></returns>
        Task<List<ProductReadModel>> SearchProductsAsync(string searchText, double easting, double northing, int maxCount = 1000, int? maxDistance = null);


    }
}
