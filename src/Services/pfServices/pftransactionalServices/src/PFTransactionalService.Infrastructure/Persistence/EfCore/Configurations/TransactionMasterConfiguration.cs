using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PFTransactionalService.Domain.Entities;

namespace PFTransactionalService.Infrastructure.Persistence.EfCore.Configurations;

public class TransactionMasterConfiguration : IEntityTypeConfiguration<TransactionMaster>
{
    public void Configure(EntityTypeBuilder<TransactionMaster> builder)
    {
        builder.ToTable("TRANSACTION_MASTER");
        builder.HasKey(e => new { e.TransactionTrustCode, e.TransactionName });

        builder.Property(e => e.TransactionTrustCode).HasColumnName("TRANSACTION_TRUST_CODE").HasMaxLength(3).IsFixedLength();
        builder.Property(e => e.TransactionCode).HasColumnName("TRANSACTION_CODE").HasMaxLength(3).IsFixedLength();
        builder.Property(e => e.TransactionName).HasColumnName("TRANSACTION_NAME").HasMaxLength(25);
        builder.Property(e => e.TransactionType).HasColumnName("TRANSACTION_TYPE").HasMaxLength(3).IsFixedLength();
        builder.Property(e => e.TransactionValue).HasColumnName("TRANSACTION_VALUE").HasMaxLength(255);

        builder.Ignore(e => e.DomainEvents);
    }
}
