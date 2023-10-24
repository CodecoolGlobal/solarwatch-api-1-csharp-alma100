using SolarWatch.Model;

namespace SolarWatch.Repository;

public interface ISunriseSunsetRepository
{
    IEnumerable<SunriseSunset> GetAll();

    SunriseSunset? GetSuntime(double lon, double lat);

    void AddSuntime(SunriseSunset city);
    
    void DeletSuntime(SunriseSunset city);
    
    void UpdateSuntime(SunriseSunset city);
}