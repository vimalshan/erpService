using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrustService.Domain.Entities;

namespace TrustService.Infrastructure.Persistence.Configurations;

public class TrustRoleConfiguration : IEntityTypeConfiguration<TrustRole>
{
    public void Configure(EntityTypeBuilder<TrustRole> builder)
    {
        builder.ToTable("TRUST_ROLE");

        builder.HasKey(r => new { r.TrTrustCode, r.TrRoleId, r.TrUserId });

        builder.Property(r => r.TrTrustCode).HasColumnName("TR_TRUST_CODE").HasMaxLength(3).IsFixedLength();
        builder.Property(r => r.TrRoleId).HasColumnName("TR_ROLE_ID");
        builder.Property(r => r.TrRoleCode).HasColumnName("TR_ROLE_CODE").HasMaxLength(3).IsFixedLength().IsRequired();
        builder.Property(r => r.TrUserId).HasColumnName("TR_USER_ID").HasMaxLength(25).IsRequired();
        builder.Property(r => r.TrUserNo).HasColumnName("TR_USER_NO").IsRequired();
        builder.Property(r => r.TrEffDate).HasColumnName("TR_EFF_DATE").HasPrecision(3).IsRequired();
        builder.Property(r => r.TrClsDate).HasColumnName("TR_CLS_DATE").HasPrecision(3);
        builder.Property(r => r.TrStatus).HasColumnName("TR_STATUS").HasMaxLength(1).IsFixedLength().HasDefaultValue("A");

        builder.HasIndex(r => new { r.TrTrustCode, r.TrUserId, r.TrStatus }).HasDatabaseName("IDX_TRUST_ROLE_USER");

        builder.Ignore(r => r.DomainEvents);
    }
}
