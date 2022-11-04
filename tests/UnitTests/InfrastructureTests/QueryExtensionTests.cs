using Bridge.Shared.Extensions;
using FluentAssertions;
using Microsoft.AspNetCore.Components;
using System.Text.RegularExpressions;

namespace Bridge.UnitTests.InfrastructureTests
{
    public class QueryExtensionTests
    {


        [Theory]
        [InlineData("https://naver.com", "id", "34")]
        [InlineData("https://naver.com?id=10", "name", "john")]
        [InlineData("https://naver.com?id=20&name=john", "age", "20")]
        public void Add_New_QueryParameter(string uri, string paramName, string paramValue)
        {
            // Arrange

            // Act
            var uriWithQuery = uri.AddQueryParam(paramName, paramValue);

            // Assert
            var regex = new Regex($"{paramName}={paramValue}(&|$)");
            regex.IsMatch(uriWithQuery).Should().BeTrue();
        }

        [Theory]
        [InlineData("https://naver.com?id=34", "id", "34", "55")]
        [InlineData("https://naver.com?id=34&name=john", "name", "john", "bruce")]
        [InlineData("https://naver.com?id=34&age=20&name=bruce", "age", "20", "30")]
        public void Override_Existing_QueryParameter(string uri, string paramName, string oldValue, string newValue)
        {
            // Arrange

            // Act
            var uriWithQuery = uri.AddQueryParam(paramName, newValue);

            // Assert
            var regex = new Regex($"{paramName}={newValue}(&|$)");
            regex.IsMatch(uriWithQuery).Should().BeTrue();
        }

    }
}

