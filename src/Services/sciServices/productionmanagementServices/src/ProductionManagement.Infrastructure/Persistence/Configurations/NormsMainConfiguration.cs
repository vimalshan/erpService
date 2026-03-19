using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductionManagement.Domain.Entities;

namespace ProductionManagement.Infrastructure.Persistence.Configurations;

public class NormsMainConfiguration : IEntityTypeConfiguration<NormsMain>
{
    public void Configure(EntityTypeBuilder<NormsMain> builder)
    {
        builder.ToTable("NORMS_MAIN");
        builder.HasKey(e => e.NormNo);

        builder.Property(e => e.NormNo)
            .HasColumnName("NORM_NO")
            .ValueGeneratedNever();

        builder.Property(e => e.NormEffDate)
            .HasColumnName("NORM_EFF_DATE")
            .HasColumnType("datetime2(3)")
            .IsRequired();

        builder.Property(e => e.NormClsDate)
            .HasColumnName("NORM_CLS_DATE")
            .HasColumnType("datetime2(3)");

        builder.HasMany(e => e.NormsMasters)
            .WithOne(e => e.NormsMain)
            .HasForeignKey(e => e.NormNo);

        builder.Ignore(e => e.DomainEvents);
    }
}
