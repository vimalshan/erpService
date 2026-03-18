using MedicineManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedicineManagement.Infrastructure.Persistence.Configurations;

public class DoctorAttendantConfiguration : IEntityTypeConfiguration<DoctorAttendant>
{
    public void Configure(EntityTypeBuilder<DoctorAttendant> builder)
    {
        builder.ToTable("DOCATTEND_MAST");
        builder.HasKey(e => e.SystemId);
        builder.Property(e => e.SystemId).HasColumnName("DM_SYSID").ValueGeneratedOnAdd();
        builder.Property(e => e.Code).HasColumnName("DM_COD").HasMaxLength(20);
        builder.Property(e => e.Flag).HasColumnName("DM_FLAG").HasColumnType("CHAR(1)");
        builder.Property(e => e.Name).HasColumnName("DM_NAME").HasMaxLength(30);
        builder.Ignore(e => e.DomainEvents);
    }
}
