namespace SolarWatch.Service;

public interface IGeocodeProvider
{
    Task<string> GetGeoCode(string city);
}