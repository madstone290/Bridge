using Bridge.Infrastructure.Services;
using Bridge.UnitTests.InfrastructureTests.Mockups;
using FluentAssertions;
using Microsoft.Extensions.Options;

namespace Bridge.UnitTests.InfrastructureTests
{
    public class FileUploadServiceTests : IClassFixture<WebHostEnvironmentMockup>
    {
        [Theory]
        [InlineData("C:\\_Home\\Sources\\VisualStudio\\managed\\Bridge\\tests\\UnitTests\\TestData\\test.jpg",  "PlaceImages" ,"test-new.jpg")]
        [InlineData("C:\\_Home\\Sources\\VisualStudio\\managed\\Bridge\\tests\\UnitTests\\TestData\\tous1.png", "PlaceImages", "tous1-new.png")]
        [InlineData("C:\\_Home\\Sources\\VisualStudio\\managed\\Bridge\\tests\\UnitTests\\TestData\\tous2.png", "PlaceImages", "tous2-new.png")]
        public void Upload(string sourceFilePath, string category, string fileName)
        {
            // Arrange
            var uploadDirectory = "C:\\_Home\\Sources\\VisualStudio\\managed\\Bridge\\tests\\UnitTests\\TestData\\UploadedFiles";
            var configOptions = Options.Create<InternalFileUploadService.Config>(new InternalFileUploadService.Config()
            {
                UploadDirectory = uploadDirectory
            });
            
            var uploadService = new InternalFileUploadService(configOptions);
            var fileData = File.ReadAllBytes(sourceFilePath);

            // Act
            var newFilePath = uploadService.UploadFile(category, fileName, fileData);

            // Assert
            newFilePath.Should().NotBeNull();
            File.Exists(Path.Combine(uploadDirectory, newFilePath!)).Should().BeTrue();
        }
    }
}
