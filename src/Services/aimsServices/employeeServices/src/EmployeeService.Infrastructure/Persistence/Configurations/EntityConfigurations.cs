using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EmployeeService.Domain.Entities;

namespace EmployeeService.Infrastructure.Persistence.Configurations;

public sealed class EmployeeTimeInfoConfiguration : IEntityTypeConfiguration<EmployeeTimeInfo>
{
    public void Configure(EntityTypeBuilder<EmployeeTimeInfo> builder)
    {
        builder.ToTable("EMP_TIMEINFO");
        builder.HasKey(x => x.TimeInfoId);
        builder.Property(x => x.TimeInfoId).HasColumnName("TIME_INFOID").ValueGeneratedNever();
        builder.Property(x => x.LastModifiedBy).HasColumnName("TIME_LASTMODIFIEDBY").IsRequired();
        builder.Property(x => x.LastModifiedOn).HasColumnName("TIME_LASTMODIFIEDON").IsRequired();

        builder.Property(x => x.EmpSysId)
            .HasColumnName("TIME_EMPSYSID")
            .HasConversion(v => v.Value, v => Domain.ValueObjects.EmployeeId.Of(v))
            .IsRequired();

        builder.Property(x => x.EmpAttFlag)
            .HasColumnName("TIME_EMPATTFLAG")
            .HasMaxLength(1)
            .HasConversion(v => v.Value.ToString(), v => Domain.ValueObjects.AttendanceFlag.Of(v[0]))
            .IsRequired();

        builder.HasIndex(x => x.EmpSysId).HasDatabaseName("IX_EMP_TIMEINFO_EMPSYSID");
        builder.Ignore(x => x.DomainEvents);
    }
}

public sealed class EmployeeApproverConfiguration : IEntityTypeConfiguration<EmployeeApprover>
{
    public void Configure(EntityTypeBuilder<EmployeeApprover> builder)
    {
        builder.ToTable("EMPLOYEE_APPROVER");
        builder.HasKey(x => x.ApproverId);
        builder.Property(x => x.ApproverId).HasColumnName("APPROVER_ID").ValueGeneratedNever();
        builder.Property(x => x.LastModifiedBy).HasColumnName("APPROVER_MODIFIEDBY").IsRequired();
        builder.Property(x => x.LastModifiedOn).HasColumnName("APPROVER_MODIFIEDON").IsRequired();
        builder.Property(x => x.ApproverSysId).HasColumnName("APPROVER_SYSID").IsRequired();
        builder.Property(x => x.EffDate).HasColumnName("APPROVER_EFFDATE").IsRequired();

        builder.Property(x => x.EmpSysId)
            .HasColumnName("APPROVER_EMPSYSID")
            .HasMaxLength(255)
            .HasConversion(v => v.Value.ToString(), v => Domain.ValueObjects.EmployeeId.Of(long.Parse(v)))
            .IsRequired();

        builder.Property(x => x.Level)
            .HasColumnName("APPROVER_LEVEL")
            .HasConversion(v => v.Value, v => Domain.ValueObjects.ApproverLevel.Of(v))
            .IsRequired();

        builder.HasIndex(x => x.EmpSysId).HasDatabaseName("IX_EMPLOYEE_APPROVER_EMPSYSID");
        builder.Ignore(x => x.DomainEvents);
    }
}

public sealed class EmployeeApprovalMailConfiguration : IEntityTypeConfiguration<EmployeeApprovalMail>
{
    public void Configure(EntityTypeBuilder<EmployeeApprovalMail> builder)
    {
        builder.ToTable("EMPLOYEE_APPROVALMAIL");
        builder.HasKey(x => x.AppMailId);
        builder.Property(x => x.AppMailId).HasColumnName("APPMAIL_ID").ValueGeneratedNever();
        builder.Property(x => x.AppMailSysId).HasColumnName("APPMAIL_SYSID").IsRequired();
        builder.Property(x => x.EffDate).HasColumnName("APPMAIL_EFFDATE").IsRequired();
        builder.Property(x => x.LastModifiedBy).HasColumnName("APPMAIL_MODIFIEDBY");
        builder.Property(x => x.LastModifiedOn).HasColumnName("APPMAIL_MODIFIEDON");

        builder.Property(x => x.EmpSysId)
            .HasColumnName("APPMAIL_EMPSYSID")
            .HasMaxLength(255)
            .HasConversion(v => v.Value.ToString(), v => Domain.ValueObjects.EmployeeId.Of(long.Parse(v)))
            .IsRequired();

        builder.Ignore(x => x.DomainEvents);
    }
}

public sealed class EmployeeCalendarConfiguration : IEntityTypeConfiguration<EmployeeCalendar>
{
    public void Configure(EntityTypeBuilder<EmployeeCalendar> builder)
    {
        builder.ToTable("EMP_CALENDAR");
        builder.HasKey(x => x.EmpCalId);
        builder.Property(x => x.EmpCalId).HasColumnName("EMPCAL_ID").ValueGeneratedNever();
        builder.Property(x => x.CalendarId).HasColumnName("EMPCAL_CALENDARID").IsRequired();
        builder.Property(x => x.SwipeId).HasColumnName("EMPCAL_SWIPEID");
        builder.Property(x => x.EffDate).HasColumnName("EMPCAL_EFFDATE").IsRequired();
        builder.Property(x => x.ClsDate).HasColumnName("EMPCAL_CLSDATE");
        builder.Property(x => x.LastModifiedBy).HasColumnName("EMPCAL_LASTMODIFIEDBY").IsRequired();
        builder.Property(x => x.LastModifiedOn).HasColumnName("EMPCAL_LASTMODIFIEDON").IsRequired();
        builder.Property(x => x.Status).HasColumnName("EMPCAL_STATUS").HasMaxLength(1);
        builder.Property(x => x.Transfer).HasColumnName("EMPCAL_TRANSFER");
        builder.Property(x => x.SettlementNo).HasColumnName("EMPCAL_SETTLEMENTNO");

        builder.Property(x => x.EmpSysId)
            .HasColumnName("EMPCAL_EMPSYSID")
            .HasConversion(v => v.Value, v => Domain.ValueObjects.EmployeeId.Of(v))
            .IsRequired();

        builder.HasIndex(x => x.EmpSysId).HasDatabaseName("IX_EMP_CALENDAR_EMPSYSID");
        builder.Ignore(x => x.DomainEvents);
    }
}

public sealed class EmployeePatternConfiguration : IEntityTypeConfiguration<EmployeePattern>
{
    public void Configure(EntityTypeBuilder<EmployeePattern> builder)
    {
        builder.ToTable("EMPLOYEE_PATTERN");
        builder.HasKey(x => x.EmpPatternId);
        builder.Property(x => x.EmpPatternId).HasColumnName("EMPPATTERN_ID").ValueGeneratedNever();
        builder.Property(x => x.PatternMastId).HasColumnName("EMPPATTERN_MASTID").IsRequired();
        builder.Property(x => x.EffDate).HasColumnName("EMPPATTERN_EFFDATE").IsRequired();
        builder.Property(x => x.ClsDate).HasColumnName("EMPPATTERN_CLSDATE");
        builder.Property(x => x.WeeklyOffDay).HasColumnName("EMPPATTERN_WEEKLYOFFDY").IsRequired();
        builder.Property(x => x.SubWeeklyDay).HasColumnName("EMPPATTERN_SUBWEEKLYDY");
        builder.Property(x => x.SubFrequency).HasColumnName("EMPPATTERN_SUBFRQ").HasMaxLength(5);
        builder.Property(x => x.LastModifiedBy).HasColumnName("EMPPATTERN_LASTMODIFIEDBY");
        builder.Property(x => x.LastModifiedOn).HasColumnName("EMPPATTERN_LASTMODIFIEDON");

        builder.Property(x => x.EmpSysId)
            .HasColumnName("EMPPATTERN_EMPSYSID")
            .HasConversion(v => v.Value, v => Domain.ValueObjects.EmployeeId.Of(v))
            .IsRequired();

        builder.HasIndex(x => x.EmpSysId).HasDatabaseName("IX_EMPLOYEE_PATTERN_EMPSYSID");
        builder.Ignore(x => x.DomainEvents);
    }
}

public sealed class EmployeeShiftConfiguration : IEntityTypeConfiguration<EmployeeShift>
{
    public void Configure(EntityTypeBuilder<EmployeeShift> builder)
    {
        builder.ToTable("EMPLOYEE_SHIFT");
        builder.HasKey(x => x.EmpShiftId);
        builder.Property(x => x.EmpShiftId).HasColumnName("EMPSHIFT_ID").ValueGeneratedNever();
        builder.Property(x => x.TimeUnitId).HasColumnName("EMPSHIFT_TIMEUNITID").IsRequired();
        builder.Property(x => x.ShiftCode).HasColumnName("EMPSHIFT_CODE").HasMaxLength(1).IsRequired();
        builder.Property(x => x.YearMonth).HasColumnName("EMPSHIFT_YEARMONTH").IsRequired();
        builder.Property(x => x.Day).HasColumnName("EMPSHIFT_DAY").IsRequired();
        builder.Property(x => x.ShiftDate).HasColumnName("EMPSHIFT_DATE").IsRequired();
        builder.Property(x => x.UpdatedBy).HasColumnName("EMPSHIFT_UPDATEDBY").IsRequired();
        builder.Property(x => x.UpdatedOn).HasColumnName("EMPSHIFT_UPDATEDON").IsRequired();

        builder.Property(x => x.EmpSysId)
            .HasColumnName("EMPSHIFT_EMPSYSID")
            .HasConversion(v => v.Value, v => Domain.ValueObjects.EmployeeId.Of(v))
            .IsRequired();

        builder.HasIndex(x => x.EmpSysId).HasDatabaseName("IX_EMPLOYEE_SHIFT_EMPSYSID");
        builder.Ignore(x => x.DomainEvents);
    }
}

public sealed class EmployeeShiftPatternConfiguration : IEntityTypeConfiguration<EmployeeShiftPattern>
{
    public void Configure(EntityTypeBuilder<EmployeeShiftPattern> builder)
    {
        builder.ToTable("EMPLOYEE_SHIFTPATTERN");
        builder.HasKey(x => x.EmpShiftId);
        builder.Property(x => x.EmpShiftId).HasColumnName("EMPSHIFT_ID").ValueGeneratedNever();
        builder.Property(x => x.TimeUnitId).HasColumnName("EMPSHIFT_TIMEUNITID");
        builder.Property(x => x.YearMonth).HasColumnName("EMPSHIFT_YEARMONTH");
        builder.Property(x => x.OrgPattern).HasColumnName("EMPSHIFT_ORGPATTERN").HasMaxLength(31);
        builder.Property(x => x.NewPattern).HasColumnName("EMPSHIFT_NEWPATTERN").HasMaxLength(31);
        builder.Property(x => x.LastModifiedBy).HasColumnName("EMPSHIFT_LASTMODIFIEDBY").IsRequired();
        builder.Property(x => x.LastModifiedOn).HasColumnName("EMPSHIFT_LASTMODIFIEDON").IsRequired();

        builder.Property(x => x.EmpSysId)
            .HasColumnName("EMPSHIFT_EMPSYSID")
            .HasConversion(v => v == null ? (long?)null : v.Value, v => v == null ? null : Domain.ValueObjects.EmployeeId.Of(v.Value));

        builder.Ignore(x => x.DomainEvents);
    }
}
