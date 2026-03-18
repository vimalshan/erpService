using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SwipeTransactionService.Domain.Entities;

namespace SwipeTransactionService.Infrastructure.Persistence.Configurations;

public sealed class DailyAvailedConfiguration : IEntityTypeConfiguration<DailyAvailed>
{
    public void Configure(EntityTypeBuilder<DailyAvailed> builder)
    {
        builder.ToTable("CANTEEN_DAYWISE_AVAILED");
        builder.HasKey(e => e.SerialNumber);

        builder.Property(e => e.SerialNumber).HasColumnName("CN_SRL_NUM").ValueGeneratedNever();
        builder.Property(e => e.CompanyCode).HasColumnName("CN_COM_COD").IsRequired();
        builder.Property(e => e.EmployeeSysId).HasColumnName("CN_SYS_ID").IsRequired();
        builder.Property(e => e.EmployeeType).HasColumnName("CN_EMP_TYP").HasColumnType("char(1)");
        builder.Property(e => e.SwipeDate).HasColumnName("CN_SWP_DAT").HasMaxLength(255);
        builder.Property(e => e.ItemCode).HasColumnName("CN_ITM_COD");
        builder.Property(e => e.ItemType).HasColumnName("CN_ITM_TYP").HasColumnType("char(1)");
        builder.Property(e => e.EmployeeContribution).HasColumnName("CN_EE_CON").HasColumnType("decimal(38,0)");
        builder.Property(e => e.EmployerContribution).HasColumnName("CN_ER_CON").HasColumnType("decimal(38,0)");
        builder.Property(e => e.CanteenNumber).HasColumnName("CN_CAN_NUM").HasMaxLength(255);
        builder.Property(e => e.ItemQuantity).HasColumnName("CN_ITM_QTY");
        builder.Property(e => e.EntryUser).HasColumnName("CN_ENT_USR");
        builder.Property(e => e.EntryDate).HasColumnName("CN_ENT_DAT").HasMaxLength(255);
        builder.Property(e => e.FlexField1).HasColumnName("CN_FLEX1").HasMaxLength(20);
        builder.Property(e => e.GradeCategory).HasColumnName("CN_GRD_CAT").HasColumnType("char(3)");
    }
}
