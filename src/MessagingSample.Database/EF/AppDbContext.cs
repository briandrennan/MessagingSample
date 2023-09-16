using Microsoft.EntityFrameworkCore;

namespace MessagingSample.Database.EF;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    // EF will lazily initialize these when they're accessed.

    public DbSet<StoreHeader> StoreHeaders { get; set; } = null!;

    public DbSet<StoreContent> StoreContents { get; set; } = null!;
}