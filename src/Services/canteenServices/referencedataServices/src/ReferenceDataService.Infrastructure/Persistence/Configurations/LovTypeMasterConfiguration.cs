using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReferenceDataService.Domain.Entities;

namespace ReferenceDataService.Infrastructure.Persistence.Configurations;

public class LovTypeMasterConfiguration : IEntityTypeConfiguration<LovTypeMaster>
{
    public void Configure(EntityTypeBuilder<LovTypeMaster> builder)
    {
        builder.ToTable("LOV_TYPEMASTER");

        builder.HasKey(e => e.LovTypeCode);

        builder.Property(e => e.LovTypeCode)
            .HasColumnName("LOV_TYPECODE")
            .HasColumnType("char(3)")
            .IsRequired();

        builder.Property(e => e.LovTypeName)
            .HasColumnName("LOV_TYPENAME")
            .HasColumnType("varchar(50)");

        builder.Ignore(e => e.DomainEvents);
    }
}
