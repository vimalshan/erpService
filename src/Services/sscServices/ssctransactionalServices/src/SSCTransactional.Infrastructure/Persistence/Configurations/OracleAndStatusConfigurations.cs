using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SSCTransactional.Domain.Entities;

namespace SSCTransactional.Infrastructure.Persistence.Configurations;

public class OracleInvoiceConfiguration : IEntityTypeConfiguration<OracleInvoice>
{
    public void Configure(EntityTypeBuilder<OracleInvoice> builder)
    {
        builder.ToTable("DOC_ORACLEINVDET");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("DOC_INVID").ValueGeneratedNever();
        builder.Property(x => x.DocId).HasColumnName("DOC_ID").IsRequired();
        builder.Property(x => x.VoucherNo).HasColumnName("DOC_VOUCHERNO").HasColumnType("decimal(38,0)");
        builder.Property(x => x.InvoiceType).HasColumnName("DOC_INVOICETYPE").HasMaxLength(25);
        builder.Property(x => x.VendorId).HasColumnName("DOC_VENDORID");
        builder.Property(x => x.VendorSiteId).HasColumnName("DOC_VENDOR_SITEID");
        builder.Property(x => x.InvoiceNum).HasColumnName("DOC_INVOICENUM").HasMaxLength(50);
        builder.Property(x => x.InvoiceDate).HasColumnName("DOC_INVOICEDATE");
        builder.Property(x => x.InvoiceAmount).HasColumnName("DOC_INVOICEAMOUNT").HasColumnType("decimal(38,0)");
        builder.Property(x => x.InvoiceId).HasColumnName("DOC_INVOICEID").IsRequired();
        builder.Property(x => x.InvoiceStatus).HasColumnName("DOC_INVOICESTATUS").HasMaxLength(4000);
        builder.Property(x => x.InvoiceCreatedOn).HasColumnName("DOC_INVOICECREATEDON");
        builder.Property(x => x.InvoiceCreatedBy).HasColumnName("DOC_INVOICECREATEDBY").HasColumnType("decimal(38,0)");
        builder.Property(x => x.PaymentMethodCode).HasColumnName("DOC_PAYMENT_METHOD_CODE").HasMaxLength(30);
        builder.Property(x => x.AccountingDate).HasColumnName("DOC_ACCOUNTING_DATE");
        builder.Ignore(x => x.DomainEvents);
    }
}

public class OraclePaymentConfiguration : IEntityTypeConfiguration<OraclePayment>
{
    public void Configure(EntityTypeBuilder<OraclePayment> builder)
    {
        builder.ToTable("DOC_ORACLEPAYDET");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("DOC_PAYID").ValueGeneratedNever();
        builder.Property(x => x.DocId).HasColumnName("DOC_ID").IsRequired();
        builder.Property(x => x.PaymentNum).HasColumnName("DOC_PAYMENTNUM").IsRequired();
        builder.Property(x => x.InvoiceId).HasColumnName("DOC_INVOICEID").IsRequired();
        builder.Property(x => x.DueDate).HasColumnName("DOC_DUEDATE");
        builder.Property(x => x.GrossAmount).HasColumnName("DOC_GROSSAMOUNT").HasColumnType("decimal(38,0)");
        builder.Property(x => x.AmountRemaining).HasColumnName("DOC_AMOUNTREMAINING").HasColumnType("decimal(38,0)");
        builder.Property(x => x.PaymentStatus).HasColumnName("DOC_PAYMENT_STATUS").HasMaxLength(14);
        builder.Property(x => x.PaymentMethod).HasColumnName("DOC_PAYMENT_METHOD").HasMaxLength(30);
        builder.Property(x => x.PrepaymentApplied).HasColumnName("DOC_PREPAYMENT_APPLIED").HasMaxLength(3);
        builder.Property(x => x.PaymentCreatedBy).HasColumnName("DOC_PAYMENT_CREATEDBY").HasColumnType("decimal(38,0)");
        builder.Property(x => x.PaymentCreatedOn).HasColumnName("DOC_PAYMENT_CREATEDON");
        builder.Property(x => x.CheckId).HasColumnName("CHECK_ID");
        builder.Property(x => x.BankStatus).HasColumnName("BNKSTATUS").HasMaxLength(1);
        builder.Property(x => x.CheckNumber).HasColumnName("CHECK_NUMBER");
        builder.Property(x => x.CheckDate).HasColumnName("CHECK_DATE");
        builder.Property(x => x.CheckAmount).HasColumnName("CHECK_AMOUNT").HasColumnType("decimal(38,0)");
        builder.Ignore(x => x.DomainEvents);
    }
}

public class OracleBankDetailConfiguration : IEntityTypeConfiguration<OracleBankDetail>
{
    public void Configure(EntityTypeBuilder<OracleBankDetail> builder)
    {
        builder.ToTable("DOC_ORACLEBNKDET");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("DOC_BNKID").ValueGeneratedNever();
        builder.Property(x => x.DocId).HasColumnName("DOC_ID").IsRequired();
        builder.Property(x => x.Type).HasColumnName("TYPE").HasMaxLength(255);
        builder.Property(x => x.CheckId).HasColumnName("CHECK_ID").HasMaxLength(255).IsRequired();
        builder.Property(x => x.Business).HasColumnName("BUSINESS").HasMaxLength(255);
        builder.Property(x => x.OrgId).HasColumnName("ORG_ID").HasMaxLength(255);
        builder.Property(x => x.VendorSiteId).HasColumnName("VENDOR_SITE_ID").HasMaxLength(255);
        builder.Property(x => x.FileName).HasColumnName("FILE_NAME").HasMaxLength(255);
        builder.Property(x => x.VendorCode).HasColumnName("VENDOR_CODE").HasMaxLength(255);
        builder.Property(x => x.Amount).HasColumnName("AMOUNT").HasMaxLength(255);
        builder.Property(x => x.Currency).HasColumnName("CURRENCY").HasMaxLength(255);
        builder.Property(x => x.PaymentNumber).HasColumnName("PAYMENT_NUMBER").HasMaxLength(255);
        builder.Property(x => x.CheckNumber).HasColumnName("CHECK_NUMBER").HasMaxLength(255);
        builder.Property(x => x.PaymentDate).HasColumnName("PAYMENT_DATE").HasMaxLength(255);
        builder.Property(x => x.BeneIfsc).HasColumnName("BENE_IFSC").HasMaxLength(255);
        builder.Property(x => x.BeneAccountType).HasColumnName("BENE_ACCOUNT_TYPE").HasMaxLength(255);
        builder.Property(x => x.BeneBankName).HasColumnName("BENE_BANK_NAME").HasMaxLength(255);
        builder.Property(x => x.BeneBankAc).HasColumnName("BENE_BANK_AC22").HasMaxLength(255);
        builder.Property(x => x.BeneBankBranch).HasColumnName("BENE_BANK_BRANCH").HasMaxLength(255);
        builder.Property(x => x.BeneMailId).HasColumnName("BENE_MAIL_ID").HasMaxLength(255);
        builder.Property(x => x.UtrNo).HasColumnName("UTR_NO").HasMaxLength(255);
        builder.Property(x => x.StatusLookupCode).HasColumnName("STATUS_LOOKUP_CODE").HasMaxLength(25);
        builder.Ignore(x => x.DomainEvents);
    }
}

public class OracleDueDetailConfiguration : IEntityTypeConfiguration<OracleDueDetail>
{
    public void Configure(EntityTypeBuilder<OracleDueDetail> builder)
    {
        builder.ToTable("DOC_ORACLEDUEDET");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("DOC_DUEID").ValueGeneratedNever();
        builder.Property(x => x.DocId).HasColumnName("DOC_ID").IsRequired();
        builder.Property(x => x.OrgId).HasColumnName("ORG_ID");
        builder.Property(x => x.InvoiceId).HasColumnName("INVOICEID").IsRequired();
        builder.Property(x => x.VoucherNo).HasColumnName("VOUCHER_NO").HasColumnType("decimal(38,0)");
        builder.Property(x => x.DocumentId).HasColumnName("DOCUMENT_ID").HasMaxLength(150);
        builder.Property(x => x.DueDate).HasColumnName("DUEDATE");
        builder.Property(x => x.PaymentNum).HasColumnName("PAYMENT_NUM");
        builder.Property(x => x.DueAmount).HasColumnName("DUE_AMOUNT").HasColumnType("decimal(38,0)");
        builder.Property(x => x.CreatedBy).HasColumnName("DOC_DUECREATEDBY").HasColumnType("decimal(38,0)");
        builder.Property(x => x.CreatedOn).HasColumnName("DOC_DUECREATEDON");
        builder.Ignore(x => x.DomainEvents);
    }
}

public class DocumentStatusConfiguration : IEntityTypeConfiguration<DocumentStatus>
{
    public void Configure(EntityTypeBuilder<DocumentStatus> builder)
    {
        builder.ToTable("DOC_STATUS");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("DOC_FLAG").HasMaxLength(2).ValueGeneratedNever();
        builder.Property(x => x.DocType).HasColumnName("DOC_TYPE").HasMaxLength(1).IsRequired();
        builder.Property(x => x.CompletedRemark).HasColumnName("DOC_COMPLETEDREM").HasMaxLength(100).IsRequired();
        builder.Property(x => x.PendingRemark).HasColumnName("DOC_PENDINGREM").HasMaxLength(100).IsRequired();
        builder.Property(x => x.StageOrder).HasColumnName("DOC_STAGEORDER");
        builder.Property(x => x.CategoryGroup).HasColumnName("DOC_CATGROUP").HasMaxLength(50);
        builder.Property(x => x.StageNo).HasColumnName("DOC_STAGENO");
        builder.Ignore(x => x.DomainEvents);
    }
}
