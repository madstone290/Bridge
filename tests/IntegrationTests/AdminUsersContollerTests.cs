using Bridge.Application.Users.Commands;
using Bridge.Application.Users.Queries;
using Bridge.Application.Users.ReadModels;
using Bridge.IntegrationTests.Config;
using Bridge.Shared;
using FluentAssertions;
using System.Net.Http.Json;

namespace Bridge.IntegrationTests
{
    public class AdminUsersContollerTests : IClassFixture<ApiTestFactory>
    {
        private readonly HttpClient _client;

        public AdminUsersContollerTests(ApiTestFactory apiTestFactory)
        {
            _client = apiTestFactory.Client;
        }


        [Fact]
        public async Task Create_AdminUser_Return_Ok_With_Id()
        {
            // Arrange
            var command = new CreateAdminUserCommand()
            {
                CreatorIdentityUserId = Seeds.RootUser.IdentityUserId,
                Name = Guid.NewGuid().ToString(),
            };

            // Act
            var request = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.AdminUsers.Create)
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
        public async Task Get_AdminUser_Return_Ok_With_Content()
        {
            // Arrange
            var command = new CreateAdminUserCommand()
            {
                CreatorIdentityUserId = Seeds.RootUser.IdentityUserId,
                Name = Guid.NewGuid().ToString()
            };
            var createRequest = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.AdminUsers.Create)
            {
                Content = JsonContent.Create(command)
            };
            var createResponse = await _client.SendAsync(createRequest);

            // Act
            var query = new GetAdminUserByIdQuery()
            {
                Id = await createResponse.Content.ReadFromJsonAsync<long>()
            };
            var request = new HttpRequestMessage(HttpMethod.Get, ApiRoutes.AdminUsers.Get.Replace("{Id}", $"{query.Id}"));
            var response = await _client.SendAsync(request);

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var user = await response.Content.ReadFromJsonAsync<UserReadModel>() ?? default!;
            user.Should().NotBeNull();
            user.Id.Should().Be(query.Id);
            user.Name.Should().Be(command.Name);

        }

    }
}
