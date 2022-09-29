using Bridge.Api.Controllers.Dtos;
using Bridge.Application.Places.Commands;
using Bridge.Application.Places.Dtos;
using Bridge.Application.Places.Queries;
using Bridge.Application.Places.ReadModels;
using Bridge.Domain.Places.Entities;
using Bridge.IntegrationTests.Config;
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

        [Fact]
        public async Task Create_Place_Return_Ok_With_Id()
        {
            // Arrange
            var command = new CreatePlaceCommand()
            {
                Name = Guid.NewGuid().ToString(),
                Address = "대구시 수성구",
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
            var response = await _client.SendAsAdminAsync(request);
            
            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var id = await response.Content.ReadFromJsonAsync<long>();
            id.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task Consumer_Cannot_Create_Place()
        {
            // Arrange
            var command = new CreatePlaceCommand()
            {
                Name = Guid.NewGuid().ToString(),
                Address = "대구시 수성구",
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
            var response = await _client.SendAsConsumerAsync(request);

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task NoAuthentication_Cannot_Create_Place()
        {
            // Arrange
            var command = new CreatePlaceCommand()
            {
                Name = Guid.NewGuid().ToString(),
                Address = "대구시 수성구",
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
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Get_Place_Return_Ok_With_Content()
        {
            // Arrange
            var command = new CreatePlaceCommand()
            {
                Name = Guid.NewGuid().ToString(),
                Address = "대구시 수성구",
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
            var createResponse = await _client.SendAsAdminAsync(createRequest);

            // Act
            var id = await createResponse.Content.ReadFromJsonAsync<long>();
            var request = new HttpRequestMessage(HttpMethod.Get, ApiRoutes.Places.Get.Replace("{id}", $"{id}"));
            var response = await _client.SendAsAdminAsync(request);

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var place = await response.Content.ReadFromJsonAsync<PlaceReadModel>() ?? default!;
            place.Should().NotBeNull();
            place.Id.Should().Be(id);
            place.Name.Should().Be(command.Name);
            place.ContactNumber.Should().Be(command.ContactNumber);
            place.Address.Should().Be(command.Address);
            place.Categories.Should().BeEquivalentTo(command.Categories);
            place.Location.Latitude.Should().NotBe(0);
            place.Location.Longitude.Should().NotBe(0);
            place.Location.Easting.Should().NotBe(0);
            place.Location.Northing.Should().NotBe(0);
            place.OpeningTimes.Should().ContainEquivalentOf(command.OpeningTimes[0]);
            place.OpeningTimes.Should().ContainEquivalentOf(command.OpeningTimes[1]);
            place.OpeningTimes.Should().ContainEquivalentOf(command.OpeningTimes[2]);

        }

        [Theory]
        [InlineData(DayOfWeek.Sunday, true, false, null, null, null, null)]
        [InlineData(DayOfWeek.Monday, false, true, null, null, null, null)]
        [InlineData(DayOfWeek.Tuesday, false, false, 6, 18, null , null)]
        [InlineData(DayOfWeek.Wednesday, false, false, 6, 18, 15, 16)]
        public async Task Add_OpeningTime_Return_Ok(DayOfWeek day, bool dayoff, bool twentyFourHours, double? openTime, double? closeTime,
            double? breakStartTime, double? breakEndTime)
        {
            // Arrange
            var placeId = await _apiClient.CreatePlaceAsync();
            var command = new AddOpeningTimeCommand()
            {
                PlaceId = placeId,
                OpeningTime = new OpeningTimeDto()
                {
                    Day = day,
                    Dayoff = dayoff,
                    TwentyFourHours = twentyFourHours,
                    OpenTime = openTime.HasValue ? TimeSpan.FromHours(openTime.Value) : null,
                    CloseTime = closeTime.HasValue ? TimeSpan.FromHours(closeTime.Value): null,
                    BreakStartTime = breakStartTime.HasValue ? TimeSpan.FromHours(breakStartTime.Value): null,
                    BreakEndTime = breakEndTime.HasValue ? TimeSpan.FromHours(breakEndTime.Value): null,
                }
            };

            // Act
            var addRequest = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Places.AddOpeningTime.Replace("{id}", $"{placeId}"))
            {
                Content = JsonContent.Create(command)
            };
            var addResponse = await _client.SendAsAdminAsync(addRequest);

            var getRequest = new HttpRequestMessage(HttpMethod.Get, ApiRoutes.Places.Get.Replace("{id}", $"{placeId}"));
            var getResponse = await _client.SendAsAdminAsync(getRequest);

            // Assert
            addResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var place = await getResponse.Content.ReadFromJsonAsync<PlaceReadModel>() ?? null!;
            place.OpeningTimes.Should().ContainEquivalentOf(command.OpeningTime);
        }
     
        [Fact]
        public async Task Update_Categories_Return_Ok()
        {
            // Arrange
            var placeId = await _apiClient.CreatePlaceAsync();
            var command = new UpdatePlaceCategoryCommand()
            {
                PlaceId = placeId,
                Categories = new List<PlaceCategory>() { PlaceCategory.Restaurant, PlaceCategory.Cafeteria, PlaceCategory.PetHospital }
            };

            // Act
            var addRequest = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Places.UpdateCategories.Replace("{id}", $"{placeId}"))
            {
                Content = JsonContent.Create(command)
            };
            var addResponse = await _client.SendAsAdminAsync(addRequest);

            var getRequest = new HttpRequestMessage(HttpMethod.Get, ApiRoutes.Places.Get.Replace("{id}", $"{placeId}"));
            var getResponse = await _client.SendAsAdminAsync(getRequest);

            // Assert
            addResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var place = await getResponse.Content.ReadFromJsonAsync<PlaceReadModel>() ?? null!;
            place.Categories.Should().Contain(command.Categories);
        }


        [Fact]
        public async Task Get_Places_Within_Region_Return_Ok_With_Content()
        {
            // Arrange
            var command1 = new CreatePlaceCommand()
            {
                Name = Guid.NewGuid().ToString(),
                Address = "대구시 수성구 utm:1000,1000",
            };
            var command2 = new CreatePlaceCommand()
            {
                Name = Guid.NewGuid().ToString(),
                Address = "대구시 수성구 utm:1000,2000",
            };
            var command3 = new CreatePlaceCommand()
            {
                Name = Guid.NewGuid().ToString(),
                Address = "대구시 수성구 utm:2000,2000",
            };
            var command4 = new CreatePlaceCommand()
            {
                Name = Guid.NewGuid().ToString(),
                Address = "대구시 수성구 utm:2000, 1000",
            };
            await _apiClient.CreatePlaceAsync(command1);
            await _apiClient.CreatePlaceAsync(command2);
            await _apiClient.CreatePlaceAsync(command3);
            await _apiClient.CreatePlaceAsync(command4);



            // Act
            var query = new GetPlacesByRegionQuery()
            {
                LeftEasting = 1000,
                RightEasting = 2000,
                BottomNorthing = 1000,
                TopNorthing = 1000
            };
            var request = new HttpRequestMessage(HttpMethod.Get, ApiRoutes.Places.GetList.AddQueryParam(query));
            var response = await _client.SendAsAdminAsync(request);

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var places = await response.Content.ReadFromJsonAsync<List<PlaceReadModel>>() ?? default!;
            places.Should().Contain(x => x.Name == command1.Name);
            places.Should().Contain(x => x.Name == command4.Name);

            foreach(var place in places)
            {
                place.Location.Easting.Should().BeInRange(query.LeftEasting, query.RightEasting);
                place.Location.Northing.Should().BeInRange(query.BottomNorthing, query.TopNorthing);
            }
        }


        [Fact]
        public async Task Get_Places_By_Name_And_Region_Return_Ok_With_Content()
        {
            // Arrange
            var command1 = new CreatePlaceCommand()
            {
                Name = "가나다",
                Address = "대구시 수성구 utm:1000,1000",
            };
            var command2 = new CreatePlaceCommand()
            {
                Name = "가나마",
                Address = "대구시 수성구 utm:1000,2000",
            };
            var command3 = new CreatePlaceCommand()
            {
                Name = "가나바",
                Address = "대구시 수성구 utm:2000,2000",
            };
            var command4 = new CreatePlaceCommand()
            {
                Name = "다라바",
                Address = "대구시 수성구 utm:2000, 1000",
            };
            await _apiClient.CreatePlaceAsync(command1);
            await _apiClient.CreatePlaceAsync(command2);
            await _apiClient.CreatePlaceAsync(command3);
            await _apiClient.CreatePlaceAsync(command4);



            // Act
            var query = new GetPlacesByNameAndRegionQuery()
            {
                Name = "다",
                LeftEasting = 1000,
                RightEasting = 2000,
                BottomNorthing = 1000,
                TopNorthing = 1000
            };
            var request = new HttpRequestMessage(HttpMethod.Get, ApiRoutes.Places.GetList.AddQueryParam(query));
            var response = await _client.SendAsAdminAsync(request);

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var places = await response.Content.ReadFromJsonAsync<List<PlaceReadModel>>() ?? default!;
            places.Should().Contain(x => x.Name == command1.Name);
            places.Should().Contain(x => x.Name == command4.Name);

            foreach (var place in places)
            {
                place.Name.Should().Contain(query.Name);
                place.Location.Easting.Should().BeInRange(query.LeftEasting, query.RightEasting);
                place.Location.Northing.Should().BeInRange(query.BottomNorthing, query.TopNorthing);
            }
        }


        [Fact]
        public async Task Get_Places_By_PlaceType_Return_Ok_With_Content()
        {
            // Arrange
            var command1 = new CreatePlaceCommand()
            {
                Name = Guid.NewGuid().ToString(),
                Type = PlaceType.Cafeteria,
                Address = "대구시 수성구 utm:1000,1000",
            };
            var command2 = new CreatePlaceCommand()
            {
                Name = Guid.NewGuid().ToString(),
                Type = PlaceType.Cafeteria,
                Address = "대구시 수성구 utm:1000,2000",
            };
            var command3 = new CreatePlaceCommand()
            {
                Name = Guid.NewGuid().ToString(),
                Type = PlaceType.Restaurant,
                Address = "대구시 수성구 utm:2000,2000",
            };
            var command4 = new CreatePlaceCommand()
            {
                Name = "다라바",
                Type = PlaceType.Restaurant,
                Address = "대구시 수성구 utm:2000, 1000",
            };
            await _apiClient.CreatePlaceAsync(command1);
            await _apiClient.CreatePlaceAsync(command2);


            // Act
            var searchDto = new PlaceSearchDto()
            {
                PlaceType = PlaceType.Cafeteria
            };
            var request = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Places.Search)
            {
                Content = JsonContent.Create(searchDto)
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
                place.Type.Should().Be(searchDto.PlaceType);
            }
        }
    }
}



