using Bridge.Application.Places.Commands;
using Bridge.Application.Places.Dtos;
using Bridge.Application.Places.Queries;
using Bridge.Application.Places.ReadModels;
using Bridge.Domain.Places.Entities;
using Bridge.IntegrationTests.Config;
using Bridge.IntegrationTests.Data;
using Bridge.Shared;
using Bridge.Shared.Extensions;
using FluentAssertions;
using System.Net.Http.Json;

namespace Bridge.IntegrationTests
{
    public class PlacesControllerTests : IClassFixture<ApiTestFactory>
    {
        private readonly TestClient _client;
        private readonly ApiClient _apiClient;

        public PlacesControllerTests(ApiTestFactory apiTestFactory)
        {
            _client = apiTestFactory.Client;
            _apiClient = apiTestFactory.ApiClient;
        }

        public static AddressDto AddressDto(string? roadAddress = null, string? detailAddress = null)
        {
            return new()
            {
                BaseAddress = roadAddress ?? "대구시 수성구 청수로 25길 118-10",
                DetailAddress = detailAddress ?? "아테네 1440호"
            };
        }
      
        [Fact]
        public async Task Get_Place_Return_Ok_With_Content()
        {
            // Arrange
            var command = new CreatePlaceCommand()
            {
                Name = Guid.NewGuid().ToString(),
                Address = AddressDto(),
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
            var placeId = await _apiClient.CreatePlaceAsync(command);

            // Act
            var request = new HttpRequestMessage(HttpMethod.Get, ApiRoutes.Places.Get.Replace("{id}", $"{placeId}"));
            var response = await _client.SendAsAdminAsync(request);

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var place = await response.Content.ReadFromJsonAsync<PlaceReadModel>() ?? default!;
            place.Should().NotBeNull();
            place.Id.Should().Be(placeId);
            place.Name.Should().Be(command.Name);
            place.ContactNumber.Should().Be(command.ContactNumber);
            place.Address.BaseAddress.Should().NotBeEmpty();
            place.Categories.Should().BeEquivalentTo(command.Categories);
            place.Location.Latitude.Should().NotBe(0);
            place.Location.Longitude.Should().NotBe(0);
            place.Location.Easting.Should().NotBe(0);
            place.Location.Northing.Should().NotBe(0);
            place.OpeningTimes.Should().ContainEquivalentOf(command.OpeningTimes[0]);
            place.OpeningTimes.Should().ContainEquivalentOf(command.OpeningTimes[1]);
            place.OpeningTimes.Should().ContainEquivalentOf(command.OpeningTimes[2]);

        }

        [Fact]
        public async Task Search_Places_Returns_Contents()
        {
            // Arrange
            var command1 = new CreatePlaceCommand()
            {
                Name = "abc",
                Type = PlaceType.Cafeteria,
                Address = AddressDto()
            };
            var command2 = new CreatePlaceCommand()
            {
                Name = "aeg",
                Type = PlaceType.Cafeteria,
                Address = AddressDto()
            };
            var command3 = new CreatePlaceCommand()
            {
                Name = "sfe",
                Type = PlaceType.Restaurant,
                Address = AddressDto()
            };
            var command4 = new CreatePlaceCommand()
            {
                Name = "fbr",
                Type = PlaceType.Restaurant,
                Address = AddressDto()
            };
            await _apiClient.CreatePlaceAsync(command1);
            await _apiClient.CreatePlaceAsync(command2);
            await _apiClient.CreatePlaceAsync(command3);
            await _apiClient.CreatePlaceAsync(command4);

            // Act
            var searchQuery = new SearchPlacesQuery()
            {
                SearchText = "a"
            };
            var request = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Places.Search)
            {
                Content = JsonContent.Create(searchQuery)
            };
            var response = await _client.SendAsAdminAsync(request);

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var places = await response.Content.ReadFromJsonAsync<List<PlaceReadModel>>() ?? default!;
            places.Should().Contain(x => x.Name == command1.Name);
            places.Should().Contain(x => x.Name == command2.Name);
            places.Should().NotContain(x => x.Name == command3.Name);
            places.Should().NotContain(x => x.Name == command4.Name);

            foreach (var place in places)
            {
                place.Name.Should().Contain(searchQuery.SearchText);
            }
        }

      
    }
}




