using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AimsTransactionService.Domain.Entities;

namespace AimsTransactionService.Infrastructure.Data.Configurations;

public class AttendanceSummaryConfiguration : IEntityTypeConfiguration<AttendanceSummary>
{
    public void Configure(EntityTypeBuilder<AttendanceSummary> builder)
    {
        builder.ToTable("ATTENDANCE_SUMMARY");

        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).HasColumnName("ATS_SYSID").ValueGeneratedNever();

        builder.Property(s => s.EmployeeSysId)
            .HasColumnName("ATS_EMPSYSID")
            .IsRequired();

        builder.Property(s => s.MonthStart)
            .HasColumnName("ATS_MONTHSTART")
            .HasColumnType("datetime2(3)")
            .IsRequired();

        builder.Property(s => s.MonthEnd)
            .HasColumnName("ATS_MONTHEND")
            .HasColumnType("datetime2(3)")
            .IsRequired();

        builder.Property(s => s.WorkingDays)
            .HasColumnName("ATS_WORKINGDAYS")
            .IsRequired();

        builder.Property(s => s.PresentDays)
            .HasColumnName("ATS_PRESENTDAYS")
            .IsRequired();

        builder.Property(s => s.AbsentDays)
            .HasColumnName("ATS_ABSENTDAYS")
            .IsRequired();

        builder.Property(s => s.OvertimeHours)
            .HasColumnName("ATS_OTHOURS")
            .HasColumnType("decimal(7,2)")
            .IsRequired();

        builder.Property(s => s.LopDays)
            .HasColumnName("ATS_LOPDAYS")
            .HasColumnType("decimal(5,2)")
            .IsRequired();

        builder.Property(s => s.CreatedBy)
            .HasColumnName("ATS_CREATEDBY")
            .IsRequired();

        builder.Property(s => s.CreatedOn)
            .HasColumnName("ATS_CREATEDON")
            .HasColumnType("datetime2(3)")
            .IsRequired();

        builder.Ignore(s => s.DomainEvents);

        builder.HasIndex(s => s.EmployeeSysId).HasDatabaseName("IX_ATTENDANCE_SUMMARY_EMPSYSID");
    }
}
