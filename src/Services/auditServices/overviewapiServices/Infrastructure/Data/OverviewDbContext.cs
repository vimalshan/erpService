using Microsoft.EntityFrameworkCore;
using OverviewService.Domain.Entities;

namespace OverviewService.Infrastructure.Data;

public class OverviewDbContext : DbContext
{
    public OverviewDbContext(DbContextOptions<OverviewDbContext> options) : base(options) { }

    public DbSet<WidgetConfig> WidgetConfigs => Set<WidgetConfig>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<WidgetConfig>(e =>
        {
            e.ToTable("WidgetConfigs");
            e.HasKey(x => x.Id);
            e.Property(x => x.WidgetKey).HasMaxLength(100).IsRequired();
            e.Property(x => x.DisplayName).HasMaxLength(200).IsRequired();
            e.Property(x => x.IsEnabled).HasDefaultValue(true);
            e.Property(x => x.DisplayOrder).HasDefaultValue(0);
            e.Property(x => x.Configuration).HasMaxLength(2000);
            e.Property(x => x.CreatedDate).HasDefaultValueSql("GETDATE()");
            e.Property(x => x.ModifiedDate).HasDefaultValueSql("GETDATE()");
            e.HasIndex(x => x.WidgetKey).IsUnique();
        });
    }
}
