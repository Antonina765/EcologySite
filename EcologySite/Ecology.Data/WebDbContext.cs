using Ecology.Data.Models;
using Ecology.Data.Models.Ecology;
using Microsoft.EntityFrameworkCore;
//using Microsoft.Identity.Client;

namespace Ecology.Data;
public class WebDbContext : DbContext
{
    public const string CONNECTION_STRING = "Host=localhost;Port=5432;Database=postgres;Username=postgres;Password=12345;";

    public DbSet<UserData> Users { get; set; }

    public DbSet<EcologyData> Ecologies { get; set; }
    
    public WebDbContext() { }

    public WebDbContext(DbContextOptions<WebDbContext> contextOptions)
        : base(contextOptions) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(CONNECTION_STRING);
        // base.OnConfiguring(optionsBuilder);
    }

    /*protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<MangaData>()
            .HasMany(x => x.Characters)
            .WithOne(x => x.Manga)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<SurveyGroupData>()
            .HasMany(x => x.Surveys)
            .WithOne(x => x.SurveyGroup)
            .OnDelete(DeleteBehavior.NoAction);


        modelBuilder.Entity<BrandData>()
            .HasMany(x => x.Coffe)
            .WithOne(x => x.Brand)
            .OnDelete(DeleteBehavior.NoAction);










        modelBuilder.Entity<TypeOfApplianceData>().ToTable("TypeOfAppliances");
        modelBuilder.Entity<ProducerData>().ToTable("Producers");
        modelBuilder.Entity<ModelData>().ToTable("Models");
        modelBuilder.Entity<ClientData>().ToTable("Clients");

        modelBuilder.Entity<ModelData>()
            .HasOne<ProducerData>()
            .WithMany()
            .HasForeignKey(m => m.ProducerId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ModelData>()
            .HasOne<TypeOfApplianceData>()
            .WithMany()
            .HasForeignKey(m => m.TypeId)
            .OnDelete(DeleteBehavior.Cascade);
    }*/
}
