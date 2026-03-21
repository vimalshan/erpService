using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TravelService.Domain.Entities.Batch;

namespace TravelService.Infrastructure.Persistence.Configurations;

public class BatchSubConfiguration : IEntityTypeConfiguration<BatchSub>
{
    public void Configure(EntityTypeBuilder<BatchSub> builder)
    {
        builder.ToTable("TRAVEL_BATCHSUB");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("BATCHSUB_ID").HasMaxLength(255);
        builder.Property(x => x.BatchId).HasColumnName("BATCHSUB_BATCHID").HasMaxLength(255);
        builder.Property(x => x.BookingConfirmId).HasColumnName("BATCHSUB_BOOKCNFID").HasMaxLength(255);
        builder.Property(x => x.BookingNo).HasColumnName("BATCHSUB_BOOKNO").HasMaxLength(255);
        builder.Property(x => x.BaseAmount).HasColumnName("BATCHSUB_BASAMT").HasConversion<string>().HasMaxLength(255);
        builder.Property(x => x.AdjustedAmount).HasColumnName("BATCHSUB_ADJAMT").HasConversion<string>().HasMaxLength(255);
        builder.Property(x => x.TotalAmount).HasColumnName("BATCHSUB_TOTAMT").HasConversion<string>().HasMaxLength(255);
        builder.Property(x => x.ApprovedAmount).HasColumnName("BATCHSUB_APPAMT").HasConversion<string>().HasMaxLength(255);
        builder.Property(x => x.ServiceTax).HasColumnName("BATCHSUB_SERTAX").HasConversion<string>().HasMaxLength(255);
        builder.Property(x => x.Cess).HasColumnName("BATCHSUB_CESTAX").HasConversion<string>().HasMaxLength(255);
        builder.Property(x => x.AdditionalTax).HasColumnName("BATCHSUB_ADLTAX").HasConversion<string>().HasMaxLength(255);
        builder.Property(x => x.NetPayable).HasColumnName("BATCHSUB_TOTPAY").HasConversion<string>().HasMaxLength(255);
        builder.Property(x => x.Details).HasColumnName("BATCHSUB_REFDET").HasMaxLength(255);
        builder.Property(x => x.VendorRemarks).HasColumnName("BATCHSUB_VENREMARKS").HasMaxLength(255);
        builder.Property(x => x.CreditType).HasColumnName("BATCHSUB_CREDITTYPE").HasMaxLength(255).IsRequired();
        builder.Property(x => x.AdminRemarks).HasColumnName("BATCHSUB_ADMREMARKS").HasMaxLength(255);
        builder.Property(x => x.TicketReference).HasColumnName("BATCHSUB_TKTREFERENCE").HasMaxLength(255);
        builder.Property(x => x.TourPlanId).HasColumnName("BATCHSUB_TPID").HasMaxLength(255);
        builder.Property(x => x.ForexRequestId).HasColumnName("BATCHSUB_FORREQID").HasMaxLength(255);
        builder.Property(x => x.InvoiceNo).HasColumnName("BATCHSUB_INVNUM").HasMaxLength(255);
        builder.Property(x => x.InvoiceDate).HasColumnName("BATCHSUB_INVDATE");
        builder.Property(x => x.VendorId).HasColumnName("BATCHSUB_VENDORID").HasMaxLength(255);
        builder.Ignore(x => x.DomainEvents);
    }
}
