using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrustService.Domain.Entities;

namespace TrustService.Infrastructure.Persistence.Configurations;

public class TrustAuditLogConfiguration : IEntityTypeConfiguration<TrustAuditLog>
{
    public void Configure(EntityTypeBuilder<TrustAuditLog> builder)
    {
        builder.ToTable("TRUST_AUDIT_LOG");

        builder.HasKey(a => a.AuditId);
        builder.Property(a => a.AuditId).HasColumnName("AUDIT_ID").ValueGeneratedOnAdd();

        builder.Property(a => a.TrustCode).HasColumnName("TRUST_CODE").HasMaxLength(3).IsFixedLength().IsRequired();
        builder.Property(a => a.AuditAction).HasColumnName("AUDIT_ACTION").HasMaxLength(50).IsRequired();
        builder.Property(a => a.AuditTable).HasColumnName("AUDIT_TABLE").HasMaxLength(100).IsRequired();
        builder.Property(a => a.AuditTimestamp).HasColumnName("AUDIT_TIMESTAMP").HasPrecision(3).IsRequired();
        builder.Property(a => a.AuditUserId).HasColumnName("AUDIT_USER_ID").IsRequired();
        builder.Property(a => a.OldValues).HasColumnName("OLD_VALUES");
        builder.Property(a => a.NewValues).HasColumnName("NEW_VALUES");

        builder.Ignore(a => a.DomainEvents);
    }
}
