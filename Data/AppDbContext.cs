using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MovieLog.Models; 

namespace MovieLog.Data;

public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Movie> Movies { get; set; }
    public DbSet<Genre> Genres { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<Watchlist> Watchlists { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Review>()
            .HasOne(r=>r.User)
            .WithMany(u=>u.Reviews)
            .HasForeignKey(r=>r.UserId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Watchlist>()
            .HasOne(w => w.User)
            .WithMany(u => u.Watchlists)
            .HasForeignKey(w => w.UserId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}