using System.Text.Json;
using SolarWatch.Model;

namespace SolarWatch.Service;

public class JsonProcessor : IJsonProcessor
{
    public GeoLocation Process(string data)
    {
        JsonDocument json = JsonDocument.Parse(data);
        
        JsonElement lat = json.RootElement[0].GetProperty("lat");
        JsonElement lon = json.RootElement[0].GetProperty("lon");
        
        GeoLocation geoLocation = new GeoLocation
        {
            Lat = lat.GetDouble(),
            Lon = lon.GetDouble(),
        };

        return geoLocation;
    }

    public WeatherForecast ProcessWeather(string data)
    {
        JsonDocument json = JsonDocument.Parse(data);
        JsonElement main = json.RootElement.GetProperty("main");
        JsonElement weather = json.RootElement.GetProperty("weather")[0];
        
        WeatherForecast forecast = new WeatherForecast
        {
            Date = GetDateTimeFromUnixTimeStamp(json.RootElement.GetProperty("dt").GetInt64()),
            TemperatureC = (int)main.GetProperty("temp").GetDouble(),
            Summary = weather.GetProperty("description").GetString()
        };

        return forecast;
    }
    
    private static DateOnly GetDateTimeFromUnixTimeStamp(long timeStamp)
    {
        DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(timeStamp);
        DateTime dateTime = dateTimeOffset.UtcDateTime;

        return DateOnly.FromDateTime(dateTime);
    }
}