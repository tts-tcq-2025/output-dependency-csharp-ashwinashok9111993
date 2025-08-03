
namespace TemperatureSpace
{    /// <summary>
     /// This is a stub for a weather sensor. For the sake of testing 
     /// we create a stub that generates weather data and allows us to
     /// test the other parts of this application in isolation
     /// without needing the actual Sensor during development
     /// </summary>
    public class SensorStub : IWeatherSensor
    {
        private readonly double _temperature;
        private readonly int _precipitation;
        private readonly int _humidity;
        private readonly int _windSpeed;

        public SensorStub(double temperature = 26, int precipitation = 70, int humidity = 72, int windSpeed = 52)
        {
            _temperature = temperature;
            _precipitation = precipitation;
            _humidity = humidity;
            _windSpeed = windSpeed;
        }

        public int Humidity()
        {
            return _humidity;
        }

        public int Precipitation()
        {
            return _precipitation;
        }

        public double TemperatureInC()
        {
            return _temperature;
        }

        public int WindSpeedKMPH()
        {
            return _windSpeed;
        }
    }

    // Specific stub configurations for different test scenarios
    public class HighPrecipitationLowWindStub : IWeatherSensor
    {
        public double TemperatureInC() => 30; // > 25
        public int Precipitation() => 70;     // > 60
        public int Humidity() => 80;
        public int WindSpeedKMPH() => 30;     // < 50
    }

    public class StormyWeatherStub : IWeatherSensor
    {
        public double TemperatureInC() => 28; // > 25
        public int Precipitation() => 80;     // > 60
        public int Humidity() => 90;
        public int WindSpeedKMPH() => 60;     // > 50
    }

    public class PartlyCloudyStub : IWeatherSensor
    {
        public double TemperatureInC() => 27; // > 25
        public int Precipitation() => 40;     // >= 20 && < 60
        public int Humidity() => 65;
        public int WindSpeedKMPH() => 25;     // < 50
    }
}
