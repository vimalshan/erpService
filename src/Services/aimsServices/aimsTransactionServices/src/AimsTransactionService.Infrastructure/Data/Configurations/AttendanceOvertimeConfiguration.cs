using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AimsTransactionService.Domain.Entities;

namespace AimsTransactionService.Infrastructure.Data.Configurations;

public class AttendanceOvertimeConfiguration : IEntityTypeConfiguration<AttendanceOvertime>
{
    public void Configure(EntityTypeBuilder<AttendanceOvertime> builder)
    {
        builder.ToTable("ATTENDANCE_OVERTIME");

        builder.HasKey(o => o.Id);
        builder.Property(o => o.Id).HasColumnName("ATO_SYSID").ValueGeneratedNever();

        builder.Property(o => o.EmployeeSysId)
            .HasColumnName("ATO_EMPSYSID")
            .IsRequired();

        builder.Property(o => o.OvertimeDate)
            .HasColumnName("ATO_OTDATE")
            .HasColumnType("datetime2(3)")
            .IsRequired();

        builder.Property(o => o.OvertimeHours)
            .HasColumnName("ATO_OTHOURS")
            .HasColumnType("decimal(5,2)")
            .IsRequired();

        builder.Property(o => o.Status)
            .HasColumnName("ATO_STATUS")
            .HasMaxLength(1)
            .IsRequired();

        builder.Property(o => o.CreatedBy)
            .HasColumnName("ATO_CREATEDBY")
            .IsRequired();

        builder.Property(o => o.CreatedOn)
            .HasColumnName("ATO_CREATEDON")
            .HasColumnType("datetime2(3)")
            .IsRequired();

        builder.Ignore(o => o.DomainEvents);

        builder.HasIndex(o => o.EmployeeSysId).HasDatabaseName("IX_ATTENDANCE_OVERTIME_EMPSYSID");
    }
}
