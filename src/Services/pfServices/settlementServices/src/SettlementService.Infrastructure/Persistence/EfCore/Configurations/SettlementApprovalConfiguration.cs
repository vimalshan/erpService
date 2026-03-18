using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SettlementService.Domain.Entities;
using SettlementService.Domain.Enums;

namespace SettlementService.Infrastructure.Persistence.EfCore.Configurations;

public class SettlementApprovalConfiguration : IEntityTypeConfiguration<SettlementApproval>
{
    public void Configure(EntityTypeBuilder<SettlementApproval> builder)
    {
        builder.ToTable("SET_APPROVAL");
        builder.HasKey(e => e.AprId);

        builder.Property(e => e.AprId).HasColumnName("APR_ID").ValueGeneratedOnAdd();
        builder.Property(e => e.SetNum).HasColumnName("SET_NUM");
        builder.Property(e => e.AprLevel).HasColumnName("APR_LEVEL");
        builder.Property(e => e.AprBySysId).HasColumnName("APR_BY_SYSID");
        builder.Property(e => e.AprStatus).HasColumnName("APR_STATUS")
            .HasConversion(
                v => ((char)v).ToString(),
                v => (ApprovalStatus)v[0])
            .HasMaxLength(1);
        builder.Property(e => e.AprRemarks).HasColumnName("APR_REMARKS").HasMaxLength(200);
        builder.Property(e => e.AprDate).HasColumnName("APR_DATE").HasPrecision(3);

        builder.Ignore(e => e.DomainEvents);
    }
}
