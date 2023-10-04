using Microsoft.AspNetCore.Mvc;
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

    public WeatherForecastController(ILogger<WeatherForecastController> logger, IGeocodeProvider geocodeProvider, IJsonProcessor jsonProcessor, ISunsetDataProvider sunsetDataProvider)
    {
        _logger = logger;
        _geocodeProvider = geocodeProvider;
        _jsonProcessor = jsonProcessor;
        _sunsetDataProvider = sunsetDataProvider;
    }

    [HttpGet("GetCityCode:{city}")]
    public async Task<ActionResult> Get(string city)
    {
        try
        {
            var geoCode = await _geocodeProvider.GetGeoCode(city);
            return Ok(_jsonProcessor.Process(geoCode));
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting weather data");
            return NotFound("City not found");
        }
        
    }
    
    [HttpGet("GetCurrent")]
    public async Task<ActionResult> GetCurrent(double lat, double lon)
    {
        try
        {
            var weatherData = await _sunsetDataProvider.Get(lat, lon);
            return Ok(_jsonProcessor.ProcessWeather(weatherData));
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting weather data");
            return NotFound("Error getting weather data");
        }
    }

    [HttpGet("test")]
    public IActionResult Test()
    {
        return Ok("igen");
    }
}