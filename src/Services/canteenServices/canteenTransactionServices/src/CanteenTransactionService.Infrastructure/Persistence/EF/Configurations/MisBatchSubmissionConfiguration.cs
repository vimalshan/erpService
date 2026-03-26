using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CanteenTransactionService.Domain.Entities;

namespace CanteenTransactionService.Infrastructure.Persistence.EF.Configurations;

public class MisBatchSubmissionConfiguration : IEntityTypeConfiguration<MisBatchSubmission>
{
    public void Configure(EntityTypeBuilder<MisBatchSubmission> builder)
    {
        builder.ToTable("CANTEEN_MIS_SBT");

        builder.HasKey(e => e.SerialNumber);

        builder.Property(e => e.CompanyCode).HasColumnName("CN_COM_COD");
        builder.Property(e => e.EmployeeNumber).HasColumnName("CN_EMP_NUM").HasMaxLength(20);
        builder.Property(e => e.SwipeTime).HasColumnName("CN_SWP_TIM").HasColumnType("datetime2(3)");
        builder.Property(e => e.ItemCode).HasColumnName("CN_ITM_COD");
        builder.Property(e => e.ItemQuantity).HasColumnName("CN_ITM_QTN");
        builder.Property(e => e.BatchDate).HasColumnName("CN_BAT_DAT").HasColumnType("datetime2(3)");
        builder.Property(e => e.BatchNumber).HasColumnName("CN_BAT_NUM");
        builder.Property(e => e.SerialNumber).HasColumnName("CN_SRL_NUM").ValueGeneratedNever();
        builder.Property(e => e.EntryDate).HasColumnName("CN_ENT_DAT").HasColumnType("datetime2(3)");
        builder.Property(e => e.CanteenNumber).HasColumnName("CN_CAN_NUM").HasColumnType("char(1)");
        builder.Property(e => e.GateNumber).HasColumnName("CN_GAT_NUM").HasColumnType("char(3)");
        builder.Property(e => e.UpdateStatus).HasColumnName("CN_UPD_STS").HasColumnType("char(1)");
        builder.Property(e => e.FlexField1).HasColumnName("CN_FLX_FLD1").HasColumnType("char(5)");
        builder.Property(e => e.FlexField2).HasColumnName("CN_FLX_FLD2").HasMaxLength(20);
        builder.Property(e => e.FlexField3).HasColumnName("CN_FLX_FLD3").HasColumnType("decimal(38,0)");
        builder.Property(e => e.FlexField4).HasColumnName("CN_FLX_FLD4").HasColumnType("datetime2(3)");
        builder.Property(e => e.FlexField5).HasColumnName("CN_FLX_FLD5").HasMaxLength(100);

        builder.Ignore(e => e.CreatedAt);
        builder.Ignore(e => e.UpdatedAt);
        builder.Ignore(e => e.Version);
    }
}
