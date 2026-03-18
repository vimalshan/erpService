using AttendanceService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AttendanceService.Infrastructure.Persistence.Configurations;

public class AttendanceLeaveAdjustConfiguration : IEntityTypeConfiguration<AttendanceLeaveAdjust>
{
    public void Configure(EntityTypeBuilder<AttendanceLeaveAdjust> builder)
    {
        builder.ToTable("ATTENDANCE_LEAVEADJUST");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("LEAVEADJ_ID").ValueGeneratedNever();
        builder.Property(x => x.LeaveAdjEmpSysId).HasColumnName("LEAVEADJ_EMPSYSID").IsRequired();
        builder.Property(x => x.LeaveAdjDate).HasColumnName("LEAVEADJ_DATE").IsRequired();
        builder.Property(x => x.LeaveAdjType).HasColumnName("LEAVEADJ_TYPE").HasMaxLength(1).IsRequired();
        builder.Property(x => x.LeaveAdjLastModifiedBy).HasColumnName("LEAVEADJ_LASTMODIFIEDBY").IsRequired();
        builder.Property(x => x.LeaveAdjLastModifiedOn).HasColumnName("LEAVEADJ_LASTMODIFIEDON").IsRequired();

        builder.Ignore(x => x.DomainEvents);
    }
}
