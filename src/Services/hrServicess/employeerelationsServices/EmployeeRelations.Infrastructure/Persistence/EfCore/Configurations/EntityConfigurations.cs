using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EmployeeRelations.Domain.Aggregates;
using EmployeeRelations.Domain.ValueObjects;

namespace EmployeeRelations.Infrastructure.Persistence.EfCore.Configurations;

public class DisciplinaryMainConfiguration : IEntityTypeConfiguration<DisciplinaryMain>
{
    public void Configure(EntityTypeBuilder<DisciplinaryMain> builder)
    {
        builder.ToTable("DISCIPLINARY_MAIN");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("DISCIPLINE_MAINID").HasColumnType("decimal(38,0)");
        builder.Property(e => e.UnitId).HasColumnName("DISCIPLINE_UNITID").HasColumnType("decimal(38,0)");
        builder.Property(e => e.Date).HasColumnName("DISCIPLINE_DATE");
        builder.Property(e => e.Details).HasColumnName("DISCIPLINE_DETAILS").HasMaxLength(500);
        builder.Property(e => e.CreatedBy).HasColumnName("DISCIPLINE_CREATEDBY").HasColumnType("decimal(38,0)");
        builder.Property(e => e.CreatedOn).HasColumnName("DISCIPLINE_CREATEDON");
        builder.Property(e => e.ModifiedBy).HasColumnName("DISCIPLINE_MODIFIEDBY").HasColumnType("decimal(38,0)");
        builder.Property(e => e.ModifiedOn).HasColumnName("DISCIPLINE_MODIFIEDON");

        builder.HasMany(e => e.Employees).WithOne().HasForeignKey(e => e.MainId);
        builder.HasMany(e => e.Actions).WithOne().HasForeignKey(e => e.MainId);
        builder.Ignore(e => e.DomainEvents);
    }
}

public class DisciplinaryEmpConfiguration : IEntityTypeConfiguration<DisciplinaryEmp>
{
    public void Configure(EntityTypeBuilder<DisciplinaryEmp> builder)
    {
        builder.ToTable("DISCIPLINARY_EMP");
        builder.HasKey(e => new { e.MainId, e.EmpSysId });
        builder.Property(e => e.MainId).HasColumnName("DISEMP_MAINID").HasColumnType("decimal(38,0)");
        builder.Property(e => e.EmpSysId).HasColumnName("DISEMP_EMPSYSID").HasColumnType("decimal(38,0)");
        builder.Property(e => e.ModifiedBy).HasColumnName("DISEMP_MODIFIEDBY").HasColumnType("decimal(38,0)");
        builder.Property(e => e.ModifiedOn).HasColumnName("DISEMP_MODIFIEDON");
        builder.Ignore(e => e.DomainEvents);
    }
}

public class DisciplinaryActionConfiguration : IEntityTypeConfiguration<DisciplinaryAction>
{
    public void Configure(EntityTypeBuilder<DisciplinaryAction> builder)
    {
        builder.ToTable("DISCIPLINARY_ACTION");
        builder.HasKey(e => e.ActionId);
        builder.Property(e => e.ActionId).HasColumnName("DISACTION_ID").HasColumnType("decimal(38,0)");
        builder.Property(e => e.MainId).HasColumnName("DISACTION_MAINID").HasColumnType("decimal(38,0)");
        builder.Property(e => e.EmpSysId).HasColumnName("DISACTION_EMPSYSID").HasColumnType("decimal(38,0)");
        builder.Property(e => e.TypeId).HasColumnName("DISACTION_TYPEID").HasColumnType("decimal(38,0)");
        builder.Property(e => e.ActionDate).HasColumnName("DISACTION_DATE");
        builder.Property(e => e.Remarks).HasColumnName("DISACTION_REMARKS").HasMaxLength(500);
        builder.Property(e => e.DocPath).HasColumnName("DISACTION_DOC").HasMaxLength(100);
        builder.Property(e => e.EntryStatus).HasColumnName("DISACTION_ENTRYSTATUS").HasMaxLength(1);
        builder.Property(e => e.CreatedBy).HasColumnName("DISACTION_CREATEDBY").HasColumnType("decimal(38,0)");
        builder.Property(e => e.CreatedOn).HasColumnName("DISACTION_CREATEDON");
        builder.Property(e => e.ModifiedBy).HasColumnName("DISACTION_MODIFIEDBY").HasColumnType("decimal(38,0)");
        builder.Property(e => e.ModifiedOn).HasColumnName("DISACTION_MODIFIEDON");
        builder.Property(e => e.ApprovedBy).HasColumnName("DISACTION_APPROVEDBY").HasColumnType("decimal(38,0)");
        builder.Property(e => e.ApprovedOn).HasColumnName("DISACTION_APPROVEDON");
        builder.Property(e => e.ReturnRemarks).HasColumnName("DISACTION_RETURNREMARKS").HasMaxLength(500);
        builder.Ignore(e => e.DomainEvents);
    }
}

public class EwsMainConfiguration : IEntityTypeConfiguration<EwsMain>
{
    public void Configure(EntityTypeBuilder<EwsMain> builder)
    {
        builder.ToTable("EWS_MAIN");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("EWS_ID");
        builder.Property(e => e.EmpSysId).HasColumnName("EWS_EMPSYSID");
        builder.Property(e => e.PeriodNo).HasColumnName("EWS_PERIODNO");
        builder.Property(e => e.HrEntryBy).HasColumnName("EWS_HRENTRYBY");
        builder.Property(e => e.HrEntryDate).HasColumnName("EWS_HRENTRYDATE");
        builder.Property(e => e.HrRemarks).HasColumnName("EWS_HRREMARKS").HasMaxLength(500);
        builder.Property(e => e.ChrRemarks).HasColumnName("EWS_CHRREMARKS").HasMaxLength(500);
        builder.Property(e => e.AprRemarks).HasColumnName("EWS_APRREMARKS").HasMaxLength(500);
        builder.Property(e => e.Reopen).HasColumnName("EWS_REOPEN").HasMaxLength(1);
        builder.Property(e => e.ReopenBy).HasColumnName("EWS_REOPENBY");
        builder.Property(e => e.GradeId).HasColumnName("EWS_GRADEID").HasColumnType("decimal(38,0)");
        builder.Property(e => e.Ctc).HasColumnName("EWS_CTC").HasColumnType("decimal(19,0)");
        builder.Property(e => e.AprScore).HasColumnName("EWS_APRSCORE").HasMaxLength(1);

        // Map value objects to single char columns
        builder.Property(e => e.HrFlag).HasColumnName("EWS_HRFLAG").HasMaxLength(1)
            .HasConversion(v => v == null ? null : v.Value, v => v == null ? null : EwsFlag.From(v));
        builder.Property(e => e.Status).HasColumnName("EWS_STATUS").HasMaxLength(1).IsRequired()
            .HasConversion(v => v.Value, v => EwsStatus.From(v));
        builder.Property(e => e.Ees).HasColumnName("EWS_EES").HasMaxLength(1)
            .HasConversion(v => v == null ? null : v.Value, v => v == null ? null : EwsFlag.From(v));
        builder.Property(e => e.Pulse).HasColumnName("EWS_PULSE").HasMaxLength(1)
            .HasConversion(v => v == null ? null : v.Value, v => v == null ? null : EwsFlag.From(v));
        builder.Property(e => e.Dd).HasColumnName("EWS_DD").HasMaxLength(1)
            .HasConversion(v => v == null ? null : v.Value, v => v == null ? null : EwsFlag.From(v));
        builder.Property(e => e.Ijp).HasColumnName("EWS_IJP").HasMaxLength(1)
            .HasConversion(v => v == null ? null : v.Value, v => v == null ? null : EwsFlag.From(v));
        builder.Property(e => e.Comp).HasColumnName("EWS_COMP").HasMaxLength(1)
            .HasConversion(v => v == null ? null : v.Value, v => v == null ? null : EwsFlag.From(v));
        builder.Property(e => e.Leave).HasColumnName("EWS_LEAVE").HasMaxLength(1)
            .HasConversion(v => v == null ? null : v.Value, v => v == null ? null : EwsFlag.From(v));
        builder.Property(e => e.Final).HasColumnName("EWS_FINAL").HasMaxLength(1)
            .HasConversion(v => v == null ? null : v.Value, v => v == null ? null : EwsFlag.From(v));
        builder.Property(e => e.ChrFlag).HasColumnName("EWS_CHRFLAG").HasMaxLength(1)
            .HasConversion(v => v == null ? null : v.Value, v => v == null ? null : EwsFlag.From(v));
        builder.Property(e => e.AprFlag).HasColumnName("EWS_APRFLAG").HasMaxLength(1)
            .HasConversion(v => v == null ? null : v.Value, v => v == null ? null : EwsFlag.From(v));

        builder.HasMany(e => e.AppInputs).WithOne().HasForeignKey(e => e.EwsId);
        builder.Ignore(e => e.DomainEvents);
    }
}

public class EwsAppInputConfiguration : IEntityTypeConfiguration<EwsAppInput>
{
    public void Configure(EntityTypeBuilder<EwsAppInput> builder)
    {
        builder.ToTable("EWS_APPINPUTS");
        builder.HasKey(e => e.InputId);
        builder.Property(e => e.InputId).HasColumnName("APP_INPUTID");
        builder.Property(e => e.EwsId).HasColumnName("APP_EWSID");
        builder.Property(e => e.EmpSysId).HasColumnName("APP_EMPSYSID");
        builder.Property(e => e.AppType).HasColumnName("APP_TYPE").HasMaxLength(1);
        builder.Property(e => e.EnteredOn).HasColumnName("APP_ENTEREDON");
        builder.Property(e => e.EngagementLevel).HasColumnName("APP_ENGLEVEL").HasMaxLength(1);
        builder.Property(e => e.LeaveFlag).HasColumnName("APP_LEAVEFLAG").HasMaxLength(1);
        builder.Property(e => e.Remarks).HasColumnName("APP_REMARKS").HasMaxLength(200);
        builder.Property(e => e.Reopen).HasColumnName("APRR_REOPEN").HasMaxLength(1);
        builder.Ignore(e => e.DomainEvents);
    }
}

public class SurveyMasterConfiguration : IEntityTypeConfiguration<SurveyMaster>
{
    public void Configure(EntityTypeBuilder<SurveyMaster> builder)
    {
        builder.ToTable("SURVEY_MASTER");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("SURVEY_ID");
        builder.Property(e => e.Name).HasColumnName("SURVEY_NAME").HasMaxLength(100).IsRequired();
        builder.Property(e => e.Image).HasColumnName("SURVEY_IMAGE").HasMaxLength(100).IsRequired();
        builder.Property(e => e.StartDate).HasColumnName("SURVEY_STARTDATE").IsRequired();
        builder.Property(e => e.EndDate).HasColumnName("SURVEY_ENDDATE");
        builder.Property(e => e.ClosureDate).HasColumnName("SURVEY_CLSDATE");
        builder.Property(e => e.AutoLock).HasColumnName("SURVEY_AUTOLOCK").HasMaxLength(1).IsRequired();
        builder.Property(e => e.Flag).HasColumnName("SURVEY_FLAG").HasMaxLength(1);
        builder.Property(e => e.TemplateId).HasColumnName("SURVEY_TEMPLATEID");
        builder.HasMany(e => e.Questions).WithOne().HasForeignKey(e => e.SurveyId);
        builder.HasMany(e => e.Responses).WithOne().HasForeignKey(e => e.SurveyId);
        builder.Ignore(e => e.DomainEvents);
    }
}

public class SurveyQuestionConfiguration : IEntityTypeConfiguration<SurveyQuestion>
{
    public void Configure(EntityTypeBuilder<SurveyQuestion> builder)
    {
        builder.ToTable("SURVEY_QUESTIONS");
        builder.HasKey(e => e.QuestId);
        builder.Property(e => e.QuestId).HasColumnName("SURVEY_QUESTID");
        builder.Property(e => e.SurveyId).HasColumnName("SURVEY_ID");
        builder.Property(e => e.QuestName).HasColumnName("SURVEY_QUESTNAME").HasMaxLength(1000).IsRequired();
        builder.Property(e => e.QuestType).HasColumnName("SURVEY_QUESTTYPE").HasMaxLength(100).IsRequired();
        builder.Property(e => e.MaxOptLimit).HasColumnName("SURVEY_MAXOPTLIMIT");
        builder.Property(e => e.SectionId).HasColumnName("SURVEY_SECTIONID");
        builder.Property(e => e.Mandatory).HasColumnName("SURVEY_MANDATORY")
            .HasConversion(v => v ? "Y" : "N", v => v == "Y");
        builder.Property(e => e.SortOrder).HasColumnName("SURVEY_SORT");
        builder.Property(e => e.MinOptLimit).HasColumnName("SURVEY_MINOPTLIMIT");
        builder.HasMany(e => e.Options).WithOne().HasForeignKey(e => e.QuestionId);
        builder.Ignore(e => e.DomainEvents);
    }
}

public class SurveyOptionConfiguration : IEntityTypeConfiguration<SurveyOption>
{
    public void Configure(EntityTypeBuilder<SurveyOption> builder)
    {
        builder.ToTable("SURVEY_OPTIONS");
        builder.HasKey(e => e.OptionId);
        builder.Property(e => e.OptionId).HasColumnName("SURVEY_OPTIONID");
        builder.Property(e => e.QuestionId).HasColumnName("SURVEY_QUESTIONID");
        builder.Property(e => e.Description).HasColumnName("SURVEY_DESCRIPTION").HasMaxLength(200).IsRequired();
        builder.Ignore(e => e.DomainEvents);
    }
}

public class SurveyResponseMainConfiguration : IEntityTypeConfiguration<SurveyResponseMain>
{
    public void Configure(EntityTypeBuilder<SurveyResponseMain> builder)
    {
        builder.ToTable("SURVEY_RESPONSEMAIN");
        builder.HasKey(e => e.ResponseId);
        builder.Property(e => e.ResponseId).HasColumnName("RESPONSE_ID");
        builder.Property(e => e.SurveyId).HasColumnName("RESPONSE_SURVEYID");
        builder.Property(e => e.EmpSysId).HasColumnName("RESPONSE_EMPSYSID");
        builder.Property(e => e.UpdatedBy).HasColumnName("RESPONSE_UPDATEDBY");
        builder.Property(e => e.UpdatedOn).HasColumnName("RESPONSE_UPDATEDON");
        builder.Property(e => e.Status).HasColumnName("RESPONSE_STATUS").HasMaxLength(1).IsRequired();
        builder.Property(e => e.Skip).HasColumnName("RESPONSE_SKIP");
        builder.HasMany(e => e.Details).WithOne().HasForeignKey(e => e.ResponseId);
        builder.Ignore(e => e.DomainEvents);
    }
}

public class SurveyResponseDetailConfiguration : IEntityTypeConfiguration<SurveyResponseDetail>
{
    public void Configure(EntityTypeBuilder<SurveyResponseDetail> builder)
    {
        builder.ToTable("SURVEY_RESPONSEDET");
        builder.HasKey(e => new { e.ResponseId, e.QuestionId });
        builder.Property(e => e.QuestionId).HasColumnName("RESPONSE_QUESTIONID");
        builder.Property(e => e.ResponseId).HasColumnName("RESPONSE_ID");
        builder.Property(e => e.ResponseOption).HasColumnName("RESPONSE_OPTION").HasMaxLength(1000);
        builder.Property(e => e.ResponseText).HasColumnName("RESPONSE_TEXT").HasMaxLength(1000);
        builder.Ignore(e => e.DomainEvents);
    }
}
