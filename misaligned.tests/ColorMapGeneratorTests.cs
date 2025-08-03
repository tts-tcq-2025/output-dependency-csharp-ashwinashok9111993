using Xunit;
using MisalignedSpace;
using System.Collections.Generic;
using System.Linq;

namespace MisalignedSpace.Tests
{
    public class ColorMapGeneratorTests
    {
        private ColorMapGenerator CreateColorMapGenerator()
        {
            var formatter = new ColorMapFormatter();
            var writer = new TestOutputWriter();
            return new ColorMapGenerator(formatter, writer);
        }

        [Fact]
        public void GenerateColorMap_ShouldReturnCorrectNumberOfEntries()
        {
            // Arrange
            var generator = CreateColorMapGenerator();
            
            // Act
            var result = generator.GenerateColorMap();
            
            // Assert
            Assert.Equal(25, result.Count);
        }

        [Fact]
        public void GetTotalEntries_ShouldReturn25()
        {
            // Arrange
            var generator = CreateColorMapGenerator();
            
            // Act
            var result = generator.GetTotalEntries();
            
            // Assert
            Assert.Equal(25, result);
        }

        [Fact]
        public void GenerateColorMap_ShouldFail_ColorCombinationsIncorrect()
        {
            // This test exposes the bug in color mapping
            // Each major color should appear with each minor color (5x5 = 25 combinations)
            // But currently minorColors[i] is used instead of minorColors[j]
            
            // Arrange
            var generator = CreateColorMapGenerator();
            
            // Act
            var result = generator.GenerateColorMap();
            
            // Assert - This will fail, exposing the bug
            // White should appear with all 5 minor colors, but currently only appears with Blue
            var whiteEntries = result.Where(entry => entry.Contains("White")).ToList();
            Assert.Equal(5, whiteEntries.Count);
            
            // Check that White appears with different minor colors
            Assert.Contains("White | Blue", whiteEntries[0]);
            Assert.Contains("White | Orange", whiteEntries[1]); // This will fail - it will be "White | Blue" again
        }

        [Fact]
        public void GenerateColorMap_ShouldFail_MinorColorVariety()
        {
            // Another test to expose the bug - each minor color should appear 5 times
            
            // Arrange
            var generator = CreateColorMapGenerator();
            
            // Act
            var result = generator.GenerateColorMap();
            
            // Assert - This will fail, exposing the bug
            var orangeCount = result.Count(entry => entry.Contains("Orange"));
            Assert.Equal(5, orangeCount); // Should appear 5 times, but currently appears only 5 times with Red
        }

        [Fact]
        public void ColorMapFormatter_ShouldTestAlignmentIssues()
        {
            // Test for potential alignment issues in formatting
            
            // Arrange
            var formatter = new ColorMapFormatter();
            
            // Act
            var result1 = formatter.FormatColorEntry(0, "White", "Blue");
            var result24 = formatter.FormatColorEntry(24, "Violet", "Slate");
            
            // Assert
            Assert.Contains("0 | White | Blue", result1);
            Assert.Contains("24 | Violet | Slate", result24);
            
            // Check if numbers are properly aligned (this might reveal formatting issues)
            Assert.Equal(result1.Split('|').Length, result24.Split('|').Length);
        }

        [Fact]
        public void ColorMapGenerator_ShouldUseDependencyInjection()
        {
            // Arrange
            var mockFormatter = new MockColorMapFormatter();
            var mockWriter = new MockOutputWriter();
            var generator = new ColorMapGenerator(mockFormatter, mockWriter);
            
            // Act
            var result = generator.GenerateColorMap();
            
            // Assert
            Assert.Equal(25, result.Count);
            Assert.True(mockFormatter.WasCalled);
            Assert.True(mockWriter.WasCalled);
            Assert.Equal(25, mockWriter.WriteCount);
        }
    }

    // Test implementations
    public class TestOutputWriter : IOutputWriter
    {
        public List<string> WrittenLines { get; } = new List<string>();
        
        public void WriteLine(string text)
        {
            WrittenLines.Add(text);
        }
    }

    // Mock implementations for testing dependency injection
    public class MockColorMapFormatter : IColorMapFormatter
    {
        public bool WasCalled { get; private set; }
        
        public string FormatColorEntry(int number, string majorColor, string minorColor)
        {
            WasCalled = true;
            return $"MOCK: {number} | {majorColor} | {minorColor}";
        }
    }

    public class MockOutputWriter : IOutputWriter
    {
        public bool WasCalled { get; private set; }
        public int WriteCount { get; private set; }
        
        public void WriteLine(string text)
        {
            WasCalled = true;
            WriteCount++;
        }
    }
}
