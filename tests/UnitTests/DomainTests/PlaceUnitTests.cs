using Bridge.Domain.Common.ValueObjects;
using Bridge.Domain.Places.Entities;
using Bridge.Domain.Places.Exceptions;
using Bridge.Domain.Users.Entities;
using Bridge.UnitTests.DomainTests.Builders;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Bridge.UnitTests.DomainTests
{
    public class PlaceUnitTests : IClassFixture<UserBuilder>, IClassFixture<PlaceBuilder>
    {
        private readonly UserBuilder _userBuilder;
        private readonly PlaceBuilder _placeBuilder;

        public PlaceUnitTests(UserBuilder userBuilder, PlaceBuilder placeBuilder)
        {
            _userBuilder = userBuilder;
            _placeBuilder = placeBuilder;
        }

        private User NewAdmin()
        {
            var user = _userBuilder.BuildAdminUser();
            return user;
        }

        private Place NewPlace()
        {
            var user = _userBuilder.BuildAdminUser();
            var place = _placeBuilder.Build(user);
            return place;
        }

        [Fact]
        public void Name_Cannot_Be_Empty()
        {
            // Arrange
            var name = string.Empty;
            var user = NewAdmin();
            var location = Location.Default();

            // Act
            var action = () =>
            {
                var place = Place.Create(user, name, location);
            };

            // Assert
            action.Should().ThrowExactly<InvalidPlaceNameException>();

        }

        [Fact]
        public void Default_Categories_Are_Empty()
        {
            // Arrange

            // Act
            var place = NewPlace();

            // Assert
            place.Categories.Should().BeEmpty();
        }

        [Fact]
        public void Category_Is_Added()
        {
            // Arrange
            var place = NewPlace();

            // Act
            var category = PlaceCategory.Pharmacy;
            place.AddCategory(category);

            // Assert
            place.Categories.Should().Contain(category);
        }

        [Fact]
        public void Duplicate_Category_Is_Not_Added()
        {
            // Arrange
            var place = NewPlace();
            var category = PlaceCategory.Pharmacy;
            place.AddCategory(category);

            // Act
            place.AddCategory(category);

            // Assert
            place.Categories.Should().HaveCount(1);
        }

        [Fact]
        public void Category_Is_Removed()
        {
            // Arrange
            var place = NewPlace();
            var category = PlaceCategory.Pharmacy;
            place.AddCategory(category);

            // Act
            place.RemoveCategory(category);

            // Assert
            place.Categories.Should().HaveCount(0);
        }

        [Fact]
        public void Name_Is_Changed()
        {
            // Arrange
            var place = NewPlace();

            // Act
            var nameToChange = Guid.NewGuid().ToString();
            place.SetName(nameToChange);

            // Assert
            place.Name.Should().Be(nameToChange);
        }

        [Fact]
        public void Location_Is_Changed()
        {
            // Arrange
            var place = NewPlace();
            // Act
            var locationToChange = Location.Create(1, 1, 0, 0);
            place.SetLocation(locationToChange);

            // Assert
            place.Location.Should().Be(locationToChange);
        }

        [Fact]
        public void ContactNumber_Is_Changed()
        {
            // Arrange
            var place = NewPlace();

            // Act
            var contactNumber = "010-1234-5678";
            place.SetContactNumber(contactNumber);

            // Assert
            place.ContactNumber.Should().Be(contactNumber);
        }

        [Fact]
        public void Default_OpeningTimes_Are_Empty()
        {
            // Arrange

            // Act
            var place = NewPlace();

            // Assert
            place.OpeningTimes.Should().BeEmpty();
        }

        [Fact]
        public void OpeningTime_Is_Added()
        {
            // Arrange
            var place = NewPlace();
            var day = DayOfWeek.Friday;
            var openTime = TimeSpan.FromHours(6);
            var closeTime = TimeSpan.FromHours(18);

            // Act
            place.AddOpeningTime(day, openTime, closeTime);

            // Assert
            place.OpeningTimes.Should().Contain(
                x => x.Day == day &&
                x.OpenTime == openTime &&
                x.CloseTime == closeTime);
        }

        [Fact]
        public void SameDay_OpeningTime_Is_Updated()
        {
            // Arrange
            var place = NewPlace();
            var day = DayOfWeek.Friday;
            var oldOpenTime = TimeSpan.FromHours(6);
            var oldCloseTime = TimeSpan.FromHours(18);
            place.AddOpeningTime(day, oldOpenTime, oldCloseTime);

            // Act
            var newOpenTime = TimeSpan.FromHours(3);
            var newCloseTime = TimeSpan.FromHours(10);
            place.AddOpeningTime(day, newOpenTime, newCloseTime);

            // Assert
            place.OpeningTimes.Should().NotContain(x =>
                x.Day == day &&
                x.OpenTime == oldOpenTime &&
                x.CloseTime == oldCloseTime);

            place.OpeningTimes.Should().Contain(x =>
                x.Day == day &&
                x.OpenTime == newOpenTime &&
                x.CloseTime == newCloseTime);
        }



    }
}
