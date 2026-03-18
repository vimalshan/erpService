using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SettlementService.Domain.Entities;
using SettlementService.Domain.Enums;

namespace SettlementService.Infrastructure.Persistence.EfCore.Configurations;

public class SettlementPaymentConfiguration : IEntityTypeConfiguration<SettlementPayment>
{
    public void Configure(EntityTypeBuilder<SettlementPayment> builder)
    {
        builder.ToTable("SET_PAYMENT");
        builder.HasKey(e => e.PayId);

        builder.Property(e => e.PayId).HasColumnName("PAY_ID").ValueGeneratedOnAdd();
        builder.Property(e => e.SetNum).HasColumnName("SET_NUM");
        builder.Property(e => e.PayMode).HasColumnName("PAY_MODE").HasMaxLength(20);
        builder.Property(e => e.PayAmount).HasColumnName("PAY_AMOUNT").HasColumnType("decimal(19,0)");
        builder.Property(e => e.PayDate).HasColumnName("PAY_DATE").HasPrecision(3);
        builder.Property(e => e.PayRefNo).HasColumnName("PAY_REF_NO").HasMaxLength(50);
        builder.Property(e => e.PayStatus).HasColumnName("PAY_STATUS")
            .HasConversion(
                v => ((char)v).ToString(),
                v => (PaymentStatus)v[0])
            .HasMaxLength(1)
            .HasDefaultValue(PaymentStatus.Pending)
            .HasSentinel(PaymentStatus.Pending);

        builder.Ignore(e => e.DomainEvents);
    }
}
