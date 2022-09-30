using Bridge.Infrastructure.Services;
using FluentAssertions;

namespace Bridge.UnitTests.InfrastructureTests
{
    public class CoordinateServiceTests
    {
        /**
         * 좌표 데이터 출처 https://map.ngii.go.kr/ms/mesrInfo/coordinate.do
         * 
         **/

        [Theory]
        [InlineData(127.348462, 37.464136, 986599, 1940558)]
        [InlineData(127.303934, 37.193653, 982599, 1910558)]
        [InlineData(127.416903, 36.923353, 992599, 1880558)]
        public void Convert_LongitudeLatitude_EastingNorthing_With_1M_Precision(double longitude, double latitude, double easting, double northing)
        {
            // Arrange
            var coordinateService = new CoordinateService();

            // Act
            var eastingAndNorthing = coordinateService.ConvertToUtmK(longitude, latitude);

            // Assert
            eastingAndNorthing.Item1.Should().BeApproximately(easting, 1);
            eastingAndNorthing.Item2.Should().BeApproximately(northing, 1);
        }
    }
}
