using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VehicleTracking.Domain.Entities;

namespace VehicleTracking.Infrastructure.Persistence.Configurations;

public class VehicleMasterConfiguration : IEntityTypeConfiguration<VehicleMaster>
{
    public void Configure(EntityTypeBuilder<VehicleMaster> builder)
    {
        builder.ToTable("VEHICLE_MAST");
        builder.HasKey(e => e.SerialNumber);
        builder.Property(e => e.SerialNumber).HasColumnName("VH_SRL_NUM").ValueGeneratedOnAdd();
        builder.Property(e => e.RegNum1).HasColumnName("VH_REG_NUM1").HasMaxLength(3).IsRequired();
        builder.Property(e => e.RegNum2).HasColumnName("VH_REG_NUM2").HasMaxLength(2);
        builder.Property(e => e.RegNum3).HasColumnName("VH_REG_NUM3").HasMaxLength(2);
        builder.Property(e => e.RegNum4).HasColumnName("VH_REG_NUM4").HasMaxLength(4).IsRequired();
        builder.Property(e => e.RegistrationDate).HasColumnName("VH_REG_DAT").HasPrecision(3);
        builder.Property(e => e.UpdatedDate).HasColumnName("VH_UPD_DAT").HasPrecision(3).IsRequired();
        builder.Property(e => e.UpdatedBy).HasColumnName("VH_UPD_USR").HasMaxLength(25).IsRequired();
        builder.Property(e => e.UpdateNumber).HasColumnName("VH_UPD_NUM").IsRequired();
        builder.Property(e => e.LogUser).HasColumnName("VH_LOG_USR").HasMaxLength(25);
        builder.Property(e => e.LogNumber).HasColumnName("VH_LOG_NUM");
        builder.Property(e => e.LogDate).HasColumnName("VH_LOG_DAT").HasPrecision(3);
        builder.Ignore(e => e.DomainEvents);
    }
}

public class VehicleStageConfiguration : IEntityTypeConfiguration<VehicleStage>
{
    public void Configure(EntityTypeBuilder<VehicleStage> builder)
    {
        builder.ToTable("VEHICLE_STAGE");
        builder.HasKey(e => new { e.TransactionNumber, e.TrackingNumber, e.StageSerial });
        builder.Property(e => e.TransactionNumber).HasColumnName("ST_TRN_NUM");
        builder.Property(e => e.TrackingNumber).HasColumnName("ST_TRK_NUM");
        builder.Property(e => e.StageSerial).HasColumnName("ST_STG_SRL");
        builder.Property(e => e.EntryDate).HasColumnName("ST_ENT_DAT").HasPrecision(3).IsRequired();
        builder.Property(e => e.EntryUser).HasColumnName("ST_ENT_USR").HasMaxLength(25);
        builder.Property(e => e.EntryNumber).HasColumnName("ST_ENT_NUM").IsRequired();
        builder.Property(e => e.LeaveDate).HasColumnName("ST_LEV_DAT").HasPrecision(3).IsRequired();
        builder.Property(e => e.RoleCode).HasColumnName("ST_ROL_COD").IsRequired();
        builder.Property(e => e.DecisionFlag).HasColumnName("ST_DEC_FLG").HasMaxLength(1);
        builder.Property(e => e.CancelStatus).HasColumnName("ST_CAN_STS").HasMaxLength(1).IsRequired();
        builder.Property(e => e.TimeTaken).HasColumnName("ST_TIM_TKN").HasPrecision(38, 0);
        builder.Property(e => e.StageCode).HasColumnName("VT_STG_COD").IsRequired();
        builder.Property(e => e.StageComment).HasColumnName("ST_STG_COM").HasMaxLength(100);
        builder.Property(e => e.DeleteDate).HasColumnName("VT_DEL_DAT").HasPrecision(3).IsRequired();
        builder.Property(e => e.DeleteUser).HasColumnName("VT_DEL_USR").HasMaxLength(25);
        builder.Property(e => e.DeleteNumber).HasColumnName("VT_DEL_NUM").IsRequired();
        builder.HasOne(e => e.Stage).WithMany().HasForeignKey(e => e.StageCode).HasPrincipalKey(s => s.StageCode);
        builder.Ignore(e => e.DomainEvents);
    }
}

public class VehicleTransactionConfiguration : IEntityTypeConfiguration<VehicleTransaction>
{
    public void Configure(EntityTypeBuilder<VehicleTransaction> builder)
    {
        builder.ToTable("VEHICLE_TRAN");
        builder.HasKey(e => e.TrackingNumber);
        builder.Property(e => e.TrackingNumber).HasColumnName("TR_TRK_NUM").ValueGeneratedOnAdd();
        builder.Property(e => e.VehicleSerial).HasColumnName("TR_VEH_SRL");
        builder.Property(e => e.PartyName).HasColumnName("TR_PTY_NAM").HasMaxLength(200);
        builder.Property(e => e.ReportDate).HasColumnName("TR_REP_DAT").HasPrecision(3);
        builder.Property(e => e.PurposeCode).HasColumnName("TR_PRP_COD");
        builder.Property(e => e.PreviousStage).HasColumnName("TR_STG_PRV");
        builder.Property(e => e.PreviousDate).HasColumnName("TR_PRV_DAT").HasPrecision(3);
        builder.Property(e => e.CurrentStage).HasColumnName("TR_STG_CUR").HasPrecision(38, 0);
        builder.Property(e => e.GateName).HasColumnName("TR_GAT_NAM").HasMaxLength(25);
        builder.Property(e => e.TransactionNumber).HasColumnName("TR_TRN_NUM").HasMaxLength(25);
        builder.Property(e => e.ProductCode).HasColumnName("TR_PRO_COD").HasMaxLength(25);
        builder.Property(e => e.ProductQuantity).HasColumnName("TR_PRO_QTY").HasPrecision(38, 0);
        builder.Property(e => e.StageComment).HasColumnName("TR_STG_COM").HasMaxLength(100);
        builder.Property(e => e.DriverName).HasColumnName("TR_DRV_NAM").HasMaxLength(100);
        builder.Property(e => e.DriverCell).HasColumnName("TR_DRV_CELL").HasMaxLength(15);
        builder.Property(e => e.TyreWeight).HasColumnName("TR_TYR_WGT").HasPrecision(38, 0);
        builder.Property(e => e.GrossWeight).HasColumnName("TR_GRS_WGT").HasPrecision(38, 0);
        builder.Property(e => e.VehicleStatus).HasColumnName("TR_VEH_STS").HasMaxLength(1);
        builder.Property(e => e.LogEntryUser).HasColumnName("TR_LOG_ENT_USR").HasMaxLength(255);
        builder.Property(e => e.LogEntryNumber).HasColumnName("TR_LOG_ENT_NUM").HasMaxLength(255);
        builder.Property(e => e.LogEntryDate).HasColumnName("TR_LOG_ENT_DATE").HasPrecision(3);
        builder.Property(e => e.MainPurpose).HasColumnName("TR_MAIN_PURPOSE");
        builder.Property(e => e.SupplierCode).HasColumnName("TR_SUP_COD").HasMaxLength(25);
        builder.HasOne(e => e.Vehicle).WithMany().HasForeignKey(e => e.VehicleSerial).HasPrincipalKey(v => v.SerialNumber).IsRequired(false);
        builder.HasOne(e => e.Purpose).WithMany().HasForeignKey(e => e.PurposeCode).HasPrincipalKey(p => p.PurposeCode).IsRequired(false);
        builder.Ignore(e => e.DomainEvents);
    }
}

public class VehicleInvoiceConfiguration : IEntityTypeConfiguration<VehicleInvoice>
{
    public void Configure(EntityTypeBuilder<VehicleInvoice> builder)
    {
        builder.ToTable("VEHICLE_INVOICE");
        builder.HasKey(e => new { e.TrackingNumber, e.ReferenceNumber, e.InvoiceSerial });
        builder.Property(e => e.TrackingNumber).HasColumnName("IN_TRK_NUM");
        builder.Property(e => e.ReferenceNumber).HasColumnName("IN_REF_NUM");
        builder.Property(e => e.InvoiceSerial).HasColumnName("IN_INV_SRL");
        builder.Property(e => e.OriginalInvoice).HasColumnName("IN_ORC_INV");
        builder.Property(e => e.ChainInvoice).HasColumnName("IN_CHN_INV").IsRequired();
        builder.Property(e => e.CustomerCode).HasColumnName("IN_CUS_COD").HasMaxLength(25);
        builder.Property(e => e.CancelFlag).HasColumnName("IN_CAN_FLG").HasMaxLength(1);
        builder.Property(e => e.ModifiedNumber).HasColumnName("IN_MOD_NUM").IsRequired();
        builder.Property(e => e.ModifiedUser).HasColumnName("IN_MOD_USR").HasMaxLength(25).IsRequired();
        builder.Property(e => e.ModifiedDate).HasColumnName("IN_MOD_DAT").HasPrecision(3).IsRequired();
        builder.Ignore(e => e.DomainEvents);
    }
}

public class VehicleDirectEntryConfiguration : IEntityTypeConfiguration<VehicleDirectEntry>
{
    public void Configure(EntityTypeBuilder<VehicleDirectEntry> builder)
    {
        builder.ToTable("VEHICLE_DIRECT_ENTRY");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("VDE_ID");
        builder.Property(e => e.TrackingNumber).HasColumnName("VDE_TRK_NUM").IsRequired();
        builder.Property(e => e.EntryDate).HasColumnName("VDE_ENT_DAT").HasPrecision(3).IsRequired();
        builder.Property(e => e.EntryUser).HasColumnName("VDE_ENT_USR").HasMaxLength(25).IsRequired();
        builder.Ignore(e => e.DomainEvents);
    }
}

public class DecisionFlagConfiguration : IEntityTypeConfiguration<DecisionFlag>
{
    public void Configure(EntityTypeBuilder<DecisionFlag> builder)
    {
        builder.ToTable("DECISION_FLAG");
        builder.HasKey(e => new { e.TrackingNumber, e.PurposeCode, e.StageCode });
        builder.Property(e => e.TrackingNumber).HasColumnName("DF_TRC_NUM");
        builder.Property(e => e.PurposeCode).HasColumnName("DF_PUR_COD");
        builder.Property(e => e.StageCode).HasColumnName("DF_STG_COD");
        builder.Property(e => e.StageDecision).HasColumnName("DF_STG_DEC").HasMaxLength(1).IsRequired();
        builder.Property(e => e.CancelFlag).HasColumnName("DF_CAN_FLG").HasMaxLength(1).IsRequired();
        builder.Property(e => e.ReferenceNumber).HasColumnName("DF_REF_NUM");
        builder.Property(e => e.UpdateDate).HasColumnName("DF_UPD_DAT").HasPrecision(3);
        builder.Property(e => e.Remark).HasColumnName("DF_REMARK").HasMaxLength(100);
        builder.Ignore(e => e.DomainEvents);
    }
}

public class StageMasterConfiguration : IEntityTypeConfiguration<StageMaster>
{
    public void Configure(EntityTypeBuilder<StageMaster> builder)
    {
        builder.ToTable("STAGE_MAST");
        builder.HasKey(e => e.StageCode);
        builder.Property(e => e.StageCode).HasColumnName("ST_STG_COD").ValueGeneratedNever();
        builder.Property(e => e.OptionName).HasColumnName("ST_OPT_NAM").HasMaxLength(25).IsRequired();
        builder.Property(e => e.UpdatedBy).HasColumnName("ST_UPD_USR").HasMaxLength(25);
        builder.Property(e => e.UpdateNumber).HasColumnName("ST_UPD_NUM");
        builder.Property(e => e.UpdatedDate).HasColumnName("ST_UPD_DAT").HasPrecision(3);
        builder.Ignore(e => e.DomainEvents);
    }
}

public class PurposeMasterConfiguration : IEntityTypeConfiguration<PurposeMaster>
{
    public void Configure(EntityTypeBuilder<PurposeMaster> builder)
    {
        builder.ToTable("PURPOSE_MAST");
        builder.HasKey(e => e.PurposeCode);
        builder.Property(e => e.PurposeCode).HasColumnName("PR_PRP_COD").ValueGeneratedNever();
        builder.Property(e => e.PurposeName).HasColumnName("PR_PRP_NAM").HasMaxLength(100);
        builder.Property(e => e.TransactionType).HasColumnName("PR_TRN_TYP").HasMaxLength(1);
        builder.Property(e => e.PurposeCategory).HasColumnName("PR_PRP_CAT").HasMaxLength(10);
        builder.Property(e => e.LastStage).HasColumnName("PR_LST_STG");
        builder.Property(e => e.ParentPurpose).HasColumnName("PR_PAR_PRP").HasPrecision(38, 0);
        builder.Ignore(e => e.DomainEvents);
    }
}

public class PurposeStageConfiguration : IEntityTypeConfiguration<PurposeStage>
{
    public void Configure(EntityTypeBuilder<PurposeStage> builder)
    {
        builder.ToTable("PURPOSE_STAGE");
        builder.HasKey(e => new { e.PurposeCode, e.StageCode });
        builder.Property(e => e.PurposeCode).HasColumnName("PS_PRP_COD");
        builder.Property(e => e.StageCode).HasColumnName("PS_STG_COD");
        builder.Property(e => e.StageSerial).HasColumnName("PS_STG_SRL").IsRequired();
        builder.Property(e => e.FlexField).HasColumnName("PS_FLX_FLD").HasMaxLength(1).IsRequired();
        builder.Property(e => e.ParallelFlag).HasColumnName("PS_PRL_FLG").HasMaxLength(1).IsRequired();
        builder.Property(e => e.RoleCode).HasColumnName("PS_ROL_COD").IsRequired();
        builder.Property(e => e.BooleanFlag).HasColumnName("PS_BOL_FLG").HasMaxLength(1).IsRequired();
        builder.Property(e => e.BooleanDescription).HasColumnName("PS_BOL_DES").HasMaxLength(200);
        builder.Property(e => e.TrueStage).HasColumnName("PS_TRU_STG");
        builder.Property(e => e.FalseStage).HasColumnName("PS_FAL_STG");
        builder.Property(e => e.Remarks).HasColumnName("PS_REM_MRK").HasMaxLength(2000);
        builder.Property(e => e.LowLimit).HasColumnName("PS_LOW_LIMIT").HasPrecision(38, 0);
        builder.Property(e => e.HighLimit).HasColumnName("PS_HIGH_LIMIT").HasPrecision(38, 0);
        builder.Property(e => e.TargetTime).HasColumnName("PS_TARGET_TIME").HasPrecision(38, 0);
        builder.HasOne(e => e.Purpose).WithMany(p => p.PurposeStages).HasForeignKey(e => e.PurposeCode);
        builder.HasOne(e => e.Stage).WithMany(s => s.PurposeStages).HasForeignKey(e => e.StageCode);
        builder.Ignore(e => e.DomainEvents);
    }
}

public class PurposeProductConfiguration : IEntityTypeConfiguration<PurposeProduct>
{
    public void Configure(EntityTypeBuilder<PurposeProduct> builder)
    {
        builder.ToTable("PURPOSE_PRODUCT");
        builder.HasKey(e => new { e.ProductCode, e.PurposeCode });
        builder.Property(e => e.ProductCode).HasColumnName("PP_PRO_COD").HasMaxLength(25);
        builder.Property(e => e.PurposeCode).HasColumnName("PP_PUR_COD");
        builder.HasOne(e => e.Purpose).WithMany(p => p.PurposeProducts).HasForeignKey(e => e.PurposeCode);
        builder.Ignore(e => e.DomainEvents);
    }
}

public class StageDecisionConfiguration : IEntityTypeConfiguration<StageDecision>
{
    public void Configure(EntityTypeBuilder<StageDecision> builder)
    {
        builder.ToTable("STAGE_DECISION");
        builder.HasKey(e => new { e.PurposeCode, e.StageCode, e.OptionName });
        builder.Property(e => e.PurposeCode).HasColumnName("SD_PUR_COD");
        builder.Property(e => e.StageCode).HasColumnName("SD_STG_COD");
        builder.Property(e => e.OptionName).HasColumnName("SD_OPT_NAM").HasMaxLength(25);
        builder.Property(e => e.OptionId).HasColumnName("SD_OPT_ID");
        builder.Property(e => e.NextStage).HasColumnName("SD_STG_NEXT").IsRequired();
        builder.Ignore(e => e.DomainEvents);
    }
}

public class StageFlexConfiguration : IEntityTypeConfiguration<StageFlex>
{
    public void Configure(EntityTypeBuilder<StageFlex> builder)
    {
        builder.ToTable("STAGE_FLEX");
        builder.HasKey(e => e.PurposeCode);
        builder.Property(e => e.PurposeCode).HasColumnName("PS_PRP_COD").ValueGeneratedNever();
        builder.Property(e => e.StageSerial).HasColumnName("PS_STG_SRL").IsRequired();
        builder.Property(e => e.FlexNumber).HasColumnName("PS_FLX_NUM").IsRequired();
        builder.Property(e => e.FlexDescription).HasColumnName("PS_FLX_DES").HasMaxLength(200);
        builder.Property(e => e.LovFlag).HasColumnName("PS_LOV_FLG").HasMaxLength(1).IsRequired();
        builder.Property(e => e.LovType).HasColumnName("PS_LOV_TYP").HasMaxLength(3);
        builder.Property(e => e.FlexType).HasColumnName("PS_FLX_TYP").HasMaxLength(1);
        builder.Ignore(e => e.DomainEvents);
    }
}

public class SparshNavigationConfiguration : IEntityTypeConfiguration<SparshNavigation>
{
    public void Configure(EntityTypeBuilder<SparshNavigation> builder)
    {
        builder.ToTable("SPARSH_NAVIGATION");
        builder.HasKey(e => e.RequestNumber);
        builder.Property(e => e.RequestNumber).HasColumnName("SN_REQ_NUM").ValueGeneratedNever();
        builder.Property(e => e.UserId).HasColumnName("SN_USR_ID").HasMaxLength(25).IsRequired();
        builder.Property(e => e.UserNumber).HasColumnName("SN_USR_NUM").IsRequired();
        builder.Property(e => e.RandomNumber).HasColumnName("SN_RAN_NUM").HasMaxLength(25);
        builder.Property(e => e.UpdateDate).HasColumnName("SN_UPD_DAT").HasPrecision(3).IsRequired();
        builder.Property(e => e.SciId).HasColumnName("SN_SCI_ID").HasMaxLength(1).IsRequired();
        builder.Property(e => e.StatusFlag).HasColumnName("SN_STS_FLG").HasMaxLength(1);
        builder.Ignore(e => e.DomainEvents);
    }
}

public class WeightInformationConfiguration : IEntityTypeConfiguration<WeightInformation>
{
    public void Configure(EntityTypeBuilder<WeightInformation> builder)
    {
        builder.ToTable("WEIGHT_INFO");
        builder.HasKey(e => e.TrackingNumber);
        builder.Property(e => e.TrackingNumber).HasColumnName("WI_TRK_NUM").ValueGeneratedNever();
        builder.Property(e => e.TyreWeight).HasColumnName("WI_TYR_WGT").HasPrecision(38, 0);
        builder.Property(e => e.GrossWeight).HasColumnName("WI_GRS_WGT").HasPrecision(38, 0);
        builder.Property(e => e.NetWeight).HasColumnName("WI_NET_WGT").HasPrecision(38, 0);
        builder.Ignore(e => e.DomainEvents);
    }
}
