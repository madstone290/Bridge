using Bridge.Domain.Places.Entities;
using Bridge.Domain.Places.Exceptions;
using Bridge.DomainTests.Builders;
using FluentAssertions;

namespace Bridge.DomainTests.ValueObjects
{
    public class OpeningTimeTests : IClassFixture<PlaceBuilder>
    {
        [Fact]
        public void Create_OpeningTime_Values_Are_Equal()
        {
            // Arrange
            var day = DayOfWeek.Friday;
            var openTime = TimeSpan.FromHours(0);
            var closeTime = TimeSpan.FromHours(10);

            // Act
            var openingTime = OpeningTime.Between(day, openTime, closeTime);

            // Assert
            openingTime.Day.Should().Be(day);
            openingTime.OpenTime.Should().Be(openTime);
            openingTime.CloseTime.Should().Be(closeTime);
        }

        [Fact]
        public void StartTime_Cannot_Be_Less_Than_Zero()
        {
            // Arrange
            var day = DayOfWeek.Friday;
            var openTime = TimeSpan.FromHours(-1);
            var closeTime = TimeSpan.FromHours(3);

            // Act
            var action = () => { var _ = OpeningTime.Between(day, openTime, closeTime); };

            // Assert
            action.Should().ThrowExactly<InvalidTimeException>();
        }

        [Fact]
        public void CloseTime_Cannot_Be_Greater_Than_24Hour()
        {
            // Arrange
            var day = DayOfWeek.Friday;
            var openTime = TimeSpan.FromHours(0);
            var closeTime = TimeSpan.FromHours(25);
            
            // Act
            var action = () => { var _ = OpeningTime.Between(day, openTime, closeTime); };

            // Assert
            action.Should().ThrowExactly<InvalidTimeException>();
        }

        [Fact]
        public void Opentime_Cannot_Be_Greater_Than_Closetime()
        {
            // Arrange
            var day = DayOfWeek.Friday;
            var opentime = TimeSpan.FromHours(3);
            var closetime = TimeSpan.FromHours(2);

            // Act
            var action = () => { var _ = OpeningTime.Between(day, opentime, closetime); };

            // Assert
            action.Should().ThrowExactly<InvalidTimeException>();
        }

        [Fact]
        public void OpeningHours_With_ZeroOpenTime_24CloseTime_Is_24Hours()
        {
            // Arrange
            var day = DayOfWeek.Friday;
            var opentime = TimeSpan.FromHours(0);
            var closetime = TimeSpan.FromHours(24);

            // Act
            var openingTime = OpeningTime.Between(day, opentime, closetime);

            // Assert
            openingTime.Is24Hours.Should().BeTrue();
        }

        [Fact]
        public void OpeningHours_From_Everytime_Is_24Hours()
        {
            // Arrange
            var day = DayOfWeek.Friday;

            // Act
            var openingTime = OpeningTime.TwentyFourHours(day);

            // Assert
            openingTime.Is24Hours.Should().BeTrue();
        }

        [Fact]
        public void OpeningHours_Without_ZeroOpenTime_24CloseTime_Is_Not_24Hours()
        {
            // Arrange
            var day = DayOfWeek.Friday;
            var opentime = TimeSpan.FromHours(1);
            var closetime = TimeSpan.FromHours(23);

            // Act
            var openingTime = OpeningTime.Between(day, opentime, closetime);

            // Assert
            openingTime.Is24Hours.Should().BeFalse();
        }
    }
}
