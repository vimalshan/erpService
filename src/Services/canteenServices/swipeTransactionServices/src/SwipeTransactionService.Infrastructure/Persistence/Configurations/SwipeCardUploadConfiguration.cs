using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SwipeTransactionService.Domain.Entities;

namespace SwipeTransactionService.Infrastructure.Persistence.Configurations;

public sealed class SwipeCardUploadConfiguration : IEntityTypeConfiguration<SwipeCardUpload>
{
    public void Configure(EntityTypeBuilder<SwipeCardUpload> builder)
    {
        builder.ToTable("CANTEEN_SWIPE_CARD_UPLOAD");
        builder.HasNoKey();

        builder.Property(e => e.CompanyCode).HasColumnName("CN_COM_COD").IsRequired();
        builder.Property(e => e.EmployeeNumber).HasColumnName("CN_EMP_NUM").HasMaxLength(20).IsRequired();
        builder.Property(e => e.SwipeTime).HasColumnName("CN_SWP_TIM").HasColumnType("datetime2(3)").IsRequired();
        builder.Property(e => e.ItemCode).HasColumnName("CN_ITM_COD").IsRequired();
        builder.Property(e => e.ItemQuantity).HasColumnName("CN_ITM_QTN").IsRequired();
        builder.Property(e => e.BatchDate).HasColumnName("CN_BAT_DAT").HasColumnType("datetime2(3)").IsRequired();
        builder.Property(e => e.BatchNumber).HasColumnName("CN_BAT_NUM").IsRequired();
        builder.Property(e => e.SerialNumber).HasColumnName("CN_SRL_NUM").IsRequired();
        builder.Property(e => e.EntryDate).HasColumnName("CN_ENT_DAT").HasColumnType("datetime2(3)").IsRequired();
        builder.Property(e => e.CanteenNumber).HasColumnName("CN_CAN_NUM").HasColumnType("char(1)").IsRequired();
        builder.Property(e => e.GateNumber).HasColumnName("CN_GAT_NUM").HasColumnType("char(3)").IsRequired();
        builder.Property(e => e.UpdateStatus).HasColumnName("CN_UPD_STS").HasColumnType("char(1)").IsRequired();
        builder.Property(e => e.FlexField1).HasColumnName("CN_FLX_FLD1").HasColumnType("char(5)");
        builder.Property(e => e.FlexField2).HasColumnName("CN_FLX_FLD2").HasMaxLength(20);
        builder.Property(e => e.FlexField3).HasColumnName("CN_FLX_FLD3").HasColumnType("decimal(38,0)");
        builder.Property(e => e.FlexField4).HasColumnName("CN_FLX_FLD4").HasColumnType("datetime2(3)");
        builder.Property(e => e.FlexField5).HasColumnName("CN_FLX_FLD5").HasMaxLength(100);
    }
}
