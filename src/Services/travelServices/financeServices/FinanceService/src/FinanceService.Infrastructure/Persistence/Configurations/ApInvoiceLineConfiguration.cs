using FinanceService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinanceService.Infrastructure.Persistence.Configurations;

public class ApInvoiceLineConfiguration : IEntityTypeConfiguration<ApInvoiceLine>
{
    public void Configure(EntityTypeBuilder<ApInvoiceLine> builder)
    {
        builder.ToTable("AP_INVOICE_LINES_INTERFACE");
        builder.HasKey(e => new { e.InvoiceId, e.LineNumber });
        builder.Property(e => e.InvoiceId).HasColumnName("INVOICE_ID");
        builder.Property(e => e.InvoiceLineId).HasColumnName("INVOICE_LINE_ID");
        builder.Property(e => e.LineNumber).HasColumnName("LINE_NUMBER");
        builder.Property(e => e.LineTypeLookupCode).HasColumnName("LINE_TYPE_LOOKUP_CODE").HasMaxLength(25);
        builder.Property(e => e.Amount).HasColumnName("AMOUNT").HasColumnType("decimal(38,0)");
        builder.Property(e => e.AccountingDate).HasColumnName("ACCOUNTING_DATE");
        builder.Property(e => e.Description).HasColumnName("DESCRIPTION").HasMaxLength(240);
        builder.Property(e => e.LastUpdatedBy).HasColumnName("LAST_UPDATED_BY");
        builder.Property(e => e.LastUpdateDate).HasColumnName("LAST_UPDATE_DATE");
        builder.Property(e => e.CreatedBy).HasColumnName("CREATED_BY");
        builder.Property(e => e.CreationDate).HasColumnName("CREATION_DATE");
        builder.Property(e => e.OrgId).HasColumnName("ORG_ID");
        builder.Property(e => e.AccountCode).HasColumnName("ACCOUNT_CODE").HasMaxLength(25);
        builder.Property(e => e.ProjectCode).HasColumnName("PROJECT_CODE").HasMaxLength(25);
        builder.Property(e => e.SgstAmt).HasColumnName("SGSTAMT").HasColumnType("decimal(19,0)");
        builder.Property(e => e.CgstAmt).HasColumnName("CGSTAMT").HasColumnType("decimal(19,0)");
        builder.Property(e => e.IgstAmt).HasColumnName("IGSTAMT").HasColumnType("decimal(19,0)");
    }
}
