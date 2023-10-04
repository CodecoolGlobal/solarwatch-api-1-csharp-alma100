using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SolarWatch.Controllers;
using SolarWatch.Model;
using SolarWatch.Service;
using OkObjectResult = Microsoft.AspNetCore.Mvc.OkObjectResult;

namespace SolarWatchTest;

public class Tests
{
    private  Mock<ILogger<WeatherForecastController>> _logger;

    private  Mock<IGeocodeProvider> _geocodeProvider;

    private  Mock<ISunsetDataProvider> _sunsetDataProvider;

    private  Mock<IJsonProcessor> _jsonProcessor;

    private WeatherForecastController _forecastController;

    [SetUp]
    public void Setup()
    {
        _logger = new Mock<ILogger<WeatherForecastController>>();
        _geocodeProvider = new Mock<IGeocodeProvider>();
        _sunsetDataProvider = new Mock<ISunsetDataProvider>();
        _jsonProcessor = new Mock<IJsonProcessor>();
        _forecastController = new WeatherForecastController(_logger.Object, _geocodeProvider.Object,
            _jsonProcessor.Object, _sunsetDataProvider.Object);
    }

    [Test]
    public void GetCurrentReturnsWithNoGeoData()
    {
        var city = "";
        
        _geocodeProvider.Setup(x => x.GetGeoCode(It.IsAny<string>())).Throws(It.IsAny<Exception>());

        var result = _forecastController.Get(city);
        
        Assert.IsInstanceOf(typeof(NotFoundObjectResult), result.Result);

    }
    
    [Test]
    public void GetCurrentReturnsWithGoodGeoData()
    {
       var city = "Budapest"; 
        var weatherData = "{}";
        _geocodeProvider.Setup(x => x.GetGeoCode(It.IsAny<string>())).ReturnsAsync(weatherData);
        _jsonProcessor.Setup(x => x.Process(weatherData)).Returns(new GeoLocation());
        var result = _forecastController.Get("Budapest");

        Assert.IsInstanceOf(typeof(OkObjectResult), result.Result);
        
    }
    
    
}