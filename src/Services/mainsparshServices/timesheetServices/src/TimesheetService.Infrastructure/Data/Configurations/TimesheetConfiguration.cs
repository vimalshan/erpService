using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TimesheetService.Domain.Entities;
using TimesheetService.Domain.ValueObjects;

namespace TimesheetService.Infrastructure.Data.Configurations;

public sealed class TimesheetConfiguration : IEntityTypeConfiguration<Timesheet>
{
    public void Configure(EntityTypeBuilder<Timesheet> builder)
    {
        builder.ToTable("TSE_TIMESHEET");

        builder.HasKey(t => t.TimesheetId);
        builder.Property(t => t.TimesheetId)
               .HasColumnName("TIMESHEET_ID")
               .UseIdentityColumn();

        builder.Property(t => t.EmployeeId)
               .HasColumnName("EMP_SYSID")
               .IsRequired();

        builder.Property(t => t.TimesheetDate)
               .HasColumnName("TIMESHEET_DATE")
               .HasColumnType("date")
               .IsRequired();

        builder.Property(t => t.WorkDate)
               .HasColumnName("WORK_DATE")
               .HasColumnType("date")
               .IsRequired();

        builder.Property(t => t.StartTime)
               .HasColumnName("START_TIME")
               .HasColumnType("time");

        builder.Property(t => t.EndTime)
               .HasColumnName("END_TIME")
               .HasColumnType("time");

        builder.Property(t => t.TotalHours)
               .HasColumnName("TOTAL_HOURS")
               .HasColumnType("decimal(5,2)");

        builder.Property(t => t.ProjectId)
               .HasColumnName("PROJECT_ID");

        builder.Property(t => t.TaskId)
               .HasColumnName("TASK_ID");

        builder.Property(t => t.WorkDescription)
               .HasColumnName("WORK_DESCRIPTION")
               .HasColumnType("nvarchar(max)");

        builder.Property(t => t.RecordedDate)
               .HasColumnName("RECORDED_DATE")
               .HasColumnType("datetime2(3)")
               .IsRequired();

        // Value Object: TimesheetStatus
        builder.Property(t => t.Status)
               .HasColumnName("TIMESHEET_STATUS")
               .HasMaxLength(20)
               .HasDefaultValue(TimesheetStatus.Draft)
               .HasConversion(v => v.Value, v => TimesheetStatus.From(v));

        // Value Object: ApprovalStatus
        builder.Property(t => t.ApprovalStatus)
               .HasColumnName("APPROVAL_STATUS")
               .HasMaxLength(20)
               .HasDefaultValue(ApprovalStatus.Pending)
               .HasConversion(v => v.Value, v => ApprovalStatus.From(v));

        builder.Property(t => t.ApprovedBy)
               .HasColumnName("APPROVED_BY");

        builder.Property(t => t.ApprovedOn)
               .HasColumnName("APPROVED_ON")
               .HasColumnType("datetime2(3)");

        builder.Property(t => t.RejectionReason)
               .HasColumnName("REJECTION_REASON")
               .HasColumnType("nvarchar(max)");

        builder.Property(t => t.CreatedBy)
               .HasColumnName("CREATED_BY")
               .IsRequired();

        builder.Property(t => t.CreatedOn)
               .HasColumnName("CREATED_ON")
               .HasColumnType("datetime2(3)")
               .IsRequired()
               .HasDefaultValueSql("GETDATE()");

        builder.Property(t => t.UpdatedBy)
               .HasColumnName("UPDATED_BY");

        builder.Property(t => t.UpdatedOn)
               .HasColumnName("UPDATED_ON")
               .HasColumnType("datetime2(3)");

        // Indexes
        builder.HasIndex(t => t.EmployeeId).HasDatabaseName("IX_TSE_TIMESHEET_EMP_SYSID");
        builder.HasIndex(t => t.WorkDate).HasDatabaseName("IX_TSE_TIMESHEET_WORK_DATE");
        builder.HasIndex(t => t.RecordedDate).HasDatabaseName("IX_TSE_TIMESHEET_RECORDED_DATE");
        builder.HasIndex(t => t.Status).HasDatabaseName("IX_TSE_TIMESHEET_STATUS");
        builder.HasIndex(t => t.ApprovalStatus).HasDatabaseName("IX_TSE_TIMESHEET_APPROVAL");
        builder.HasIndex(t => t.ProjectId).HasDatabaseName("IX_TSE_TIMESHEET_PROJECT");
        builder.HasIndex(t => t.TaskId).HasDatabaseName("IX_TSE_TIMESHEET_TASK");

        // Ignore domain events
        builder.Ignore(t => t.DomainEvents);
    }
}
