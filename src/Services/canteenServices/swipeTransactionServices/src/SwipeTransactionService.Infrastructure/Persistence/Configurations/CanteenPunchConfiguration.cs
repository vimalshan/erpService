using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SwipeTransactionService.Domain.Entities;

namespace SwipeTransactionService.Infrastructure.Persistence.Configurations;

public sealed class CanteenPunchConfiguration : IEntityTypeConfiguration<CanteenPunch>
{
    public void Configure(EntityTypeBuilder<CanteenPunch> builder)
    {
        builder.ToTable("CAN_DAYWISE_EMP_PUNCH");
        builder.HasKey(e => e.SerialNumber);

        builder.Property(e => e.SerialNumber).HasColumnName("CN_SRL_NUM").ValueGeneratedNever();
        builder.Property(e => e.CompanyCode).HasColumnName("CN_COM_COD").IsRequired();
        builder.Property(e => e.EmployeeSysId).HasColumnName("CN_SYSID").IsRequired();
        builder.Property(e => e.CanteenUnit).HasColumnName("CN_CAN_NUM").IsRequired();
        builder.Property(e => e.PunchDate).HasColumnName("CN_PUN_DAT").HasColumnType("datetime2(3)").IsRequired();
        builder.Property(e => e.TimeIn).HasColumnName("CN_TIM_IN").HasMaxLength(255);
        builder.Property(e => e.TimeOut).HasColumnName("CN_TIM_OUT").HasMaxLength(255);
        builder.Property(e => e.WorkHours).HasColumnName("CN_WRK_HRS").HasColumnType("decimal(38,0)");
    }
}
