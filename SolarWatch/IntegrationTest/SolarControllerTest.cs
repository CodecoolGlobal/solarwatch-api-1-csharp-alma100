using System.Net;

namespace IntegrationTest;

public class SolarControllerTest : IDisposable
{
    private WebFactory _factory;

    private HttpClient _client;

    public SolarControllerTest()
    {
        _factory = new WebFactory();
        _client = _factory.CreateClient();
        //_client.DefaultRequestHeaders.Add("Authorization", "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJUb2tlbkZvclRoZUFwaVdpdGhBdXRoIiwianRpIjoiNDZkNjFiZGUtZDhkNC00M2Y4LWI4NzMtOTU5ZjBiMzY0YTFkIiwiaWF0IjoiMTEvMDQvMjAyMyAxNjoyMjoyMCIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWVpZGVudGlmaWVyIjoiZmVhY2RlYzItMGNhMy00MDViLWJiNjktZDc5OTcwMDYxMTRjIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZSI6ImFkbWluIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvZW1haWxhZGRyZXNzIjoiYWRtaW5AYWRtaW4uY29tIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiYWRtaW4iLCJleHAiOjE2OTkxMTY3NDAsImlzcyI6ImFwaVdpdGhBdXRoQmFja2VuZCIsImF1ZCI6ImFwaVdpdGhBdXRoQmFja2VuZCJ9.OLVvZlUyT-gpsU9bPEIjGc_RLLY8eReKEF5hKZmWws0");
    }

    public void Dispose()
    {
        _client.Dispose();
        _factory.Dispose();
    }

    [Fact]
    public async Task GetCity_Always_Return_OK()
    {
        var city = "Budapest";
        
        var respons = await _client.GetAsync($"/WeatherForecast/GetCityCode?city={city}");
        
        Assert.Equal(HttpStatusCode.OK, respons.StatusCode);
    }

    [Fact]
    public async Task GetWeather_Always_Ok()
    {
        var lat = 47.4979937;
        
        var lon = 19.0403594;
        
        //var respons = await _client.GetAsync($"/WeatherForecast/GetCurrent?lat=47.4979937&lon=19.0403594");
        var respons = await _client.GetAsync($"/WeatherForecast/GetCurrent?lat={lat}&lon={lon}");
        Assert.Equal(HttpStatusCode.OK, respons.StatusCode);
    }
    
    [Fact]
    public async Task GetTest()
    {
        var respons = await _client.GetAsync("/WeatherForecast/test"); // /WeatherForecast/GetCityCode
        
        Assert.Equal(HttpStatusCode.OK, respons.StatusCode);
    }
}