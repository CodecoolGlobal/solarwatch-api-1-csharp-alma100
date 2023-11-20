namespace SolarWatch.Service;

public interface ISunsetDataProvider
{
    Task<string> Get(double lat, double lon);

    Task<List<string>> GetMoreDay(double lat, double lon, int cycle);
}