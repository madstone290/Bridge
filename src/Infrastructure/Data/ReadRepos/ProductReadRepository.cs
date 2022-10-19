using Bridge.Application.Common;
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

        public override Expression<Func<Product, ProductReadModel>> SelectExpression { get; } = x => new ProductReadModel()
        {
            Id = x.Id,
            Type = x.Type,
            Status = x.Status,
            CreationDateTime = x.CreationDateTime,
            Name = x.Name,
            PlaceId = x.PlaceId,
            Price = x.Price,
            Categories = x.Categories.ToList(),
        };

        public async Task<PaginatedList<ProductReadModel>> GetPaginatedProductsAsync(long placeId, int pageNumber = 1, int pageSize = 50)
        {
            return await Set
                .Where(x=> x.Status == Domain.Places.Entities.ProductStatus.Used)
                .Where(x => x.PlaceId == placeId)
                .Select(SelectExpression)
                .OrderByDescending(x => x.CreationDateTime)
                .ThenByDescending(x => x.Id)
                .PaginateAsync(pageNumber, pageSize);
        }
    }

}
