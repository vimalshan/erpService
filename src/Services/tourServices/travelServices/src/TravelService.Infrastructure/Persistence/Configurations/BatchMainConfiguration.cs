using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TravelService.Domain.Entities.Batch;

namespace TravelService.Infrastructure.Persistence.Configurations;

public class BatchMainConfiguration : IEntityTypeConfiguration<BatchMain>
{
    public void Configure(EntityTypeBuilder<BatchMain> builder)
    {
        builder.ToTable("TRAVEL_BATCHMAIN");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("BATCH_ID").HasMaxLength(255);
        builder.Property(x => x.AdminId).HasColumnName("BATCH_ADMINID").HasMaxLength(255);
        builder.Property(x => x.PayrollUnitId).HasColumnName("BATCH_PAYUNITID").HasMaxLength(255);
        builder.Property(x => x.BatchDate).HasColumnName("BATCH_BATCHDATE");
        builder.Property(x => x.InvoiceNo).HasColumnName("BATCH_INVNUM").HasMaxLength(255);
        builder.Property(x => x.InvoiceDate).HasColumnName("BATCH_INVDATE");
        builder.Property(x => x.InvoiceAmount).HasColumnName("BATCH_INVAMOUNT").HasConversion<string>().HasMaxLength(255);
        builder.Property(x => x.Status).HasColumnName("BATCH_STATUS").HasMaxLength(255);
        builder.Property(x => x.AdminRemarks).HasColumnName("BATCH_ADMREMARK").HasMaxLength(255);
        builder.Property(x => x.FinanceRemarks).HasColumnName("BATCH_FINREMARK").HasMaxLength(255);
        builder.Property(x => x.VendorId).HasColumnName("BATCH_VENDORID").HasMaxLength(255);
        builder.Property(x => x.ApprovedAmount).HasColumnName("BATCH_APPAMT").HasConversion<string>().HasMaxLength(255);
        builder.Property(x => x.BilledAmount).HasColumnName("BATCH_BILAMT").HasConversion<string>().HasMaxLength(255);
        builder.Property(x => x.ServiceTax).HasColumnName("BATCH_SERTAX").HasConversion<string>().HasMaxLength(255);
        builder.Property(x => x.Cess).HasColumnName("BATCH_CESTAX").HasConversion<string>().HasMaxLength(255);
        builder.Property(x => x.AdditionalTax).HasColumnName("BATCH_ADLTAX").HasConversion<string>().HasMaxLength(255);
        builder.Property(x => x.TotalPayable).HasColumnName("BATCH_TOTPAY").HasConversion<string>().HasMaxLength(255);
        builder.Property(x => x.JvId).HasColumnName("BATCH_JVID").HasMaxLength(255);
        builder.Property(x => x.PaymentTerms).HasColumnName("BATCH_TERM").HasMaxLength(255);
        builder.Property(x => x.BillDate).HasColumnName("BATCH_BILLDATE");
        builder.Property(x => x.BatchType).HasColumnName("BATCH_TYPE").HasMaxLength(255);
        builder.Property(x => x.CreatedBy).HasColumnName("BATCH_CREATEDBY").HasMaxLength(255);
        builder.Property(x => x.CreatedOn).HasColumnName("BATCH_CREATEDON");
        builder.Property(x => x.ApprovedBy).HasColumnName("BATCH_APPROVEDBY").HasMaxLength(255);
        builder.Property(x => x.ApprovedOn).HasColumnName("BATCH_APPROVEDON");
        builder.Property(x => x.FinanceApprovedBy).HasColumnName("BATCH_FINAPPROVEDBY").HasMaxLength(255);
        builder.Property(x => x.FinanceApprovedOn).HasColumnName("BATCH_FINAPPROVEDON");
        builder.Property(x => x.CabType).HasColumnName("BATCH_CABTYPE").HasMaxLength(255);
        builder.Property(x => x.DocumentRefNo).HasColumnName("BATCH_DOCREFNO").HasMaxLength(255);
        builder.Property(x => x.SourceUid).HasColumnName("BATCH_SOURCEUID").HasMaxLength(255);
        builder.HasMany(x => x.BatchSubs).WithOne().HasForeignKey(s => s.BatchId);
        builder.Ignore(x => x.DomainEvents);
    }
}
