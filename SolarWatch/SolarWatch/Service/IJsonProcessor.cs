using SolarWatch.Model;

namespace SolarWatch.Service;

public interface IJsonProcessor
{
    City Process(string data);

    SunriseSunset ProcessWeather(string data);
}