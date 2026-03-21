using FinanceService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinanceService.Infrastructure.Persistence.Configurations;

public class PaymentDetailConfiguration : IEntityTypeConfiguration<PaymentDetail>
{
    public void Configure(EntityTypeBuilder<PaymentDetail> builder)
    {
        builder.ToTable("PAYMENT_DETAILS");
        builder.HasNoKey();
        builder.Property(e => e.Sno).HasColumnName("SNO");
        builder.Property(e => e.BookNo).HasColumnName("BOOK_NO");
        builder.Property(e => e.Vendor).HasColumnName("VENDOR").HasMaxLength(200);
        builder.Property(e => e.TsTicketCost).HasColumnName("TS_TKT_CST").HasColumnType("decimal(19,0)");
        builder.Property(e => e.TsTicketAdj).HasColumnName("TS_TKT_ADJ").HasColumnType("decimal(19,0)");
        builder.Property(e => e.TsBaseStax).HasColumnName("TS_BASE_STAX").HasColumnType("decimal(19,0)");
        builder.Property(e => e.TsApproveAmt).HasColumnName("TS_APPROVE_AMT").HasColumnType("decimal(19,0)");
        builder.Property(e => e.TsStatus).HasColumnName("TS_STATUS").HasMaxLength(65);
        builder.Property(e => e.TmInvNum).HasColumnName("TM_INV_NUM").HasMaxLength(65);
        builder.Property(e => e.TmInvDat).HasColumnName("TM_INV_DAT").HasMaxLength(255);
        builder.Property(e => e.TmInvAmt).HasColumnName("TM_INV_AMT").HasColumnType("decimal(19,0)");
        builder.Property(e => e.TmTotApprAmt).HasColumnName("TM_TOTAPPRAMT").HasColumnType("decimal(19,0)");
        builder.Property(e => e.TmTotal).HasColumnName("TM_TOTAL").HasColumnType("decimal(19,0)");
        builder.Property(e => e.TmJvNo).HasColumnName("TM_JVNO");
        builder.Property(e => e.TmPaymentTerms).HasColumnName("TM_PAYMENTTERMS").HasMaxLength(20);
        builder.Property(e => e.ServiceTax).HasColumnName("SERVICETAX").HasColumnType("decimal(19,0)");
    }
}
