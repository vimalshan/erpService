using AccountingService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccountingService.Infrastructure.Data.Configurations;

public class TransactionMasterConfiguration : IEntityTypeConfiguration<TransactionMaster>
{
    public void Configure(EntityTypeBuilder<TransactionMaster> builder)
    {
        builder.ToTable("TRANSACTION_MASTER");
        builder.HasKey(x => new { x.TransactionTrustCode, x.TransactionCode });
        builder.Property(x => x.TransactionTrustCode).HasColumnName("TRANSACTION_TRUST_CODE").HasColumnType("CHAR(3)").IsRequired();
        builder.Property(x => x.TransactionCode).HasColumnName("TRANSACTION_CODE").HasColumnType("CHAR(3)").IsRequired();
        builder.Property(x => x.TransactionName).HasColumnName("TRANSACTION_NAME").HasMaxLength(25).IsRequired();
        builder.Property(x => x.TransactionType).HasColumnName("TRANSACTION_TYPE").HasColumnType("CHAR(3)").IsRequired();
        builder.Property(x => x.TransactionValue).HasColumnName("TRANSACTION_VALUE").HasMaxLength(255).IsRequired();
    }
}
