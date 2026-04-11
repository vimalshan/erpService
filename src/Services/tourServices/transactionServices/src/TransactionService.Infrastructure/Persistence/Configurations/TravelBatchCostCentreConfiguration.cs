using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TransactionService.Domain.Entities;

namespace TransactionService.Infrastructure.Persistence.Configurations;

public sealed class TravelBatchCostCentreConfiguration : IEntityTypeConfiguration<TravelBatchCostCentre>
{
    public void Configure(EntityTypeBuilder<TravelBatchCostCentre> builder)
    {
        builder.ToTable("TRAVEL_BATCHCC");
        builder.HasKey(x => x.CostNum);

        builder.Property(x => x.CostNum).HasColumnName("BATCHSCOST_NUM").HasColumnType("DECIMAL(38)").ValueGeneratedNever();
        builder.Property(x => x.BatchSubNum).HasColumnName("BATCHSCOST_BATSUBNUM").HasColumnType("DECIMAL(38)");
        builder.Property(x => x.UnitId).HasColumnName("BATCHSCOST_UNITID").HasMaxLength(255);
        builder.Property(x => x.SubAcc).HasColumnName("BATCHSCOST_SUBACC").HasMaxLength(255);
        builder.Property(x => x.CostCode).HasColumnName("BATCHSCOST_CSTCOD").HasMaxLength(255);
        builder.Property(x => x.ProjectCode).HasColumnName("BATCHSCOST_PRJCOD").HasMaxLength(255);
        builder.Property(x => x.LocationCode).HasColumnName("BATCHSCOST_LOCCOD").HasMaxLength(255);
        builder.Property(x => x.IutaCode).HasColumnName("BATCHSCOST_IUTACOD").HasMaxLength(255);

        builder.Ignore(x => x.DomainEvents);
    }
}
