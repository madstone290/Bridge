using Bridge.Application.Common;
using Bridge.Application.Products.ReadModels;
using Bridge.Domain.Products.Entities;

namespace Bridge.Application.Products.Repos
{
    public interface IProductReadRepository : IReadRepository<Product, ProductReadModel>
    {
    }
}
