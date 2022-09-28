using Bridge.Domain.Places.Entities;
using Bridge.Domain.Products.Entities;

namespace Bridge.UnitTests.DomainTests.Builders
{
    public class ProductBuilder
    {
        public Product Build(Place place, string? name = null)
        {
            return Product.Create(
                name ?? Guid.NewGuid().ToString(),
                place);
        }


    }
}
