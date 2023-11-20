using System.Globalization;
using System.Text.Json;
using SolarWatch.Model;

namespace SolarWatch.Service;

public class JsonProcessor : IJsonProcessor
{
    public City Process(string data)
    {
        JsonDocument json = JsonDocument.Parse(data);

        Console.WriteLine(json.RootElement[0]);
        JsonElement lat = json.RootElement[0].GetProperty("lat");
        JsonElement lon = json.RootElement[0].GetProperty("lon");
        
        City city = new City()
        {
            Name = json.RootElement[0].GetProperty("name").ToString(),
            Lat = lat.GetDouble(),
            Long = lon.GetDouble(),
            State = json.RootElement[0].TryGetProperty("state", out var stateValue) ? stateValue.ToString() : "no State av.", //megnézi hogy van-e
            Country = json.RootElement[0].GetProperty("country").ToString()
        };

        return city;
    }

    public SunriseSunset ProcessWeather(string data)
    {
        JsonDocument json = JsonDocument.Parse(data);
        JsonElement coord = json.RootElement.GetProperty("coord");
        JsonElement sys = json.RootElement.GetProperty("sys");

        Console.WriteLine(GetDateTimeFromUnixTimeStamp(json.RootElement.GetProperty("sys").GetProperty("sunrise").GetInt64()));
        SunriseSunset forecast = new SunriseSunset()
        {
            Long = coord.GetProperty("lon").GetDouble(),
            Lat = coord.GetProperty("lat").GetDouble(),
            Sunrise = GetDateTimeFromUnixTimeStamp(sys.GetProperty("sunrise").GetInt64()),
            Sunset = GetDateTimeFromUnixTimeStamp(sys.GetProperty("sunset").GetInt64())
        };

        return forecast;
    }
    
    public List<SunriseSunset> ProcessWeather(List<string> datas, double lat, double lon, int cycle)
    {

        var res = new List<SunriseSunset>();
        var currentdate = DateTime.Now;
        var counter = 0;
        foreach (var data in datas)
        {
            counter++;
            JsonDocument json = JsonDocument.Parse(data);
            JsonElement results = json.RootElement.GetProperty("results");
            
            SunriseSunset forecast = new SunriseSunset()
            {
                Id = counter,
                Long = lon,
                Lat = lat,
                Sunrise = DateTime.ParseExact(results.GetProperty("sunrise").ToString(), "h:mm:ss tt", CultureInfo.InvariantCulture),
                Sunset = DateTime.ParseExact(results.GetProperty("sunset").ToString(), "h:mm:ss tt", CultureInfo.InvariantCulture)
            };
            
            forecast.Sunrise = new DateTime(currentdate.Year, currentdate.Month, currentdate.Day, forecast.Sunrise.Hour, forecast.Sunrise.Minute, forecast.Sunrise.Second);
            forecast.Sunset = new DateTime(currentdate.Year, currentdate.Month, currentdate.Day, forecast.Sunset.Hour, forecast.Sunset.Minute, forecast.Sunset.Second);
            
            currentdate = currentdate.AddDays(1);
            
            res.Add(forecast);
        }
        
        return res;
    }
    
    private static DateTime GetDateTimeFromUnixTimeStamp(long timeStamp)
    {
        DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(timeStamp);
        DateTime dateTime = dateTimeOffset.UtcDateTime;
        return dateTime;
        //return DateOnly.FromDateTime(dateTime);
    }
}