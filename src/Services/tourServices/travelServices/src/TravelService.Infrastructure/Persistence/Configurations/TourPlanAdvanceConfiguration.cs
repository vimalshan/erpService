using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TravelService.Domain.Entities.TourPlan;

namespace TravelService.Infrastructure.Persistence.Configurations;

public class TourPlanAdvanceConfiguration : IEntityTypeConfiguration<TourPlanAdvance>
{
    public void Configure(EntityTypeBuilder<TourPlanAdvance> builder)
    {
        builder.ToTable("TOURPLAN_ADVANCE");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("ADV_ID").HasMaxLength(255);
        builder.Property(x => x.TourPlanId).HasColumnName("ADV_TPID").HasMaxLength(255).IsRequired();
        builder.Property(x => x.Amount).HasColumnName("ADV_AMOUNT").HasPrecision(18, 4);
        builder.Property(x => x.JvId).HasColumnName("ADV_JVID").HasMaxLength(255);
        builder.Property(x => x.Remarks).HasColumnName("ADV_REMARKS").HasMaxLength(255);
        builder.Property(x => x.ApprovalStatus).HasColumnName("ADV_APPSTATUS").HasMaxLength(255);
        builder.Property(x => x.ApprovedBy).HasColumnName("ADV_APPBY").HasMaxLength(255);
        builder.Property(x => x.ApprovedOn).HasColumnName("ADV_APPON");
        builder.Property(x => x.Currency).HasColumnName("ADV_CURRENCY").HasMaxLength(255);
        builder.Property(x => x.Rate).HasColumnName("ADV_RATE").HasPrecision(18, 4);
        builder.Property(x => x.TotalInr).HasColumnName("ADV_TOTAL").HasPrecision(18, 4);
        builder.Property(x => x.LastModifiedOn).HasColumnName("ADV_MODIFIEDON");
        builder.Property(x => x.ApproverRemarks).HasColumnName("ADV_APPREMARKS").HasMaxLength(200);
        builder.Property(x => x.FinanceRemarks).HasColumnName("ADV_FINREMARKS").HasMaxLength(200);
        builder.Property(x => x.Type).HasColumnName("ADV_TYPE").HasMaxLength(1);
        builder.Property(x => x.PayMode).HasColumnName("ADV_PAYMODE").HasMaxLength(255);
        builder.Ignore(x => x.DomainEvents);
    }
}
