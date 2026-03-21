using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TravelService.Domain.Entities;

namespace TravelService.Infrastructure.Persistence.Configurations;

public class ApproverDetailConfiguration : IEntityTypeConfiguration<ApproverDetail>
{
    public void Configure(EntityTypeBuilder<ApproverDetail> builder)
    {
        builder.ToTable("TRAVEL_APPRDETAILS");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("TRAVEL_APRDETID").HasMaxLength(255).IsRequired();
        builder.Property(x => x.TourPlanId).HasColumnName("TRAVEL_TPID").HasMaxLength(255).IsRequired();
        builder.Property(x => x.Source).HasColumnName("TRAVEL_SOURCE").HasMaxLength(255).IsRequired();
        builder.Property(x => x.SourceId).HasColumnName("TRAVEL_SOURCEID").HasMaxLength(255).IsRequired();
        builder.Property(x => x.ApprovedStatus).HasColumnName("TRAVEL_APPROVEDSTATUS").HasMaxLength(255).IsRequired();
        builder.Property(x => x.ApproverSysId).HasColumnName("TRAVEL_APPROVERSYSID").HasMaxLength(255).IsRequired();
        builder.Property(x => x.ApprovedOn).HasColumnName("TRAVEL_APPROVEDON").IsRequired();
        builder.Property(x => x.Remarks).HasColumnName("TRAVEL_REMARKS").HasMaxLength(255).IsRequired();
        builder.Property(x => x.ApproverType).HasColumnName("TRAVEL_APPROVERTYPE").HasMaxLength(255).IsRequired();
        builder.Ignore(x => x.DomainEvents);
    }
}
