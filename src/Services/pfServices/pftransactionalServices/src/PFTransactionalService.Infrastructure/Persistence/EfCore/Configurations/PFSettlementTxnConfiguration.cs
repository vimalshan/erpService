using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PFTransactionalService.Domain.Entities;
using PFTransactionalService.Domain.Enums;

namespace PFTransactionalService.Infrastructure.Persistence.EfCore.Configurations;

public class PFSettlementTxnConfiguration : IEntityTypeConfiguration<PFSettlementTxn>
{
    public void Configure(EntityTypeBuilder<PFSettlementTxn> builder)
    {
        builder.ToTable("PF_SETTLEMENT_TXN");
        builder.HasKey(e => e.PfSettlementTxnId);

        builder.Property(e => e.PfSettlementTxnId).HasColumnName("PF_SETTLEMENT_TXN_ID").ValueGeneratedOnAdd();
        builder.Property(e => e.PfSettlementId).HasColumnName("PF_SETTLEMENT_ID");
        builder.Property(e => e.EmpSysId).HasColumnName("EMP_SYS_ID");
        builder.Property(e => e.PfSettlementTxnAmount).HasColumnName("PF_SETTLEMENT_TXN_AMOUNT").HasColumnType("decimal(19,0)");
        builder.Property(e => e.PfSettlementTxnDate).HasColumnName("PF_SETTLEMENT_TXN_DATE").HasPrecision(3);
        builder.Property(e => e.PfSettlementTxnStatus).HasColumnName("PF_SETTLEMENT_TXN_STATUS")
            .HasConversion(
                v => ((char)v).ToString(),
                v => (TransactionStatus)v[0])
            .HasMaxLength(1)
            .HasDefaultValue(TransactionStatus.Posted)
            .HasSentinel(TransactionStatus.Posted);
        builder.Property(e => e.CreatedBy).HasColumnName("CREATED_BY");
        builder.Property(e => e.CreatedOn).HasColumnName("CREATED_ON").HasPrecision(3);

        builder.Ignore(e => e.DomainEvents);
    }
}
