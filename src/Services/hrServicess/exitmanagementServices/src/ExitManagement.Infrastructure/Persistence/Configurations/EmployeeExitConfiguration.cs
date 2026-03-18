using ExitManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExitManagement.Infrastructure.Persistence.Configurations;

public class EmployeeExitConfiguration : IEntityTypeConfiguration<EmployeeExit>
{
    public void Configure(EntityTypeBuilder<EmployeeExit> builder)
    {
        builder.ToTable("TTBT_EXIT_TEV");

        builder.HasKey(e => e.ExitNo);
        builder.Property(e => e.ExitNo).HasColumnName("EXIT_NO").HasColumnType("decimal(38,0)");
        builder.Property(e => e.EmployeeSysId).HasColumnName("EXIT_EMP_SYSID").HasColumnType("decimal(38,0)").IsRequired();
        builder.Property(e => e.LetterGivenOn).HasColumnName("EXIT_LET_GIVON");
        builder.Property(e => e.ExpectedRelieveDate).HasColumnName("EXIT_EXP_RELDT");
        builder.Property(e => e.ResignationType).HasColumnName("EXIT_RES_TYPE").HasMaxLength(25);
        builder.Property(e => e.ResignationId).HasColumnName("EXIT_RES_ID").HasColumnType("decimal(38,0)").IsRequired();
        builder.Property(e => e.Remarks).HasColumnName("EXIT_REMARKS").HasMaxLength(300);
        builder.Property(e => e.Status).HasColumnName("EXIT_STATUS").HasMaxLength(1);
        builder.Property(e => e.RelieveGivenOn).HasColumnName("EXIT_REL_GIVON");
        builder.Property(e => e.InterviewCondductedOn).HasColumnName("EXIT_INTCONDON");
        builder.Property(e => e.InterviewConductedBy).HasColumnName("EXIT_INTCONDBY").HasMaxLength(200);
        builder.Property(e => e.RevokeReason).HasColumnName("EXIT_REVOKE_REASON").HasMaxLength(300);
        builder.Property(e => e.RevokeDate).HasColumnName("EXIT_REVOKE_DATE");
        builder.Property(e => e.UpdatedBy).HasColumnName("EXIT_UPDATED_BY").HasColumnType("decimal(38,0)");
        builder.Property(e => e.UpdatedOn).HasColumnName("EXIT_UPDATED_ON");
        builder.Property(e => e.SignId).HasColumnName("EXIT_SIGN_ID").HasColumnType("decimal(38,0)");
        builder.Property(e => e.RevokeResignation).HasColumnName("EXIT_REVOKE_RESIGNATION").HasMaxLength(1);
        builder.Property(e => e.PayrollSettlement).HasColumnName("EXIT_PAYROLL_SETTLEMENT").HasMaxLength(1);
        builder.Property(e => e.StopSalaryDate).HasColumnName("EXIT_STOPSAL_DATE");
        builder.Property(e => e.NextOfficer).HasColumnName("EXIT_NEXTOFFICER").HasColumnType("decimal(38,0)");
        builder.Property(e => e.SettlementTypeId).HasColumnName("EXIT_SETTYPEID").HasColumnType("decimal(38,0)");
        builder.Property(e => e.MailDisableDate).HasColumnName("EXIT_MAILDIS_DATE");
        builder.Property(e => e.MailDeleteDate).HasColumnName("EXIT_MAILDEL_DATE");
        builder.Property(e => e.MailForwardSysId).HasColumnName("EXIT_MAILFWD_SYSID").HasColumnType("decimal(22,0)");
        builder.Property(e => e.FormalityBy).HasColumnName("EXIT_FORMALITYBY").HasColumnType("decimal(22,0)");
        builder.Property(e => e.FormalityOn).HasColumnName("EXIT_FORMALITYON");
        builder.Property(e => e.UserConfirmStatus).HasColumnName("EXIT_USERCONFSTATUS").HasMaxLength(1);
        builder.Property(e => e.BypassFormality).HasColumnName("EXIT_BYPASSFORMALITY").HasMaxLength(1);
        builder.Property(e => e.ApprovalStatus).HasColumnName("EXIT_APPSTATUS").HasMaxLength(1);
        builder.Property(e => e.ApprovedBy).HasColumnName("EXIT_APPBY").HasColumnType("decimal(22,0)");
        builder.Property(e => e.ApprovedOn).HasColumnName("EXIT_APPON");
        builder.Property(e => e.NoticeDate).HasColumnName("EXIT_NOTDATE");
        builder.Property(e => e.MailStatus).HasColumnName("EXIT_MAILSTATUS").HasMaxLength(1);
        builder.Property(e => e.CheckStatus).HasColumnName("EXIT_CHKSTATUS").HasMaxLength(1);
        builder.Property(e => e.LastSerialNo).HasColumnName("EXIT_LSNO").HasColumnType("decimal(22,0)");
        builder.Property(e => e.FirstSerialNo).HasColumnName("EXIT_FSNO").HasColumnType("decimal(22,0)");
        builder.Property(e => e.FsBy).HasColumnName("EXIT_FSBY").HasColumnType("decimal(22,0)");
        builder.Property(e => e.FsOn).HasColumnName("EXIT_FSON");
        builder.Property(e => e.NoticePeriodPaid).HasColumnName("EXIT_NPP").HasColumnType("decimal(38,0)");
        builder.Property(e => e.MailToUser).HasColumnName("EXIT_MAILTOUSER").HasMaxLength(1);
        builder.Property(e => e.ConductDescription).HasColumnName("EXIT_CONDUCTDESC").HasMaxLength(500);
        builder.Property(e => e.JvBatchId).HasColumnName("EXIT_JVBATCHID").HasColumnType("decimal(38,0)");
        builder.Property(e => e.JvPostedBy).HasColumnName("EXIT_JVPOSTEDBY").HasColumnType("decimal(38,0)");
        builder.Property(e => e.JvPostedOn).HasColumnName("EXIT_JVPOSTEDON");
        builder.Property(e => e.DesignationOnJoining).HasColumnName("EXIT_DESGONJOINING").HasMaxLength(100);
        builder.Property(e => e.ReasonForLeaving).HasColumnName("EXIT_REASONFORLEAVING").HasMaxLength(100);

        builder.Ignore(e => e.CreatedOn);
    }
}
