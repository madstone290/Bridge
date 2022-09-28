using Bridge.Domain.Common.ValueObjects;
using Bridge.Domain.Places.Entities;
using Bridge.Domain.Places.Exceptions;
using Bridge.UnitTests.DomainTests.Builders;
using FluentAssertions;

namespace Bridge.UnitTests.DomainTests
{
    public class PlaceUnitTests : IClassFixture<PlaceBuilder>
    {
        private readonly PlaceBuilder _placeBuilder;

        public PlaceUnitTests(PlaceBuilder placeBuilder)
        {
            _placeBuilder = placeBuilder;
        }

        private Place NewPlace()
        {
            var place = _placeBuilder.Build();
            return place;
        }

        [Fact]
        public void Name_Cannot_Be_Empty()
        {
            // Arrange
            var name = string.Empty;
            var address = "대구시 수성구";
            var location = PlaceLocation.Create(0,0,0,0);

            // Act
            var action = () =>
            {
                var place = Place.Create(PlaceType.Restaurant, name, address, location);
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
            var address = "서울시 황금동";
            var locationToChange = PlaceLocation.Create(1, 1, 0, 0);
            place.SetAddressLocation(address, locationToChange);

            // Assert
            place.Address.Should().Be(address);
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
        public void Default_OpeningTimes_Have_7()
        {
            // Arrange

            // Act
            var place = NewPlace();

            // Assert
            place.OpeningTimes.Should().HaveCount(7);
        }

        [Fact]
        public void OpeningTime_Is_Updated()
        {
            // Arrange
            var place = NewPlace();
            var day = DayOfWeek.Friday;
            var oldOpenTime = TimeSpan.FromHours(6);
            var oldCloseTime = TimeSpan.FromHours(18);
            place.SetOpenCloseTime(day, oldOpenTime, oldCloseTime);

            // Act
            var newOpenTime = TimeSpan.FromHours(3);
            var newCloseTime = TimeSpan.FromHours(10);
            place.SetOpenCloseTime(day, newOpenTime, newCloseTime);

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
