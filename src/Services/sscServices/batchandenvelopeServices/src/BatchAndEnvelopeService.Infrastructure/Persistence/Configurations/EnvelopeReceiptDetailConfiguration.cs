using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BatchAndEnvelopeService.Domain.Entities;

namespace BatchAndEnvelopeService.Infrastructure.Persistence.Configurations;

public class EnvelopeReceiptDetailConfiguration : IEntityTypeConfiguration<EnvelopeReceiptDetail>
{
    public void Configure(EntityTypeBuilder<EnvelopeReceiptDetail> builder)
    {
        builder.ToTable("ENV_RECDET");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("REC_ID").ValueGeneratedNever();
        builder.Property(x => x.EnvelopeId).HasColumnName("REC_ENVID").IsRequired();
        builder.Property(x => x.DocumentId).HasColumnName("REC_DOCID").IsRequired();
        builder.Property(x => x.UpdatedBy).HasColumnName("REC_UPDATEDBY").IsRequired();
        builder.Property(x => x.UpdatedOn).HasColumnName("REC_UPDATEDON").IsRequired();
        builder.Property(x => x.EnvelopeType).HasColumnName("REC_ENVTYPE").HasMaxLength(3);
        builder.Property(x => x.ScanLocationId).HasColumnName("REC_SCANLOCATIONID");
        builder.Ignore(x => x.DomainEvents);
    }
}
