using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrustService.Domain.Entities;

namespace TrustService.Infrastructure.Persistence.Configurations;

public class TrustUnitConfiguration : IEntityTypeConfiguration<TrustUnit>
{
    public void Configure(EntityTypeBuilder<TrustUnit> builder)
    {
        builder.ToTable("TRUST_UNITS");

        builder.HasKey(u => u.UnitId);
        builder.Property(u => u.UnitId).HasColumnName("UNIT_ID").ValueGeneratedOnAdd();

        builder.Property(u => u.TrustCode).HasColumnName("TRUST_CODE").HasMaxLength(3).IsFixedLength().IsRequired();
        builder.Property(u => u.UnitCode).HasColumnName("UNIT_CODE").HasMaxLength(3).IsFixedLength().IsRequired();
        builder.Property(u => u.UnitName).HasColumnName("UNIT_NAME").HasMaxLength(100).IsRequired();
        builder.Property(u => u.UnitType).HasColumnName("UNIT_TYPE").HasMaxLength(50).IsRequired();
        builder.Property(u => u.AddressLine1).HasColumnName("ADDRESS_LINE1").HasMaxLength(200).IsRequired();
        builder.Property(u => u.AddressLine2).HasColumnName("ADDRESS_LINE2").HasMaxLength(200);
        builder.Property(u => u.City).HasColumnName("CITY").HasMaxLength(50).IsRequired();
        builder.Property(u => u.State).HasColumnName("STATE").HasMaxLength(50).IsRequired();
        builder.Property(u => u.UnitHeadSysId).HasColumnName("UNIT_HEAD_SYSID");
        builder.Property(u => u.EffDate).HasColumnName("EFF_DATE").HasPrecision(3).IsRequired();
        builder.Property(u => u.ClsDate).HasColumnName("CLS_DATE").HasPrecision(3);
        builder.Property(u => u.UnitStatus).HasColumnName("UNIT_STATUS").HasMaxLength(1).IsFixedLength().HasDefaultValue("A");

        builder.HasIndex(u => u.UnitCode).IsUnique();
        builder.HasIndex(u => new { u.TrustCode, u.UnitCode, u.UnitStatus }).HasDatabaseName("IDX_TRUST_UNITS_CODE");

        builder.Ignore(u => u.DomainEvents);
    }
}
