using Microsoft.EntityFrameworkCore;
using SolarWatch.Model;

namespace SolarWatch.Context;

public class SolarApiContext : DbContext
{
    public DbSet<City> Cities { get; set; }
    
    public DbSet<SunriseSunset> SunriseSunsets { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        //UseSqlServer --> eléréséhez --> Microsoft.EntityFrameworkCore.Sql kell!
        //Encrypt=false --> ha nem akarsz szopni a tanúsítvánnyal :D
        optionsBuilder.UseSqlServer("Server=localhost,1433;Database=Solar;User Id=sa;Password=yourStrong(!)Password;Encrypt=false;"); 
    }
    
    
}