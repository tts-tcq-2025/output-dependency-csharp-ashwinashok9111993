using Xunit;
using TemperatureSpace;

namespace TemperatureSpace.Tests
{
    public class WeatherReporterTests
    {
        [Fact]
        public void Report_ShouldReturnSunnyDay_ForLowTemperature()
        {
            // Arrange
            var reporter = new WeatherReporter();
            var sensor = new SensorStub(temperature: 20, precipitation: 10);
            
            // Act
            var result = reporter.Report(sensor);
            
            // Assert
            Assert.Equal("Sunny Day", result);
        }

        [Fact]
        public void Report_ShouldReturnPartlyCloudy_ForHighTempAndModeratePrecipitation()
        {
            // Arrange
            var reporter = new WeatherReporter();
            var sensor = new SensorStub(temperature: 30, precipitation: 40, windSpeed: 25);
            
            // Act
            var result = reporter.Report(sensor);
            
            // Assert
            Assert.Equal("Partly Cloudy", result);
        }

        [Fact]
        public void Report_ShouldReturnStormy_ForHighTempPrecipitationAndWind()
        {
            // Arrange
            var reporter = new WeatherReporter();
            var sensor = new SensorStub(temperature: 30, precipitation: 80, windSpeed: 60);
            
            // Act
            var result = reporter.Report(sensor);
            
            // Assert
            Assert.Equal("Alert, Stormy with heavy rain", result);
        }

        [Fact]
        public void Report_ShouldFail_ForHighPrecipitationLowWind()
        {
            // This test exposes the bug in the weather logic
            // When temperature > 25, precipitation >= 60, but wind speed <= 50
            // The logic doesn't handle this case and returns "Sunny Day" instead of predicting rain
            
            // Arrange
            var reporter = new WeatherReporter();
            var sensor = new SensorStub(temperature: 30, precipitation: 70, windSpeed: 30);
            
            // Act
            var result = reporter.Report(sensor);
            
            // Assert - This will fail, exposing the bug
            Assert.Contains("rain", result.ToLower());
        }

        [Fact]
        public void Report_ShouldFail_BoundaryCase_Precipitation60()
        {
            // Test the boundary case where precipitation is exactly 60
            // Current logic: if (precipitation >= 20 && precipitation < 60) -> "Partly Cloudy"
            // else if (windSpeed > 50) -> "Stormy"
            // else -> "Sunny Day" (the bug)
            
            // Arrange
            var reporter = new WeatherReporter();
            var sensor = new SensorStub(temperature: 30, precipitation: 60, windSpeed: 30);
            
            // Act
            var result = reporter.Report(sensor);
            
            // Assert - This will fail, exposing the boundary bug
            Assert.NotEqual("Sunny Day", result);
        }
    }

    public class WeatherTests
    {
        [Fact]
        public void GetReport_ShouldUseDependencyInjection()
        {
            // Arrange
            var mockReporter = new MockWeatherReporter();
            var weather = new Weather(mockReporter);
            var sensor = new SensorStub();
            
            // Act
            var result = weather.GetReport(sensor);
            
            // Assert
            Assert.Equal("MOCK_REPORT", result);
            Assert.True(mockReporter.WasCalled);
        }
    }

    // Mock implementations for testing dependency injection
    public class MockWeatherReporter : IWeatherReporter
    {
        public bool WasCalled { get; private set; }
        
        public string Report(IWeatherSensor sensor)
        {
            WasCalled = true;
            return "MOCK_REPORT";
        }
    }

    public class TestWeatherSensor : IWeatherSensor
    {
        public double Temperature { get; set; }
        public int PrecipitationValue { get; set; }
        public int HumidityValue { get; set; }
        public int WindSpeed { get; set; }

        public double TemperatureInC() => Temperature;
        public int Precipitation() => PrecipitationValue;
        public int Humidity() => HumidityValue;
        public int WindSpeedKMPH() => WindSpeed;
    }
}
