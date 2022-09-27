using Bridge.Application.Products.ReadModels;
using Bridge.Application.Products.Repos;
using Bridge.Domain.Products.Entities;
using System.Linq.Expressions;

namespace Bridge.Infrastructure.Data.ReadRepos
{
    public class ProductReadRepository : ReadRepository<Product, ProductReadModel>, IProductReadRepository
    {
        public ProductReadRepository(BridgeContext context) : base(context)
        {
        }

        protected override Expression<Func<Product, ProductReadModel>> SelectExpression { get; } = x => new ProductReadModel()
        {
            Id = x.Id,
            Name = x.Name,
            PlaceId = x.PlaceId,
            Price = x.Price,
            Categories = x.CategoryItems.Select(x=> x.Category).ToList(),
        };

        public async Task<ProductReadModel?> GetProductByIdAsync(long id)
        {
            return (await FilterAsync(x => x.Id == id))
                .FirstOrDefault();
        }

        public async Task<List<ProductReadModel>> GetProductsByPlaceIdAsync(long placeId)
        {
            return await FilterAsync(x => x.PlaceId == placeId);
        }
    }

}
