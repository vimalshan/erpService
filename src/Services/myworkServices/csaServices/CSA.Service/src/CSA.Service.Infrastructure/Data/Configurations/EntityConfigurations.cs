using CSA.Service.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CSA.Service.Infrastructure.Data.Configurations;

public class ControlConfiguration : IEntityTypeConfiguration<Control>
{
    public void Configure(EntityTypeBuilder<Control> builder)
    {
        builder.ToTable("CSA_MAIN");
        builder.HasKey(e => e.ControlId);
        builder.Property(e => e.ControlId).HasColumnName("CONTROL_ID").ValueGeneratedNever();
        builder.Property(e => e.Title).HasColumnName("CONTROL_TITLE").HasColumnType("varchar(200)").IsRequired();
        builder.Property(e => e.Description).HasColumnName("CONTROL_DESCRIPTION").HasColumnType("varchar(2000)");
        builder.Property(e => e.ControlType).HasColumnName("CONTROL_TYPE").HasColumnType("char(1)");
        builder.Property(e => e.ControlMethod).HasColumnName("CONTROL_METHOD").HasColumnType("char(1)");
        builder.Property(e => e.Risk).HasColumnName("CONTROL_RISK").HasColumnType("varchar(2000)");
        builder.Property(e => e.Priority).HasColumnName("CONTROL_PRIORITY").HasColumnType("char(1)");
        builder.Property(e => e.ProcessId).HasColumnName("CONTROL_PROCESS");
        builder.Property(e => e.SubProcessId).HasColumnName("CONTROL_SUBPROCESS");
        builder.Property(e => e.Periodicity).HasColumnName("CONTROL_PERIODICITY").HasColumnType("char(1)");
        builder.Property(e => e.EvidenceFlag).HasColumnName("CONTROL_EVIDENCEFLAG").HasColumnType("char(1)");
        builder.Property(e => e.ApproverFlag).HasColumnName("CONTROL_APPROVERFLAG").HasColumnType("char(1)");
        builder.Property(e => e.CreatedBy).HasColumnName("CONTROL_CREATEDBY");
        builder.Property(e => e.CreatedOn).HasColumnName("CONTROL_CREATEDON").HasColumnType("datetime2(3)");
        builder.Property(e => e.ModifiedBy).HasColumnName("CONTROL_MODIFIEDBY");
        builder.Property(e => e.ModifiedOn).HasColumnName("CONTROL_MODIFIEDON").HasColumnType("datetime2(3)");

        builder.HasIndex(e => e.Title).HasDatabaseName("IDX_CSA_MAIN_CONTROL_TITLE");

        builder.HasOne(e => e.Process).WithMany(p => p.Controls).HasForeignKey(e => e.ProcessId).OnDelete(DeleteBehavior.NoAction);
        builder.HasOne(e => e.SubProcess).WithMany(sp => sp.Controls).HasForeignKey(e => e.SubProcessId).OnDelete(DeleteBehavior.NoAction);

        builder.Ignore(e => e.DomainEvents);
    }
}

public class EvidenceConfiguration : IEntityTypeConfiguration<Evidence>
{
    public void Configure(EntityTypeBuilder<Evidence> builder)
    {
        builder.ToTable("CSA_EVIDENCE");
        builder.HasKey(e => e.EvidenceId);
        builder.Property(e => e.EvidenceId).HasColumnName("CONTROLEV_ID").ValueGeneratedNever();
        builder.Property(e => e.ControlId).HasColumnName("CONTROLEV_CONTROLID");
        builder.Property(e => e.Name).HasColumnName("CONTROLEV_NAME").HasColumnType("varchar(2000)");
        builder.Property(e => e.TempName).HasColumnName("CONTROLEV_TEMPNAME").HasColumnType("varchar(2000)");

        builder.HasIndex(e => e.ControlId).HasDatabaseName("IDX_CSA_EVIDENCE_CONTROLEV_CONTROLID");
        builder.HasOne(e => e.Control).WithMany(c => c.Evidences).HasForeignKey(e => e.ControlId).OnDelete(DeleteBehavior.NoAction);

        builder.Ignore(e => e.DomainEvents);
    }
}

public class ProcessConfiguration : IEntityTypeConfiguration<Process>
{
    public void Configure(EntityTypeBuilder<Process> builder)
    {
        builder.ToTable("CSA_PROCESSMAST");
        builder.HasKey(e => e.ProcessId);
        builder.Property(e => e.ProcessId).HasColumnName("PROCESS_ID").ValueGeneratedNever();
        builder.Property(e => e.Name).HasColumnName("PROCESS_NAME").HasColumnType("varchar(2000)").IsRequired();
        builder.Property(e => e.CreatedBy).HasColumnName("PROCESS_CREATEDBY");
        builder.Property(e => e.CreatedOn).HasColumnName("PROCESS_CREATEDON").HasColumnType("datetime2(3)");
        builder.Property(e => e.ModifiedBy).HasColumnName("PROCESS_MODIFIEDBY");
        builder.Property(e => e.ModifiedOn).HasColumnName("PROCESS_MODIFIEDON").HasColumnType("datetime2(3)");

        builder.Ignore(e => e.DomainEvents);
    }
}

public class SubProcessConfiguration : IEntityTypeConfiguration<SubProcess>
{
    public void Configure(EntityTypeBuilder<SubProcess> builder)
    {
        builder.ToTable("CSA_SUBPROCESSMAST");
        builder.HasKey(e => e.SubProcessId);
        builder.Property(e => e.SubProcessId).HasColumnName("SUBPROCESS_ID").ValueGeneratedNever();
        builder.Property(e => e.ProcessId).HasColumnName("SUBPROCESS_PROCESSID");
        builder.Property(e => e.Name).HasColumnName("SUBPROCESS_NAME").HasColumnType("varchar(2000)").IsRequired();
        builder.Property(e => e.CreatedBy).HasColumnName("SUBPROCESS_CREATEDBY");
        builder.Property(e => e.CreatedOn).HasColumnName("SUBPROCESS_CREATEDON").HasColumnType("datetime2(3)");
        builder.Property(e => e.ModifiedBy).HasColumnName("SUBPROCESS_MODIFIEDBY");
        builder.Property(e => e.ModifiedOn).HasColumnName("SUBPROCESS_MODIFIEDON").HasColumnType("datetime2(3)");

        builder.HasIndex(e => e.ProcessId).HasDatabaseName("IDX_CSA_SUBPROCESSMAST_SUBPROCESS_PROCESSID");
        builder.HasOne(e => e.Process).WithMany(p => p.SubProcesses).HasForeignKey(e => e.ProcessId).OnDelete(DeleteBehavior.NoAction);

        builder.Ignore(e => e.DomainEvents);
    }
}

public class SurveyConfiguration : IEntityTypeConfiguration<Survey>
{
    public void Configure(EntityTypeBuilder<Survey> builder)
    {
        builder.ToTable("CSA_RCSURVEYMAIN");
        builder.HasKey(e => e.SurveyId);
        builder.Property(e => e.SurveyId).HasColumnName("SURVEY_ID").ValueGeneratedNever();
        builder.Property(e => e.Title).HasColumnName("SURVEY_TITLE").HasColumnType("varchar(1000)").IsRequired();
        builder.Property(e => e.DueDate).HasColumnName("SURVEY_DUEDATE").HasColumnType("datetime2(3)");
        builder.Property(e => e.CloseDate).HasColumnName("SURVEY_CLOSEDATE").HasColumnType("datetime2(3)");
        builder.Property(e => e.StartDate).HasColumnName("SURVEY_STARTDATE").HasColumnType("datetime2(3)");
        builder.Property(e => e.EndDate).HasColumnName("SURVEY_ENDDATE").HasColumnType("datetime2(3)");
        builder.Property(e => e.Alert1).HasColumnName("SURVEY_ALERT1");
        builder.Property(e => e.Alert2).HasColumnName("SURVEY_ALERT2");
        builder.Property(e => e.CreatedBy).HasColumnName("SURVEY_CREATEDBY");
        builder.Property(e => e.CreatedOn).HasColumnName("SURVEY_CREATEDON").HasColumnType("datetime2(3)");
        builder.Property(e => e.ModifiedBy).HasColumnName("SURVEY_MODIFIEDBY");
        builder.Property(e => e.ModifiedOn).HasColumnName("SURVEY_MODIFIEDON").HasColumnType("datetime2(3)");

        builder.Ignore(e => e.DomainEvents);
    }
}

public class SurveyQuestionConfiguration : IEntityTypeConfiguration<SurveyQuestion>
{
    public void Configure(EntityTypeBuilder<SurveyQuestion> builder)
    {
        builder.ToTable("CSA_RCSURVEYQUESTION");
        builder.HasKey(e => e.SurveyQuestionId);
        builder.Property(e => e.SurveyQuestionId).HasColumnName("SURQ_ID").ValueGeneratedNever();
        builder.Property(e => e.SurveyId).HasColumnName("SURQ_SURVEYID");
        builder.Property(e => e.ControlId).HasColumnName("SURQ_CONTROLID");
        builder.Property(e => e.UnitId).HasColumnName("SURQ_UNITID");
        builder.Property(e => e.OwnerId).HasColumnName("SURQ_OWNERID");
        builder.Property(e => e.ApproverId).HasColumnName("SURQ_APPROVERID");
        builder.Property(e => e.OriginalDueDate).HasColumnName("SURQ_ORGDUEDATE").HasColumnType("datetime2(3)");
        builder.Property(e => e.DueDate).HasColumnName("SURQ_DUEDATE").HasColumnType("datetime2(3)");
        builder.Property(e => e.CancelDate).HasColumnName("SURQ_CANCELDATE").HasColumnType("datetime2(3)");
        builder.Property(e => e.CreatedBy).HasColumnName("SURQ_CREATEDBY");
        builder.Property(e => e.CreatedOn).HasColumnName("SURQ_CREATEDON").HasColumnType("datetime2(3)");
        builder.Property(e => e.ModifiedBy).HasColumnName("SURQ_MODIFIEDBY");
        builder.Property(e => e.ModifiedOn).HasColumnName("SURQ_MODIFIEDON").HasColumnType("datetime2(3)");
        builder.Property(e => e.AssessmentFlag).HasColumnName("SURQ_ASSFLAG").HasColumnType("char(1)");
        builder.Property(e => e.ApprovalFlag).HasColumnName("SURQ_APPFLAG").HasColumnType("char(1)");
        builder.Property(e => e.RemedialFlag).HasColumnName("SURQ_REMFLAG").HasColumnType("char(1)");
        builder.Property(e => e.RemedialDate).HasColumnName("SURQ_REMDATE").HasColumnType("datetime2(3)");
        builder.Property(e => e.AssessmentDate).HasColumnName("SURQ_ASSDATE").HasColumnType("datetime2(3)");
        builder.Property(e => e.ApprovalDate).HasColumnName("SURQ_APPDATE").HasColumnType("datetime2(3)");
        builder.Property(e => e.DelayDays).HasColumnName("SURQ_DELAYDAYS");
        builder.Property(e => e.RemedialCount).HasColumnName("SURQ_REMNOS");
        builder.Property(e => e.UnitName).HasColumnName("SURQ_UNITNAME").HasColumnType("varchar(50)");
        builder.Property(e => e.EntryFlag).HasColumnName("SURQ_ENTFLG").HasColumnType("char(1)");

        builder.HasIndex(e => e.SurveyId).HasDatabaseName("IDX_CSA_RCSURVEYQUESTION_SURQ_SURVEYID");
        builder.HasOne(e => e.Survey).WithMany(s => s.Questions).HasForeignKey(e => e.SurveyId).OnDelete(DeleteBehavior.NoAction);
        builder.HasOne(e => e.Control).WithMany(c => c.SurveyQuestions).HasForeignKey(e => e.ControlId).OnDelete(DeleteBehavior.NoAction);
        builder.HasOne(e => e.Unit).WithMany(u => u.SurveyQuestions).HasForeignKey(e => e.UnitId).OnDelete(DeleteBehavior.NoAction);

        builder.Ignore(e => e.DomainEvents);
    }
}

public class SurveyFeedbackConfiguration : IEntityTypeConfiguration<SurveyFeedback>
{
    public void Configure(EntityTypeBuilder<SurveyFeedback> builder)
    {
        builder.ToTable("CSA_RCSURVEYFEED");
        builder.HasKey(e => e.FeedbackId);
        builder.Property(e => e.FeedbackId).HasColumnName("SURQFEED_ID").ValueGeneratedNever();
        builder.Property(e => e.SurveyQuestionId).HasColumnName("SURQFEED_SURQID");
        builder.Property(e => e.EmployeeSysId).HasColumnName("SURQFEED_EMPSYSID");
        builder.Property(e => e.Status).HasColumnName("SURQFEED_STATUS").HasColumnType("char(1)");
        builder.Property(e => e.Type).HasColumnName("SURQFEED_TYPE").HasColumnType("char(1)");
        builder.Property(e => e.RemedialFlag).HasColumnName("SURQFEED_REMFLAG").HasColumnType("char(1)");
        builder.Property(e => e.RemedialDate).HasColumnName("SURQFEED_REMDATE").HasColumnType("datetime2(3)");
        builder.Property(e => e.Remarks).HasColumnName("SURQFEED_REMARKS").HasColumnType("varchar(2000)");
        builder.Property(e => e.EnteredOn).HasColumnName("SURQFEED_ENTEREDON").HasColumnType("datetime2(3)");
        builder.Property(e => e.EvidenceFlag).HasColumnName("SURQFEED_EVIDENCEFLAG").HasColumnType("char(1)");
        builder.Property(e => e.ApprovalFlag).HasColumnName("SURQFEED_APPFLAG").HasColumnType("char(1)");
        builder.Property(e => e.ApproverRemarks).HasColumnName("SURQFEED_APPREMARKS").HasColumnType("varchar(2000)");
        builder.Property(e => e.ApprovalDate).HasColumnName("SURQFEED_APPDATE").HasColumnType("datetime2(3)");
        builder.Property(e => e.ApprovedBy).HasColumnName("SURQFEED_APPBY");
        builder.Property(e => e.EntryDate).HasColumnName("SURQFEED_ENTDATE").HasColumnType("datetime2(3)");

        builder.HasIndex(e => e.SurveyQuestionId).HasDatabaseName("IDX_CSA_RCSURVEYFEED_SURQFEED_SURQID");
        builder.HasOne(e => e.SurveyQuestion).WithMany(q => q.Feedbacks).HasForeignKey(e => e.SurveyQuestionId).OnDelete(DeleteBehavior.NoAction);

        builder.Ignore(e => e.DomainEvents);
    }
}

public class SurveyAttachmentConfiguration : IEntityTypeConfiguration<SurveyAttachment>
{
    public void Configure(EntityTypeBuilder<SurveyAttachment> builder)
    {
        builder.ToTable("CSA_RCSURVEYATTACHMENT");
        builder.HasKey(e => e.AttachmentId);
        builder.Property(e => e.AttachmentId).HasColumnName("SURQATT_ID").ValueGeneratedNever();
        builder.Property(e => e.FeedbackId).HasColumnName("SURQATT_FEEDID");
        builder.Property(e => e.ControlEvidenceId).HasColumnName("SURQATT_CONTROLEVID");
        builder.Property(e => e.Attachment).HasColumnName("SURQATT_ATTACHMENT").HasColumnType("varchar(200)");

        builder.HasIndex(e => e.FeedbackId).HasDatabaseName("IDX_CSA_RCSURVEYATTACHMENT_SURQATT_FEEDID");
        builder.HasIndex(e => e.ControlEvidenceId).HasDatabaseName("IDX_CSA_RCSURVEYATTACHMENT_SURQATT_CONTROLEVID");
        builder.HasOne(e => e.Feedback).WithMany(f => f.Attachments).HasForeignKey(e => e.FeedbackId).OnDelete(DeleteBehavior.NoAction);
        builder.HasOne(e => e.Evidence).WithMany(ev => ev.SurveyAttachments).HasForeignKey(e => e.ControlEvidenceId).OnDelete(DeleteBehavior.NoAction);

        builder.Ignore(e => e.DomainEvents);
    }
}

public class UnitConfiguration : IEntityTypeConfiguration<Unit>
{
    public void Configure(EntityTypeBuilder<Unit> builder)
    {
        builder.ToTable("CSA_UNITMASTER");
        builder.HasKey(e => e.UnitId);
        builder.Property(e => e.UnitId).HasColumnName("UNIT_ID").ValueGeneratedNever();
        builder.Property(e => e.Name).HasColumnName("UNIT_NAME").HasColumnType("varchar(200)").IsRequired();
        builder.Property(e => e.ShortName).HasColumnName("UNIT_SHTNAME").HasColumnType("varchar(200)");
        builder.Property(e => e.Code).HasColumnName("UNIT_CODE").HasColumnType("char(3)");
        builder.Property(e => e.BusinessId).HasColumnName("UNIT_BUSINESSID");
        builder.Property(e => e.LiveFlag).HasColumnName("UNIT_LIVFLAG").HasColumnType("char(1)");
        builder.Property(e => e.OrgId).HasColumnName("UNIT_ORGID");
        builder.Property(e => e.CreatedBy).HasColumnName("UNIT_CREATEDBY");
        builder.Property(e => e.CreatedOn).HasColumnName("UNIT_CREATEDON").HasColumnType("datetime2(3)");
        builder.Property(e => e.ModifiedBy).HasColumnName("UNIT_MODIFIEDBY");
        builder.Property(e => e.ModifiedOn).HasColumnName("UNIT_MODIFIEDON").HasColumnType("datetime2(3)");

        builder.Ignore(e => e.DomainEvents);
    }
}

public class UnitMapDetailConfiguration : IEntityTypeConfiguration<UnitMapDetail>
{
    public void Configure(EntityTypeBuilder<UnitMapDetail> builder)
    {
        builder.ToTable("CSA_RCUNITMAPDET");
        builder.HasKey(e => e.MapId);
        builder.Property(e => e.MapId).HasColumnName("RCMAP_ID").ValueGeneratedNever();
        builder.Property(e => e.ControlId).HasColumnName("RCMAP_CONTROLID");
        builder.Property(e => e.UnitId).HasColumnName("RCMAP_UNITID");
        builder.Property(e => e.OwnerId).HasColumnName("RCMAP_OWNERID");
        builder.Property(e => e.ApproverId).HasColumnName("RCMAP_APPROVERID");
        builder.Property(e => e.ReportingManager).HasColumnName("RCMAP_REPMANAGER").HasColumnType("char(1)");
        builder.Property(e => e.EffectiveDate).HasColumnName("RCMAP_EFFDATE").HasColumnType("datetime2(3)");
        builder.Property(e => e.ClosureDate).HasColumnName("RCMAP_CLSDATE").HasColumnType("datetime2(3)");
        builder.Property(e => e.DueDate).HasColumnName("RCMAP_DUEDATE").HasColumnType("datetime2(3)");
        builder.Property(e => e.CreatedBy).HasColumnName("RCMAP_CREATEDBY");
        builder.Property(e => e.CreatedOn).HasColumnName("RCMAP_CREATEDON").HasColumnType("datetime2(3)");
        builder.Property(e => e.ModifiedBy).HasColumnName("RCMAP_MODIFIEDBY");
        builder.Property(e => e.ModifiedOn).HasColumnName("RCMAP_MODIFIEDON").HasColumnType("datetime2(3)");

        builder.HasIndex(e => e.ControlId).HasDatabaseName("IDX_CSA_RCUNITMAPDET_RCMAP_CONTROLID");
        builder.HasOne(e => e.Control).WithMany(c => c.UnitMappings).HasForeignKey(e => e.ControlId).OnDelete(DeleteBehavior.NoAction);
        builder.HasOne(e => e.Unit).WithMany(u => u.UnitMappings).HasForeignKey(e => e.UnitId).OnDelete(DeleteBehavior.NoAction);

        builder.Ignore(e => e.DomainEvents);
    }
}

public class CsaUserConfiguration : IEntityTypeConfiguration<CsaUser>
{
    public void Configure(EntityTypeBuilder<CsaUser> builder)
    {
        builder.ToTable("CSA_USERS");
        builder.HasNoKey();
        builder.Property(e => e.EmployeeNo).HasColumnName("USER_EMPNO").HasColumnType("decimal(38)");
        builder.Property(e => e.PinNumber).HasColumnName("USER_PINNUM").HasColumnType("decimal(20,0)");
        builder.Property(e => e.Name).HasColumnName("USER_NAME").HasColumnType("varchar(65)");
        builder.Property(e => e.SystemId).HasColumnName("USER_SYSID");
        builder.Property(e => e.Email).HasColumnName("USER_EMAIL").HasColumnType("varchar(65)");
    }
}

public class CsaMainUploadConfiguration : IEntityTypeConfiguration<CsaMainUpload>
{
    public void Configure(EntityTypeBuilder<CsaMainUpload> builder)
    {
        builder.ToTable("CSA_MAIN_UPLOAD");
        builder.HasNoKey();
        builder.Property(e => e.Title).HasColumnName("CONTROL_TITLE").HasColumnType("varchar(200)").IsRequired();
        builder.Property(e => e.Description).HasColumnName("CONTROL_DESCRIPTION").HasColumnType("varchar(2000)");
        builder.Property(e => e.ControlType).HasColumnName("CONTROL_TYPE").HasColumnType("char(30)");
        builder.Property(e => e.ControlMethod).HasColumnName("CONTROL_METHOD").HasColumnType("char(30)");
        builder.Property(e => e.Risk).HasColumnName("CONTROL_RISK").HasColumnType("varchar(2000)");
        builder.Property(e => e.Priority).HasColumnName("CONTROL_PRIORITY").HasColumnType("char(30)");
        builder.Property(e => e.ProcessId).HasColumnName("CONTROL_PROCESS");
        builder.Property(e => e.SubProcessId).HasColumnName("CONTROL_SUBPROCESS");
        builder.Property(e => e.Periodicity).HasColumnName("CONTROL_PERIODICITY").HasColumnType("char(30)");
        builder.Property(e => e.EvidenceFlag).HasColumnName("CONTROL_EVIDENCEFLAG").HasColumnType("char(1)");
        builder.Property(e => e.Evidence).HasColumnName("CONTROL_EVIDENCE").HasColumnType("varchar(200)");
        builder.Property(e => e.ApproverFlag).HasColumnName("CONTROL_APPROVERFLAG").HasColumnType("char(1)");
        builder.Property(e => e.SessionId).HasColumnName("SESSIONID").HasColumnType("varchar(200)");
    }
}

public class CsaMainUploadErrConfiguration : IEntityTypeConfiguration<CsaMainUploadErr>
{
    public void Configure(EntityTypeBuilder<CsaMainUploadErr> builder)
    {
        builder.ToTable("CSA_MAIN_UPLOADERR");
        builder.HasNoKey();
        builder.Property(e => e.Title).HasColumnName("CONTROL_TITLE").HasColumnType("varchar(200)").IsRequired();
        builder.Property(e => e.Description).HasColumnName("CONTROL_DESCRIPTION").HasColumnType("varchar(2000)");
        builder.Property(e => e.ControlType).HasColumnName("CONTROL_TYPE").HasColumnType("char(30)");
        builder.Property(e => e.ControlMethod).HasColumnName("CONTROL_METHOD").HasColumnType("char(30)");
        builder.Property(e => e.Risk).HasColumnName("CONTROL_RISK").HasColumnType("varchar(2000)");
        builder.Property(e => e.Priority).HasColumnName("CONTROL_PRIORITY").HasColumnType("char(30)");
        builder.Property(e => e.ProcessId).HasColumnName("CONTROL_PROCESS");
        builder.Property(e => e.SubProcessId).HasColumnName("CONTROL_SUBPROCESS");
        builder.Property(e => e.Periodicity).HasColumnName("CONTROL_PERIODICITY").HasColumnType("char(30)");
        builder.Property(e => e.EvidenceFlag).HasColumnName("CONTROL_EVIDENCEFLAG").HasColumnType("char(1)");
        builder.Property(e => e.Evidence).HasColumnName("CONTROL_EVIDENCE").HasColumnType("varchar(200)");
        builder.Property(e => e.ApproverFlag).HasColumnName("CONTROL_APPROVERFLAG").HasColumnType("char(1)");
        builder.Property(e => e.ErrorMessage).HasColumnName("ERRMSG").HasColumnType("varchar(200)");
        builder.Property(e => e.SessionId).HasColumnName("SESSIONID").HasColumnType("varchar(200)");
    }
}

public class CsaDataConfiguration : IEntityTypeConfiguration<CsaData>
{
    public void Configure(EntityTypeBuilder<CsaData> builder)
    {
        builder.ToTable("CSADATA");
        builder.HasNoKey();
        builder.Property(e => e.Title).HasColumnName("TITLE").HasColumnType("nvarchar(2000)");
        builder.Property(e => e.ControlMethod).HasColumnName("CONTROL_METHOD").HasColumnType("varchar(4000)");
        builder.Property(e => e.ControlType).HasColumnName("CONTROL_TYPE").HasColumnType("varchar(4000)");
        builder.Property(e => e.Priority).HasColumnName("PRIORITY").HasColumnType("varchar(4000)");
        builder.Property(e => e.ControlDescription).HasColumnName("CONTROL_DESCRIPTION").HasColumnType("nvarchar(2000)");
        builder.Property(e => e.Risk).HasColumnName("RISK").HasColumnType("nvarchar(2000)");
        builder.Property(e => e.ApprovalRequired).HasColumnName("APPROVAL_REQUIRED").HasColumnType("varchar(4000)");
        builder.Property(e => e.ControlRecordRequired).HasColumnName("CONTROLRECORD_REQUIRED").HasColumnType("varchar(4000)");
        builder.Property(e => e.FrequencyOfControl).HasColumnName("FREQUENCYOFCONTROL").HasColumnType("varchar(4000)");
        builder.Property(e => e.Periodicity).HasColumnName("PERIODICITY").HasColumnType("varchar(4000)");
        builder.Property(e => e.Process).HasColumnName("PROCESS").HasColumnType("varchar(4000)");
        builder.Property(e => e.SubProcess).HasColumnName("SUB_PROCESS").HasColumnType("varchar(4000)");
        builder.Property(e => e.Created).HasColumnName("CREATED").HasColumnType("decimal(38)");
        builder.Property(e => e.Modified).HasColumnName("MODIFIED").HasColumnType("decimal(38)");
        builder.Property(e => e.ModifiedBy).HasColumnName("MODIFIED_BY").HasColumnType("varchar(4000)");
        builder.Property(e => e.Id).HasColumnName("ID");
        builder.Property(e => e.ItemType).HasColumnName("ITEM_TYPE").HasColumnType("varchar(4000)");
        builder.Property(e => e.Path).HasColumnName("PATH").HasColumnType("varchar(4000)");
    }
}
