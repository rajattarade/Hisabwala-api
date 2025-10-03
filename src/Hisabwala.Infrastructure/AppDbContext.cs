using Hisabwala.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hisabwala.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Tag> Tags => Set<Tag>();
    public DbSet<Party> Parties => Set<Party>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Optional: enforce unique constraint on TagName
        modelBuilder.Entity<Tag>()
            .HasIndex(t => t.TagName)
            .IsUnique();

        modelBuilder.Entity<Party>()
            .ToTable("Party")
            .HasIndex(p => p.PartyCode)
            .IsUnique();
    }
}
