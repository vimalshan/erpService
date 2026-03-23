using AttendanceService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AttendanceService.Infrastructure.Persistence.Configurations;

public class AttendanceGraceAdjustConfiguration : IEntityTypeConfiguration<AttendanceGraceAdjust>
{
    public void Configure(EntityTypeBuilder<AttendanceGraceAdjust> builder)
    {
        builder.ToTable("ATTENDANCE_GRACEADJUST");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("GRACE_ID").ValueGeneratedNever();
        builder.Property(x => x.GraceEmpSysId).HasColumnName("GRACE_EMPSYSID").IsRequired();
        builder.Property(x => x.GraceDate).HasColumnName("GRACE_DATE").IsRequired();
        builder.Property(x => x.GraceMinutes).HasColumnName("GRACE_MINUTES").IsRequired();
        builder.Property(x => x.GraceLastModifiedBy).HasColumnName("GRACE_LASTMODIFIEDBY").IsRequired();
        builder.Property(x => x.GraceLastModifiedOn).HasColumnName("GRACE_LASTMODIFIEDON").IsRequired();

        builder.Ignore(x => x.DomainEvents);
    }
}
