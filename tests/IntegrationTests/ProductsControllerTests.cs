using Bridge.Application.Products.Commands;
using Bridge.Application.Products.Queries;
using Bridge.Application.Products.ReadModels;
using Bridge.Domain.Products.Entities;
using Bridge.IntegrationTests.ApiServices;
using Bridge.IntegrationTests.Config;
using Bridge.Shared;
using Bridge.Shared.Extensions;
using FluentAssertions;
using System.Net.Http.Json;

namespace Bridge.IntegrationTests
{
    public class ProductsControllerTests : IClassFixture<ApiTestFactory>, IClassFixture<ApiService>
    {
        private readonly HttpClient _client;
        private readonly ApiService _apiService;

        public ProductsControllerTests(ApiTestFactory apiTestFactory, ApiService apiService)
        {
            _client = apiTestFactory.Client;
            _apiService = apiService;
        }


        [Fact]
        public async Task Create_Product_Return_Ok_With_Id()
        {
            // Arrange
            var placeId = await _apiService.CreatePlaceAsync(_client);
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
            var response = await _client.SendAsync(request);

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var id = await response.Content.ReadFromJsonAsync<long>();
            id.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task Update_Product_Return_Ok()
        {
            // Arrange
            var productId = await _apiService.CreateProductAsync(_client);
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
            var response = await _client.SendAsync(request);

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            
            var product = await _apiService.GetProductAsync(_client, productId) ?? default!;
            product.Should().NotBeNull();
            product.Name.Should().Be(command.Name.ToString());
            product.Price.Should().Be(command.Price);
            product.Categories.Should().BeEquivalentTo(command.Categories);
        }

        [Fact]
        public async Task Get_Product_Return_Ok_With_Content()
        {
            // Arrange
            var placeId = await _apiService.CreatePlaceAsync(_client);
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
            var productId = await _apiService.CreateProductAsync(_client, command);

            // Act
            var getRequest = new HttpRequestMessage(HttpMethod.Get, ApiRoutes.Products.Get.Replace("{id}", $"{productId}"));
            var getResponse = await _client.SendAsync(getRequest);

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
            var placeId = await _apiService.CreatePlaceAsync(_client);
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

            await _apiService.CreateProductAsync(_client, product1);
            await _apiService.CreateProductAsync(_client, product2);

            // Act
            var query = new GetProductsByPlaceIdQuery() { PlaceId = placeId };
            var request = new HttpRequestMessage(HttpMethod.Get, ApiRoutes.Products.GetList.AddQueryParam(query));
            var response = await _client.SendAsync(request);

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
