using Microsoft.EntityFrameworkCore;
using TwitterClone.Domain.Entities;

namespace TwitterClone.Infrastructure.Data;

public class TwitterDbContext : DbContext
{
    public TwitterDbContext(DbContextOptions<TwitterDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Tweet> Tweets => Set<Tweet>();
    public DbSet<Comment> Comments => Set<Comment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Tweet>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Content).HasMaxLength(280).IsRequired();
            entity.HasOne(e => e.User).WithMany(u => u.Tweets).HasForeignKey(e => e.UserId);
            entity.HasIndex(e => e.CreatedAt).IsDescending();
        });
    }
}