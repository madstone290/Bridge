using Bridge.Application.Products.Commands;
using Bridge.Application.Products.Queries;
using Bridge.Application.Products.ReadModels;
using Bridge.Domain.Products.Enums;
using Bridge.IntegrationTests.Config;
using Bridge.Shared;
using Bridge.Shared.Extensions;
using FluentAssertions;
using System.Net.Http.Json;

namespace Bridge.IntegrationTests.Admin
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



        async Task<CreateProductCommand> DefaultCreateProductCommandAsync(Guid? placeId = null)
        {
            placeId ??= await _apiClient.CreatePlaceAsync();
            return new CreateProductCommand()
            {
                PlaceId = placeId.Value,
                Name = Guid.NewGuid().ToString(),
                Price = new Random().Next(100000),
                Categories = new List<ProductCategory>()
                {
                    ProductCategory.Stationery,
                    ProductCategory.VeganFood,
                    ProductCategory.Beverage
                }
            };
        }
       
        [Fact]
        public async Task Cosummer_Is_Forbidden()
        {
            // Arrange
            var command = await DefaultCreateProductCommandAsync();
            var createRequest = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Admin.Products.Create) { Content = JsonContent.Create(command) };
            var getRequest = new HttpRequestMessage(HttpMethod.Get, ApiRoutes.Admin.Products.Get.AddQueryParam("id", 1));

            // Act
            var createResponse = await _client.SendAsConsumerAsync(createRequest);
            var getResponse = await _client.SendAsConsumerAsync(getRequest);


            // Assert
            createResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.Forbidden);
            getResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task Create_Product_Without_Token_Return_Unauthorized()
        {
            // Arrange
            var command = await DefaultCreateProductCommandAsync();

            // Act
            var request = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Admin.Products.Create)
            {
                Content = JsonContent.Create(command)
            };
            var response = await _client.SendAsync(request);

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Create_Product_Return_Ok_With_Id()
        {
            // Arrange
            var command = await DefaultCreateProductCommandAsync();

            // Act
            var request = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Admin.Products.Create)
            {
                Content = JsonContent.Create(command)
            };
            var response = await _client.SendAsAdminAsync(request);

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var id = await response.Content.ReadFromJsonAsync<Guid>();
            id.Should().NotBeEmpty();
        }


        [Fact]
        public async Task Update_Product_Return_Ok()
        {
            // Arrange
            var productId = await _apiClient.CreateProductAsync();
            var command = new UpdateProductCommand()
            {
                Id = productId,
                Name = Guid.NewGuid().ToString(),
                Price = 45M,
                Categories = new List<ProductCategory>() { ProductCategory.VeganFood, ProductCategory.VeganBeverage, ProductCategory.Beverage }
            };

            // Act
            var request = new HttpRequestMessage(HttpMethod.Put, ApiRoutes.Admin.Products.Update.Replace("{id}", $"{productId}"))
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
            var command = await DefaultCreateProductCommandAsync();
            var productId = await _apiClient.CreateProductAsync(command);

            // Act
            var getRequest = new HttpRequestMessage(HttpMethod.Get, ApiRoutes.Admin.Products.Get.Replace("{id}", $"{productId}"));
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
            var product1 = await DefaultCreateProductCommandAsync(placeId);
            var product2 = await DefaultCreateProductCommandAsync(placeId);

            await _apiClient.CreateProductAsync(product1);
            await _apiClient.CreateProductAsync(product2);

            // Act
            var query = new GetProductsByPlaceIdQuery() { PlaceId = placeId };
            var request = new HttpRequestMessage(HttpMethod.Get, ApiRoutes.Admin.Products.GetList.AddQueryParam(query));
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
