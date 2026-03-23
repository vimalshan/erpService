using AttendanceService.Domain.Entities;
using AttendanceService.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AttendanceService.Infrastructure.Persistence.Configurations;

public class AttendanceBatchConfiguration : IEntityTypeConfiguration<AttendanceBatch>
{
    public void Configure(EntityTypeBuilder<AttendanceBatch> builder)
    {
        builder.ToTable("ATTENDANCE_BATCH");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("BATCH_ID").ValueGeneratedNever();
        builder.Property(x => x.BatchMonthFrom).HasColumnName("BATCH_MONTHFROM").IsRequired();
        builder.Property(x => x.BatchMonthTo).HasColumnName("BATCH_MONTHTO").IsRequired();
        builder.Property(x => x.BatchYearFrom).HasColumnName("BATCH_YEARFROM").IsRequired();
        builder.Property(x => x.BatchYearEnd).HasColumnName("BATCH_YEAREND").IsRequired();
        builder.Property(x => x.BatchStatus)
            .HasColumnName("BATCH_STATUS")
            .HasMaxLength(1)
            .IsRequired()
            .HasConversion(v => v.Value, v => BatchStatus.From(v));
        builder.Property(x => x.BatchCreatedBy).HasColumnName("BATCH_CREATEDBY").IsRequired();
        builder.Property(x => x.BatchCreatedOn).HasColumnName("BATCH_CREATEDON").IsRequired();
        builder.Property(x => x.BatchLastModifiedBy).HasColumnName("BATCH_LASTMODIFIEDBY").IsRequired();
        builder.Property(x => x.BatchLastModifiedOn).HasColumnName("BATCH_LASTMODIFIEDON").IsRequired();

        builder.HasIndex(x => x.BatchStatus).HasDatabaseName("IX_ATTENDANCE_BATCH_STATUS");
        builder.Ignore(x => x.DomainEvents);
        builder.Ignore(x => x.Summaries);
        builder.Ignore(x => x.LopRecords);
    }
}
