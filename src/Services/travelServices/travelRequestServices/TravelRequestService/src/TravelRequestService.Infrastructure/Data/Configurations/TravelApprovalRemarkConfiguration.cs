using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TravelRequestService.Domain.Entities;

namespace TravelRequestService.Infrastructure.Data.Configurations;

public class TravelApprovalRemarkConfiguration : IEntityTypeConfiguration<TravelApprovalRemark>
{
    public void Configure(EntityTypeBuilder<TravelApprovalRemark> builder)
    {
        builder.ToTable("TRAVEL_APPRREMARKS");

        builder.HasKey(e => new { e.RequestNumber, e.SerialNumber });

        builder.Property(e => e.RequestNumber).HasColumnName("TR_REQNO").HasColumnType("bigint");
        builder.Property(e => e.RequestType).HasColumnName("TR_REQTYP").HasMaxLength(10);
        builder.Property(e => e.Remarks).HasColumnName("TR_REM").HasMaxLength(2000);
        builder.Property(e => e.ApprovedBy).HasColumnName("TR_APPBY").HasMaxLength(60);
        builder.Property(e => e.ApprovedOn).HasColumnName("TR_APP_ON").HasColumnType("datetime2(3)");
        builder.Property(e => e.SerialNumber).HasColumnName("TR_SRLNO");

        builder.Ignore(e => e.DomainEvents);
    }
}
