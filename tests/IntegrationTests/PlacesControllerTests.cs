using Bridge.Application.Places.Commands;
using Bridge.Application.Places.Dtos;
using Bridge.Application.Places.Queries;
using Bridge.Application.Places.ReadModels;
using Bridge.Domain.Places.Entities;
using Bridge.IntegrationTests.ApiServices;
using Bridge.IntegrationTests.Config;
using Bridge.Shared;
using FluentAssertions;
using System.Net.Http.Json;

namespace Bridge.IntegrationTests
{
    public class PlacesControllerTests : IClassFixture<ApiTestFactory>, IClassFixture<ApiService>
    {
        private readonly HttpClient _client;
        private readonly ApiService _apiService;

        public PlacesControllerTests(ApiTestFactory apiTestFactory, ApiService apiService)
        {
            _client = apiTestFactory.Client;
            _apiService = apiService;
        }

        [Fact]
        public async Task Create_Place_Return_Ok_With_Id()
        {
            // Arrange
            var command = new CreatePlaceCommand()
            {
                UserId = await _apiService.CreateAdminUserAsync(_client),
                Name = Guid.NewGuid().ToString(),
                Latitude = 1.422334,
                Longitude = 1.233222,
                Categories = new List<PlaceCategory>()
                {
                    PlaceCategory.Restaurant,
                    PlaceCategory.Cafeteria,
                    PlaceCategory.Pharmacy
                }
            };

            // Act
            var request = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Places.Create)
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
        public async Task Get_Place_Return_Ok_With_Content()
        {
            // Arrange
            var command = new CreatePlaceCommand()
            {
                UserId = await _apiService.CreateAdminUserAsync(_client),
                Name = Guid.NewGuid().ToString(),
                Latitude = 1.422334,
                Longitude = 1.233222,
                Categories = new List<PlaceCategory>()
                {
                    PlaceCategory.Restaurant,
                    PlaceCategory.Cafeteria,
                    PlaceCategory.Pharmacy
                },
                OpeningTimes = new List<OpeningTimeDto>()
                {
                    new OpeningTimeDto()
                    {
                        Day = DayOfWeek.Monday,
                        OpenTime = TimeSpan.FromHours(10),
                        CloseTime = TimeSpan.FromHours(20),
                        BreakStartTime = TimeSpan.FromHours(14),
                        BreakEndTime = TimeSpan.FromHours(16),
                    },
                    new OpeningTimeDto()
                    {
                        Day = DayOfWeek.Tuesday,
                        OpenTime = TimeSpan.FromHours(10),
                        CloseTime = TimeSpan.FromHours(20),
                    },
                     new OpeningTimeDto()
                    {
                        Day = DayOfWeek.Sunday,
                        OpenTime = TimeSpan.FromHours(10),
                        CloseTime = TimeSpan.FromHours(16),
                    }
                }
            };
            var createRequest = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Places.Create)
            {
                Content = JsonContent.Create(command)
            };
            var createResponse = await _client.SendAsync(createRequest);

            // Act
            var query = new GetPlaceByIdQuery()
            {
                Id = await createResponse.Content.ReadFromJsonAsync<long>()
            };
            var request = new HttpRequestMessage(HttpMethod.Get, ApiRoutes.Places.Get.Replace("{Id}", $"{query.Id}"));
            var response = await _client.SendAsync(request);

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var place = await response.Content.ReadFromJsonAsync<PlaceReadModel>() ?? default!;
            place.Should().NotBeNull();
            place.Id.Should().Be(query.Id);
            place.Name.Should().Be(command.Name);
            place.Latitude.Should().Be(command.Latitude);
            place.Longitude.Should().Be(command.Longitude);
            place.Categories.Should().BeEquivalentTo(command.Categories);
            place.OpeningTimes.Should().BeEquivalentTo(command.OpeningTimes);


        }

        [Fact]
        public async Task Add_OpeningTime_Return_Ok()
        {
            // Arrange
            var userId = await _apiService.CreateAdminUserAsync(_client);
            var placeId = await _apiService.CreatePlaceAsync(_client, userId);
            var command = new AddOpeningTimeCommand()
            {
                PlaceId = placeId,
                OpeningTime = new OpeningTimeDto()
                {
                    Day = DayOfWeek.Monday,
                    OpenTime = TimeSpan.FromHours(8),
                    CloseTime = TimeSpan.FromHours(18),
                    BreakStartTime = TimeSpan.FromHours(14),
                    BreakEndTime = TimeSpan.FromHours(16),
                }
            };

            // Act
            var addRequest = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Places.AddOpeningTime.Replace("{Id}", $"{placeId}"))
            {
                Content = JsonContent.Create(command)
            };
            var addResponse = await _client.SendAsync(addRequest);

            var getRequest = new HttpRequestMessage(HttpMethod.Get, ApiRoutes.Places.Get.Replace("{Id}", $"{placeId}"));
            var getResponse = await _client.SendAsync(getRequest);

            // Assert
            addResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var place = await getResponse.Content.ReadFromJsonAsync<PlaceReadModel>() ?? null!;
            place.OpeningTimes.Should().ContainEquivalentOf(command.OpeningTime);
        }

        [Fact]
        public async Task Update_Categories_Return_Ok()
        {
            // Arrange
            var userId = await _apiService.CreateAdminUserAsync(_client);
            var placeId = await _apiService.CreatePlaceAsync(_client, userId);
            var command = new UpdatePlaceCategoryCommand()
            {
                PlaceId = placeId,
                Categories = new List<PlaceCategory>() { PlaceCategory.Restaurant, PlaceCategory.Cafeteria, PlaceCategory.PetHospital }
            };

            // Act
            var addRequest = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Places.UpdateCategories.Replace("{Id}", $"{placeId}"))
            {
                Content = JsonContent.Create(command)
            };
            var addResponse = await _client.SendAsync(addRequest);

            var getRequest = new HttpRequestMessage(HttpMethod.Get, ApiRoutes.Places.Get.Replace("{Id}", $"{placeId}"));
            var getResponse = await _client.SendAsync(getRequest);

            // Assert
            addResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var place = await getResponse.Content.ReadFromJsonAsync<PlaceReadModel>() ?? null!;
            place.Categories.Should().Contain(command.Categories);
        }
    }
}


