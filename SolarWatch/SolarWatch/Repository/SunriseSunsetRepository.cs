using SolarWatch.Context;
using SolarWatch.Model;

namespace SolarWatch.Repository;

public class SunriseSunsetRepository : ISunriseSunsetRepository
{
    public IEnumerable<SunriseSunset> GetAll()
    {
        using var dbContext = new SolarApiContext();
        return dbContext.SunriseSunsets.ToList();
    }

    public SunriseSunset? GetSuntime(double lon, double lat)
    {
        using var dbContext = new SolarApiContext();
        //var sunRiseSet = dbContext.SunriseSunsets.FirstOrDefault(s => s.Lat.Equals(lat) && s.Long.Equals(lon));
        var tolerance = 0.0001; // tolerancia az összehasonlításhoz

        var sunRiseSet = dbContext.SunriseSunsets.FirstOrDefault(s => 
            Math.Abs(s.Lat - lat) < tolerance && Math.Abs(s.Long - lon) < tolerance);
        return sunRiseSet;
    }

    public void AddSuntime(SunriseSunset sunData)
    {
        using var dbCotext = new SolarApiContext();
        dbCotext.SunriseSunsets.Add(sunData);
        dbCotext.SaveChanges();
    }

    public void DeletSuntime(SunriseSunset sunData)
    {
        using var dbCotext = new SolarApiContext();
        dbCotext.SunriseSunsets.Remove(sunData);
        dbCotext.SaveChanges();
    }

    public void UpdateSuntime(SunriseSunset sunData)
    {
        using var dbCotext = new SolarApiContext();
        dbCotext.SunriseSunsets.Update(sunData);
        dbCotext.SaveChanges();
    }
    
}