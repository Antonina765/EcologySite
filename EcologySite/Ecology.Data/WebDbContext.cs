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
    
    public DbSet<CommentData> Comments { get; set; }
    
    public WebDbContext() { }

    public WebDbContext(DbContextOptions<WebDbContext> contextOptions)
        : base(contextOptions) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(CONNECTION_STRING);
        // base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        /*modelBuilder.Entity<EcologyData>()
            .HasOne(p => p.User)
            .WithMany(x => x.Ecologies)
            .HasForeignKey(p =>p.UserId);*/

        modelBuilder.Entity<CommentData>()
            .HasOne(x => x.Post)
            .WithMany(x => x.Comments)
            .HasForeignKey(x => x.PostId);
        
        /*modelBuilder.Entity<CommentData>()
            .HasOne(x => x.User)
            .WithMany(x => x.Comments)
            .HasForeignKey(x => x.UserId);*/
    }
}
