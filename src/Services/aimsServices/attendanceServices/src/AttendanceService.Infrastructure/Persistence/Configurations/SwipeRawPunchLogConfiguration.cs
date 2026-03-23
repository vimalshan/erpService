using AttendanceService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AttendanceService.Infrastructure.Persistence.Configurations;

public class SwipeRawPunchLogConfiguration : IEntityTypeConfiguration<SwipeRawPunchLog>
{
    public void Configure(EntityTypeBuilder<SwipeRawPunchLog> builder)
    {
        builder.ToTable("SWIPE_RAWPUNCH_LOG");
        builder.HasNoKey();
        builder.Property(x => x.Id).HasColumnName("SWIPE_ID");
        builder.Property(x => x.SwipeEmpSysId).HasColumnName("SWIPE_EMPSYSID").IsRequired();
        builder.Property(x => x.SwipePunchTime).HasColumnName("SWIPE_PUNCHTIME").IsRequired();
        builder.Property(x => x.SwipeGateNo).HasColumnName("SWIPE_GATENO").HasMaxLength(10).IsRequired();
        builder.Property(x => x.SwipePunchStatus).HasColumnName("SWIPE_PUNCHSTATUS").HasMaxLength(1).IsRequired();
        builder.Property(x => x.SwipePullStatus).HasColumnName("SWIPE_PULLSTATUS").HasMaxLength(1);
        builder.Property(x => x.LogCreatedOn).HasColumnName("LOG_CREATEDON").IsRequired();
        builder.Property(x => x.LogCreatedBy).HasColumnName("LOG_CREATEDBY");

        builder.Ignore(x => x.DomainEvents);
    }
}
