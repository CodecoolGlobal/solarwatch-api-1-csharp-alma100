using System.Net;

namespace SolarWatch.Service;

public class GeocodeProvider : IGeocodeProvider
{
    private readonly ILogger<GeocodeProvider> _logger;
    
    public GeocodeProvider(ILogger<GeocodeProvider> logger)
    {
        _logger = logger;
    }
    
    
    public async Task<string> GetGeoCode(string city)
    {
        var apiKey = "ec361c67c4e9b30722db84d2ea6ccc76";

        var url = $"http://api.openweathermap.org/geo/1.0/direct?q={city}&appid={apiKey}";
        //var url = $"http://api.openweathermap.org/geo/1.0/direct?q=London&limit=5&appid={apiKey}";
        var client = new HttpClient();
        
        _logger.LogInformation("Calling OpenWeather API with url: {url}", url);
       
        var res = await client.GetAsync(url);
        return await res.Content.ReadAsStringAsync();
        
    }
}