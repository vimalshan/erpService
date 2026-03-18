using BankService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BankService.Infrastructure.Persistence.Configurations;

public class ChequeDetailConfiguration : IEntityTypeConfiguration<ChequeDetail>
{
    public void Configure(EntityTypeBuilder<ChequeDetail> builder)
    {
        builder.ToTable("CHEQUE_DET");
        builder.HasKey(e => e.ChequeId);

        builder.Property(e => e.ChequeActranNo).HasColumnName("CHEQUE_ACTRAN_NO");
        builder.Property(e => e.ChequeId).HasColumnName("CHEQUE_ID").ValueGeneratedNever();
        builder.Property(e => e.ChequeBranch).HasColumnName("CHEQUE_BRANCH").HasMaxLength(200);
        builder.Property(e => e.ChequeNo).HasColumnName("CHEQUE_NO").HasColumnType("decimal(20,0)");
        builder.Property(e => e.ChequeDate).HasColumnName("CHEQUE_DATE").HasPrecision(3);
        builder.Property(e => e.ChequeBank).HasColumnName("CHEQUE_BANK");
        builder.Property(e => e.ChequeRemarks).HasColumnName("CHEQUE_REMARKS").HasMaxLength(200);
        builder.Property(e => e.ChequeAmount).HasColumnName("CHEQUE_AMOUNT").HasColumnType("decimal(19,0)");
        builder.Property(e => e.ChequeStatus).HasColumnName("CHEQUE_STATUS").HasMaxLength(1).IsFixedLength().HasDefaultValue("I");
        builder.Property(e => e.ChequePayee).HasColumnName("CHEQUE_PAYEE").HasMaxLength(100);
        builder.Property(e => e.ChequeClearedDate).HasColumnName("CHEQUE_CLEARED_DATE").HasPrecision(3);

        builder.HasIndex(e => new { e.ChequeStatus, e.ChequeDate }).HasDatabaseName("IDX_CHEQUE_DET_STATUS");

        builder.Ignore(e => e.DomainEvents);
    }
}
