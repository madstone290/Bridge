using Bridge.Domain.Common.Exceptions;
using Bridge.Domain.Places.Entities;
using Bridge.Domain.Products.Entities;
using Bridge.Domain.Products.Exception;
using Bridge.Domain.Users.Entities;
using Bridge.UnitTests.DomainTests.Builders;
using FluentAssertions;

namespace Bridge.UnitTests.DomainTests
{
    public class ProductUnitTests : IClassFixture<UserBuilder>, IClassFixture<PlaceBuilder>, IClassFixture<ProductBuilder>
    {
        private readonly UserBuilder _userBuilder;
        private readonly PlaceBuilder _placeBuilder;
        private readonly ProductBuilder _productBuilder;

        public ProductUnitTests(UserBuilder userBuilder, PlaceBuilder placeBuilder, ProductBuilder productBuilder)
        {
            _userBuilder = userBuilder;
            _placeBuilder = placeBuilder;
            _productBuilder = productBuilder;
        }
        private User NewAdminUser()
        {
            return _userBuilder.BuildAdminUser();
        }

        private Place NewPlace()
        {
            var user = _userBuilder.BuildAdminUser();
            var place = _placeBuilder.Build(user);
            return place;
        }

        private Product NewProduct()
        {
            var user = _userBuilder.BuildAdminUser();
            var place = _placeBuilder.Build(user);
            var product = _productBuilder.Build(user, place);
            return product;
        }

        [Fact]
        public void NoAdminUser_Cannot_Create_Product()
        {
            // Arrange
            var user = _userBuilder.BuildNormalUser();
            var place = NewPlace();
            var name = Guid.NewGuid().ToString();

            // Act
            var action = () => Product.Create(user, name, place);

            // Assert
            action.Should().ThrowExactly<NoPermissionException>();
        }

        [Fact]
        public void AdminUser_Can_Create_Product()
        {
            // Arrange
            var user = _userBuilder.BuildAdminUser();
            var place = NewPlace();
            var name = Guid.NewGuid().ToString();

            // Act
            var product = Product.Create(user, name, place);

            // Assert
            product.Name.Should().Be(name);
        }



        [Fact]
        public void Product_Name_Cannot_Be_Empty()
        {
            // Arrange
            var name = string.Empty;
            var user = NewAdminUser();
            var place = NewPlace();

            // Act
            var action = () =>
            {
                var product = Product.Create(user, name, place);
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


