﻿namespace SolarWatch.Model;

public class City
{
    public int Id { get; init; }
    public string Name { get; set; }
    
    public double Long { get; set; }
    
    public double Lat { get; set; }
    
    public string State { get; set; }
    
    public string Country { get; set; }
}