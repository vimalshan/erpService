using FinanceService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinanceService.Infrastructure.Persistence.Configurations;

public class TravelBatchSubConfiguration : IEntityTypeConfiguration<TravelBatchSub>
{
    public void Configure(EntityTypeBuilder<TravelBatchSub> builder)
    {
        builder.ToTable("TRAVEL_BATCH_SUB");
        builder.HasKey(e => new { e.UnitCode, e.BatchNumber, e.SerialNumber });
        builder.Property(e => e.UnitCode).HasColumnName("TS_UNT_COD").HasMaxLength(3).IsFixedLength();
        builder.Property(e => e.BatchNumber).HasColumnName("TS_BAT_NUM").HasColumnType("decimal(38,0)");
        builder.Property(e => e.SerialNumber).HasColumnName("TS_SRL_NUM").HasColumnType("decimal(38,0)");
        builder.Property(e => e.BookingNumber).HasColumnName("TS_BOK_NUM").HasColumnType("decimal(38,0)");
        builder.Property(e => e.TicketCost).HasColumnName("TS_TKT_CST").HasColumnType("decimal(19,0)");
        builder.Property(e => e.TicketAdjustment).HasColumnName("TS_TKT_ADJ").HasColumnType("decimal(19,0)");
        builder.Property(e => e.ApprovedAmount).HasColumnName("TS_APPROVE_AMT").HasColumnType("decimal(19,0)");
        builder.Property(e => e.Reason).HasColumnName("TS_REASON").HasMaxLength(255);
        builder.Property(e => e.Status).HasColumnName("TS_STATUS").HasMaxLength(255);
        builder.Property(e => e.CgstAmount).HasColumnName("TS_CGSTAMT").HasMaxLength(255);
        builder.Property(e => e.SgstAmount).HasColumnName("TS_SGSTAMT").HasMaxLength(255);
        builder.Property(e => e.IgstAmount).HasColumnName("TS_IGSTAMT").HasMaxLength(255);
    }
}
