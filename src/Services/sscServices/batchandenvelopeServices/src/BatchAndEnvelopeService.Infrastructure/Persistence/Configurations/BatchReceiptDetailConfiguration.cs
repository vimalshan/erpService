using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BatchAndEnvelopeService.Domain.Entities;

namespace BatchAndEnvelopeService.Infrastructure.Persistence.Configurations;

public class BatchReceiptDetailConfiguration : IEntityTypeConfiguration<BatchReceiptDetail>
{
    public void Configure(EntityTypeBuilder<BatchReceiptDetail> builder)
    {
        builder.ToTable("BATCH_RECDET");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("REC_ID").ValueGeneratedNever();
        builder.Property(x => x.BatchId).HasColumnName("REC_BATCHID").IsRequired();
        builder.Property(x => x.EnvelopeId).HasColumnName("REC_ENVID").IsRequired();
        builder.Property(x => x.UpdatedBy).HasColumnName("REC_UPDATEDBY").IsRequired();
        builder.Property(x => x.UpdatedOn).HasColumnName("REC_UPDATEDON").IsRequired();
        builder.Property(x => x.ScanLocationId).HasColumnName("REC_SCANLOCATIONID");
        builder.Ignore(x => x.DomainEvents);
    }
}
