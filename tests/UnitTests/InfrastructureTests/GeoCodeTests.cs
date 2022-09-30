using Bridge.Infrastructure.NaverMaps;
using Bridge.Infrastructure.NaverMaps.Data;
using Bridge.UnitTests.InfrastructureTests.Fixtures;
using FluentAssertions;

namespace Bridge.UnitTests.InfrastructureTests
{
    public class GeoCodeTests : IClassFixture<GeoCodeApiFixture>
    {
        private readonly GeoCodeApi.Config _config;

        public GeoCodeTests(GeoCodeApiFixture fixture)
        {
            _config = fixture.Config;
        }

        [Fact]
        public async Task Api_Does_Not_Support_Empty_String()
        {
            // Arrange
            var httpClient = new HttpClient();
            var naverApi = new GeoCodeApi(httpClient, _config);

            // Act
            var action = async () => await naverApi.GetAddressInfo(string.Empty);

            // Assert
            await action.Should().ThrowAsync<ArgumentNullException>();
        }

        [Theory]
        [InlineData("청수로25길 118-10")]
        [InlineData("강남구 논현로 710")]
        [InlineData("연제구 중앙대로 1001")]
        public async Task Valid_Address_Get_Response_With_Result(string address)
        {
            // Arrange
            var httpClient = new HttpClient();
            var naverApi = new GeoCodeApi(httpClient, _config);

            // Act
            var responseBody = await naverApi.GetAddressInfo(address);

            // Assert
            responseBody.Status.Should().Be(GeoCodeResponseBody.StatusOk);
            responseBody.Meta.TotalCount.Should().BeGreaterThan(0);
            responseBody.Addresses.Should().NotBeEmpty();
            responseBody.Addresses.First().RoadAddress.Should().NotBeEmpty();
            responseBody.Addresses.First().JibunAddress.Should().NotBeEmpty();
            responseBody.Addresses.First().SIDO.Should().NotBeEmpty();
            responseBody.Addresses.First().SIGUGUN.Should().NotBeEmpty();
            responseBody.Addresses.First().DONGMYUN.Should().NotBeEmpty();
            responseBody.Addresses.First().ROAD_NAME.Should().NotBeEmpty();
            responseBody.Addresses.First().BUILDING_NUMBER.Should().NotBeEmpty();
            responseBody.Addresses.First().POSTAL_CODE.Should().NotBeEmpty();
            responseBody.Addresses.First().X.Should().NotBeEmpty();
            responseBody.Addresses.First().Y.Should().NotBeEmpty();

        }

        [Theory]
        [InlineData("주소가 아님")]
        [InlineData("청수로25길 118-111111")]
        [InlineData("광주시 동대문구 중앙대로 34길")]
        public async Task Invalid_Address_Get_Response_With_No_Result(string address)
        {
            // Arrange
            var httpClient = new HttpClient();
            var naverApi = new GeoCodeApi(httpClient, _config);

            // Act
            var responseBody = await naverApi.GetAddressInfo(address);

            // Assert
            responseBody.Status.Should().Be(GeoCodeResponseBody.StatusOk);
            responseBody.Meta.TotalCount.Should().Be(0);

        }

    }
}
