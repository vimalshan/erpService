using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TravelService.Domain.Entities.TourPlan;

namespace TravelService.Infrastructure.Persistence.Configurations;

public class TourPlanLeaveConfiguration : IEntityTypeConfiguration<TourPlanLeave>
{
    public void Configure(EntityTypeBuilder<TourPlanLeave> builder)
    {
        builder.ToTable("TOURPLAN_LEAVE");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("LEAVE_TPLEAVEID").HasMaxLength(255);
        builder.Property(x => x.TourPlanId).HasColumnName("LEAVE_TPID").HasMaxLength(255).IsRequired();
        builder.Property(x => x.FromDate).HasColumnName("LEAVE_FROMDATE").IsRequired();
        builder.Property(x => x.ToDate).HasColumnName("LEAVE_TODATE").IsRequired();
        builder.Property(x => x.FromSession).HasColumnName("LEAVE_FROMSESSION").HasMaxLength(255).IsRequired();
        builder.Property(x => x.ToSession).HasColumnName("LEAVE_TOSESSION").HasMaxLength(255).IsRequired();
        builder.Property(x => x.LeaveType).HasColumnName("LEAVE_TYPE").HasMaxLength(255).IsRequired();
        builder.Property(x => x.LeaveDays).HasColumnName("LEAVE_DAYS").HasPrecision(18, 4);
        builder.Property(x => x.Remarks).HasColumnName("LEAVE_REMARKS").HasMaxLength(255).IsRequired();
        builder.Property(x => x.LeaveId).HasColumnName("LEAVE_ID").HasMaxLength(255).IsRequired();
        builder.Ignore(x => x.DomainEvents);
    }
}
