using Bridge.Infrastructure.Services;
using Bridge.UnitTests.InfrastructureTests.Mockups;
using FluentAssertions;

namespace Bridge.UnitTests.InfrastructureTests
{
    public class FileUploadServiceTests : IClassFixture<WebHostEnvironmentMockup>
    {
        private readonly WebHostEnvironmentMockup _environmentMockup = new WebHostEnvironmentMockup()
        {
            ContentRootPath = "C:\\_Home\\Sources\\VisualStudio\\managed\\Bridge\\tests\\UnitTests\\TestData"
        };

        [Theory]
        [InlineData("C:\\_Home\\Sources\\VisualStudio\\managed\\Bridge\\tests\\UnitTests\\TestData\\test.jpg", "TempFiles", "test-new.jpg")]
        [InlineData("C:\\_Home\\Sources\\VisualStudio\\managed\\Bridge\\tests\\UnitTests\\TestData\\tous1.png", "TempFiles", "tous1-new.png")]
        [InlineData("C:\\_Home\\Sources\\VisualStudio\\managed\\Bridge\\tests\\UnitTests\\TestData\\tous2.png", "TempFiles", "tous2-new.png")]
        public void Upload(string filePath, string directory, string fileName)
        {
            // Arrange
           
            var uploadService = new InternalFileUploadService(_environmentMockup);
            var fileData = File.ReadAllBytes(filePath);

            // Act
            var newFilePath = uploadService.UploadFile(directory, fileName, fileData);

            // Assert
            newFilePath.Should().NotBeNull();
            File.Exists(Path.Combine(_environmentMockup.ContentRootPath, newFilePath!)).Should().BeTrue();
        }
    }
}
