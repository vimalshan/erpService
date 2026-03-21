using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UnitService.Domain.Entities;
using UnitService.Domain.ValueObjects;

namespace UnitService.Infrastructure.Data.Configurations;

public class AccessMasterConfiguration : IEntityTypeConfiguration<AccessMaster>
{
    public void Configure(EntityTypeBuilder<AccessMaster> builder)
    {
        builder.ToTable("UM_ACCESS_MASTER");

        builder.HasKey(e => e.AccessId);
        builder.Property(e => e.AccessId).HasColumnName("UA_ID");
        builder.Property(e => e.UnitCode).HasColumnName("UA_UNIT_CODE").HasMaxLength(3).IsRequired()
            .HasConversion(v => v.Value, v => UnitCode.From(v));
        builder.Property(e => e.EmployeeSysId).HasColumnName("UA_EMP_SYSID").IsRequired();
        builder.Property(e => e.AccessType).HasColumnName("UA_ACCESS_TYPE").HasMaxLength(1).IsRequired()
            .HasConversion(v => v.Value, v => AccessType.From(v));
        builder.Property(e => e.StartDate).HasColumnName("UA_START_DATE").HasPrecision(3).IsRequired();
        builder.Property(e => e.CloseDate).HasColumnName("UA_CLOSE_DATE").HasMaxLength(255);
        builder.Property(e => e.LastModifiedBy).HasColumnName("UA_LAST_MODIFIEDBY").IsRequired();
        builder.Property(e => e.LastModifiedOn).HasColumnName("UA_LAST_MODIFIEDON").HasPrecision(3).IsRequired();
        builder.Property(e => e.Module).HasColumnName("UA_MODULE").HasMaxLength(5).IsRequired();
    }
}
