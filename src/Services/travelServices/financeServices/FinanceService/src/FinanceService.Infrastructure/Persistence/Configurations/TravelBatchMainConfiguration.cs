using FinanceService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinanceService.Infrastructure.Persistence.Configurations;

public class TravelBatchMainConfiguration : IEntityTypeConfiguration<TravelBatchMain>
{
    public void Configure(EntityTypeBuilder<TravelBatchMain> builder)
    {
        builder.ToTable("TRAVEL_BATCH_MAIN");
        builder.HasKey(e => new { e.UnitCode, e.BatchNumber });
        builder.Property(e => e.UnitCode).HasColumnName("TM_UNT_COD").HasMaxLength(3).IsFixedLength();
        builder.Property(e => e.BatchNumber).HasColumnName("TM_BAT_NUM").HasColumnType("decimal(38,0)");
        builder.Property(e => e.BatchDate).HasColumnName("TM_BAT_DAT");
        builder.Property(e => e.InvoiceNumber).HasColumnName("TM_INV_NUM").HasMaxLength(25);
        builder.Property(e => e.InvoiceDate).HasColumnName("TM_INV_DAT");
        builder.Property(e => e.BatchStatus).HasColumnName("TM_BAT_STS").HasMaxLength(1).IsFixedLength();
        builder.Property(e => e.AdminRemarks).HasColumnName("TM_ADM_REM").HasMaxLength(200);
        builder.Property(e => e.FinanceRemarks).HasColumnName("TM_FIN_REM").HasMaxLength(200);
        builder.Property(e => e.AgencyCode).HasColumnName("TM_AGN_COD").HasColumnType("decimal(38,0)");
        builder.Property(e => e.TotalApprovedAmount).HasColumnName("TM_TOTAPPRAMT").HasColumnType("decimal(19,0)");
        builder.Property(e => e.Total).HasColumnName("TM_TOTAL").HasColumnType("decimal(19,0)");
        builder.Property(e => e.JvNo).HasColumnName("TM_JVNO");
        builder.Property(e => e.CgstAmount).HasColumnName("TM_CGSTAMT").HasColumnType("decimal(19,0)");
        builder.Property(e => e.SgstAmount).HasColumnName("TM_SGSTAMT").HasColumnType("decimal(19,0)");
        builder.Property(e => e.IgstAmount).HasColumnName("TM_IGSTAMT").HasColumnType("decimal(19,0)");

        builder.HasMany(e => e.BatchLines)
            .WithOne(e => e.BatchMain)
            .HasForeignKey(e => new { e.UnitCode, e.BatchNumber });
    }
}
