using AttendanceService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AttendanceService.Infrastructure.Persistence.Configurations;

public class AttendanceSummaryConfiguration : IEntityTypeConfiguration<AttendanceSummary>
{
    public void Configure(EntityTypeBuilder<AttendanceSummary> builder)
    {
        builder.ToTable("ATTENDANCE_SUMMARY");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("SUMMARY_ID").ValueGeneratedNever();
        builder.Property(x => x.SummaryEmpSysId).HasColumnName("SUMMARY_EMPSYSID").IsRequired();
        builder.Property(x => x.SummaryBatchId).HasColumnName("SUMMARY_BATCHID").IsRequired();
        builder.Property(x => x.SummaryAttType).HasColumnName("SUMMARY_ATTTYPE").HasMaxLength(10).IsRequired();
        builder.Property(x => x.SummaryDays).HasColumnName("SUMMARY_DAYS").IsRequired();
        builder.Property(x => x.SummaryLastModifiedBy).HasColumnName("SUMMARY_LASTMODIFIEDBY").IsRequired();
        builder.Property(x => x.SummaryLastModifiedOn).HasColumnName("SUMMARY_LASTMODIFIEDON").IsRequired();

        builder.HasOne(x => x.Batch).WithMany().HasForeignKey(x => x.SummaryBatchId)
            .HasConstraintName("FK_SUMMARY_BATCHID");
        builder.Ignore(x => x.DomainEvents);
    }
}
