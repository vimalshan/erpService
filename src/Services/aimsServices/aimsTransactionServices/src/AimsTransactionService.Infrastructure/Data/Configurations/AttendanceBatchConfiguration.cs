using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AimsTransactionService.Domain.Aggregates;
using AimsTransactionService.Domain.Enums;

namespace AimsTransactionService.Infrastructure.Data.Configurations;

public class AttendanceBatchConfiguration : IEntityTypeConfiguration<AttendanceBatchAggregate>
{
    public void Configure(EntityTypeBuilder<AttendanceBatchAggregate> builder)
    {
        builder.ToTable("ATTENDANCE_BATCH");

        builder.HasKey(b => b.Id);
        builder.Property(b => b.Id).HasColumnName("ATB_SYSID").ValueGeneratedNever();

        builder.Property(b => b.MonthStart)
            .HasColumnName("ATB_MONTHSTART")
            .HasColumnType("datetime2(3)")
            .IsRequired();

        builder.Property(b => b.MonthEnd)
            .HasColumnName("ATB_MONTHEND")
            .HasColumnType("datetime2(3)")
            .IsRequired();

        builder.Property(b => b.Status)
            .HasColumnName("ATB_STATUS")
            .HasMaxLength(1)
            .IsRequired()
            .HasConversion(
                v => ((char)(int)v).ToString(),
                s => (BatchStatus)s[0]);

        builder.Property(b => b.CreatedBy)
            .HasColumnName("ATB_CREATEDBY")
            .IsRequired();

        builder.Property(b => b.CreatedOn)
            .HasColumnName("ATB_CREATEDON")
            .HasColumnType("datetime2(3)")
            .IsRequired();

        builder.Ignore(b => b.DomainEvents);
        builder.Ignore(b => b.LopRecords);

        builder.HasIndex(b => b.Status).HasDatabaseName("IX_ATTENDANCE_BATCH_STATUS");
    }
}
