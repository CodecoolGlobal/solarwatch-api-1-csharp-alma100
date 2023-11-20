using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolarWatch.Repository;
using SolarWatch.Service;

namespace SolarWatch.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
   

    private readonly ILogger<WeatherForecastController> _logger;

    private readonly IGeocodeProvider _geocodeProvider;

    private readonly ISunsetDataProvider _sunsetDataProvider;

    private readonly IJsonProcessor _jsonProcessor;

    private readonly ICityRepository _cityRepository;

    private readonly ISunriseSunsetRepository _sunriseSunsetRepository;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, IGeocodeProvider geocodeProvider, IJsonProcessor jsonProcessor, ISunsetDataProvider sunsetDataProvider, ICityRepository cityRepository, ISunriseSunsetRepository sunriseSunsetRepository)
    {
        _logger = logger;
        _geocodeProvider = geocodeProvider;
        _jsonProcessor = jsonProcessor;
        _sunsetDataProvider = sunsetDataProvider;
        _cityRepository = cityRepository;
        _sunriseSunsetRepository = sunriseSunsetRepository;
    }

    [HttpGet("GetCityCode"), Authorize(Roles = "user, admin")]
    public async Task<ActionResult> Get(string city)
    {
        try
        {
            var res = _cityRepository.GetCityByName(city);
            if (res == null)
            {
                var geoCode = await _geocodeProvider.GetGeoCode(city);
                var newCity = _jsonProcessor.Process(geoCode);
                _cityRepository.AddCity(newCity);
                return Ok(newCity);
            }

            return Ok(res);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting weather data");
            return NotFound("City not found");
        }
        
    }
    
    [HttpGet("GetCurrent"), Authorize(Roles="admin")]
    public async Task<ActionResult> GetCurrent(double lat, double lon)
    {
        try
        {
            var res = _sunriseSunsetRepository.GetSuntime(lon, lat);
            Console.WriteLine(res);
            if (res == null)
            {
                var weatherData = await _sunsetDataProvider.Get(lat, lon);
                var newSunData = _jsonProcessor.ProcessWeather(weatherData);
                _sunriseSunsetRepository.AddSuntime(newSunData);
                return Ok(newSunData);
            }

            return Ok(res);

        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting weather data");
            return NotFound("Error getting weather data");
        }
    }

    [HttpGet("GetMoreDay")]
    public async Task<ActionResult> GetMoreDay(double lat, double lon, int cycle)
    {
        try
        {
            var weatherData = await _sunsetDataProvider.GetMoreDay(lat, lon, cycle);
            var newSunDatas = _jsonProcessor.ProcessWeather(weatherData, lat, lon, cycle);
            return Ok(newSunDatas);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    [HttpGet("test")]
    public IActionResult Test()
    {
        return Ok("igen");
    }
}