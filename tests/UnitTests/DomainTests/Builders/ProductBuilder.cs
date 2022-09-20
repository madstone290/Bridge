using Bridge.Domain.Places.Entities;
using Bridge.Domain.Products.Entities;
using Bridge.Domain.Users.Entities;

namespace Bridge.UnitTests.DomainTests.Builders
{
    public class ProductBuilder
    {
        public Product Build(User user, Place place, string? name = null)
        {
            return Product.Create(
                user,
                name ?? Guid.NewGuid().ToString(),
                place);
        }


    }
}
