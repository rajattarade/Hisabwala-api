using Hisabwala.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hisabwala.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Tag> Tags => Set<Tag>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Optional: enforce unique constraint on TagName
        modelBuilder.Entity<Tag>()
            .HasIndex(t => t.TagName)
            .IsUnique();
    }
}
