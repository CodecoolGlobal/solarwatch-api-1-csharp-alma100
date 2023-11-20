using SolarWatch.Model;

namespace SolarWatch.Service;

public interface IJsonProcessor
{
    City Process(string data);

    SunriseSunset ProcessWeather(string data);

    List<SunriseSunset> ProcessWeather(List<string> datas, double lat, double lon, int cycle);
}