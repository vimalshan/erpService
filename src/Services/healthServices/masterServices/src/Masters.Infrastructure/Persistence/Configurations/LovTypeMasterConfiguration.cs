using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Masters.Domain.Entities;
using Masters.Domain.ValueObjects;

namespace Masters.Infrastructure.Persistence.Configurations;

public class LovTypeMasterConfiguration : IEntityTypeConfiguration<LovTypeMaster>
{
    public void Configure(EntityTypeBuilder<LovTypeMaster> builder)
    {
        builder.ToTable("LOV_TYPEMASTER");

        builder.HasKey(e => e.LovTypeCode);

        builder.Property(e => e.LovTypeCode)
            .HasColumnName("LOV_TYPECODE")
            .HasMaxLength(3)
            .IsFixedLength()
            .HasConversion(
                v => v.Value,
                v => LovTypeCode.Create(v))
            .IsRequired();

        builder.Property(e => e.LovTypeName)
            .HasColumnName("LOV_TYPENAME")
            .HasMaxLength(50);

        builder.HasMany(e => e.LovValues)
            .WithOne(e => e.LovTypeMaster)
            .HasForeignKey(e => e.LovType)
            .HasPrincipalKey(e => e.LovTypeCode);

        builder.Ignore(e => e.DomainEvents);
    }
}
