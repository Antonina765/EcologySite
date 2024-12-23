using Microsoft.EntityFrameworkCore;
using MapMinimalApi.Models;

namespace MapMinimalApi.Data;
public class LocationContext : DbContext
{
    public const string CONNECTION_STRING = "Host=localhost;Port=5432;Database=Net18Chat;Username=postgres;Password=12345;";

    public LocationContext(DbContextOptions<LocationContext> options) : base(options)
    {
    }
    
    public DbSet<Location> Locations { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseNpgsql(CONNECTION_STRING);
        }
    }
}
