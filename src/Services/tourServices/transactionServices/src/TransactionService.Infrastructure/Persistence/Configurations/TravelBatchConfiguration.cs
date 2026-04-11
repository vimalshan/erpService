using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TransactionService.Domain.Aggregates;

namespace TransactionService.Infrastructure.Persistence.Configurations;

public sealed class TravelBatchConfiguration : IEntityTypeConfiguration<TravelBatch>
{
    public void Configure(EntityTypeBuilder<TravelBatch> builder)
    {
        builder.ToTable("TRAVEL_BATCHMAIN");
        builder.HasKey(x => x.BatchId);

        builder.Property(x => x.BatchId).HasColumnName("BATCH_ID").HasMaxLength(255).ValueGeneratedNever();
        builder.Property(x => x.AdminId).HasColumnName("BATCH_ADMINID").HasMaxLength(255);
        builder.Property(x => x.PayUnitId).HasColumnName("BATCH_PAYUNITID").HasMaxLength(255);
        builder.Property(x => x.BatchDate).HasColumnName("BATCH_BATCHDATE");
        builder.Property(x => x.InvNum).HasColumnName("BATCH_INVNUM").HasMaxLength(255);
        builder.Property(x => x.InvDate).HasColumnName("BATCH_INVDATE");
        builder.Property(x => x.InvAmount).HasColumnName("BATCH_INVAMOUNT").HasMaxLength(255);
        builder.Property(x => x.Status).HasColumnName("BATCH_STATUS").HasMaxLength(255);
        builder.Property(x => x.AdminRemarks).HasColumnName("BATCH_ADMREMARK").HasMaxLength(255);
        builder.Property(x => x.FinanceRemarks).HasColumnName("BATCH_FINREMARK").HasMaxLength(255);
        builder.Property(x => x.VendorId).HasColumnName("BATCH_VENDORID").HasMaxLength(255);
        builder.Property(x => x.ApprovedAmount).HasColumnName("BATCH_APPAMT").HasMaxLength(255);
        builder.Property(x => x.BillAmount).HasColumnName("BATCH_BILAMT").HasMaxLength(255);
        builder.Property(x => x.ServiceTax).HasColumnName("BATCH_SERTAX").HasMaxLength(255);
        builder.Property(x => x.CessTax).HasColumnName("BATCH_CESTAX").HasMaxLength(255);
        builder.Property(x => x.AdditionalTax).HasColumnName("BATCH_ADLTAX").HasMaxLength(255);
        builder.Property(x => x.TotalPayable).HasColumnName("BATCH_TOTPAY").HasMaxLength(255);
        builder.Property(x => x.JvId).HasColumnName("BATCH_JVID").HasMaxLength(255);
        builder.Property(x => x.PaymentTerms).HasColumnName("BATCH_TERM").HasMaxLength(255);
        builder.Property(x => x.BillDate).HasColumnName("BATCH_BILLDATE");
        builder.Property(x => x.BatchType).HasColumnName("BATCH_TYPE").HasMaxLength(255);
        builder.Property(x => x.CreatedBy).HasColumnName("BATCH_CREATEDBY").HasMaxLength(255);
        builder.Property(x => x.CreatedOn).HasColumnName("BATCH_CREATEDON");
        builder.Property(x => x.ApprovedBy).HasColumnName("BATCH_APPROVEDBY").HasMaxLength(255);
        builder.Property(x => x.ApprovedOn).HasColumnName("BATCH_APPROVEDON");
        builder.Property(x => x.FinApprovedBy).HasColumnName("BATCH_FINAPPROVEDBY").HasMaxLength(255);
        builder.Property(x => x.FinApprovedOn).HasColumnName("BATCH_FINAPPROVEDON");
        builder.Property(x => x.HigherCess).HasColumnName("BATCH_HIGCES").HasMaxLength(255);
        builder.Property(x => x.RoundingOff).HasColumnName("BATCH_RNDOFF").HasMaxLength(255);
        builder.Property(x => x.CabType).HasColumnName("BATCH_CABTYPE").HasMaxLength(255);
        builder.Property(x => x.Surcharge).HasColumnName("BATCH_SURTAX").HasColumnType("DECIMAL(38)");
        builder.Property(x => x.BookingCharges).HasColumnName("BATCH_chrTAX").HasColumnType("DECIMAL(38)");
        builder.Property(x => x.CenvatApplicable).HasColumnName("BATCH_CENVATAPPLICABLE").HasMaxLength(255);
        builder.Property(x => x.DocRefNo).HasColumnName("BATCH_DOCREFNO").HasMaxLength(255);
        builder.Property(x => x.SourceUid).HasColumnName("BATCH_SOURCEUID").HasMaxLength(255);

        builder.HasMany(x => x.SubItems)
            .WithOne()
            .HasForeignKey(x => x.BatchId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(x => x.SubItems).UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.Ignore(x => x.DomainEvents);
    }
}
