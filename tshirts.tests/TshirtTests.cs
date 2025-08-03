using Xunit;
using TshirtSpace;

namespace TshirtSpace.Tests
{
    public class TshirtSizeClassifierTests
    {
        [Fact]
        public void Size_ShouldReturnS_ForMeasurementBelow38()
        {
            // Arrange
            var classifier = new TshirtSizeClassifier();
            
            // Act
            var result = classifier.Size(37);
            
            // Assert
            Assert.Equal("S", result);
        }

        [Fact]
        public void Size_ShouldReturnM_ForMeasurementBetween38And42()
        {
            // Arrange
            var classifier = new TshirtSizeClassifier();
            
            // Act
            var result = classifier.Size(40);
            
            // Assert
            Assert.Equal("M", result);
        }

        [Fact]
        public void Size_ShouldReturnL_ForMeasurementAbove42()
        {
            // Arrange
            var classifier = new TshirtSizeClassifier();
            
            // Act
            var result = classifier.Size(43);
            
            // Assert
            Assert.Equal("L", result);
        }

        [Fact]
        public void Size_ShouldFail_ForBoundaryValue38()
        {
            // This test exposes the bug - measurement 38 falls through cracks
            // Current logic: if(cms < 38) return "S"; else if(cms > 38 && cms < 42) return "M";
            // cms == 38 doesn't match first condition (not < 38) and doesn't match second (not > 38)
            // So it returns "L" but should probably be "M"
            
            // Arrange
            var classifier = new TshirtSizeClassifier();
            
            // Act
            var result = classifier.Size(38);
            
            // Assert - This will fail, exposing the bug
            Assert.Equal("M", result);
        }

        [Fact]
        public void Size_ShouldFail_ForBoundaryValue42()
        {
            // This test exposes the bug - measurement 42 falls through cracks
            // Current logic: else if(cms > 38 && cms < 42) return "M"; else return "L";
            // cms == 42 doesn't match condition (not < 42) so it returns "L"
            // But should it be "M" or "L"? The boundary is unclear.
            
            // Arrange
            var classifier = new TshirtSizeClassifier();
            
            // Act
            var result = classifier.Size(42);
            
            // Assert - This will fail, exposing the boundary issue
            Assert.Equal("M", result);
        }
    }

    public class TshirtTests
    {
        [Fact]
        public void GetSize_ShouldUseInjectedClassifier()
        {
            // Arrange
            var mockClassifier = new MockTshirtSizeClassifier();
            var tshirt = new Tshirt(mockClassifier);
            
            // Act
            var result = tshirt.GetSize(40);
            
            // Assert
            Assert.Equal("MOCK_SIZE", result);
            Assert.True(mockClassifier.WasCalled);
        }
    }

    // Mock implementation for testing dependency injection
    public class MockTshirtSizeClassifier : ITshirtSizeClassifier
    {
        public bool WasCalled { get; private set; }
        
        public string Size(int cms)
        {
            WasCalled = true;
            return "MOCK_SIZE";
        }
    }
}
