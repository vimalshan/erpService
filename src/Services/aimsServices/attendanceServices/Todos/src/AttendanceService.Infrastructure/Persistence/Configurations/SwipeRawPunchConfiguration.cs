using AttendanceService.Domain.Entities;
using AttendanceService.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AttendanceService.Infrastructure.Persistence.Configurations;

public class SwipeRawPunchConfiguration : IEntityTypeConfiguration<SwipeRawPunch>
{
    public void Configure(EntityTypeBuilder<SwipeRawPunch> builder)
    {
        builder.ToTable("SWIPE_RAWPUNCH");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("SWIPE_ID").ValueGeneratedNever();
        builder.Property(x => x.SwipeEmpSysId).HasColumnName("SWIPE_EMPSYSID").IsRequired();
        builder.Property(x => x.SwipePunchTime).HasColumnName("SWIPE_PUNCHTIME").IsRequired();
        builder.Property(x => x.SwipeGateNo).HasColumnName("SWIPE_GATENO").HasMaxLength(10).IsRequired();
        builder.Property(x => x.SwipePunchStatus)
            .HasColumnName("SWIPE_PUNCHSTATUS")
            .HasMaxLength(1)
            .IsRequired()
            .HasConversion(v => v.Value, v => PunchStatus.From(v));
        builder.Property(x => x.SwipePullStatus).HasColumnName("SWIPE_PULLSTATUS").HasMaxLength(1);
        builder.Property(x => x.SwipeVerified).HasColumnName("SWIPE_VERIFIED").HasMaxLength(1);
        builder.Property(x => x.SwipeLastModifiedBy).HasColumnName("SWIPE_LASTMODIFIEDBY");
        builder.Property(x => x.SwipeLastModifiedOn).HasColumnName("SWIPE_LASTMODIFIEDON");

        builder.HasIndex(x => x.SwipeEmpSysId).HasDatabaseName("IX_SWIPE_EMPSYSID");
        builder.HasIndex(x => x.SwipePunchTime).HasDatabaseName("IX_SWIPE_PUNCHTIME");

        builder.Ignore(x => x.DomainEvents);
    }
}
