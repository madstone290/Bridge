using Bridge.Domain.Products.Entities;
using Bridge.Domain.Products.Repos;

namespace Bridge.Infrastructure.Data.Repos
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(BridgeContext context) : base(context)
        {
        }
    }
}
