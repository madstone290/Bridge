using Bridge.Application.Common;
using Bridge.Application.Products.ReadModels;
using Bridge.Domain.Products.Entities;

namespace Bridge.Application.Products.Repos
{
    public interface IProductReadRepository : IReadRepository<Product, ProductReadModel>
    {
        /// <summary>
        /// 아이디로 제품 조회
        /// </summary>
        /// <param name="id">제품 아이디</param>
        /// <returns></returns>
        Task<ProductReadModel?> GetProductByIdAsync(long id);

        /// <summary>
        /// 장소에 포함된 모든 제품 조회
        /// </summary>
        /// <param name="placeId">장소 아이디</param>
        /// <returns></returns>
        Task<List<ProductReadModel>> GetProductsByPlaceIdAsync(long placeId);

    }
}
