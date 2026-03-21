using EnergyService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EnergyService.Infrastructure.Persistence.Configurations;

public class EcProcessAccessConfiguration : IEntityTypeConfiguration<EcProcessAccess>
{
    public void Configure(EntityTypeBuilder<EcProcessAccess> builder)
    {
        builder.ToTable("EC_PROCESS_ACCESS");
        builder.HasKey(e => e.PaId);

        builder.Property(e => e.PaId).HasColumnName("PA_ID").ValueGeneratedOnAdd();
        builder.Property(e => e.PaProcessId).HasColumnName("PA_PROCESS_ID").IsRequired();
        builder.Property(e => e.PaEmpSysId).HasColumnName("PA_EMP_SYSID").IsRequired();
        builder.Property(e => e.PaStartDate).HasColumnName("PA_START_DATE").HasPrecision(3).IsRequired();
        builder.Property(e => e.PaCloseDate).HasColumnName("PA_CLOSE_DATE").HasPrecision(3);
        builder.Property(e => e.PaLastModifiedBy).HasColumnName("PA_LAST_MODIFIEDBY").IsRequired();
        builder.Property(e => e.PaLastModifiedOn).HasColumnName("PA_LAST_MODIFIEDON").HasMaxLength(30).IsRequired();

        builder.Ignore(e => e.DomainEvents);
    }
}
