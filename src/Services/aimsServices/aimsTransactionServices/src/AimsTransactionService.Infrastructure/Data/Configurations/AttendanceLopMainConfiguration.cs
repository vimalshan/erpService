using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AimsTransactionService.Domain.Entities;

namespace AimsTransactionService.Infrastructure.Data.Configurations;

public class AttendanceLopMainConfiguration : IEntityTypeConfiguration<AttendanceLopMain>
{
    public void Configure(EntityTypeBuilder<AttendanceLopMain> builder)
    {
        builder.ToTable("ATTENDANCE_LOPMAIN");

        builder.HasKey(l => l.Id);
        builder.Property(l => l.Id).HasColumnName("ALM_SYSID").ValueGeneratedNever();

        builder.Property(l => l.EmployeeSysId)
            .HasColumnName("ALM_EMPSYSID")
            .IsRequired();

        builder.Property(l => l.MonthStart)
            .HasColumnName("ALM_MONTHSTART")
            .HasColumnType("datetime2(3)")
            .IsRequired();

        builder.Property(l => l.MonthEnd)
            .HasColumnName("ALM_MONTHEND")
            .HasColumnType("datetime2(3)")
            .IsRequired();

        builder.Property(l => l.CalendarDays)
            .HasColumnName("ALM_CALENDARDAYS")
            .IsRequired();

        builder.Property(l => l.CreatedBy)
            .HasColumnName("ALM_CREATEDBY")
            .IsRequired();

        builder.Property(l => l.CreatedOn)
            .HasColumnName("ALM_CREATEDON")
            .HasColumnType("datetime2(3)")
            .IsRequired();

        builder.Ignore(l => l.DomainEvents);
        builder.Ignore(l => l.Details);

        builder.HasIndex(l => l.EmployeeSysId).HasDatabaseName("IX_ATTENDANCE_LOPMAIN_EMPSYSID");
    }
}
