namespace SolarWatch.Service;

public interface ISunsetDataProvider
{
    Task<string> Get(double lat, double lon);
}