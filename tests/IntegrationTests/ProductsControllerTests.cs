using Bridge.Application.Products.Commands;
using Bridge.Application.Products.Queries;
using Bridge.Application.Products.ReadModels;
using Bridge.Domain.Products.Entities;
using Bridge.IntegrationTests.Config;
using Bridge.Shared;
using Bridge.Shared.Extensions;
using FluentAssertions;
using System.Net.Http.Json;

namespace Bridge.IntegrationTests
{
    public class ProductsControllerTests : IClassFixture<ApiTestFactory>
    {
        private readonly TestClient _client;
        private readonly ApiClient _apiClient;

        public ProductsControllerTests(ApiTestFactory apiTestFactory)
        {
            _client = apiTestFactory.Client;
            _apiClient = apiTestFactory.ApiClient;
        }


        [Fact]
        public async Task Create_Product_Return_Ok_With_Id()
        {
            // Arrange
            var placeId = await _apiClient.CreatePlaceAsync();
            var command = new CreateProductCommand()
            {
                PlaceId = placeId,
                Name = Guid.NewGuid().ToString(),
                Price = 30M,
                Categories = new List<ProductCategory>()
                {
                    ProductCategory.Stationery,
                    ProductCategory.VeganFood,
                    ProductCategory.Beverage
                }
            };

            // Act
            var request = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Products.Create)
            {
                Content = JsonContent.Create(command)
            };
            var response = await _client.SendAsAdminAsync(request);

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var id = await response.Content.ReadFromJsonAsync<long>();
            id.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task Update_Product_Return_Ok()
        {
            // Arrange
            var productId = await _apiClient.CreateProductAsync();
            var command = new UpdateProductCommand()
            {
                ProductId = productId,
                Name = Guid.NewGuid().ToString(),
                Price = 45M,
                Categories = new List<ProductCategory>() { ProductCategory.VeganFood, ProductCategory.VeganBeverage, ProductCategory.Beverage }
            };

            // Act
            var request = new HttpRequestMessage(HttpMethod.Put, ApiRoutes.Products.Update.Replace("{id}", $"{productId}"))
            {
                Content = JsonContent.Create(command)
            };
            var response = await _client.SendAsAdminAsync(request);

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            
            var product = await _apiClient.GetProductAsync(productId) ?? default!;
            product.Should().NotBeNull();
            product.Name.Should().Be(command.Name.ToString());
            product.Price.Should().Be(command.Price);
            product.Categories.Should().BeEquivalentTo(command.Categories);
        }

        [Fact]
        public async Task Get_Product_Return_Ok_With_Content()
        {
            // Arrange
            var placeId = await _apiClient.CreatePlaceAsync();
            var command = new CreateProductCommand()
            {
                PlaceId = placeId,
                Name = Guid.NewGuid().ToString(),
                Price = 30M,
                Categories = new List<ProductCategory>()
                {
                    ProductCategory.Stationery,
                    ProductCategory.VeganFood,
                    ProductCategory.Beverage
                }
            };
            var productId = await _apiClient.CreateProductAsync(command);

            // Act
            var getRequest = new HttpRequestMessage(HttpMethod.Get, ApiRoutes.Products.Get.Replace("{id}", $"{productId}"));
            var getResponse = await _client.SendAsAdminAsync(getRequest);

            // Assert
            getResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var product = await getResponse.Content.ReadFromJsonAsync<ProductReadModel>() ?? default!;
            product.Should().NotBeNull();
            product.Name.Should().Be(command.Name.ToString());
            product.PlaceId.Should().Be(command.PlaceId);
            product.Price.Should().Be(command.Price);
            product.Categories.Should().Contain(command.Categories);
        }

        [Fact]
        public async Task Get_Product_List_Return_Ok_With_Content()
        {
            // Arrange
            var placeId = await _apiClient.CreatePlaceAsync();
            var product1 = new CreateProductCommand()
            {
                PlaceId = placeId,
                Name = Guid.NewGuid().ToString(),
                Price = 30M,
            };
            var product2 = new CreateProductCommand()
            {
                PlaceId = placeId,
                Name = Guid.NewGuid().ToString(),
                Price = 40M,
            };

            await _apiClient.CreateProductAsync(product1);
            await _apiClient.CreateProductAsync(product2);

            // Act
            var query = new GetProductsByPlaceIdQuery() { PlaceId = placeId };
            var request = new HttpRequestMessage(HttpMethod.Get, ApiRoutes.Products.GetList.AddQueryParam(query));
            var response = await _client.SendAsAdminAsync(request);

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var products = await response.Content.ReadFromJsonAsync<List<ProductReadModel>>();
            products.Should().Contain(x =>
                x.Name == product1.Name &&
                x.Price == product1.Price);
            products.Should().Contain(x =>
                x.Name == product2.Name &&
                x.Price == product2.Price);
        }

    }
}
