using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Masters.Domain.Entities;
using Masters.Domain.ValueObjects;

namespace Masters.Infrastructure.Persistence.Configurations;

public class LovMasterConfiguration : IEntityTypeConfiguration<LovMaster>
{
    public void Configure(EntityTypeBuilder<LovMaster> builder)
    {
        builder.ToTable("LOV_MASTER");

        builder.HasKey(e => e.LovId);

        builder.Property(e => e.LovId)
            .HasColumnName("LOV_ID")
            .ValueGeneratedNever();

        builder.Property(e => e.LovType)
            .HasColumnName("LOV_TYPE")
            .HasMaxLength(3)
            .IsFixedLength()
            .HasConversion(
                v => v.Value,
                v => LovTypeCode.Create(v));

        builder.Property(e => e.LovName)
            .HasColumnName("LOV_NAME")
            .HasMaxLength(2000);

        builder.HasIndex(e => e.LovType)
            .HasDatabaseName("IDX_LOV_MASTER_LOV_TYPE");

        builder.Ignore(e => e.DomainEvents);
    }
}
