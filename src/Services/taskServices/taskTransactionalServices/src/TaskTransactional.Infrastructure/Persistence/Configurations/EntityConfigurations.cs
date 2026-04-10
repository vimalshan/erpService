using TaskTransactional.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TaskTransactional.Infrastructure.Persistence.Configurations;

public class ComplaintMainConfiguration : IEntityTypeConfiguration<ComplaintMain>
{
    public void Configure(EntityTypeBuilder<ComplaintMain> builder)
    {
        builder.ToTable("COMPL_MAIN");
        builder.HasKey(e => e.CmGroupId);
        builder.Property(e => e.CmUnitCode).HasColumnName("CM_UNIT_CODE").HasColumnType("char(3)").IsRequired();
        builder.Property(e => e.CmGroupId).HasColumnName("CM_GROUPID").HasMaxLength(255).IsRequired();
        builder.Property(e => e.CmGroupName).HasColumnName("CM_GROUP_NAME").HasMaxLength(2000).IsRequired();
        builder.Property(e => e.CmGroupDesc).HasColumnName("CM_GROUP_DESC").HasMaxLength(2000);
        builder.Property(e => e.CmGroupSrc).HasColumnName("CM_GROUP_SRC").HasColumnType("decimal(38,0)").IsRequired();
        builder.Property(e => e.CmBehalfFlg).HasColumnName("CM_BEHALF_FLG").HasColumnType("char(1)");
        builder.Property(e => e.CmBehalfPin).HasColumnName("CM_BEHALF_PIN").HasColumnType("decimal(38,0)");
        builder.Property(e => e.CmRegPin).HasColumnName("CM_REG_PIN").HasColumnType("decimal(38,0)");
        builder.Property(e => e.CmShift).HasColumnName("CM_SHIFT").HasMaxLength(255);
        builder.Property(e => e.CmMail).HasColumnName("CM_MAIL").HasMaxLength(255);
        builder.Property(e => e.CmSubmit).HasColumnName("CM_SUBMIT").HasMaxLength(255);
        builder.Property(e => e.CmRegDate).HasColumnName("CM_REG_DATE").HasColumnType("datetime2(3)");
        builder.Property(e => e.CmUpdatedBy).HasColumnName("CM_UPDATEDBY").HasMaxLength(255);
        builder.Property(e => e.CmUpdatedOn).HasColumnName("CM_UPDATEDON").HasColumnType("datetime2(3)");
        builder.Ignore(e => e.DomainEvents);
    }
}

public class ComplaintDetailConfiguration : IEntityTypeConfiguration<ComplaintDetail>
{
    public void Configure(EntityTypeBuilder<ComplaintDetail> builder)
    {
        builder.ToTable("COMPL_DET");
        builder.HasKey(e => e.CdTicketNum);
        builder.Property(e => e.CdTicketNum).HasColumnName("CD_TICKET_NUM").HasColumnType("decimal(38,0)").ValueGeneratedNever();
        builder.Property(e => e.CdGroupId).HasColumnName("CD_GROUPID").HasColumnType("decimal(38,0)").IsRequired();
        builder.Property(e => e.CdType).HasColumnName("CD_TYPE").HasColumnType("decimal(38,0)").IsRequired();
        builder.Property(e => e.CdLocation).HasColumnName("CD_LOCATION").HasColumnType("decimal(38,0)").IsRequired();
        builder.Property(e => e.CdDepartment).HasColumnName("CD_DEPARTMENT").HasColumnType("decimal(38,0)").IsRequired();
        builder.Property(e => e.CdProcess).HasColumnName("CD_PROCESS").HasColumnType("decimal(38,0)").IsRequired();
        builder.Property(e => e.CdSubject).HasColumnName("CD_SUBJECT").HasMaxLength(500);
        builder.Property(e => e.CdDescription).HasColumnName("CD_DESCRIPTION").HasMaxLength(4000);
        builder.Property(e => e.CdNcr).HasColumnName("CD_NCR").HasColumnType("char(1)");
        builder.Property(e => e.CdPicturePath).HasColumnName("CD_PICTUREPATH").HasMaxLength(200);
        builder.Property(e => e.CdFilePath).HasColumnName("CD_FILEPATH").HasMaxLength(200);
        builder.Property(e => e.CdTargetDate).HasColumnName("CD_TARGET_DATE").HasMaxLength(255).IsRequired();
        builder.Property(e => e.CdClosureDate).HasColumnName("CD_CLOSURE_DATE").HasColumnType("datetime2(3)");
        builder.HasMany(e => e.Tasks).WithOne(e => e.Detail).HasForeignKey(e => e.CtTicketNum).HasPrincipalKey(e => e.CdTicketNum);
        builder.Ignore(e => e.DomainEvents);
    }
}

public class ComplaintTaskConfiguration : IEntityTypeConfiguration<ComplaintTask>
{
    public void Configure(EntityTypeBuilder<ComplaintTask> builder)
    {
        builder.ToTable("COMPL_TASK");
        builder.HasKey(e => e.CtTaskNum);
        builder.Property(e => e.CtTaskNum).HasColumnName("CT_TASK_NUM").HasColumnType("decimal(38,0)").ValueGeneratedNever();
        builder.Property(e => e.CtTicketNum).HasColumnName("CT_TICKET_NUM").HasColumnType("decimal(38,0)").IsRequired();
        builder.Property(e => e.CtScheduleFreq).HasColumnName("CT_SCHEDULE_FREQ").HasColumnType("char(2)").IsRequired();
        builder.Property(e => e.CtScheduleValue).HasColumnName("CT_SCHEDULE_VALUE").HasMaxLength(300);
        builder.Property(e => e.CtScheduleTime).HasColumnName("CT_SCHEDULE_TIME").HasMaxLength(12);
        builder.Property(e => e.CtScheduleDay).HasColumnName("CT_SCHEDULE_DAY").HasMaxLength(65);
        builder.Property(e => e.CtEffDate).HasColumnName("CT_EFF_DATE").HasColumnType("datetime2(3)");
        builder.Property(e => e.CtClsDate).HasColumnName("CT_CLS_DATE").HasColumnType("datetime2(3)");
        builder.Property(e => e.CtUpdatedBy).HasColumnName("CT_UPDATED_BY").HasColumnType("decimal(38,0)");
        builder.Property(e => e.CtUpdatedOn).HasColumnName("CT_UPDATED_ON").HasColumnType("datetime2(3)");
        builder.Ignore(e => e.DomainEvents);
    }
}

public class ComplaintActionConfiguration : IEntityTypeConfiguration<ComplaintAction>
{
    public void Configure(EntityTypeBuilder<ComplaintAction> builder)
    {
        builder.ToTable("COMPL_ACTION");
        builder.HasKey(e => e.CaActionNum);
        builder.Property(e => e.CaActionNum).HasColumnName("CA_ACTION_NUM").HasColumnType("decimal(38,0)").ValueGeneratedNever();
        builder.Property(e => e.CaTaskNum).HasColumnName("CA_TASK_NUM").HasColumnType("decimal(38,0)").IsRequired();
        builder.Property(e => e.CaPrmResp).HasColumnName("CA_PRM_RESP").HasMaxLength(300);
        builder.Property(e => e.CaPrmActBy).HasColumnName("CA_PRM_ACTBY").HasColumnType("decimal(38,0)");
        builder.Property(e => e.CaPrmActDate).HasColumnName("CA_PRM_ACTDATE").HasColumnType("datetime2(3)");
        builder.Property(e => e.CaPrmSolution).HasColumnName("CA_PRM_SOLUTION").HasMaxLength(4000);
        builder.Property(e => e.CaSecEscHrs).HasColumnName("CA_SEC_ESCHRS").HasColumnType("decimal(38,0)");
        builder.Property(e => e.CaSecResp).HasColumnName("CA_SEC_RESP").HasMaxLength(300);
        builder.Property(e => e.CaSecActBy).HasColumnName("CA_SEC_ACTBY").HasColumnType("decimal(38,0)");
        builder.Property(e => e.CaSecActDate).HasColumnName("CA_SEC_ACTDATE").HasColumnType("datetime2(3)");
        builder.Property(e => e.CaSecSolution).HasColumnName("CA_SEC_SOLUTION").HasMaxLength(4000);
        builder.Property(e => e.CaFwdRemarks).HasColumnName("CA_FWD_REMARKS").HasMaxLength(4000);
        builder.Property(e => e.CaFwdResp).HasColumnName("CA_FWD_RESP").HasMaxLength(300);
        builder.Property(e => e.CaFwdActBy).HasColumnName("CA_FWD_ACTBY").HasColumnType("decimal(38,0)");
        builder.Property(e => e.CaFwdActDate).HasColumnName("CA_FWD_ACTDATE").HasColumnType("datetime2(3)");
        builder.Property(e => e.CaFwdSolution).HasColumnName("CA_FWD_SOLUTION").HasMaxLength(4000);
        builder.Property(e => e.CaCurEscLevel).HasColumnName("CA_CUR_ESCLEVEL").HasColumnType("decimal(38,0)");
        builder.Property(e => e.CaCorrActReq).HasColumnName("CA_CORR_ACTREQ").HasColumnType("char(1)");
        builder.Property(e => e.CaCorrRemarks).HasColumnName("CA_CORR_REMARKS").HasMaxLength(4000);
        builder.Property(e => e.CaCorrResp).HasColumnName("CA_CORR_RESP").HasMaxLength(300);
        builder.Property(e => e.CaCorrActBy).HasColumnName("CA_CORR_ACTBY").HasColumnType("decimal(38,0)");
        builder.Property(e => e.CaCorrActDate).HasColumnName("CA_CORR_ACTDATE").HasColumnType("datetime2(3)");
        builder.Property(e => e.CaCorrSolution).HasColumnName("CA_CORR_SOLUTION").HasMaxLength(4000);
        builder.Property(e => e.CaReopenFlg).HasColumnName("CA_REOPEN_FLG").HasColumnType("char(1)");
        builder.Property(e => e.CaReopenRemarks).HasColumnName("CA_REOPEN_REMARKS").HasMaxLength(4000);
        builder.Property(e => e.CaTrgDat).HasColumnName("CA_TRG_DAT").HasColumnType("datetime2(3)");
        builder.Property(e => e.CaClsDat).HasColumnName("CA_CLS_DAT").HasColumnType("datetime2(3)");
        builder.Property(e => e.CaUpdatedBy).HasColumnName("CA_UPATEDBY").HasColumnType("decimal(38,0)");
        builder.Ignore(e => e.DomainEvents);
    }
}

public class ComplaintHistoryConfiguration : IEntityTypeConfiguration<ComplaintHistory>
{
    public void Configure(EntityTypeBuilder<ComplaintHistory> builder)
    {
        builder.ToTable("COMPL_HIST");
        builder.HasKey(e => e.ChHistoryNum);
        builder.Property(e => e.ChHistoryNum).HasColumnName("CH_HISTORY_NUM").HasColumnType("decimal(38,0)").ValueGeneratedNever();
        builder.Property(e => e.ChActionNum).HasColumnName("CH_ACTION_NUM").HasColumnType("decimal(38,0)").IsRequired();
        builder.Property(e => e.ChSerialNum).HasColumnName("CH_SERIAL_NUM").HasColumnType("decimal(38,0)").IsRequired();
        builder.Property(e => e.ChFrom).HasColumnName("CH_FROM").HasMaxLength(65);
        builder.Property(e => e.ChTo).HasColumnName("CH_TO").HasMaxLength(1000);
        builder.Property(e => e.ChActionDate).HasColumnName("CH_ACTION_DATE").HasColumnType("datetime2(3)").IsRequired();
        builder.Property(e => e.ChActionType).HasColumnName("CH_ACTION_TYPE").HasColumnType("char(1)").IsRequired();
        builder.Property(e => e.ChRemarks).HasColumnName("CH_REMARKS").HasMaxLength(4000);
        builder.Property(e => e.ChUpdatedBy).HasColumnName("CH_UPDATEDBY").HasColumnType("decimal(38,0)");
        builder.Property(e => e.ChUpdatedOn).HasColumnName("CH_UPDATEDON").HasColumnType("datetime2(3)");
        builder.Property(e => e.ChFilePath).HasColumnName("CH_FILEPATH").HasMaxLength(200);
        builder.Ignore(e => e.DomainEvents);
    }
}

public class ComplaintEscalationConfiguration : IEntityTypeConfiguration<ComplaintEscalation>
{
    public void Configure(EntityTypeBuilder<ComplaintEscalation> builder)
    {
        builder.ToTable("COMPL_ESC");
        builder.HasKey(e => new { e.CeTicketNum, e.CeLevelNum });
        builder.Property(e => e.CeTicketNum).HasColumnName("CE_TICKET_NUM").HasColumnType("decimal(38,0)");
        builder.Property(e => e.CeLevelNum).HasColumnName("CE_LEVEL_NUM").HasColumnType("decimal(38,0)");
        builder.Property(e => e.CeEscNoHrs).HasColumnName("CE_ESC_NOHRS").HasColumnType("decimal(38,0)").IsRequired();
        builder.Property(e => e.CeUserPin).HasColumnName("CE_USER_PIN").HasColumnType("decimal(38,0)").IsRequired();
        builder.Property(e => e.CeEffDate).HasColumnName("CE_EFF_DATE").HasColumnType("datetime2(3)").IsRequired();
        builder.Property(e => e.CeClsDate).HasColumnName("CE_CLS_DATE").HasColumnType("datetime2(3)");
        builder.Property(e => e.CeExclude).HasColumnName("CE_EXCLUDE").HasColumnType("char(1)");
        builder.Property(e => e.CeUpdatedBy).HasColumnName("CE_UPDATEDBY").HasColumnType("decimal(38,0)");
        builder.Property(e => e.CeUpdatedOn).HasColumnName("CE_UPDATEDON").HasColumnType("datetime2(3)");
        builder.Ignore(e => e.DomainEvents);
    }
}
