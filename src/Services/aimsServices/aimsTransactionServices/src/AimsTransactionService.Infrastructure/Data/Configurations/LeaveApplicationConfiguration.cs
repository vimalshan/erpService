using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AimsTransactionService.Domain.Aggregates;
using AimsTransactionService.Domain.Enums;

namespace AimsTransactionService.Infrastructure.Data.Configurations;

public class LeaveApplicationConfiguration : IEntityTypeConfiguration<LeaveApplicationAggregate>
{
    public void Configure(EntityTypeBuilder<LeaveApplicationAggregate> builder)
    {
        builder.ToTable("LEAVE_DETAILS");

        builder.HasKey(l => l.Id);
        builder.Property(l => l.Id).HasColumnName("LVD_SYSID").ValueGeneratedNever();

        builder.Property(l => l.EmployeeSysId)
            .HasColumnName("LVD_EMPSYSID")
            .IsRequired();

        builder.Property(l => l.LeaveId)
            .HasColumnName("LVD_LEAVEID")
            .IsRequired();

        builder.Property(l => l.FromDate)
            .HasColumnName("LVD_FROMDATE")
            .HasColumnType("datetime2(3)")
            .IsRequired();

        builder.Property(l => l.ToDate)
            .HasColumnName("LVD_TODATE")
            .HasColumnType("datetime2(3)")
            .IsRequired();

        builder.Property(l => l.LeaveDays)
            .HasColumnName("LVD_LEAVEDAYS")
            .HasColumnType("decimal(5,2)")
            .IsRequired();

        builder.Property(l => l.Reason)
            .HasColumnName("LVD_REASON")
            .HasMaxLength(500);

        builder.Property(l => l.Status)
            .HasColumnName("LVD_STATUS")
            .HasMaxLength(1)
            .IsRequired()
            .HasConversion(
                v => ((char)(int)v).ToString(),
                s => (LeaveStatus)s[0]);

        builder.Property(l => l.AppliedOn)
            .HasColumnName("LVD_APPLIEDON")
            .HasColumnType("datetime2(3)")
            .IsRequired();

        builder.Property(l => l.AppliedBy)
            .HasColumnName("LVD_APPLIEDBY")
            .IsRequired();

        builder.Property(l => l.ApprovedOn)
            .HasColumnName("LVD_APPROVEDON")
            .HasColumnType("datetime2(3)");

        builder.Property(l => l.ApprovedBy)
            .HasColumnName("LVD_APPROVEDBY");

        builder.Property(l => l.Remarks)
            .HasColumnName("LVD_REMARKS")
            .HasMaxLength(500);

        builder.Ignore(l => l.DomainEvents);
        builder.Ignore(l => l.Approvals);

        builder.HasIndex(l => l.EmployeeSysId).HasDatabaseName("IX_LEAVE_DETAILS_EMPSYSID");
        builder.HasIndex(l => l.Status).HasDatabaseName("IX_LEAVE_DETAILS_STATUS");
    }
}
