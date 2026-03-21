using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TravelRequestService.Domain.Entities;

namespace TravelRequestService.Infrastructure.Data.Configurations;

public class TravelAdvanceConfiguration : IEntityTypeConfiguration<TravelAdvance>
{
    public void Configure(EntityTypeBuilder<TravelAdvance> builder)
    {
        builder.ToTable("TRAVEL_ADVANCE");

        builder.HasKey(e => new { e.RequestNumber, e.AdvanceNumber });

        builder.Property(e => e.RequestNumber).HasColumnName("AD_REQ_NUM").HasColumnType("bigint");
        builder.Property(e => e.AdvanceNumber).HasColumnName("AD_ADV_NUM").HasColumnType("bigint");
        builder.Property(e => e.AdvanceDate).HasColumnName("AD_ADV_DAT").HasColumnType("datetime2(3)");
        builder.Property(e => e.AdvanceAmount).HasColumnName("AD_ADV_AMT").HasColumnType("decimal(19,0)");
        builder.Property(e => e.UnitCode).HasColumnName("AD_UNT_COD");
        builder.Property(e => e.ApprovedAmount).HasColumnName("AD_APP_AMT").HasColumnType("decimal(19,0)");
        builder.Property(e => e.PaidAmount).HasColumnName("AD_PAY_AMT").HasColumnType("decimal(19,0)");
        builder.Property(e => e.PaidDate).HasColumnName("AD_PAY_DAT").HasColumnType("datetime2(3)");
        builder.Property(e => e.AdjustedAmount).HasColumnName("AD_ADV_ADJ").HasColumnType("decimal(19,0)");
        builder.Property(e => e.PayNumber).HasColumnName("AD_PAY_NUM");
        builder.Property(e => e.PayType).HasColumnName("AD_PAY_TYP").HasMaxLength(255);
        builder.Property(e => e.EmployeeUnit).HasColumnName("AD_EMP_UNT").HasMaxLength(255);
        builder.Property(e => e.EmployeeNumber).HasColumnName("AD_EMP_NUM");
        builder.Property(e => e.TransactionNumber).HasColumnName("AD_TRN_NUM");

        builder.Ignore(e => e.DomainEvents);
    }
}
