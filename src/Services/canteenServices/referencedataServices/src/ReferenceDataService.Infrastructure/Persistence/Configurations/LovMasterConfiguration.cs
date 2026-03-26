using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReferenceDataService.Domain.Entities;

namespace ReferenceDataService.Infrastructure.Persistence.Configurations;

public class LovMasterConfiguration : IEntityTypeConfiguration<LovMaster>
{
    public void Configure(EntityTypeBuilder<LovMaster> builder)
    {
        builder.ToTable("LOV_MASTER");

        builder.HasKey(e => e.LovId);

        builder.Property(e => e.LovId)
            .HasColumnName("LOV_ID")
            .HasColumnType("char(3)")
            .IsRequired();

        builder.Property(e => e.LovType)
            .HasColumnName("LOV_TYPE")
            .HasColumnType("char(3)");

        builder.Property(e => e.LovName)
            .HasColumnName("LOV_NAME")
            .HasColumnType("varchar(200)");

        builder.Ignore(e => e.DomainEvents);
    }
}
