using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductionManagement.Domain.Entities;

namespace ProductionManagement.Infrastructure.Persistence.Configurations;

public class NormsMasterConfiguration : IEntityTypeConfiguration<NormsMaster>
{
    public void Configure(EntityTypeBuilder<NormsMaster> builder)
    {
        builder.ToTable("NORMS_MASTER");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).ValueGeneratedOnAdd();

        builder.Property(e => e.NormId)
            .HasColumnName("NORM_ID");

        builder.Property(e => e.NormInputCode)
            .HasColumnName("NORM_INPUT_CODE");

        builder.Property(e => e.NormOutputCode)
            .HasColumnName("NORM_OUTPUT_CODE");

        builder.Property(e => e.NormRate)
            .HasColumnName("NORM_RATE");

        builder.Property(e => e.NormNo)
            .HasColumnName("NORM_NO");

        builder.Ignore(e => e.DomainEvents);
    }
}
