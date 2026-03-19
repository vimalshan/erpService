using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BatchAndEnvelopeService.Domain.Entities;

namespace BatchAndEnvelopeService.Infrastructure.Persistence.Configurations;

public class EnvelopeDetailConfiguration : IEntityTypeConfiguration<EnvelopeDetail>
{
    public void Configure(EntityTypeBuilder<EnvelopeDetail> builder)
    {
        builder.ToTable("ENV_DET");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("ENV_DETID").ValueGeneratedNever();
        builder.Property(x => x.EnvelopeId).HasColumnName("ENV_ID").IsRequired();
        builder.Property(x => x.EnvelopeType).HasColumnName("ENV_TYPE").HasMaxLength(3).IsRequired();
        builder.Property(x => x.DocumentId).HasColumnName("ENV_DOCID").IsRequired();
        builder.Property(x => x.CreatedBy).HasColumnName("ENV_CREATEDBY").IsRequired();
        builder.Property(x => x.CreatedOn).HasColumnName("ENV_CREATEDON").IsRequired();
        builder.Property(x => x.ReceiveFlag).HasColumnName("ENV_RECEIVEFLAG").HasMaxLength(1).IsRequired();
        builder.Property(x => x.ReceivedBy).HasColumnName("ENV_RECEIVEDBY").IsRequired();
        builder.Property(x => x.ReceivedOn).HasColumnName("ENV_RECEIVEDON").IsRequired();
        builder.Property(x => x.CancelDate).HasColumnName("ENV_CANCELDATE").IsRequired();
        builder.Property(x => x.CancelBy).HasColumnName("ENV_CANCELBY").IsRequired();
        builder.Ignore(x => x.DomainEvents);
    }
}
