using Bridge.Shared;
using FluentAssertions;

namespace Bridge.UnitTests.UtilTests
{
    public class ObjectConvertTests
    {

        [Theory]
        [InlineData("System.Int32", 50)]
        [InlineData("System.Int64", 100)]
        [InlineData("System.Boolean", true)]
        [InlineData("Bridge.UnitTests.UtilTests.ObjectConverterTestsEnum", ObjectConverterTestsEnum.Two)]
        public void Conversion_With_CustomConverter(string typeName, object output)
        {
            // Arrange
            var converter = new ObjectConverter();
            var type = Type.GetType(typeName)!;
            converter.CustomConverters[type] = (input) => output;

            // Act
            var convertedValue = converter.Execute(type, "dontcare");
            
            // Assert
            convertedValue.Should().Be(output);
        }

        [Theory]
        [InlineData("System.Int32")]
        [InlineData("System.Int64")]
        [InlineData("System.Boolean")]
        [InlineData("Bridge.UnitTests.UtilTests.ObjectConverterTestsEnum")]
        public void Null_To_Default_When_Type_Is_ValueType(string typeName)
        {
            // Arrange
            Type type = Type.GetType(typeName)!;

            // Act
            var convertedValue = new ObjectConverter().Execute(type, null);

            // Assert
            convertedValue.Should().Be(Activator.CreateInstance(type));
        }

        [Theory]
        [InlineData("System.Object")]
        [InlineData("System.Type")]
        [InlineData("System.Nullable`1[System.Int32]")]
        [InlineData("System.Nullable`1[System.Boolean]")]
        public void Null_To_Null_When_Type_Is_Not_ValueType(string typeName)
        {
            // Arrange
            Type type = Type.GetType(typeName)!;

            // Act
            var convertedValue = new ObjectConverter().Execute(type, null);

            // Assert
            convertedValue.Should().BeNull();
        }

        [Theory]
        [InlineData("System.Int32", "_")]
        [InlineData("System.Int64", "_")]
        [InlineData("System.Boolean", "_")]
        [InlineData("Bridge.UnitTests.UtilTests.ObjectConverterTestsEnum", "_")]
        public void Invalid_Input_To_Default_When_Type_Is_ValueType(string typeName, object input)
        {
            // Arrange
            Type type = Type.GetType(typeName)!;

            // Act
            var convertedValue = new ObjectConverter().Execute(type, input);

            // Assert
            convertedValue.Should().Be(Activator.CreateInstance(type));
        }

        [Theory]
        [InlineData("System.Type", "_")]
        [InlineData("System.Nullable`1[System.Int32]", "_")]
        [InlineData("System.Nullable`1[System.Boolean]", "_")]
        [InlineData("System.Nullable`1[Bridge.UnitTests.UtilTests.ObjectConverterTestsEnum]", "_")]
        public void Invalid_Input_To_Null_When_Type_Is_Not_ValueType(string typeName, object input)
        {
            // Arrange
            Type type = Type.GetType(typeName)!;

            // Act
            var convertedValue = new ObjectConverter().Execute(type, input);

            // Assert
            convertedValue.Should().BeNull();
        }

        [Theory]
        [InlineData("System.Int32", "10", 10)]
        [InlineData("System.Int64", "-100", -100)]
        [InlineData("System.Double", "4.34", 4.34d)]
        [InlineData("System.Boolean", "true", true)]
        [InlineData("System.Boolean", "false", false)]
        [InlineData("Bridge.UnitTests.UtilTests.ObjectConverterTestsEnum", 0, ObjectConverterTestsEnum.Zero)]
        [InlineData("Bridge.UnitTests.UtilTests.ObjectConverterTestsEnum", "One", ObjectConverterTestsEnum.One)]
        [InlineData("Bridge.UnitTests.UtilTests.ObjectConverterTestsEnum", "Two", ObjectConverterTestsEnum.Two)]
        [InlineData("System.Nullable`1[Bridge.UnitTests.UtilTests.ObjectConverterTestsEnum]", 0, ObjectConverterTestsEnum.Zero)]
        [InlineData("System.Nullable`1[System.Boolean]", "true", true)]
        public void Valid_Input_To_Output(string typeName, object input, object output)
        {
            // Arrange
            Type type = Type.GetType(typeName)!;

            // Act
            var convertedValue = new ObjectConverter().Execute(type, input);

            // Assert
            convertedValue.Should().Be(output);
        }

        [Theory]
        [InlineData("Bridge.UnitTests.UtilTests.ObjectConverterTestsEnum", "영", ObjectConverterTestsEnum.Zero)]
        [InlineData("Bridge.UnitTests.UtilTests.ObjectConverterTestsEnum", "일", ObjectConverterTestsEnum.One)]
        [InlineData("Bridge.UnitTests.UtilTests.ObjectConverterTestsEnum", "이", ObjectConverterTestsEnum.Two)]
        public void Conversion_Enum_Using_DisplayAttribute(string typeName, object input, object output)
        {
            // Arrange
            Type type = Type.GetType(typeName)!;

            // Act
            var convertedValue = new ObjectConverter().Execute(type, input);

            // Assert
            convertedValue.Should().Be(output);
        }
    }
}
