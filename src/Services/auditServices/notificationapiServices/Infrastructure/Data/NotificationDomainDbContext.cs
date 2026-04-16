using NotificationService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace NotificationService.Infrastructure.Data;

public class NotificationDomainDbContext : DbContext
{
    public NotificationDomainDbContext(DbContextOptions<NotificationDomainDbContext> options) : base(options) { }

    public DbSet<Notification> Notifications => Set<Notification>();
    public DbSet<NotificationCategory> NotificationCategories => Set<NotificationCategory>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<NotificationCategory>(e =>
        {
            e.ToTable("NotificationCategories"); e.HasKey(x => x.CategoryId);
            e.Property(x => x.CategoryName).HasMaxLength(100).IsRequired();
            e.Property(x => x.CategoryCode).HasMaxLength(50).IsRequired();
            e.Property(x => x.Description).HasMaxLength(500);
            e.Property(x => x.IsActive).HasDefaultValue(true);
            e.Property(x => x.CreatedDate).HasDefaultValueSql("GETDATE()");
            e.Property(x => x.ModifiedDate).HasDefaultValueSql("GETDATE()");
            e.Property(x => x.Color).HasMaxLength(7);
            e.Property(x => x.Icon).HasMaxLength(50);
            e.Property(x => x.Priority).HasDefaultValue(5);
            e.Property(x => x.DisplayOrder).HasDefaultValue(999);
            e.HasIndex(x => x.CategoryName).IsUnique();
            e.HasIndex(x => x.CategoryCode).IsUnique();
            e.HasIndex(x => x.IsActive);
        });

        modelBuilder.Entity<Notification>(e =>
        {
            e.ToTable("Notifications"); e.HasKey(x => x.NotificationId);
            e.Property(x => x.Title).HasMaxLength(200).IsRequired();
            e.Property(x => x.Message).IsRequired();
            e.Property(x => x.Priority).HasMaxLength(50).HasDefaultValue("Medium");
            e.Property(x => x.Status).HasMaxLength(50).HasDefaultValue("Active");
            e.Property(x => x.IsActive).HasDefaultValue(true);
            e.Property(x => x.CreatedDate).HasDefaultValueSql("GETDATE()");
            e.Property(x => x.ModifiedDate).HasDefaultValueSql("GETDATE()");
            e.Property(x => x.TargetAudience).HasMaxLength(100);
            e.Property(x => x.ActionRequired).HasDefaultValue(false);
            e.Property(x => x.ActionUrl).HasMaxLength(500);
            e.Property(x => x.AttachmentPath).HasMaxLength(500);
            e.Property(x => x.RelatedEntityType).HasMaxLength(50);
            e.HasIndex(x => x.CategoryId); e.HasIndex(x => x.CompanyId); e.HasIndex(x => x.SiteId);
            e.HasIndex(x => x.Priority); e.HasIndex(x => x.Status);
            e.HasIndex(x => x.CreatedDate); e.HasIndex(x => x.IsActive);
            e.Ignore(x => x.DomainEvents);
            e.HasOne(x => x.Category).WithMany(c => c.Notifications).HasForeignKey(x => x.CategoryId);
        });
    }
}
