using Bridge.Domain.Places.Entities;
using Bridge.Domain.Products.Entities;
using Bridge.Domain.Products.Enums;
using Bridge.Domain.Products.Exception;
using Bridge.UnitTests.DomainTests.Builders;
using FluentAssertions;

namespace Bridge.UnitTests.DomainTests
{
    public class ProductUnitTests : IClassFixture<PlaceBuilder>, IClassFixture<ProductBuilder>
    {
        private readonly PlaceBuilder _placeBuilder;
        private readonly ProductBuilder _productBuilder;

        public ProductUnitTests(PlaceBuilder placeBuilder, ProductBuilder productBuilder)
        {
            _placeBuilder = placeBuilder;
            _productBuilder = productBuilder;
        }

        private Place NewPlace()
        {
            var place = _placeBuilder.Build();
            return place;
        }

        private Product NewProduct()
        {
            var place = _placeBuilder.Build();
            var product = _productBuilder.Build(place);
            return product;
        }

        [Fact]
        public void Product_Name_Cannot_Be_Empty()
        {
            // Arrange
            var name = string.Empty;
            var place = NewPlace();

            // Act
            var action = () =>
            {
                var product = Product.Create(name, place);
            };

            // Assert
            action.Should().ThrowExactly<InvalidProductNameException>();
        }

        [Fact]
        public void Default_Price_Is_Null()
        {
            // Arrange

            // Act
            var product = NewProduct();

            // Assert
            product.Price.Should().Be(null);
        }

        [Fact]
        public void Negative_Price_Is_Not_Allowed()
        {
            // Arrange
            var product = NewProduct();

            // Act
            var action = () => product.SetPrice(-1);

            // Assert
            action.Should().ThrowExactly<InvalidPriceException>();
        }

        [Fact]
        public void Price_Is_Changed()
        {
            // Arrange
            var product = NewProduct();

            // Act
            var priceToChange = 1000;
            product.SetPrice(priceToChange);

            // Assert
            product.Price.Should().Be(priceToChange);
        }

        [Fact]
        public void Default_Categories_Are_Empty()
        {
            // Arrange

            // Act
            var product = NewProduct();

            // Assert
            product.Categories.Should().BeEmpty();
        }

        [Fact]
        public void Category_Is_Added()
        {
            // Arrange
            var product = NewProduct();
            var category = ProductCategory.Beverage;
            product.AddCategory(category);

            // Act

            product.AddCategory(category);

            // Assert
            product.Categories.Should().Contain(category);
        }

        [Fact]
        public void Duplicate_Category_Is_Not_Added()
        {
            // Arrange
            var product = NewProduct();
            var category = ProductCategory.Beverage;
            product.AddCategory(category);

            // Act
            product.AddCategory(category);

            // Assert
            product.Categories.Should().HaveCount(1);
        }

        [Fact]
        public void Category_Is_Removed()
        {
            // Arrange
            var product = NewProduct();
            var category = ProductCategory.Beverage;
            product.AddCategory(category);

            // Act
            product.RemoveCategory(category);

            // Assert
            product.Categories.Should().HaveCount(0);
        }

        [Fact]
        public void Categories_Are_Updated()
        {
            // Arrange
            var product = NewProduct();
            var categories = new ProductCategory[]
            {
                ProductCategory.Beverage,
                ProductCategory.Stationery,
                ProductCategory.VeganBeverage
            };

            // Act
            product.UpdateCategories(categories);

            // Assert
            product.Categories.Should().Contain(categories);
        }

    }
}


