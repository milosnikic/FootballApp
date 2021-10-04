using FootballApp.API.Helpers;
using Xunit;

namespace FootballAppTests.ExtensionsTests
{
    public class ImageValidatorTests
    {
        [Fact]
        public void ImageSizeValidation_ShouldReturnTrue()
        {
            // Arrange
            var imageSize = 1;
            // Act
            var result = ImageValidator.ImageSizeValidation(imageSize);
            
            // Assert
            Assert.True(result);
        }
        
        [Fact]
        public void ImageExtensionValidation_ShouldReturnTrue()
        {
            // Arrange
            var imageExtension = ".png";
            // Act
            var result = ImageValidator.ImageExtensionValidation(imageExtension);
            
            // Assert
            Assert.True(result);
        }
        
    }
}