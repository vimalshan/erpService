using AttendanceService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AttendanceService.Infrastructure.Persistence.Configurations;

public class AttendanceNightConfiguration : IEntityTypeConfiguration<AttendanceNight>
{
    public void Configure(EntityTypeBuilder<AttendanceNight> builder)
    {
        builder.ToTable("ATTENDANCE_NIGHT");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("NIGHT_ID").ValueGeneratedNever();
        builder.Property(x => x.NightEmpSysId).HasColumnName("NIGHT_EMPSYSID").IsRequired();
        builder.Property(x => x.NightDate).HasColumnName("NIGHT_DATE").IsRequired();
        builder.Property(x => x.NightNightType).HasColumnName("NIGHT_NIGHTTYPE").HasMaxLength(1).IsRequired();
        builder.Property(x => x.NightLastModifiedBy).HasColumnName("NIGHT_LASTMODIFIEDBY").IsRequired();
        builder.Property(x => x.NightLastModifiedOn).HasColumnName("NIGHT_LASTMODIFIEDON").IsRequired();

        builder.Ignore(x => x.DomainEvents);
    }
}
