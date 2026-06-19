using Microsoft.EntityFrameworkCore;
using StravaRpg.Api.Models;

namespace StravaRpg.Api.Infrastructure;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("users");
            entity.HasKey(user => user.Id);
            entity.HasIndex(user => user.Email).IsUnique();
            entity.Property(user => user.Id).HasColumnName("id");
            entity.Property(user => user.Email).HasColumnName("email").HasMaxLength(320);
            entity.Property(user => user.Name).HasColumnName("name").HasMaxLength(160);
            entity.Property(user => user.AvatarUrl).HasColumnName("avatar_url").HasMaxLength(500);
            entity.Property(user => user.Level).HasColumnName("level").HasDefaultValue(1);
            entity.Property(user => user.Xp).HasColumnName("xp").HasDefaultValue(0);
        });
    }
}
