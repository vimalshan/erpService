using FinanceService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinanceService.Infrastructure.Persistence.Configurations;

public class ApInvoiceConfiguration : IEntityTypeConfiguration<ApInvoice>
{
    public void Configure(EntityTypeBuilder<ApInvoice> builder)
    {
        builder.ToTable("AP_INVOICES_INTERFACE");
        builder.HasKey(e => e.InvoiceId);
        builder.Property(e => e.InvoiceId).HasColumnName("INVOICE_ID");
        builder.Property(e => e.InvoiceNum).HasColumnName("INVOICE_NUM").HasMaxLength(50);
        builder.Property(e => e.InvoiceTypeLookupCode).HasColumnName("INVOICE_TYPE_LOOKUP_CODE").HasMaxLength(25);
        builder.Property(e => e.InvoiceDate).HasColumnName("INVOICE_DATE").HasMaxLength(255);
        builder.Property(e => e.VendorId).HasColumnName("VENDOR_ID");
        builder.Property(e => e.VendorSiteId).HasColumnName("VENDOR_SITE_ID");
        builder.Property(e => e.InvoiceAmount).HasColumnName("INVOICE_AMOUNT").HasMaxLength(255);
        builder.Property(e => e.InvoiceCurrencyCode).HasColumnName("INVOICE_CURRENCY_CODE").HasMaxLength(255);
        builder.Property(e => e.ExchangeRate).HasColumnName("EXCHANGE_RATE").HasMaxLength(255);
        builder.Property(e => e.ExchangeRateType).HasColumnName("EXCHANGE_RATE_TYPE").HasMaxLength(30);
        builder.Property(e => e.TermsId).HasColumnName("TERMS_ID");
        builder.Property(e => e.PaymentMethodLookupCode).HasColumnName("PAYMENT_METHOD_LOOKUP_CODE").HasMaxLength(25);
        builder.Property(e => e.Description).HasColumnName("DESCRIPTION").HasMaxLength(240);
        builder.Property(e => e.LastUpdateDate).HasColumnName("LAST_UPDATE_DATE").HasMaxLength(255);
        builder.Property(e => e.LastUpdatedBy).HasColumnName("LAST_UPDATED_BY");
        builder.Property(e => e.CreationDate).HasColumnName("CREATION_DATE").HasMaxLength(255);
        builder.Property(e => e.CreatedBy).HasColumnName("CREATED_BY");
        builder.Property(e => e.OrgId).HasColumnName("ORG_ID").HasColumnType("decimal(18,0)");
        builder.Property(e => e.Status).HasColumnName("STATUS").HasMaxLength(1).IsFixedLength();
        builder.Property(e => e.AgencyId).HasColumnName("AGENCY_ID");

        builder.HasMany(e => e.InvoiceLines)
            .WithOne(e => e.Invoice)
            .HasForeignKey(e => e.InvoiceId);
    }
}
