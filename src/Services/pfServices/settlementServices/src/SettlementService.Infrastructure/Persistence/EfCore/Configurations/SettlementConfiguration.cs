using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SettlementService.Domain.Aggregates;
using SettlementService.Domain.Enums;

namespace SettlementService.Infrastructure.Persistence.EfCore.Configurations;

public class SettlementConfiguration : IEntityTypeConfiguration<Settlement>
{
    public void Configure(EntityTypeBuilder<Settlement> builder)
    {
        builder.ToTable("SET_MAIN");
        builder.HasKey(e => e.StSetNum);

        builder.Property(e => e.StSetNum).HasColumnName("ST_SET_NUM").ValueGeneratedNever();
        builder.Property(e => e.StTrustCode).HasColumnName("ST_TRUST_CODE").HasMaxLength(3).IsFixedLength();
        builder.Property(e => e.StMemberNo).HasColumnName("ST_MEMBER_NO");
        builder.Property(e => e.StSetType).HasColumnName("ST_SET_TYPE").HasMaxLength(1).IsFixedLength();
        builder.Property(e => e.StSetDate).HasColumnName("ST_SET_DATE").HasPrecision(3);
        builder.Property(e => e.StDolDat).HasColumnName("ST_DOL_DAT").HasPrecision(3);
        builder.Property(e => e.StReason).HasColumnName("ST_REASON").HasMaxLength(200);
        builder.Property(e => e.StUpdOn).HasColumnName("ST_UPDON").HasPrecision(3);
        builder.Property(e => e.StUpdByEmpSysId).HasColumnName("ST_UPDBY_EMP_SYSID");
        builder.Property(e => e.StAccDate).HasColumnName("ST_ACC_DATE").HasPrecision(3);
        builder.Property(e => e.StFinYear).HasColumnName("ST_FINYEAR");
        builder.Property(e => e.StJvVoucherType).HasColumnName("ST_JV_VOUCHER_TYPE").HasMaxLength(3).IsFixedLength();
        builder.Property(e => e.StJvNo).HasColumnName("ST_JV_NO");
        builder.Property(e => e.StSetIntFlg).HasColumnName("ST_SET_INT_FLG").HasMaxLength(1).IsFixedLength();
        builder.Property(e => e.StTaxSts).HasColumnName("ST_TAXSTS").HasMaxLength(200);
        builder.Property(e => e.StTaxRate).HasColumnName("ST_TAXRATE");
        builder.Property(e => e.StSettlementAmount).HasColumnName("ST_SETTLEMENT_AMOUNT").HasColumnType("decimal(19,0)");
        builder.Property(e => e.StStatus).HasColumnName("ST_STATUS")
            .HasConversion(
                v => ((char)v).ToString(),
                v => (SettlementStatus)v[0])
            .HasMaxLength(1)
            .HasDefaultValue(SettlementStatus.Pending)
            .HasSentinel(SettlementStatus.Pending);

        builder.HasMany(e => e.Deductions)
            .WithOne()
            .HasForeignKey(d => d.SetNum)
            .HasPrincipalKey(e => e.StSetNum);

        builder.HasMany(e => e.Approvals)
            .WithOne()
            .HasForeignKey(a => a.SetNum)
            .HasPrincipalKey(e => e.StSetNum);

        builder.HasMany(e => e.Payments)
            .WithOne()
            .HasForeignKey(p => p.SetNum)
            .HasPrincipalKey(e => e.StSetNum);

        builder.HasIndex(e => e.StMemberNo).HasDatabaseName("IDX_SET_MAIN_MEMBER");
        builder.HasIndex(e => new { e.StStatus, e.StSetDate }).HasDatabaseName("IDX_SET_MAIN_STATUS");

        builder.Ignore(e => e.DomainEvents);
    }
}
