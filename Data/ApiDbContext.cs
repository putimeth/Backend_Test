using Microsoft.EntityFrameworkCore;
using Backend_Test.Models;

namespace Backend_Test.Data;

public class ApiDbContext : DbContext
{
    public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<UserLike> UserLikes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Ensure Username is unique
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Username)
            .IsUnique();

        // Ensure a user cannot like the same book twice
        modelBuilder.Entity<UserLike>()
            .HasIndex(ul => new { ul.UserId, ul.BookId })
            .IsUnique();

        base.OnModelCreating(modelBuilder);
    }
}
