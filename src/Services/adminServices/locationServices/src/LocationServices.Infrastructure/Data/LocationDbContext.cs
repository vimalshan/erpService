using LocationServices.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LocationServices.Infrastructure.Data;

public sealed class LocationDbContext : DbContext
{
    public LocationDbContext(DbContextOptions<LocationDbContext> options) : base(options) { }

    public DbSet<LocationAppMapAggregate> LocationAppMaps => Set<LocationAppMapAggregate>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // ── LOCATION_APP_MAP ─────────────────────────────────────────────────
        modelBuilder.Entity<LocationAppMapAggregate>(e =>
        {
            e.ToTable("LOCATION_APP_MAP");

            // Composite primary key (LOCATION_ID + APP_NAME)
            e.HasKey(x => new { x.LocationId, x.AppName });

            e.Property(x => x.LocationId)
                .HasColumnName("LOCATION_ID")
                .HasColumnType("DECIMAL(18,0)")
                .IsRequired();

            e.Property(x => x.AppName)
                .HasColumnName("APP_NAME")
                .HasMaxLength(255)
                .IsRequired();

            e.Property(x => x.SiteCategoryCode)
                .HasColumnName("SITE_CATEGORY_CODE")
                .HasColumnType("BIGINT")
                .IsRequired(false);

            e.Property(x => x.SelfAccess)
                .HasColumnName("SELF_ACCESS")
                .HasMaxLength(255)
                .IsRequired(false);

            e.Property(x => x.DeemedApproval)
                .HasColumnName("DEEMED_APPROVAL")
                .HasMaxLength(1)
                .IsRequired(false);

            e.Property(x => x.IsActive)
                .HasColumnName("IS_ACTIVE")
                .HasDefaultValue(1);

            e.Property(x => x.CreatedAt)
                .HasColumnName("CREATED_DATE")
                .HasDefaultValueSql("GETDATE()");

            e.Property(x => x.CreatedBy)
                .HasColumnName("CREATED_BY")
                .HasMaxLength(100)
                .IsRequired();

            e.Property(x => x.ModifiedDate)
                .HasColumnName("MODIFIED_DATE")
                .IsRequired(false);

            e.Property(x => x.ModifiedBy)
                .HasColumnName("MODIFIED_BY")
                .HasMaxLength(100)
                .IsRequired(false);

            // Ignore domain event collection — not persisted
            e.Ignore("_domainEvents");
        });
    }
}
