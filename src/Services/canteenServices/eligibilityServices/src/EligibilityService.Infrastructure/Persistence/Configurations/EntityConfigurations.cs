using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EligibilityService.Domain.Entities;

namespace EligibilityService.Infrastructure.Persistence.Configurations;

public class EligibilityMasterConfiguration : IEntityTypeConfiguration<EligibilityMaster>
{
    public void Configure(EntityTypeBuilder<EligibilityMaster> builder)
    {
        builder.ToTable("CAN_ELIGIBILITY_MASTER");

        builder.HasKey(e => new { e.CanteenUnit, e.ShiftCode, e.ItemCode });

        builder.Property(e => e.CanteenUnit).HasColumnName("CN_COM_COD").IsRequired();
        builder.Property(e => e.ShiftCode).HasColumnName("CN_SFT_COD").HasMaxLength(1).IsRequired();
        builder.Property(e => e.ItemCode).HasColumnName("CN_ITM_COD").HasColumnType("decimal(38,0)").IsRequired();
        builder.Property(e => e.EligibleLimit).HasColumnName("CN_ELG_LMT");
        builder.Property(e => e.EnteredUser).HasColumnName("CN_ENT_USR");
        builder.Property(e => e.EnteredOn).HasColumnName("CN_ENT_DAT").HasColumnType("datetime2(3)");
        builder.Property(e => e.TimeOfficeUnit).HasColumnName("CN_TIM_UNT").HasMaxLength(3);
    }
}

public class EligibilityMasterHistoryConfiguration : IEntityTypeConfiguration<EligibilityMasterHistory>
{
    public void Configure(EntityTypeBuilder<EligibilityMasterHistory> builder)
    {
        builder.ToTable("CAN_ELIGIBILITY_MASTER_HIS");
        builder.HasNoKey();

        builder.Property(e => e.CanteenUnit).HasColumnName("CN_COM_COD").IsRequired();
        builder.Property(e => e.ShiftCode).HasColumnName("CN_SFT_COD").HasMaxLength(1).IsRequired();
        builder.Property(e => e.ItemCode).HasColumnName("CN_ITM_COD").HasColumnType("decimal(38,0)").IsRequired();
        builder.Property(e => e.EligibleLimit).HasColumnName("CN_ELG_LMT");
        builder.Property(e => e.ModifiedUser).HasColumnName("CN_MOD_USR").HasColumnType("decimal(38,0)");
        builder.Property(e => e.ModifiedOn).HasColumnName("CN_MOD_DAT").HasColumnType("datetime2(3)");
    }
}

public class ShiftMappingConfiguration : IEntityTypeConfiguration<ShiftMapping>
{
    public void Configure(EntityTypeBuilder<ShiftMapping> builder)
    {
        builder.ToTable("CAN_SHIFT_MAPPING");
        builder.HasKey(e => new { e.CompanyCode, e.ShiftCode });

        builder.Property(e => e.CompanyCode).HasColumnName("CN_COM_COD").IsRequired();
        builder.Property(e => e.ShiftCode).HasColumnName("CN_SFT_COD").HasMaxLength(1).IsRequired();
        builder.Property(e => e.BeforeShiftCode).HasColumnName("CN_SFT_BEF").HasMaxLength(1).IsRequired();
        builder.Property(e => e.AfterShiftCode).HasColumnName("CN_SFT_AFT").HasMaxLength(1).IsRequired();
    }
}

public class DaywiseEligibilityConfiguration : IEntityTypeConfiguration<DaywiseEligibility>
{
    public void Configure(EntityTypeBuilder<DaywiseEligibility> builder)
    {
        builder.ToTable("CANTEEN_DAYWISE_ELIGIBILITY");
        builder.HasKey(e => e.SerialNumber);

        builder.Property(e => e.SerialNumber).HasColumnName("CN_SRL_NUM").IsRequired();
        builder.Property(e => e.CompanyCode).HasColumnName("CN_COM_COD").IsRequired();
        builder.Property(e => e.EmployeeSysId).HasColumnName("CN_SYS_ID").IsRequired();
        builder.Property(e => e.AttendanceDate).HasColumnName("CN_ATT_DAT").HasColumnType("datetime2(3)");
        builder.Property(e => e.ProcessNumber).HasColumnName("CN_PRC_NUM");
        builder.Property(e => e.ShiftCode).HasColumnName("CN_SFT_COD").HasMaxLength(1);
        builder.Property(e => e.ItemCode).HasColumnName("CN_ITM_COD");
        builder.Property(e => e.ShiftQuantity).HasColumnName("CN_SFT_QTY");
        builder.Property(e => e.BeforeShiftQty).HasColumnName("CN_SFT_BEF");
        builder.Property(e => e.AfterShiftQty).HasColumnName("CN_SFT_AFT");
        builder.Property(e => e.EnteredUser).HasColumnName("CN_ENT_USR");
        builder.Property(e => e.EnteredOn).HasColumnName("CN_ENT_DAT").HasColumnType("datetime2(3)");
        builder.Property(e => e.FlexField1).HasColumnName("CN_FLEX1").HasMaxLength(20);
        builder.Property(e => e.GradeType).HasColumnName("CN_GRD_TYP").HasMaxLength(3);

        builder.HasIndex(e => e.CompanyCode).HasDatabaseName("IDX_CANTEEN_DAYWISE_ELIGIBILITY_CN_COM_COD");
    }
}
