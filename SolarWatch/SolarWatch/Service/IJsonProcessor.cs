using SolarWatch.Model;

namespace SolarWatch.Service;

public interface IJsonProcessor
{
    GeoLocation Process(string data);

    WeatherForecast ProcessWeather(string data);
}