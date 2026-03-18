using AuditService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuditService.Infrastructure.Data.Configurations;

public class AuditGoodPracticeConfiguration : IEntityTypeConfiguration<AuditGoodPractice>
{
    public void Configure(EntityTypeBuilder<AuditGoodPractice> builder)
    {
        builder.ToTable("AUDIT_GOODPRACTICE");
        builder.HasKey(x => x.PracticeId);
        builder.Property(x => x.PracticeId).HasColumnName("PRACTICE_ID").ValueGeneratedNever();
        builder.Property(x => x.PracticeTitle).HasColumnName("PRACTICE_TITLE").HasMaxLength(200).IsRequired();
        builder.Property(x => x.PracticeDescription).HasColumnName("PRACTICE_DESCRIPTION").IsRequired();
        builder.Property(x => x.PracticeBenefits).HasColumnName("PRACTICE_BENEFITS").HasMaxLength(200).IsRequired();
        builder.Property(x => x.PracticeRemarks).HasColumnName("PRACTICE_REMARKS").IsRequired();
        builder.Property(x => x.PracticeProcess).HasColumnName("PRACTICE_PROCESS").IsRequired();
        builder.Property(x => x.PracticeEmpSysId).HasColumnName("PRACTICE_EMPSYSID").IsRequired();
        builder.Property(x => x.PracticeUnit).HasColumnName("PRACTICE_UNIT").IsRequired();
        builder.Property(x => x.PracticeLastModifiedBy).HasColumnName("PRACTICE_LASTMODIFIEDBY").IsRequired();
        builder.Property(x => x.PracticeLastModifiedOn).HasColumnName("PRACTICE_LASTMODIFIEDON").IsRequired();
        builder.Property(x => x.PracticeAttachment1).HasColumnName("PRACTICE_ATTACHMENT1").HasMaxLength(200);
        builder.Property(x => x.PracticeAttachment2).HasColumnName("PRACTICE_ATTACHMENT2").HasMaxLength(200);

        builder.HasMany(x => x.Ratings)
               .WithOne()
               .HasForeignKey(r => r.PracticeId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.Ignore(x => x.DomainEvents);
        builder.Ignore(x => x.AverageRating);
    }
}

public class AuditGoodPracticeRatingConfiguration : IEntityTypeConfiguration<AuditGoodPracticeRating>
{
    public void Configure(EntityTypeBuilder<AuditGoodPracticeRating> builder)
    {
        builder.ToTable("AUDIT_GOODPRACTICERATING");
        builder.HasKey(x => x.PracticeRatingId);
        builder.Property(x => x.PracticeRatingId).HasColumnName("PRACTICE_RATINGID").ValueGeneratedNever();
        builder.Property(x => x.PracticeId).HasColumnName("PRACTICE_ID").IsRequired();
        builder.Property(x => x.PracticeRatingBy).HasColumnName("PRACTICE_RATINGBY").IsRequired();
        builder.Property(x => x.PracticeRating).HasColumnName("PRACTICE_RATING").IsRequired();
        builder.Property(x => x.PracticeLastModifiedOn).HasColumnName("PRACTICE_LASTMODIFIEDON").IsRequired();
        builder.Ignore(x => x.DomainEvents);
    }
}

public class AuditObservationAppConfiguration : IEntityTypeConfiguration<AuditObservationApp>
{
    public void Configure(EntityTypeBuilder<AuditObservationApp> builder)
    {
        builder.ToTable("AUDIT_OBSERVATIONAPP");
        builder.HasKey(x => x.AppId);
        builder.Property(x => x.AppId).HasColumnName("APP_ID").HasColumnType("decimal(38,0)").ValueGeneratedNever();
        builder.Property(x => x.AppObvId).HasColumnName("APP_OBVID").HasColumnType("decimal(38,0)");
        builder.Property(x => x.AppEscSysId).HasColumnName("APP_ESCSYSID").HasColumnType("decimal(38,0)");
        builder.Property(x => x.AppStatus).HasColumnName("APP_STATUS").HasMaxLength(1);
        builder.Property(x => x.AppOn).HasColumnName("APP_ON");
        builder.Property(x => x.AppRemarks).HasColumnName("APP_REMARKS").HasMaxLength(200);
        builder.Property(x => x.AppObvStatus).HasColumnName("APP_OBVSTATUS").HasMaxLength(1);
        builder.Property(x => x.AppDueDate).HasColumnName("APP_DUEDATE");
        builder.Property(x => x.AppRevDueDate).HasColumnName("APP_REVDUEDATE");
        builder.Ignore(x => x.DomainEvents);
    }
}

public class AuditProcessMasterConfiguration : IEntityTypeConfiguration<AuditProcessMaster>
{
    public void Configure(EntityTypeBuilder<AuditProcessMaster> builder)
    {
        builder.ToTable("AUDIT_PROCESS_MASTER");
        builder.HasKey(x => x.AuditProcessId);
        builder.Property(x => x.AuditProcessId).HasColumnName("AUDITPROCESS_ID").HasColumnType("decimal(38,0)").ValueGeneratedNever();
        builder.Property(x => x.AuditProcessName).HasColumnName("AUDITPROCESS_NAME").HasMaxLength(50);
        builder.Property(x => x.AuditProcessCreatedBy).HasColumnName("AUDITPROCESS_CREATEDBY");
        builder.Property(x => x.AuditProcessCreatedOn).HasColumnName("AUDIT_PROCESS_CREATEDON");
        builder.Ignore(x => x.DomainEvents);
    }
}

public class AuditUserAccessConfiguration : IEntityTypeConfiguration<AuditUserAccess>
{
    public void Configure(EntityTypeBuilder<AuditUserAccess> builder)
    {
        builder.ToTable("AUDIT_USERACCESS");
        builder.HasKey(x => x.AucId);
        builder.Property(x => x.AucId).HasColumnName("AUC_ID").HasColumnType("decimal(38,0)").ValueGeneratedNever();
        builder.Property(x => x.AucEmpSysId).HasColumnName("AUC_EMPSYSID").HasColumnType("decimal(38,0)");
        builder.Property(x => x.AucBusinessId).HasColumnName("AUC_BUSINESSID").HasColumnType("decimal(38,0)");
        builder.Property(x => x.AucUnitId).HasColumnName("AUC_UNITID").HasColumnType("decimal(38,0)");
        builder.Property(x => x.AucCreatedBy).HasColumnName("AUC_CREATEDBY").HasColumnType("decimal(38,0)");
        builder.Property(x => x.AucCreatedOn).HasColumnName("AUC_CREATEDON");
        builder.Property(x => x.AucModifiedBy).HasColumnName("AUC_MODIFIEDBY").HasColumnType("decimal(38,0)");
        builder.Property(x => x.AucModifiedOn).HasColumnName("AUC_MODIFIEDON");
        builder.Ignore(x => x.DomainEvents);
    }
}

public class AuditUserMasterConfiguration : IEntityTypeConfiguration<AuditUserMaster>
{
    public void Configure(EntityTypeBuilder<AuditUserMaster> builder)
    {
        builder.ToTable("AUDIT_USERMASTER");
        builder.HasKey(x => x.AumEmpSysId);
        builder.Property(x => x.AumEmpSysId).HasColumnName("AUM_EMPSYSID").HasColumnType("decimal(38,0)").ValueGeneratedNever();
        builder.Property(x => x.AumLiveStatus).HasColumnName("AUM_LIVESTATUS").HasMaxLength(1);
        builder.Property(x => x.AumLastModifiedBy).HasColumnName("AUM_LASTMODIFIEDBY").HasColumnType("decimal(38,0)");
        builder.Property(x => x.AumLastModifiedOn).HasColumnName("AUM_LASTMODIFIEDON");
        builder.Property(x => x.AumMailStatus).HasColumnName("AUM_MAILSTATUS").HasMaxLength(1);
        builder.Property(x => x.AumUserType).HasColumnName("AUM_USERTYPE").HasMaxLength(1);
        builder.Property(x => x.AumHrmsOpted).HasColumnName("AUM_HRMSOPTED").HasMaxLength(1);
        builder.Ignore(x => x.DomainEvents);
    }
}

public class AuditYearMasterConfiguration : IEntityTypeConfiguration<AuditYearMaster>
{
    public void Configure(EntityTypeBuilder<AuditYearMaster> builder)
    {
        builder.ToTable("AUDIT_YEARMASTER");
        builder.HasKey(x => x.AymYearId);
        builder.Property(x => x.AymYearId).HasColumnName("AYM_YEARID").HasColumnType("decimal(38,0)").ValueGeneratedNever();
        builder.Property(x => x.AymFrom).HasColumnName("AYM_FROM");
        builder.Property(x => x.AymTo).HasColumnName("AYM_TO");
        builder.Property(x => x.AymLastModifiedBy).HasColumnName("AYM_LASTMODIFIEDBY").HasColumnType("decimal(38,0)");
        builder.Property(x => x.AymLastModifiedOn).HasColumnName("AYM_LASTMODIFIEDON");
        builder.Ignore(x => x.DomainEvents);
    }
}

public class IaHtmlEmailConfiguration : IEntityTypeConfiguration<IaHtmlEmail>
{
    public void Configure(EntityTypeBuilder<IaHtmlEmail> builder)
    {
        builder.ToTable("IA_HTML_EMAIL");
        builder.HasNoKey();
        builder.Property(x => x.ObvId).HasColumnName("OBV_ID").HasMaxLength(100);
        builder.Property(x => x.MFrom).HasColumnName("MFROM").HasMaxLength(100);
        builder.Property(x => x.MTo).HasColumnName("MTO").HasMaxLength(100);
        builder.Property(x => x.MCc).HasColumnName("MCC").HasMaxLength(100);
        builder.Property(x => x.MBcc).HasColumnName("MBCC").HasMaxLength(100);
        builder.Property(x => x.Subject).HasColumnName("SUBJECT").HasMaxLength(200);
        builder.Property(x => x.Message).HasColumnName("MESSAGE");
        builder.Property(x => x.MServer).HasColumnName("MSERVER").HasMaxLength(20);
        builder.Property(x => x.MPort).HasColumnName("MPORT").HasMaxLength(5);
        builder.Property(x => x.OnDate).HasColumnName("ONDATE").HasMaxLength(100);
        builder.Ignore(x => x.DomainEvents);
    }
}

public class IaEscalationMailConfiguration : IEntityTypeConfiguration<IaEscalationMail>
{
    public void Configure(EntityTypeBuilder<IaEscalationMail> builder)
    {
        builder.ToTable("IAESCALATION_MAILS");
        builder.HasKey(x => x.MailId);
        builder.Property(x => x.MailId).HasColumnName("MAIL_ID").HasColumnType("decimal(18,0)").ValueGeneratedNever();
        builder.Property(x => x.MailObservationAuditId).HasColumnName("MAIL_OBSERVATIONAUDITID").HasColumnType("decimal(18,0)");
        builder.Property(x => x.MailAuditeeSysId).HasColumnName("MAIL_AUDITEESYSID").HasColumnType("decimal(18,0)");
        builder.Property(x => x.MailEscalatoSysId).HasColumnName("MAIL_ESCALATOSYSID").HasColumnType("decimal(18,0)");
        builder.Property(x => x.MailSubject).HasColumnName("MAIL_SUBJECT").HasMaxLength(1000);
        builder.Property(x => x.MailContent).HasColumnName("MAIL_CONTENT");
        builder.Property(x => x.MailTo).HasColumnName("MAIL_TO").HasMaxLength(500);
        builder.Property(x => x.MailCc).HasColumnName("MAIL_CC").HasMaxLength(500);
        builder.Property(x => x.MailSentBy).HasColumnName("MAIL_SENTBY").HasColumnType("decimal(38,0)");
        builder.Property(x => x.MailSentOn).HasColumnName("MAIL_SENTON");
        builder.Ignore(x => x.DomainEvents);
    }
}
