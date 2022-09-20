using Bridge.Domain.Common.Exceptions;
using Bridge.Domain.Common.ValueObjects;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bridge.UnitTests.DomainTests
{
    public class LocationTests
    {
        [Fact]
        public void Create_Latitude_Longitude_Are_Equal()
        {
            // Arrange
            var latitude = new Random().Next(90);
            var longitude = new Random().Next(180);

            // Act
            var location = Location.From(latitude, longitude);

            // Assert
            location.Latitude.Should().Be(latitude);
            location.Longitude.Should().Be(longitude);
        }

        [Fact]
        public void Default_Latitude_Is_Zero()
        {
            // Arrange

            // Act
            var location = Location.Default();

            // Assert
            location.Latitude.Should().Be(0);
        }

        [Fact]
        public void Default_Longitude_Is_Zero()
        {
            // Arrange

            // Act
            var location = Location.Default();

            // Assert
            location.Longitude.Should().Be(0);
        }

        [Fact]
        public void Latitude_Cannot_Be_Less_Than_Negative90()
        {
            // Arrange

            // Act
            var action = () => { var location = Location.From(-91, 0); };

            // Assert
            action.Should().ThrowExactly<InvalidLocationException>();
        }

        [Fact]
        public void Latitude_Cannot_Be_Greater_Than_Positive90()
        {
            // Arrange

            // Act
            var action = () => { var location = Location.From(91, 0); };

            // Assert
            action.Should().ThrowExactly<InvalidLocationException>();
        }

        [Fact]
        public void Longitude_Cannot_Be_Less_Than_Negative180()
        {
            // Arrange

            // Act
            var action = () => { var location = Location.From(0, -181); };

            // Assert
            action.Should().ThrowExactly<InvalidLocationException>();
        }


        [Fact]
        public void Longitude_Cannot_Be_Greater_Than_Positive180()
        {
            // Arrange

            // Act
            var action = () => { var location = Location.From(0, 181); };

            // Assert
            action.Should().ThrowExactly<InvalidLocationException>();
        }
    }
}
