using Bridge.Application.Places.Commands;
using Bridge.Application.Places.Dtos;
using Bridge.Application.Products.Commands;
using Bridge.Application.Products.Queries;
using Bridge.Application.Products.ReadModels;
using Bridge.Domain.Places.Entities;
using Bridge.Domain.Products.Entities;
using Bridge.IntegrationTests.Config.ApiClients;

namespace Bridge.IntegrationTests.Config
{
    /// <summary>
    /// 테스트에 자주 사용되는 API를 제공한다.
    /// </summary>
    public class ApiClient
    {
        public PlaceApiClient PlaceApiClient { get; }
        public ProductApiClient ProductApiClient { get; }

        public ApiClient(PlaceApiClient placeApiClient, ProductApiClient productApiClient)
        {
            PlaceApiClient = placeApiClient;
            ProductApiClient = productApiClient;
        }

        public async Task<long> CreateProductAsync(CreateProductCommand command)
        {
            return await ProductApiClient.CreateProductAsync(command);
        }

        public async Task<long> CreateProductAsync(long? placeId = null)
        {
            placeId ??= await CreatePlaceAsync();

            var command = new CreateProductCommand()
            {
                PlaceId = placeId.Value,
                Name = Guid.NewGuid().ToString(),
                Categories = new List<ProductCategory>() { ProductCategory.Food }
            };
            return await CreateProductAsync(command);
        }

        public async Task<ProductReadModel?> GetProductAsync(long productId)
        {
            return await ProductApiClient.GetProductAsync(new GetProductByIdQuery() { Id = productId });
        }

        public async Task<long> CreatePlaceAsync(CreatePlaceCommand command)
        {
            return await PlaceApiClient.CreatePlaceAsync(command);
        }

        public async Task<long> CreatePlaceAsync()
        {
            var command = new CreatePlaceCommand()
            {
                Name = Guid.NewGuid().ToString(),
                Categories = new List<PlaceCategory>() { PlaceCategory.Pharmacy },
                Address = new AddressDto() { BaseAddress ="대구시 수성구 청수로 25길 118-10"},
                OpeningTimes = new List<OpeningTimeDto>()
                {
                    new OpeningTimeDto()
                    {
                        Day = DayOfWeek.Monday,
                        OpenTime = TimeSpan.FromHours(8),
                        CloseTime= TimeSpan.FromHours(16),
                    }
                }

            };
            return await CreatePlaceAsync(command);
        }
    }
}
