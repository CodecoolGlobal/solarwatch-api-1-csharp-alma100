using SolarWatch.Context;
using SolarWatch.Model;

namespace SolarWatch.Repository;

public class CityRepository : ICityRepository
{
    public IEnumerable<City> GetAll()
    {
        using var dbContext = new SolarApiContext();
        return dbContext.Cities.ToList();
    }

    public City? GetCityByName(string name)
    {
        using var dbContext = new SolarApiContext();
        return dbContext.Cities.FirstOrDefault(c => c.Name == name);
    }

    public void AddCity(City city)
    {
        using var dbContext = new SolarApiContext();
        dbContext.Add(city);
        dbContext.SaveChanges();
    }

    public void DeletCity(City city)
    {
        using var dbContext = new SolarApiContext();
        dbContext.Remove(city);
        dbContext.SaveChanges();
    }

    public void UpdateCity(City city)
    {
        using var dbContext = new SolarApiContext();
        dbContext.Update(city);
        dbContext.SaveChanges();
    }
}