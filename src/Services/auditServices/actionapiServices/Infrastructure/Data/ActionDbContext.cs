using ActionService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ActionService.Infrastructure.Data;

public class ActionDbContext : DbContext
{
    public ActionDbContext(DbContextOptions<ActionDbContext> options) : base(options) { }

    public DbSet<ActionItem> Actions => Set<ActionItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ActionItem>(entity =>
        {
            entity.ToTable("Actions");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Action).HasColumnName("action").HasMaxLength(255).IsRequired();
            entity.Property(e => e.DueDate).HasColumnName("dueDate");
            entity.Property(e => e.HighPriority).HasColumnName("highPriority");
            entity.Property(e => e.Message).HasColumnName("message");
            entity.Property(e => e.Language).HasColumnName("language").HasMaxLength(50);
            entity.Property(e => e.Service).HasColumnName("service").HasMaxLength(100);
            entity.Property(e => e.Site).HasColumnName("site").HasMaxLength(100);
            entity.Property(e => e.EntityType).HasColumnName("entityType").HasMaxLength(100);
            entity.Property(e => e.EntityId).HasColumnName("entityId");
            entity.Property(e => e.Subject).HasColumnName("subject").HasMaxLength(255);
            entity.Property(e => e.SnowLink).HasColumnName("snowLink").HasMaxLength(255);
            entity.Ignore(e => e.DomainEvents);
        });
    }
}
