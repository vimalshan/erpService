using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BatchAndEnvelopeService.Domain.Aggregates;

namespace BatchAndEnvelopeService.Infrastructure.Persistence.Configurations;

public class EnvelopeMainConfiguration : IEntityTypeConfiguration<EnvelopeAggregate>
{
    public void Configure(EntityTypeBuilder<EnvelopeAggregate> builder)
    {
        builder.ToTable("ENV_MAIN");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("ENV_ID").ValueGeneratedNever();
        builder.Property(x => x.EnvelopeType).HasColumnName("ENV_TYPE").HasMaxLength(3).IsRequired();
        builder.Property(x => x.CreatedBy).HasColumnName("ENV_CREATEDBY").IsRequired();
        builder.Property(x => x.CreatedOn).HasColumnName("ENV_CREATEDON").IsRequired();
        builder.Property(x => x.ReceivedBy).HasColumnName("ENV_RECEIVEDBY");
        builder.Property(x => x.ReceivedOn).HasColumnName("ENV_RECEIVEDON");
        builder.Property(x => x.SummaryFlag).HasColumnName("ENV_SUMMARYFLAG").HasMaxLength(1).IsRequired();
        builder.Property(x => x.CancelledBy).HasColumnName("ENV_CANCELLEDBY");
        builder.Property(x => x.CancelledOn).HasColumnName("ENV_CANCELLEDON");
        builder.Property(x => x.ConfirmedBy).HasColumnName("ENV_CONFIRMEDBY");
        builder.Property(x => x.ConfirmedOn).HasColumnName("ENV_CONFIRMEDON");
        builder.Property(x => x.ScanLotNo).HasColumnName("ENV_SCANLOTNO");
        builder.Property(x => x.LocationId).HasColumnName("ENV_LOCID").IsRequired();

        builder.Ignore(x => x.DomainEvents);

        builder.HasMany(x => x.Details)
            .WithOne()
            .HasForeignKey(d => d.EnvelopeId);

        builder.HasMany(x => x.ReceiptDetails)
            .WithOne()
            .HasForeignKey(d => d.EnvelopeId);
    }
}
