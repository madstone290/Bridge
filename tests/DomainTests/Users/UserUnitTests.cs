using Bridge.Domain.Users.Entities;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bridge.DomainTests.Users
{
    public class UserUnitTests
    {
        [Fact]
        public void NormalUser_Is_Created()
        {
            // Arrange
            var name = Guid.NewGuid().ToString();

            // Act
            var user = User.Create(string.Empty, name);

            // Assert
            user.Name.Should().Be(name);
            user.IsAdmin.Should().Be(false);
        }

    }
}
