using BankService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BankService.Infrastructure.Persistence.Configurations;

public class PaymentReconciliationConfiguration : IEntityTypeConfiguration<PaymentReconciliation>
{
    public void Configure(EntityTypeBuilder<PaymentReconciliation> builder)
    {
        builder.ToTable("PAYMENT_RECONCILIATION");
        builder.HasKey(e => e.ReconId);

        builder.Property(e => e.ReconId).HasColumnName("RECON_ID").UseIdentityColumn();
        builder.Property(e => e.ChequeId).HasColumnName("CHEQUE_ID");
        builder.Property(e => e.ReconReference).HasColumnName("RECON_REFERENCE").HasMaxLength(100);
        builder.Property(e => e.ReconAmount).HasColumnName("RECON_AMOUNT").HasColumnType("decimal(19,0)");
        builder.Property(e => e.ReconDate).HasColumnName("RECON_DATE").HasPrecision(3);
        builder.Property(e => e.ReconStatus).HasColumnName("RECON_STATUS").HasMaxLength(1).IsFixedLength().HasDefaultValue("O");

        builder.HasOne(e => e.Cheque)
            .WithOne(c => c.Reconciliation)
            .HasForeignKey<PaymentReconciliation>(e => e.ChequeId)
            .HasConstraintName("FK_PAYMENT_RECON_CHEQUE");

        builder.Ignore(e => e.DomainEvents);
    }
}
