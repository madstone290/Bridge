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

namespace Bridge.IntegrationTests.Admin
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

        public static AddressDto AddressDto(string? roadAddress = null, string? details = null)
        {
            return new()
            {
                BaseAddress = roadAddress ?? "대구시 수성구 청수로 25길 118-10",
                DetailAddress = details ?? "아테네 1440호"
            };
        }
      
        [Fact]
        public async Task Consumer_Cannot_Access()
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
                }
            };
            var createRequest = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Admin.Places.Create) { Content = JsonContent.Create(command) };
            var getRequest = new HttpRequestMessage(HttpMethod.Get, ApiRoutes.Admin.Places.Get.AddQueryParam("id", 1));
            
            // Act
            var createResponse = await _client.SendAsConsumerAsync(createRequest);
            var getResponse = await _client.SendAsConsumerAsync(getRequest);

            // Assert
            createResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.Forbidden);
            getResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task NoAuthentication_Cannot_Create_Place()
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
                }
            };

            // Act
            var request = new HttpRequestMessage(HttpMethod.Post,ApiRoutes.Admin.Places.Create)
            {
                Content = JsonContent.Create(command)
            };
            var response = await _client.SendAsync(request);

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Create_Place_Return_Ok_With_Id()
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
                }
            };

            // Act
            var request = new HttpRequestMessage(HttpMethod.Post,ApiRoutes.Admin.Places.Create)
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
            var createRequest = new HttpRequestMessage(HttpMethod.Post,ApiRoutes.Admin.Places.Create)
            {
                Content = JsonContent.Create(command)
            };
            var createResponse = await _client.SendAsAdminAsync(createRequest);

            // Act
            var id = await createResponse.Content.ReadFromJsonAsync<long>();
            var request = new HttpRequestMessage(HttpMethod.Get,ApiRoutes.Admin.Places.Get.Replace("{id}", $"{id}"));
            var response = await _client.SendAsAdminAsync(request);

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var place = await response.Content.ReadFromJsonAsync<PlaceReadModel>() ?? default!;
            place.Should().NotBeNull();
            place.Id.Should().Be(id);
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
        [Theory]
        [InlineData("슈퍼마켓", "대구광역시 남구 대명동 1796-17", "1층", "Cafeteria", "Pharmacy", "053-444-5552")]
        [InlineData("화장실", "대구광역시 남구 대명동 1819-31", "1층", "Hospital", "Pharmacy", "053-333-5552")]
        public async Task Update_BaseInfo_Return_Ok(string name, string baseAddress, string detailAddress, string category1, string category2, string contactNumber)
        {
            // Arrange
            var placeId = await _apiClient.CreatePlaceAsync();
            var command = new UpdatePlaceBaseInfoCommand()
            {
                Id = placeId,
                Name = name,
                Address = new AddressDto() 
                { 
                    BaseAddress = baseAddress,
                    DetailAddress = detailAddress
                },
                Categories = new List<PlaceCategory>()
                {
                    Enum.Parse<PlaceCategory>(category1, true),
                    Enum.Parse<PlaceCategory>(category2, true),
                },
                ContactNumber = contactNumber,
            };

            // Act
            var addRequest = new HttpRequestMessage(HttpMethod.Put, ApiRoutes.Admin.Places.UpdateBaseInfo.Replace("{id}", $"{placeId}"))
            {
                Content = JsonContent.Create(command)
            };
            var addResponse = await _client.SendAsAdminAsync(addRequest);

            var getRequest = new HttpRequestMessage(HttpMethod.Get, ApiRoutes.Admin.Places.Get.Replace("{id}", $"{placeId}"));
            var getResponse = await _client.SendAsAdminAsync(getRequest);

            // Assert
            addResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var place = await getResponse.Content.ReadFromJsonAsync<PlaceReadModel>() ?? null!;
            place.Name.Should().Be(name);
            place.Address.DetailAddress.Should().Be(command.Address.DetailAddress);
            place.Categories.Should().BeEquivalentTo(command.Categories);
            place.ContactNumber.Should().Be(command.ContactNumber);
        }

        [Theory]
        [InlineData(DayOfWeek.Sunday, true, false, null, null, null, null)]
        [InlineData(DayOfWeek.Monday, false, true, null, null, null, null)]
        [InlineData(DayOfWeek.Tuesday, false, false, 6, 18, null, null)]
        [InlineData(DayOfWeek.Wednesday, false, false, 6, 18, 15, 16)]
        public async Task Update_OpeningTimes_Return_Ok(DayOfWeek day, bool dayoff, bool twentyFourHours, double? openTime, double? closeTime,
            double? breakStartTime, double? breakEndTime)
        {
            // Arrange
            var placeId = await _apiClient.CreatePlaceAsync();
            var command = new UpdatePlaceOpeningTimesCommand()
            {
                Id = placeId,
                OpeningTimes = new List<OpeningTimeDto>()
                {
                     new OpeningTimeDto()
                    {
                        Day = day,
                        Dayoff = dayoff,
                        TwentyFourHours = twentyFourHours,
                        OpenTime = openTime.HasValue ? TimeSpan.FromHours(openTime.Value) : null,
                        CloseTime = closeTime.HasValue ? TimeSpan.FromHours(closeTime.Value) : null,
                        BreakStartTime = breakStartTime.HasValue ? TimeSpan.FromHours(breakStartTime.Value) : null,
                        BreakEndTime = breakEndTime.HasValue ? TimeSpan.FromHours(breakEndTime.Value) : null,
                    }
                }
            };

            // Act
            var addRequest = new HttpRequestMessage(HttpMethod.Put, ApiRoutes.Admin.Places.UpdateOpeningTimes.Replace("{id}", $"{placeId}"))
            {
                Content = JsonContent.Create(command)
            };
            var addResponse = await _client.SendAsAdminAsync(addRequest);

            var getRequest = new HttpRequestMessage(HttpMethod.Get,ApiRoutes.Admin.Places.Get.Replace("{id}", $"{placeId}"));
            var getResponse = await _client.SendAsAdminAsync(getRequest);

            // Assert
            addResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var place = await getResponse.Content.ReadFromJsonAsync<PlaceReadModel>() ?? null!;
            place.OpeningTimes.Should().ContainEquivalentOf(command.OpeningTimes.First());
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
            var addRequest = new HttpRequestMessage(HttpMethod.Post,ApiRoutes.Admin.Places.UpdateCategories.Replace("{id}", $"{placeId}"))
            {
                Content = JsonContent.Create(command)
            };
            var addResponse = await _client.SendAsAdminAsync(addRequest);

            var getRequest = new HttpRequestMessage(HttpMethod.Get,ApiRoutes.Admin.Places.Get.Replace("{id}", $"{placeId}"));
            var getResponse = await _client.SendAsAdminAsync(getRequest);

            // Assert
            addResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var place = await getResponse.Content.ReadFromJsonAsync<PlaceReadModel>() ?? null!;
            place.Categories.Should().Contain(command.Categories);
        }

        [Fact]
        public async Task Get_Places_By_PlaceType_Return_Ok_With_Content()
        {
            // Arrange
            var command1 = new CreatePlaceCommand()
            {
                Name = Guid.NewGuid().ToString(),
                Type = PlaceType.Cafeteria,
                Address = AddressDto()
            };
            var command2 = new CreatePlaceCommand()
            {
                Name = Guid.NewGuid().ToString(),
                Type = PlaceType.Cafeteria,
                Address = AddressDto()
            };
            var command3 = new CreatePlaceCommand()
            {
                Name = Guid.NewGuid().ToString(),
                Type = PlaceType.Restaurant,
                Address = AddressDto()
            };
            var command4 = new CreatePlaceCommand()
            {
                Name = "다라바",
                Type = PlaceType.Restaurant,
                Address = AddressDto()
            };
            await _apiClient.CreatePlaceAsync(command1);
            await _apiClient.CreatePlaceAsync(command2);
            await _apiClient.CreatePlaceAsync(command3);
            await _apiClient.CreatePlaceAsync(command4);
            var placeType = PlaceType.Cafeteria;

            // Act
            var request = new HttpRequestMessage(HttpMethod.Get,ApiRoutes.Admin.Places.GetList.AddQueryParam("placeType", placeType.ToString()));
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
                place.Type.Should().Be(placeType);
            }
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
            var request = new HttpRequestMessage(HttpMethod.Post,ApiRoutes.Admin.Places.Search)
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

        [Theory]
        [InlineData("킴 미용실", "대구광역시 수성구 황금동 887-4", "3층", "053-442-2345", "Restaurant", "Cafeteria", false, false, "12:00", "20:00", null, null)]
        [InlineData("다이소 황금점", "대구광역시 수성구 청수로 81", "다이소", "053-411-2345", "Restaurant", "PetStore", true, false, null, null, null, null)]
        public async Task Update_Place_Return_Ok(string name, string baseAddress, string detailAddress,
            string contactNumber, string category1, string category2, bool dayoff, bool twentyFourHours, string openTime,
            string closeTime, string? breakStartTime, string? breakEndTime)
        {
            // Arrange
            var placeId = await _apiClient.CreatePlaceAsync();
            var command = new UpdatePlaceCommand()
            {
                Id = placeId,
                Name = name,
                Address = new AddressDto()
                {
                    BaseAddress = baseAddress,
                    DetailAddress = detailAddress
                },
                ContactNumber = contactNumber,
                Categories = new List<PlaceCategory>() { Enum.Parse<PlaceCategory>(category1), Enum.Parse<PlaceCategory>(category2) },
                OpeningTimes = new List<OpeningTimeDto>()
                {
                    new OpeningTimeDto(){
                        Day = DayOfWeek.Monday,
                        Dayoff = dayoff,
                        TwentyFourHours = twentyFourHours,
                        OpenTime = openTime == null ? null :  TimeSpan.Parse(openTime),
                        CloseTime =  closeTime == null ? null : TimeSpan.Parse(closeTime),
                        BreakStartTime = breakStartTime == null ? null : TimeSpan.Parse(breakStartTime),
                        BreakEndTime = breakEndTime == null ? null :TimeSpan.Parse(breakEndTime)
                    }
                }
            };

            // Act
            var updateRequest = new HttpRequestMessage(HttpMethod.Put,ApiRoutes.Admin.Places.Update.Replace("{id}", $"{placeId}"))
            {
                Content = JsonContent.Create(command)
            };
            var updateResponse = await _client.SendAsAdminAsync(updateRequest);

            var getRequest = new HttpRequestMessage(HttpMethod.Get,ApiRoutes.Admin.Places.Get.Replace("{id}", $"{placeId}"));
            var getResponse = await _client.SendAsAdminAsync(getRequest);

            // Assert
            updateResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var place = await getResponse.Content.ReadFromJsonAsync<PlaceReadModel>() ?? null!;
            place.Categories.Should().Contain(command.Categories);
            
            place.Name.Should().Be(command.Name);
            place.Address.BaseAddress.Should().NotBeEmpty();
            place.Address.DetailAddress.Should().Be(command.Address.DetailAddress);
            place.ContactNumber.Should().Be(command.ContactNumber);
            place.Categories.Should().BeEquivalentTo(command.Categories);
            place.OpeningTimes.Should().ContainEquivalentOf(command.OpeningTimes.First());
        }
    }
}




