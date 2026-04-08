using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PFTransactionalService.Domain.Entities;
using PFTransactionalService.Domain.Enums;

namespace PFTransactionalService.Infrastructure.Persistence.EfCore.Configurations;

public class PFSettlementConfiguration : IEntityTypeConfiguration<PFSettlement>
{
    public void Configure(EntityTypeBuilder<PFSettlement> builder)
    {
        builder.ToTable("PF_SETTLEMENT");
        builder.HasKey(e => e.PfSettlementId);

        builder.Property(e => e.PfSettlementId).HasColumnName("PF_SETTLEMENT_ID").ValueGeneratedOnAdd();
        builder.Property(e => e.EmpSysId).HasColumnName("EMP_SYS_ID");
        builder.Property(e => e.PfSettlementAmount).HasColumnName("PF_SETTLEMENT_AMOUNT").HasColumnType("decimal(19,0)");
        builder.Property(e => e.PfSettlementType).HasColumnName("PF_SETTLEMENT_TYPE").HasMaxLength(10);
        builder.Property(e => e.PfSettlementDate).HasColumnName("PF_SETTLEMENT_DATE").HasPrecision(3);
        builder.Property(e => e.PfSettlementStatus).HasColumnName("PF_SETTLEMENT_STATUS")
            .HasConversion(
                v => ((char)v).ToString(),
                v => (TransactionStatus)v[0])
            .HasMaxLength(1)
            .HasDefaultValue(TransactionStatus.Posted)
            .HasSentinel(TransactionStatus.Posted);
        builder.Property(e => e.ApprovedBy).HasColumnName("APPROVED_BY");
        builder.Property(e => e.CreatedBy).HasColumnName("CREATED_BY");
        builder.Property(e => e.CreatedOn).HasColumnName("CREATED_ON").HasPrecision(3);

        builder.HasMany(e => e.Transactions)
            .WithOne()
            .HasForeignKey(t => t.PfSettlementId)
            .HasPrincipalKey(e => e.PfSettlementId);

        builder.HasIndex(e => e.EmpSysId).HasDatabaseName("IDX_PF_SETTLEMENT_EMPSYSID");

        builder.Ignore(e => e.DomainEvents);
    }
}
