using SolarWatch.Model;

namespace SolarWatch.Repository;

public interface ICityRepository
{
    IEnumerable<City> GetAll();

    City? GetCityByName(string name);

    void AddCity(City city);
    
    void DeletCity(City city);
    
    void UpdateCity(City city);
}