using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BatchAndEnvelopeService.Domain.Entities;

namespace BatchAndEnvelopeService.Infrastructure.Persistence.Configurations;

public class BatchDetailConfiguration : IEntityTypeConfiguration<BatchDetail>
{
    public void Configure(EntityTypeBuilder<BatchDetail> builder)
    {
        builder.ToTable("BATCH_DET");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("BATCH_DETID").ValueGeneratedNever();
        builder.Property(x => x.BatchId).HasColumnName("BATCH_ID").IsRequired();
        builder.Property(x => x.EnvelopeId).HasColumnName("BATCH_ENVID").IsRequired();
        builder.Property(x => x.CreatedBy).HasColumnName("BATCH_CREATEDBY").IsRequired();
        builder.Property(x => x.CreatedOn).HasColumnName("BATCH_CREATEDON").IsRequired();
        builder.Property(x => x.ReceiveFlag).HasColumnName("BATCH_RECEIVEFLAG").HasMaxLength(1).IsRequired();
        builder.Property(x => x.ReceivedBy).HasColumnName("BATCH_RECEIVEDBY");
        builder.Property(x => x.ReceivedOn).HasColumnName("BATCH_RECEIVEDON");
        builder.Property(x => x.CancelDate).HasColumnName("BATCH_CANCELDATE").IsRequired();
        builder.Property(x => x.CancelBy).HasColumnName("BATCH_CANCELBY").IsRequired();
        builder.Ignore(x => x.DomainEvents);
    }
}
