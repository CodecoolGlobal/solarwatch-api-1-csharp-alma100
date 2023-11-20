using System.Net;

namespace SolarWatch.Service;

public class SunsetDataProvider : ISunsetDataProvider
{
    
    private readonly ILogger<SunsetDataProvider> _logger;
    
    public SunsetDataProvider(ILogger<SunsetDataProvider> logger)
    {
        _logger = logger;
    }
    
    public async Task<string> Get(double lat, double lon)
    {
        var apiKey = "ec361c67c4e9b30722db84d2ea6ccc76";
        
        var url = $"https://api.openweathermap.org/data/2.5/weather?lat={lat}&lon={lon}&appid={apiKey}&units=metric";

        var client = new HttpClient();

        _logger.LogInformation("Calling OpenWeather API with url: {url}", url);

        var res = await client.GetAsync(url);
        return await res.Content.ReadAsStringAsync();
    }
    
    public async Task<List<string>> GetMoreDay(double lat, double lon, int cycle)
    {
        
        var currentTime = DateTime.Now;

        var urls = new List<string>();
        for (int i = 0; i < cycle; i++)
        {
            var date = currentTime.AddDays(i);
            var url = $"https://api.sunrise-sunset.org/json?lat={lat}&lng={lon}&date={date.ToString("yyyy-MM-dd")}";
            urls.Add(url);
        }
        

        //var client = new HttpClient();
        var results = new List<string>();
        foreach (var url in urls)
        {
            Console.WriteLine(url);
            var client = new HttpClient();
            //_logger.LogInformation("Calling OpenWeather API with url: {url}", url);
            var res = await client.GetAsync(url);
            var content = await res.Content.ReadAsStringAsync();
            Console.WriteLine(content);
            results.Add(content);
        }

        return results;
    }
}