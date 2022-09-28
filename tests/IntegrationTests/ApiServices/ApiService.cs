using Bridge.Application.Places.Commands;
using Bridge.Application.Places.Dtos;
using Bridge.Application.Products.Commands;
using Bridge.Application.Products.Queries;
using Bridge.Application.Products.ReadModels;
using Bridge.Domain.Places.Entities;
using Bridge.Domain.Products.Entities;

namespace Bridge.IntegrationTests.ApiServices
{
    public class ApiService
    {
        private readonly PlaceApiService _placeApiService = new();
        private readonly ProductApiService _productApiService = new();

        #region Product

        public async Task<long> CreateProductAsync(HttpClient client, CreateProductCommand command)
        {
            return await _productApiService.CreateProductAsync(client, command);
        }

        public async Task<long> CreateProductAsync(HttpClient client, long? placeId = null)
        {
            placeId ??= await CreatePlaceAsync(client);

            var command = new CreateProductCommand()
            {
                PlaceId = placeId.Value,
                Name = Guid.NewGuid().ToString(),
                Categories = new List<ProductCategory>() { ProductCategory.Food }
            };
            return await CreateProductAsync(client, command);
        }

        public async Task<ProductReadModel?> GetProductAsync(HttpClient client, long productId)
        {
            return await _productApiService.GetProductAsync(client, new GetProductByIdQuery() { Id = productId });
        }

        #endregion

        #region Place

        public async Task<long> CreatePlaceAsync(HttpClient client, CreatePlaceCommand command)
        {
            return await _placeApiService.CreatePlaceAsync(client, command);
        }

        public async Task<long> CreatePlaceAsync(HttpClient client)
        {
            var command = new CreatePlaceCommand()
            {
                Name = Guid.NewGuid().ToString(),
                Categories = new List<PlaceCategory>() { PlaceCategory.Pharmacy },
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
            return await CreatePlaceAsync(client, command);
        }

        #endregion

    }
}
