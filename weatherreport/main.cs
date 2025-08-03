using System;

namespace TemperatureSpace
{
    public interface IWeatherReporter
    {
        string Report(IWeatherSensor sensor);
    }

    public class WeatherReporter : IWeatherReporter
    {
        public string Report(IWeatherSensor sensor)
        {
            int precipitation = sensor.Precipitation();
            // precipitation < 20 is a sunny day
            string report = "Sunny Day";

            if (sensor.TemperatureInC() > 25)
            {
                if (precipitation >= 20 && precipitation < 60)
                    report = "Partly Cloudy";
                else if (sensor.WindSpeedKMPH() > 50)
                    report = "Alert, Stormy with heavy rain";
                // Bug: Missing condition for high precipitation (>= 60) with low wind speed
                // This should predict rain but currently returns "Sunny Day"
            }
            return report;
        }
    }

    public class Weather
    {
        private readonly IWeatherReporter _reporter;

        public Weather(IWeatherReporter reporter)
        {
            _reporter = reporter;
        }

        public string GetReport(IWeatherSensor sensor)
        {
            return _reporter.Report(sensor);
        }

        // Legacy method for backward compatibility
        internal static string Report(IWeatherSensor sensor)
        {
            var reporter = new WeatherReporter();
            return reporter.Report(sensor);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var reporter = new WeatherReporter();
            var weather = new Weather(reporter);
            
            // Production usage examples
            var sunnySensor = new SensorStub(temperature: 20, precipitation: 10);
            var cloudySensor = new SensorStub(temperature: 30, precipitation: 40);
            var stormySensor = new SensorStub(temperature: 30, precipitation: 80, windSpeed: 60);
            
            Console.WriteLine($"Cool weather report: {weather.GetReport(sunnySensor)}");
            Console.WriteLine($"Warm weather report: {weather.GetReport(cloudySensor)}");
            Console.WriteLine($"Stormy weather report: {weather.GetReport(stormySensor)}");
            
            Console.WriteLine("Weather application running. Run the tests to check for bugs.");
        }
    }
}

