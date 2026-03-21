using AuditLogService.Domain.Entities;
using AuditLogService.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuditLogService.Infrastructure.Persistence.Configurations;

public class AuditLogConfiguration : IEntityTypeConfiguration<AuditLogEntry>
{
    public void Configure(EntityTypeBuilder<AuditLogEntry> builder)
    {
        builder.ToTable("AuditLog");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id)
            .HasColumnName("log_id")
            .UseIdentityColumn();

        builder.Property(e => e.TableName)
            .HasColumnName("table_name")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(e => e.RecordId)
            .HasColumnName("record_id")
            .IsRequired();

        builder.Property(e => e.Action)
            .HasColumnName("action")
            .HasMaxLength(10)
            .IsRequired()
            .HasConversion(
                v => v.Value,
                v => AuditAction.From(v));

        builder.Property(e => e.ChangedBy)
            .HasColumnName("changed_by")
            .HasMaxLength(50);

        builder.Property(e => e.ChangeDate)
            .HasColumnName("change_date")
            .IsRequired()
            .HasDefaultValueSql("GETDATE()");

        builder.OwnsOne(e => e.ChangeData, cd =>
        {
            cd.Property(c => c.OldValues)
                .HasColumnName("old_values");
            cd.Property(c => c.NewValues)
                .HasColumnName("new_values");
        });
    }
}
