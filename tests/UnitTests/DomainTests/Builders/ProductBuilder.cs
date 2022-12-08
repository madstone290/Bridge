using Bridge.Domain.Places.Entities;
using Bridge.Domain.Products.Entities;

namespace Bridge.UnitTests.DomainTests.Builders
{
    public class ProductBuilder
    {
        public Product Build(Place place, string? name = null)
        {
            return new Product(Guid.NewGuid().ToString(),
                name ?? Guid.NewGuid().ToString(),
                place);
        }


    }
}
