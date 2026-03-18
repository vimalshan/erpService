using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrustService.Domain.Entities;

namespace TrustService.Infrastructure.Persistence.Configurations;

public class TrustConfigurationEntityConfiguration : IEntityTypeConfiguration<TrustConfiguration>
{
    public void Configure(EntityTypeBuilder<TrustConfiguration> builder)
    {
        builder.ToTable("TRUST_CONFIGURATION");

        builder.HasKey(c => c.ConfigId);
        builder.Property(c => c.ConfigId).HasColumnName("CONFIG_ID").ValueGeneratedOnAdd();

        builder.Property(c => c.TrustCode).HasColumnName("TRUST_CODE").HasMaxLength(3).IsFixedLength().IsRequired();
        builder.Property(c => c.ConfigName).HasColumnName("CONFIG_NAME").HasMaxLength(100).IsRequired();
        builder.Property(c => c.ConfigValue).HasColumnName("CONFIG_VALUE").HasMaxLength(500).IsRequired();
        builder.Property(c => c.ConfigCategory).HasColumnName("CONFIG_CATEGORY").HasMaxLength(50).IsRequired();
        builder.Property(c => c.EffDate).HasColumnName("EFF_DATE").HasPrecision(3).IsRequired();
        builder.Property(c => c.ClsDate).HasColumnName("CLS_DATE").HasPrecision(3);

        builder.Ignore(c => c.DomainEvents);
    }
}
