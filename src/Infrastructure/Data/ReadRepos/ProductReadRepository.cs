using Bridge.Application.Products.ReadModels;
using Bridge.Application.Products.Repos;
using Bridge.Domain.Products.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bridge.Infrastructure.Data.ReadRepos
{
    public class ProductReadRepository : Repository<Product>, IProductReadRepository
    {
        public ProductReadRepository(BridgeContext context) : base(context)
        {
        }

        public async Task<ProductReadModel?> GetProductByIdAsync(long id)
        {
            return await Set
                .Where(x => x.Id == id)
                .SelectProductReadModel()
                .FirstOrDefaultAsync();
        }

        public async Task<List<ProductReadModel>> GetProductsByPlaceIdAsync(long placeId)
        {
            return await Set
                .Where(x => x.PlaceId == placeId)
                .SelectProductReadModel()
                .ToListAsync();
        }
    }

    public static class ProductReadRepositoryExtensions
    {
        public static IQueryable<ProductReadModel> SelectProductReadModel(this IQueryable<Product> query)
        {
            return query
                .Select(x => new ProductReadModel()
                {
                    Id = x.Id,
                    Name = x.Name,
                    PlaceId = x.PlaceId,
                    Price = x.Price,
                    Categories = x.Categories.ToList(),
                });
        }
    }
}
