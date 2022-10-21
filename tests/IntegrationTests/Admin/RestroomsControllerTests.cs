using Bridge.Application.Places.Commands;
using Bridge.Application.Places.Dtos;
using Bridge.Application.Places.ReadModels;
using Bridge.IntegrationTests.Config;
using Bridge.Shared;
using FluentAssertions;
using System.Net.Http.Json;
using Xunit.Abstractions;

namespace Bridge.IntegrationTests.Admin
{
    public class RestroomsControllerTests : IClassFixture<ApiTestFactory>
    {
        private readonly TestClient _client;
        private readonly ITestOutputHelper _testOutputHelper;

        public RestroomsControllerTests(ApiTestFactory apiTestFactory, ITestOutputHelper testOutputHelper)
        {
            _client = apiTestFactory.Client;
            _testOutputHelper = testOutputHelper;
        }

        public static AddressDto AddressDto(string? roadAddress = null, string? details = null)
        {
            return new()
            {
                BaseAddress = roadAddress ?? "대구시 수성구 청수로 25길 118-10",
                DetailAddress = details ?? "1층"
            };
        }

        [Fact]
        public async Task Create_Restroom_Return_Ok_With_Id()
        {
            // Arrange
            var command = new CreateRestroomCommand()
            {
                Name = Guid.NewGuid().ToString(),
                Address = AddressDto(),
                IsUnisex = false,
                HasDiaperTable = false
            };

            // Act
            var request = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Admin.Restrooms.Create)
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
        public async Task Get_Restroom_Return_Ok_With_Content()
        {
            // Arrange
            var command = new CreateRestroomCommand()
            {
                Name = Guid.NewGuid().ToString(),
                Address = AddressDto(),
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
                },
                IsUnisex = false,
                HasDiaperTable = true,
                DiaperTableLocation = Domain.Places.Entities.Places.DiaperTableLocation.FemaleToilet,
                MaleToilet = 2,
                MaleUrinal = 3,
                MaleDisabledToilet = 1,
                MaleDisabledUrinal = 1,
                MaleKidToilet = 0,
                MaleKidUrinal = 0,
                FemaleToilet = 4,
                FemaleDisabledToilet = 1,
                FemaleKidToilet = 0
            };
            var createRequest = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Admin.Restrooms.Create)
            {
                Content = JsonContent.Create(command)
            };
            var createResponse = await _client.SendAsAdminAsync(createRequest);

            // Act
            var id = await createResponse.Content.ReadFromJsonAsync<long>();
            var request = new HttpRequestMessage(HttpMethod.Get, ApiRoutes.Admin.Restrooms.Get.Replace("{id}", $"{id}"));
            var response = await _client.SendAsAdminAsync(request);

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var restroom = await response.Content.ReadFromJsonAsync<RestroomReadModel>() ?? default!;
            restroom.Should().NotBeNull();
            restroom.Id.Should().Be(id);
            restroom.Name.Should().Be(command.Name);
            restroom.Address.BaseAddress.Should().NotBeEmpty();
            restroom.Location.Latitude.Should().NotBe(0);
            restroom.Location.Longitude.Should().NotBe(0);
            restroom.Location.Easting.Should().NotBe(0);
            restroom.Location.Northing.Should().NotBe(0);
            restroom.OpeningTimes.Should().ContainEquivalentOf(command.OpeningTimes[0]);
            restroom.OpeningTimes.Should().ContainEquivalentOf(command.OpeningTimes[1]);
            restroom.OpeningTimes.Should().ContainEquivalentOf(command.OpeningTimes[2]);
            restroom.IsUnisex.Should().Be(command.IsUnisex);
            restroom.HasDiaperTable.Should().Be(command.HasDiaperTable);
            restroom.DiaperTableLocation.Should().Be(command.DiaperTableLocation);
            restroom.MaleToilet.Should().Be(command.MaleToilet);
            restroom.MaleUrinal.Should().Be(command.MaleUrinal);
            restroom.MaleDisabledToilet.Should().Be(command.MaleDisabledToilet);
            restroom.MaleDisabledUrinal.Should().Be(command.MaleDisabledUrinal);
            restroom.MaleKidToilet.Should().Be(command.MaleKidToilet);
            restroom.MaleKidUrinal.Should().Be(command.MaleKidUrinal);
            restroom.FemaleToilet.Should().Be(command.FemaleToilet);
            restroom.FemaleDisabledToilet.Should().Be(command.FemaleDisabledToilet);
            restroom.FemaleKidToilet.Should().Be(command.FemaleKidToilet);

        }

        [Fact]
        public async Task Update_Restroom_Return_Ok()
        {
            // Arrange
            var createCommand = new CreateRestroomCommand()
            {
                Name = "화장실 생성",
                Address = AddressDto(),
                IsUnisex = false,
                HasDiaperTable = false
            };
            var createRequest = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Admin.Restrooms.Create) { Content = JsonContent.Create(createCommand) };
            var createResponse = await _client.SendAsAdminAsync(createRequest);
            var restroomId = await createResponse.Content.ReadFromJsonAsync<long>();

            var updateCommand = new UpdateRestroomCommand()
            {
                Id = restroomId,
                Name = "화장실 수정",
                Address = AddressDto(),
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
                },
                IsUnisex = false,
                HasDiaperTable = true,
                DiaperTableLocation = Domain.Places.Entities.Places.DiaperTableLocation.FemaleToilet,
                MaleToilet = 2,
                MaleUrinal = 3,
                MaleDisabledToilet = 1,
                MaleDisabledUrinal = 1,
                MaleKidToilet = 0,
                MaleKidUrinal = 0,
                FemaleToilet = 4,
                FemaleDisabledToilet = 1,
                FemaleKidToilet = 0

            };

            // Act
            var updateRequest = new HttpRequestMessage(HttpMethod.Put, ApiRoutes.Admin.Restrooms.Update.Replace("{id}", $"{restroomId}")) { Content = JsonContent.Create(updateCommand) };
            var updateResponse = await _client.SendAsAdminAsync(updateRequest);

            var getRequest = new HttpRequestMessage(HttpMethod.Get, ApiRoutes.Admin.Restrooms.Get.Replace("{id}", $"{restroomId}"));
            var getResponse = await _client.SendAsAdminAsync(getRequest);

            // Assert
            updateResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var restroom = await getResponse.Content.ReadFromJsonAsync<RestroomReadModel>() ?? null!;
            restroom.Should().NotBeNull();
            restroom.Id.Should().Be(restroomId);
            restroom.Name.Should().Be(updateCommand.Name);
            restroom.Address.BaseAddress.Should().NotBeEmpty();
            restroom.Location.Latitude.Should().NotBe(0);
            restroom.Location.Longitude.Should().NotBe(0);
            restroom.Location.Easting.Should().NotBe(0);
            restroom.Location.Northing.Should().NotBe(0);
            restroom.OpeningTimes.Should().ContainEquivalentOf(updateCommand.OpeningTimes[0]);
            restroom.OpeningTimes.Should().ContainEquivalentOf(updateCommand.OpeningTimes[1]);
            restroom.OpeningTimes.Should().ContainEquivalentOf(updateCommand.OpeningTimes[2]);
            restroom.IsUnisex.Should().Be(updateCommand.IsUnisex);
            restroom.HasDiaperTable.Should().Be(updateCommand.HasDiaperTable);
            restroom.DiaperTableLocation.Should().Be(updateCommand.DiaperTableLocation);
            restroom.MaleToilet.Should().Be(updateCommand.MaleToilet);
            restroom.MaleUrinal.Should().Be(updateCommand.MaleUrinal);
            restroom.MaleDisabledToilet.Should().Be(updateCommand.MaleDisabledToilet);
            restroom.MaleDisabledUrinal.Should().Be(updateCommand.MaleDisabledUrinal);
            restroom.MaleKidToilet.Should().Be(updateCommand.MaleKidToilet);
            restroom.MaleKidUrinal.Should().Be(updateCommand.MaleKidUrinal);
            restroom.FemaleToilet.Should().Be(updateCommand.FemaleToilet);
            restroom.FemaleDisabledToilet.Should().Be(updateCommand.FemaleDisabledToilet);
            restroom.FemaleKidToilet.Should().Be(updateCommand.FemaleKidToilet);
        }

        [Fact]
        public async Task Create_Restroom_Batch_Return_Ok_With_Id()
        {
            // Arrange
            var batchCommand = new CreateRestroomBatchCommand();
            var subcommandList = new List<CreateRestroomCommand>();
            for (int i = 0; i < 100; i++)
            {
                var subcommand = new CreateRestroomCommand()
                {
                    LastUpdateDateTimeLocal = DateTime.Today,
                    Name = i.ToString(),
                    Address = AddressDto(),
                    IsUnisex = false,
                    HasDiaperTable = false
                };
                subcommandList.Add(subcommand);
            }
            batchCommand.Commands = subcommandList;

            // Act
            var request = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Admin.Restrooms.CreateBatch) { Content = JsonContent.Create(batchCommand) };
            var response = await _client.SendAsAdminAsync(request);

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

    }
}
