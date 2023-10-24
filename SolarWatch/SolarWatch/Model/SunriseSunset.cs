namespace SolarWatch.Model;

public class SunriseSunset
{
    public int Id { get; init; }
    
    public DateTime Sunrise { get; set; }
    
    public DateTime Sunset { get; set; }
    
    public double Lat { get; set; }
    
    public double Long { get; set; }
}