using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrustService.Domain.Entities;

namespace TrustService.Infrastructure.Persistence.Configurations;

public class TrustApproverConfiguration : IEntityTypeConfiguration<TrustApprover>
{
    public void Configure(EntityTypeBuilder<TrustApprover> builder)
    {
        builder.ToTable("TRUST_APPROVERS");

        builder.HasKey(a => a.ApproverId);
        builder.Property(a => a.ApproverId).HasColumnName("APPROVER_ID").ValueGeneratedOnAdd();

        builder.Property(a => a.TrustCode).HasColumnName("TRUST_CODE").HasMaxLength(3).IsFixedLength().IsRequired();
        builder.Property(a => a.ApproverSysId).HasColumnName("APPROVER_SYSID").IsRequired();
        builder.Property(a => a.ApproverLevel).HasColumnName("APPROVER_LEVEL").IsRequired();
        builder.Property(a => a.ApproverType).HasColumnName("APPROVER_TYPE").HasMaxLength(50).IsRequired();
        builder.Property(a => a.EffDate).HasColumnName("EFF_DATE").HasPrecision(3).IsRequired();
        builder.Property(a => a.ClsDate).HasColumnName("CLS_DATE").HasPrecision(3);
        builder.Property(a => a.ApproverStatus).HasColumnName("APPROVER_STATUS").HasMaxLength(1).IsFixedLength().HasDefaultValue("A");

        builder.HasIndex(a => new { a.TrustCode, a.ApproverLevel, a.ApproverStatus }).HasDatabaseName("IDX_TRUST_APPROVERS_LEVEL");

        builder.Ignore(a => a.DomainEvents);
    }
}
