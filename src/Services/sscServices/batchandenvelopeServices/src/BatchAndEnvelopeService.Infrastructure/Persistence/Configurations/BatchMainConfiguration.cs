using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BatchAndEnvelopeService.Domain.Aggregates;

namespace BatchAndEnvelopeService.Infrastructure.Persistence.Configurations;

public class BatchMainConfiguration : IEntityTypeConfiguration<BatchAggregate>
{
    public void Configure(EntityTypeBuilder<BatchAggregate> builder)
    {
        builder.ToTable("BATCH_MAIN");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("BATCH_ID").ValueGeneratedNever();
        builder.Property(x => x.CreatedBy).HasColumnName("BATCH_CREATEDBY").IsRequired();
        builder.Property(x => x.CreatedOn).HasColumnName("BATCH_CREATEDON").IsRequired();
        builder.Property(x => x.LocationId).HasColumnName("BATCH_LOCATIONID").IsRequired();
        builder.Property(x => x.ReceivedBy).HasColumnName("BATCH_RECEIVEDBY").IsRequired();
        builder.Property(x => x.ReceivedOn).HasColumnName("BATCH_RECEIVEDON").IsRequired();
        builder.Property(x => x.PodNo).HasColumnName("BATCH_PODNO").HasMaxLength(25).IsRequired();
        builder.Property(x => x.SummaryFlag).HasColumnName("BATCH_SUMMARYFLAG").HasMaxLength(1).IsRequired();
        builder.Property(x => x.CancelBy).HasColumnName("BATCH_CANCELBY");
        builder.Property(x => x.CancelDate).HasColumnName("BATCH_CANCELDATE");
        builder.Property(x => x.ConfirmedBy).HasColumnName("BATCH_CONFIRMEDBY");
        builder.Property(x => x.ConfirmedOn).HasColumnName("BATCH_CONFIRMEDON");
        builder.Property(x => x.CourierName).HasColumnName("BATCH_COURIERNAME").HasMaxLength(100);
        builder.Property(x => x.ScanFlag).HasColumnName("BATCH_SCANFLAG").HasMaxLength(25).IsRequired();

        builder.Ignore(x => x.DomainEvents);

        builder.HasMany(x => x.Details)
            .WithOne()
            .HasForeignKey(d => d.BatchId);

        builder.HasMany(x => x.ReceiptDetails)
            .WithOne()
            .HasForeignKey(d => d.BatchId);
    }
}
