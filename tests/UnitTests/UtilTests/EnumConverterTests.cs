using Bridge.Shared;
using Bridge.Shared.Converters;
using FluentAssertions;
using System.ComponentModel.DataAnnotations;

namespace Bridge.UnitTests.UtilTests
{
    public class EnumConvertTests
    {
        public enum Color 
        {
            [Display(Name ="적")]
            Red = 0,
            [Display(Name = "녹")]
            Green = 1,
            [Display(Name = "청")]
            Blue = 2,
        }

        public enum Size
        {
            [Display(Name = "소")]
            Small = 0,
            [Display(Name = "중")]
            Medium = 1,
            [Display(Name = "대")]
            Big = 2
        }

        [Theory]
        [InlineData("Bridge.UnitTests.UtilTests.EnumConvertTests+Color")]
        [InlineData("Bridge.UnitTests.UtilTests.EnumConvertTests+Size")]
        public void Conversion_Null_Input_Should_Fail(string typeName)
        {
            var type1 = typeof(Size);
            // Arrange
            Type type = Type.GetType(typeName)!;

            // Act
            var canConverter = new EnumConverter().TryConvert(type, null, out object output);

            // Assert
            canConverter.Should().BeFalse();
        }

        [Theory]
        [InlineData("Bridge.UnitTests.UtilTests.EnumConvertTests+Color", "")]
        [InlineData("Bridge.UnitTests.UtilTests.EnumConvertTests+Color", "_")]
        [InlineData("Bridge.UnitTests.UtilTests.EnumConvertTests+Color", -1)]
        [InlineData("Bridge.UnitTests.UtilTests.EnumConvertTests+Color", 10)]
        [InlineData("Bridge.UnitTests.UtilTests.EnumConvertTests+Size", "")]
        [InlineData("Bridge.UnitTests.UtilTests.EnumConvertTests+Size", "_")]
        [InlineData("Bridge.UnitTests.UtilTests.EnumConvertTests+Size", -1)]
        [InlineData("Bridge.UnitTests.UtilTests.EnumConvertTests+Size", 10)]
        public void Conversion_With_Invalid_Input_Shoud_Fail(string typeName, object input)
        {
            // Arrange
            Type type = Type.GetType(typeName)!;

            // Act
            var canConverter = new EnumConverter().TryConvert(type, input, out object output);

            // Assert
            canConverter.Should().BeFalse();
        }

        [Theory]
        [InlineData("Bridge.UnitTests.UtilTests.EnumConvertTests+Color", 0, Color.Red)]
        [InlineData("Bridge.UnitTests.UtilTests.EnumConvertTests+Color", 1, Color.Green)]
        [InlineData("Bridge.UnitTests.UtilTests.EnumConvertTests+Color", 2, Color.Blue)]
        [InlineData("Bridge.UnitTests.UtilTests.EnumConvertTests+Size", 0, Size.Small)]
        [InlineData("Bridge.UnitTests.UtilTests.EnumConvertTests+Size", 1, Size.Medium)]
        [InlineData("Bridge.UnitTests.UtilTests.EnumConvertTests+Size", 2, Size.Big)]
        public void Conversion_Integer_Should_Succed(string typeName, object input, object output)
        {
            // Arrange
            Type type = Type.GetType(typeName)!;

            // Act
            var canConvert = new EnumConverter().TryConvert(type, input, out object enumValue);

            // Assert
            canConvert.Should().BeTrue();
            output.Should().Be(enumValue);
        }
        [Theory]
        [InlineData("Bridge.UnitTests.UtilTests.EnumConvertTests+Color", "Red", Color.Red)]
        [InlineData("Bridge.UnitTests.UtilTests.EnumConvertTests+Color", "Green", Color.Green)]
        [InlineData("Bridge.UnitTests.UtilTests.EnumConvertTests+Color", "Blue", Color.Blue)]
        [InlineData("Bridge.UnitTests.UtilTests.EnumConvertTests+Size", "Small", Size.Small)]
        [InlineData("Bridge.UnitTests.UtilTests.EnumConvertTests+Size", "Medium", Size.Medium)]
        [InlineData("Bridge.UnitTests.UtilTests.EnumConvertTests+Size", "Big", Size.Big)]
        public void Conversion_With_String_Should_Succced(string typeName, object input, object output)
        {
            // Arrange
            Type type = Type.GetType(typeName)!;

            // Act
            var canConvert = new EnumConverter().TryConvert(type, input, out object enumValue);

            // Assert
            canConvert.Should().BeTrue();
            output.Should().Be(enumValue);
        }
        [Theory]
        [InlineData("Bridge.UnitTests.UtilTests.EnumConvertTests+Color", "적", Color.Red)]
        [InlineData("Bridge.UnitTests.UtilTests.EnumConvertTests+Color", "녹", Color.Green)]
        [InlineData("Bridge.UnitTests.UtilTests.EnumConvertTests+Color", "청", Color.Blue)]
        [InlineData("Bridge.UnitTests.UtilTests.EnumConvertTests+Size", "소", Size.Small)]
        [InlineData("Bridge.UnitTests.UtilTests.EnumConvertTests+Size", "중", Size.Medium)]
        [InlineData("Bridge.UnitTests.UtilTests.EnumConvertTests+Size", "대", Size.Big)]
        public void Conversion_With_DisplayName_Should_Succeed(string typeName, object input, object output)
        {
            // Arrange
            Type type = Type.GetType(typeName)!;

            // Act
            var canConvert = new EnumConverter().TryConvert(type, input, out object enumValue);

            // Assert
            canConvert.Should().BeTrue();
            output.Should().Be(enumValue);
        }
    }
}
