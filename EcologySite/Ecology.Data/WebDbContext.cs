using Ecology.Data.Models;
using Ecology.Data.Models.Ecology;
using Ecology.Data.DataLayerModels;
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

        modelBuilder.Entity<UserData>().HasKey(us => us.Id);
        
        modelBuilder.Entity<UserData>()
            .HasMany(p => p.Ecologies)
            .WithOne(x => x.User)
            .OnDelete(DeleteBehavior.Restrict)
            .HasForeignKey(p =>p.UserId);
        

        modelBuilder.Entity<EcologyData>().HasKey(ec => ec.Id);
        
        modelBuilder.Entity<EcologyData>()
            .HasMany(x => x.Comments)
            .WithOne(x => x.Ecology)
            .OnDelete(DeleteBehavior.Cascade)
            .HasForeignKey(x => x.PostId); //удалять ком при удалении поста
        
        
        modelBuilder.Entity<CommentData>().HasKey(c => c.Id);

        modelBuilder.Entity<CommentData>()
            .HasOne(x => x.User)
            .WithMany(x => x.Comments)
            .OnDelete(DeleteBehavior.Restrict)
            .HasForeignKey(x => x.UserId);
        
        modelBuilder.Entity<UserEcologyLikesData>()
            .HasKey(ue => new { ue.UserId, ue.EcologyDataId });

        modelBuilder.Entity<UserEcologyLikesData>()
            .HasOne(ue => ue.User)
            .WithMany(u => u.PostsWhichUsersLike)
            .HasForeignKey(ue => ue.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<UserEcologyLikesData>()
            .HasOne(ue => ue.EcologyData)
            .WithMany(ec => ec.UsersWhoLikeIt)
            .HasForeignKey(ue => ue.EcologyDataId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
