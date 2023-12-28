using Microsoft.EntityFrameworkCore;

namespace MessagingSample.Database.EF;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<StoreHeader> StoreHeaders { get; set; } = null!;

    public DbSet<StoreContent> StoreContents { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);

        modelBuilder.Entity<StoreHeader>(entity =>
        {
            entity.HasKey(e => e.StoreId);
            entity.Property(e => e.StoreName)
                .IsUnicode(false)
                .HasMaxLength(100);
        });

        modelBuilder.Entity<StoreHeader>()
            .HasIndex(e => e.StoreName)
            .HasDatabaseName("ix__store_header__store_name")
            .IsUnique();

        modelBuilder.Entity<StoreContent>(entity =>
        {
            entity.HasKey(e => e.StoreContentId);
            entity.Property(e => e.StoreId);

            entity.Property(e => e.Key)
                .IsRequired()
                .HasMaxLength(1000);

            entity.Property(e => e.Value)
                .IsRequired()
                .HasMaxLength(1000)
                .IsUnicode();
        });

        modelBuilder.Entity<StoreContent>()
            .HasIndex(e => e.StoreId);

        modelBuilder.Entity<StoreContent>()
            .HasIndex(p => new { p.StoreId, p.Key })
            .HasDatabaseName("IX__store_content__store_id__key")
            .IsUnique();

        modelBuilder.Entity<StoreContent>()
            .HasOne(e => e.Store)
            .WithMany(e => e.Contents)
            .HasForeignKey(e => e.StoreId)
            .HasConstraintName("ix__store_content__store_id");
    }
}