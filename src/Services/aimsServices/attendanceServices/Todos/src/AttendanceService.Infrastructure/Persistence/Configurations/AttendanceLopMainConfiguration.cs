using AttendanceService.Domain.Entities;
using AttendanceService.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AttendanceService.Infrastructure.Persistence.Configurations;

public class AttendanceLopMainConfiguration : IEntityTypeConfiguration<AttendanceLopMain>
{
    public void Configure(EntityTypeBuilder<AttendanceLopMain> builder)
    {
        builder.ToTable("ATTENDANCE_LOPMAIN");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("LOP_ID").ValueGeneratedNever();
        builder.Property(x => x.LopEmpSysId).HasColumnName("LOP_EMPSYSID").IsRequired();
        builder.Property(x => x.LopBatchId).HasColumnName("LOP_BATCHID").IsRequired();
        builder.Property(x => x.LopDays).HasColumnName("LOP_DAYS").HasColumnType("decimal(5,2)").IsRequired();
        builder.Property(x => x.LopType)
            .HasColumnName("LOP_TYPE")
            .HasMaxLength(1)
            .IsRequired()
            .HasConversion(v => v.Value, v => LopType.From(v));
        builder.Property(x => x.LopLastModifiedBy).HasColumnName("LOP_LASTMODIFIEDBY").IsRequired();
        builder.Property(x => x.LopLastModifiedOn).HasColumnName("LOP_LASTMODIFIEDON").IsRequired();

        builder.HasOne(x => x.Batch).WithMany().HasForeignKey(x => x.LopBatchId)
            .HasConstraintName("FK_LOP_BATCHID");
        builder.HasIndex(x => x.LopEmpSysId).HasDatabaseName("IX_LOP_EMPSYSID");
        builder.Ignore(x => x.DomainEvents);
    }
}
