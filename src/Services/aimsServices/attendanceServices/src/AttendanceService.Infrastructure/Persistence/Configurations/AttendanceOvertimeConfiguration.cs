using AttendanceService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AttendanceService.Infrastructure.Persistence.Configurations;

public class AttendanceOvertimeConfiguration : IEntityTypeConfiguration<AttendanceOvertime>
{
    public void Configure(EntityTypeBuilder<AttendanceOvertime> builder)
    {
        builder.ToTable("ATTENDANCE_OVERTIME");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("OT_ID").ValueGeneratedNever();
        builder.Property(x => x.OtEmpSysId).HasColumnName("OT_EMPSYSID").IsRequired();
        builder.Property(x => x.OtDate).HasColumnName("OT_DATE").IsRequired();
        builder.Property(x => x.OtHours).HasColumnName("OT_HOURS").HasColumnType("decimal(5,2)").IsRequired();
        builder.Property(x => x.OtType).HasColumnName("OT_TYPE").HasMaxLength(20).IsRequired();
        builder.Property(x => x.OtApproved).HasColumnName("OT_APPROVED").HasMaxLength(1).IsRequired();
        builder.Property(x => x.OtLastModifiedBy).HasColumnName("OT_LASTMODIFIEDBY").IsRequired();
        builder.Property(x => x.OtLastModifiedOn).HasColumnName("OT_LASTMODIFIEDON").IsRequired();

        builder.HasIndex(x => x.OtEmpSysId).HasDatabaseName("IX_OT_EMPSYSID");
        builder.Ignore(x => x.DomainEvents);
    }
}
